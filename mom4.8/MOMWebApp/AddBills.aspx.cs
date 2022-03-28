using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.OleDb;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.payroll;
using Newtonsoft.Json;
using BusinessEntity.Payroll;
using BusinessEntity.CommonModel;
using System.Text;

public partial class AddBills : System.Web.UI.Page
{
    #region Variables

    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    PO objPO = new PO();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();
    CD _objCD = new CD();
    bool isCopy = false;
    double sum = 0;
    string clientID;
    BL_Inventory _objInventory = new BL_Inventory();
    BusinessEntity.Inventory _objInv = new BusinessEntity.Inventory();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetOutStandingPOByIdParam _getOutStandingPOById = new GetOutStandingPOByIdParam();
    GetVendorSearchParam _getVendorSearch = new GetVendorSearchParam();
    GetReceivePOListParam _getReceivePOList = new GetReceivePOListParam();
    AddReceivePOItemParam _addReceivePOItem = new AddReceivePOItemParam();
    UpdatePOItemBalanceParam _updatePOItemBalance = new UpdatePOItemBalanceParam();
    UpdatePOItemQuanParam _updatePOItemQuan = new UpdatePOItemQuanParam();
    AddEditReceivePOParam _addEditReceivePO = new AddEditReceivePOParam();
    updateJobCommParam _updateJobComm = new updateJobCommParam();
    GetPOReceivePOByIdParam _getPOReceivePOById = new GetPOReceivePOByIdParam();
    UpdatePOItemWarehouseLocationParam _updatePOItemWarehouseLocation = new UpdatePOItemWarehouseLocationParam();
    AddReceiveInventoryWHTransParam _AddReceiveInventoryWHTrans = new AddReceiveInventoryWHTransParam();
    CreateReceivePOInvWarehouseTransParam _CreateReceivePOInvWarehouseTrans = new CreateReceivePOInvWarehouseTransParam();
    ReceivePOInvWarehouseTransParam _ReceivePOInvWarehouseTrans = new ReceivePOInvWarehouseTransParam();
    GetPJDetailByIDParam _GetPJDetailByID = new GetPJDetailByIDParam();
    GetBillTransDetailsParam _GetBillTransDetails = new GetBillTransDetailsParam();
    GetPJRecurrDetailByIDParam _GetPJRecurrDetailByID = new GetPJRecurrDetailByIDParam();
    GetBillRecurrTransactionsParam _GetBillRecurrTransactions = new GetBillRecurrTransactionsParam();
    UpdatePOStatusParam _UpdatePOStatus = new UpdatePOStatusParam();
    UpdateReceivePOStatusByPOIDParam _UpdateReceivePOStatusByPOID = new UpdateReceivePOStatusByPOIDParam();
    UpdateReceivePOStatusParam _UpdateReceivePOStatus = new UpdateReceivePOStatusParam();
    GetMaxReceivePOIdParam _GetMaxReceivePOId = new GetMaxReceivePOIdParam();
    GetInventoryItemStatusParam _GetInventoryItemStatus = new GetInventoryItemStatusParam();
    GetBillHistoryPaymentParam _GetBillHistoryPayment = new GetBillHistoryPaymentParam();
    GetBomTypeParam _GetBomType = new GetBomTypeParam();
    GetInvDefaultAcctParam _GetInvDefaultAcct = new GetInvDefaultAcctParam();
    IsExistPOParam _IsExistPO = new IsExistPOParam();
    GetClosePOCheckParam _GetClosePOCheck = new GetClosePOCheckParam();
    getCustomFieldParam _getCustomField = new getCustomFieldParam();
    getSTaxParam _getSTax = new getSTaxParam();
    getCustomFieldsControlParam _getCustomFieldsControl = new getCustomFieldsControlParam();
    GetChartParam _GetChart = new GetChartParam();
    GetVendorByNameParam _GetVendorByName = new GetVendorByNameParam();
    GetAutoFillOnHandBalanceParam _GetAutoFillOnHandBalance = new GetAutoFillOnHandBalanceParam();
    AddBillsParam _AddBills = new AddBillsParam();
    UpdateRecurrBillsParam _UpdateRecurrBills = new UpdateRecurrBillsParam();
    UpdateBillsParam _UpdateBills = new UpdateBillsParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    UpdateVendorSTaxParam _UpdateVendorSTax = new UpdateVendorSTaxParam();
    GetUserByIdParam _GetUserById = new GetUserByIdParam();
    UpdateBillsJobDetailsParam _UpdateBillsJobDetails = new UpdateBillsJobDetailsParam();
    GetBillingItemsParam _GetBillingItems = new GetBillingItemsParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetBillsLogsParam _GetBillsLogs = new GetBillsLogsParam();
    UpdateApplyCreditDateParam _UpdateApplyCreditDate = new UpdateApplyCreditDateParam();
    StringBuilder _JobIds = new StringBuilder();

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

            WebBaseUtility.UpdatePageTitle(this, "Bills", Request.QueryString["id"], Request.QueryString["t"]);
            if (!IsPostBack)
            {
                if (!System.IO.Directory.Exists(Server.MapPath(Request.ApplicationPath) + "/TempPDF/ImportBills"))
                    System.IO.Directory.CreateDirectory(Server.MapPath(Request.ApplicationPath) + "/TempPDF/ImportBills");

                imgPaid.Visible = false;
                imgVoid.Visible = false;

                userpermissions();
                FillFrequency();
                SetTax();
                FillSalesTax();
                divSuccess.Visible = false;
                if (Request.QueryString["id"] != null)
                {
                    if (Request.QueryString["r"] != null)
                    {
                        if (Request.QueryString["r"].ToString() == "1")
                        {

                            Page.Title = "Edit Recurring Bills || MOM";
                            //lblFrequency.Visible = true;
                            //ddlFrequency.Visible = true;

                            GetDateRecurr();


                        }
                    }
                    else
                    {
                        Page.Title = "Edit Bills || MOM";
                        GetDate();
                        SetPaidBtn();
                        liLogs.Style["display"] = "inline-block";
                        tbLogs.Style["display"] = "block";
                        liHistoryPayment.Style["display"] = "inline-block";
                        tblPayment.Style["display"] = "block";

                    }

                }
                else
                {
                    Page.Title = "Add Bills || MOM";
                    SetBillForm();
                }
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        divSuccess.Visible = false;
                        liLogs.Style["display"] = "none";
                        tbLogs.Style["display"] = "none";

                        liHistoryPayment.Style["display"] = "none";
                        tblPayment.Style["display"] = "none";
                    }
                }
                else
                {

                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
                }

                FillBomType();
                Permission();
                HighlightSideMenu("acctPayable", "lnkAddBill", "acctPayableSub");
                txtQty.Text = "0.00";
                txtBudgetUnit.Text = "0.00";
                lblBudgetExt.Text = "0.00";
                // for copy functionality from managebill screen 
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        //  txtRef.Text = "";
                        txtDate.Text = DateTime.Now.ToShortDateString();
                        txtPostingDate.Text = DateTime.Now.ToShortDateString();

                    }
                }
                GetInvDefaultAcct();
            }

            if (Request.QueryString["id"] != null && Convert.ToString(Request.QueryString["t"]) != "c")
            {
                hdEditCase.Value = "true";
            }
            fillPOCustom();

            #region TrackingInventory

            ////TrackingInventory
            General _objPropGeneral = new General();
            BL_General _objBLGeneral = new BL_General();

            DataSet _dsCustom = new DataSet();
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _getCustomField.ConnConfig = Session["config"].ToString();
                _getCustomField.fieldName = "InvGL";

                string APINAME = "BillAPI/AddBills_GetCustomField";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomField);

                _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
            }
            else
            {
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
            }

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


                RadGrid_gvJobCostItems.Columns.FindByUniqueName("Warehouse").Visible = false;
                RadGrid_gvJobCostItems.Columns.FindByUniqueName("WHLocID").Visible = false;

            }
            else
            {
                RadGrid_gvJobCostItems.Columns.FindByUniqueName("Warehouse").Visible = true;
                RadGrid_gvJobCostItems.Columns.FindByUniqueName("WHLocID").Visible = true;
                GetInvDefaultAcct();
            }
            #endregion

           DocumentPermission();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        //_objPropUser.ConnConfig = Session["config"].ToString();
        //_getSTax.ConnConfig = Session["config"].ToString();

        //List<STaxViewModel> _lstSTaxViewModel = new List<STaxViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetSTax";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSTax);

        //    _lstSTaxViewModel = (new JavaScriptSerializer()).Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
        //    ds = CommonMethods.ToDataSet<STaxViewModel>(_lstSTaxViewModel);
        //}
        //else
        //{
        //    ds = _objBLUser.getSTax(_objPropUser);
        //}

        ///////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
        //DataTable dtaxdt = new DataTable();
        //dtaxdt = ds.Tables[0];
        //DataView dv = new DataView(dtaxdt);
        //dv.RowFilter = "state='' OR state='" + hdnSTaxState.Value.ToString().Trim() + "'"; // query example = "id = 10"
        //if (dv.ToTable().Rows.Count > 0)
        //{
        //    ddlSTax.DataSource = dv.ToTable();
        //    /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
        //    //objBL_User.getSalesTax(objPropUser);
        //    //ddlSTax.DataSource = ds.Tables[0];
        //    ddlSTax.DataTextField = "StaxName";
        //    ddlSTax.DataValueField = "Name";
        //    ddlSTax.DataBind();
        //}
        //ddlSTax.Items.Insert(0, new ListItem(":: Select ::", ""));

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _getSTax.ConnConfig = Session["config"].ToString();

            List<STaxViewModel> _lstSTaxViewModel = new List<STaxViewModel>();

            string APINAME = "BillAPI/AddBills_GetSTax";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSTax);

            _lstSTaxViewModel = (new JavaScriptSerializer()).Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<STaxViewModel>(_lstSTaxViewModel);


            /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
            DataTable dtaxdt = new DataTable();
            dtaxdt = ds.Tables[0];
            DataView dv = new DataView(dtaxdt);
            dv.RowFilter = "state='' OR state='" + hdnSTaxState.Value.ToString().Trim() + "'"; // query example = "id = 10"
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlSTax.DataSource = dv.ToTable();
                /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
                //objBL_User.getSalesTax(objPropUser);
                //ddlSTax.DataSource = ds.Tables[0];
                ddlSTax.DataTextField = "StaxName";
                ddlSTax.DataValueField = "Name";
                ddlSTax.DataBind();
            }
            ddlSTax.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        else
        {
            _objPropUser.ConnConfig = Session["config"].ToString();

            ds = _objBLUser.getSTax(_objPropUser);

            /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
            DataTable dtaxdt = new DataTable();
            dtaxdt = ds.Tables[0];
            DataView dv = new DataView(dtaxdt);
            dv.RowFilter = "state='' OR state='" + hdnSTaxState.Value.ToString().Trim() + "'"; // query example = "id = 10"
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlSTax.DataSource = dv.ToTable();
                /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
                //objBL_User.getSalesTax(objPropUser);
                //ddlSTax.DataSource = ds.Tables[0];
                ddlSTax.DataTextField = "StaxName";
                ddlSTax.DataValueField = "Name";
                ddlSTax.DataBind();
            }
            ddlSTax.Items.Insert(0, new ListItem(":: Select ::", ""));
        }


        /////////////// SALE Tax Label Text Changed///////////////////
        //_objPropGeneral.ConnConfig = Session["config"].ToString();
        //_objPropGeneral.CustomName = "Country";
        //DataSet dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);

        //if (dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
        //    {
        //        spansalestax.InnerText = "Provincial Tax";
        //        //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
        //        ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
        //        string gst_gstgl = "";
        //        string gst_gstrate = "";
        //        _objPropGeneral.ConnConfig = Session["config"].ToString();
        //        DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
        //        if (_dsCustom.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
        //            {


        //                if (_dr["Name"].ToString().Equals("GSTGL"))
        //                {
        //                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //                    {
        //                        _objChart.ConnConfig = Session["config"].ToString();
        //                        _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());
        //                        DataSet _dsChart = _objBLChart.GetChart(_objChart);

        //                        if (_dsChart.Tables[0].Rows.Count > 0)
        //                        {
        //                            //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
        //                            gst_gstgl = _dr["Label"].ToString();
        //                        }

        //                    }
        //                    else
        //                    {
        //                        gst_gstgl = "0";
        //                    }
        //                }
        //                else if (_dr["Name"].ToString().Equals("GSTRate"))
        //                {
        //                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //                    {
        //                        gst_gstrate = _dr["Label"].ToString();

        //                    }
        //                    else
        //                    {
        //                        gst_gstrate = "0";
        //                    }
        //                }

        //            }

        //        }

        //        if (gst_gstrate == "")
        //        {
        //            gst_gstrate = "0";
        //        }
        //        if (gst_gstrate == "0" || gst_gstrate == "0.0000")
        //        {
        //            spansalestax.InnerText = "Sales Tax";
        //        }
        //        ////////////////////////////////////////////////////////
        //    }
        //    else
        //    {
        //        spansalestax.InnerText = "Sales Tax";
        //    }
        //}
        //else
        //{
        //    spansalestax.InnerText = "Sales Tax";
        //}



        /////////////// SALE Tax Label Text Changed///////////////////


    }
    private void SetTax()
    {

        //_objPropGeneral.ConnConfig = Session["config"].ToString();

        //_getCustomFieldsControl.ConnConfig = Session["config"].ToString();

        //DataSet _dsCustom = new DataSet();
        //List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetCustomFieldsControl";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

        //    _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
        //    _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        //}
        //else
        //{
        //    _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
        //}

        //if (_dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
        //    {


        //        if (_dr["Name"].ToString().Equals("GSTGL"))
        //        {
        //            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //            {
        //                _objChart.ConnConfig = Session["config"].ToString();
        //                _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

        //                _GetChart.ConnConfig = Session["config"].ToString();
        //                _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

        //                DataSet _dsChart = new DataSet();

        //                List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

        //                if (IsAPIIntegrationEnable == "YES")
        //                //if (Session["APAPIEnable"].ToString() == "YES")
        //                {
        //                    string APINAME = "BillAPI/AddBills_GetChart";

        //                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChart);

        //                    _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
        //                    _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);
        //                }
        //                else
        //                {
        //                    _dsChart = _objBLChart.GetChart(_objChart);
        //                }

        //                if (_dsChart.Tables[0].Rows.Count > 0)
        //                {
        //                    //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
        //                    hdnGSTGL.Value = _dr["Label"].ToString();
        //                }

        //            }
        //            else
        //            {
        //                hdnGSTGL.Value = "0";
        //            }
        //        }
        //        else if (_dr["Name"].ToString().Equals("GSTRate"))
        //        {
        //            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //            {
        //                txtgst.Text = _dr["Label"].ToString();
        //                hdnGST.Value = _dr["Label"].ToString();
        //                if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
        //                {
        //                    hdnGST.Value = _dr["Label"].ToString();
        //                }
        //                else
        //                {
        //                    hdnGST.Value = "0";
        //                }
        //            }
        //            else
        //            {
        //                txtgst.Text = "0";
        //                hdnGST.Value = "0";
        //            }
        //        }

        //    }

        //}
        //if (txtgst.Text.Trim() == "")
        //{
        //    txtgst.Text = "0";
        //}
        //if (hdnGST.Value.Trim() == "")
        //{
        //    hdnGST.Value = "0";
        //}

        //if (txtgstgv.Visible == true)
        //{
        //    if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
        //    {
        //        txtgstgv.Visible = false;
        //        RadGrid_gvJobCostItems.Columns[12].Visible = false;
        //    }

        //}

        DataSet _dsCustom = new DataSet();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();


            string APINAME = "BillAPI/AddBills_GetCustomFieldsControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

            _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);


            if (_dsCustom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                {


                    if (_dr["Name"].ToString().Equals("GSTGL"))
                    {
                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                        {

                            _GetChart.ConnConfig = Session["config"].ToString();
                            _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                            DataSet _dsChart = new DataSet();

                            List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

                            string APINAME1 = "BillAPI/AddBills_GetChart";

                            APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetChart);

                            _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse1.ResponseData);
                            _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);


                            if (_dsChart.Tables[0].Rows.Count > 0)
                            {
                                //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                hdnGSTGL.Value = _dr["Label"].ToString();
                            }

                        }
                        else
                        {
                            hdnGSTGL.Value = "0";
                        }
                    }
                    else if (_dr["Name"].ToString().Equals("GSTRate"))
                    {
                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                        {
                            txtgst.Text = _dr["Label"].ToString();
                            hdnGST.Value = _dr["Label"].ToString();
                            if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
                            {
                                hdnGST.Value = _dr["Label"].ToString();
                            }
                            else
                            {
                                hdnGST.Value = "0";
                            }
                        }
                        else
                        {
                            txtgst.Text = "0";
                            hdnGST.Value = "0";
                        }
                    }

                }

            }
            if (txtgst.Text.Trim() == "")
            {
                txtgst.Text = "0";
            }
            if (hdnGST.Value.Trim() == "")
            {
                hdnGST.Value = "0";
            }

            if (txtgstgv.Visible == true)
            {
                if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                {
                    txtgstgv.Visible = false;
                    RadGrid_gvJobCostItems.Columns[12].Visible = false;
                }

            }

        }
        else
        {
            _objPropGeneral.ConnConfig = Session["config"].ToString();

            _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);

            if (_dsCustom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                {


                    if (_dr["Name"].ToString().Equals("GSTGL"))
                    {
                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                        {
                            _objChart.ConnConfig = Session["config"].ToString();
                            _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                            DataSet _dsChart = new DataSet();

                            _dsChart = _objBLChart.GetChart(_objChart);

                            if (_dsChart.Tables[0].Rows.Count > 0)
                            {
                                //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                hdnGSTGL.Value = _dr["Label"].ToString();
                            }

                        }
                        else
                        {
                            hdnGSTGL.Value = "0";
                        }
                    }
                    else if (_dr["Name"].ToString().Equals("GSTRate"))
                    {
                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                        {
                            txtgst.Text = _dr["Label"].ToString();
                            hdnGST.Value = _dr["Label"].ToString();
                            if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
                            {
                                hdnGST.Value = _dr["Label"].ToString();
                            }
                            else
                            {
                                hdnGST.Value = "0";
                            }
                        }
                        else
                        {
                            txtgst.Text = "0";
                            hdnGST.Value = "0";
                        }
                    }

                }

            }
            if (txtgst.Text.Trim() == "")
            {
                txtgst.Text = "0";
            }
            if (hdnGST.Value.Trim() == "")
            {
                hdnGST.Value = "0";
            }

            if (txtgstgv.Visible == true)
            {
                if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                {
                    txtgstgv.Visible = false;
                    RadGrid_gvJobCostItems.Columns[12].Visible = false;
                }

            }

        }

        //_objPropGeneral.ConnConfig = Session["config"].ToString();
        //_objPropGeneral.CustomName = "Country";
        //DataSet dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);

        //if (dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
        //    {
        //        txtgstgv.Visible = true;
        //        //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
        //    }
        //    else
        //    {
        //        txtgstgv.Visible = false;
        //    }
        //}
        //else
        //{
        //    txtgstgv.Visible = false;
        //}


    }
    #endregion
    private void SetPaidBtn()
    {
        //_objPJ.ConnConfig = Session["config"].ToString();
        //_objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
        //DataSet ds = _objBLBills.GetCheckByPaidBill(_objPJ);
        //string imgLnk = "";
        ////= "  e.preventDefault(); ";
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    if (!string.IsNullOrEmpty(dr["PITR"].ToString()))
        //    {
        //        imgLnk += "window.open('editcheck.aspx?id=" + dr["PITR"].ToString() + "');";
        //    }
        //}
        //imgPaid.Attributes.Add("onClick", imgLnk);
    }
    //protected void imgPaid_Click(object sender, ImageClickEventArgs e)
    //{
    //    _objPJ.ConnConfig = Session["config"].ToString();
    //    _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
    //    DataSet ds = _objBLBills.GetCheckByPaidBill(_objPJ);
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        if (!string.IsNullOrEmpty(dr["PITR"].ToString()))
    //        {
    //            Response.Redirect("editcheck.aspx?id="+ dr["PITR"].ToString());
    //            //imgPaid.OnClientClick = "window.open('editcheck.aspx?id=" + dr["PITR"].ToString() + "');";
    //        }
    //    }
    //}

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

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Bills"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addbills.aspx?id=" + dt.Rows[index - 1]["ID"]);
            }
            if (index == 0)
            {
                Response.Redirect("addbills.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
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
            DataTable dt = (DataTable)Session["Bills"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addbills.aspx?id=" + dt.Rows[index + 1]["ID"]);
            }
            if (index == c)
            {
                Response.Redirect("addbills.aspx?id=" + dt.Rows[0]["ID"]);

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
            DataTable dt = (DataTable)Session["Bills"];
            Response.Redirect("addbills.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Bills"];
            Response.Redirect("addbills.aspx?id=" + dt.Rows[0]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
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
        else if (Request.QueryString["frm"] != null)
        {
            if (Request.QueryString["frm"].ToString() == "MNG2")
            {
                Response.Redirect("ManageBills.aspx");
            }
            else if (Session["alId"] != null)
            {
                Response.Redirect("AccountLedger.aspx?id=" + Session["alId"]);
            }
            else
            {
                Response.Redirect("ManageBills.aspx");
            }
        }
        else
        {
            Response.Redirect("ManageBills.aspx");
        }
        //else if (Session["alId"] != null)
        //{
        //    if(Request.QueryString["frm"].ToString() != null)
        //    {
        //        if (Request.QueryString["frm"].ToString() == "MNG2")
        //        {
        //            Response.Redirect("ManageBills.aspx");
        //        }
        //    }
        //    else
        //    Response.Redirect("AccountLedger.aspx?id=" + Session["alId"]);
        //}
        //else
        //{
        //    Response.Redirect("ManageBills.aspx");
        //}
    }

    private bool ValidateGrid(DataTable dt)
    {
        bool Flag = false;
        bool IsExist;
        bool IsValid = true;

        //if (string.IsNullOrEmpty(hdnVendorID.Value) && !string.IsNullOrEmpty(txtVendor.Text))
        //{

        //    DataSet ds = new DataSet();
        //    _objVendor.SearchValue = txtVendor.Text;
        //    _objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();

        //    _GetVendorByName.SearchValue = txtVendor.Text;
        //    _GetVendorByName.ConnConfig = HttpContext.Current.Session["config"].ToString();

        //    if (Session["CmpChkDefault"].ToString() == "1")
        //    {
        //        _objVendor.EN = 1;
        //        _GetVendorByName.EN = 1;
        //    }
        //    else
        //    {
        //        _objVendor.EN = 0;
        //        _GetVendorByName.EN = 0;
        //    }


        //    List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        //    if (IsAPIIntegrationEnable == "YES")
        //    //if (Session["APAPIEnable"].ToString() == "YES")
        //    {
        //        string APINAME = "BillAPI/AddBills_GetVendorByName";

        //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetVendorByName);

        //        _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
        //        ds = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
        //    }
        //    else
        //    {
        //        ds = _objBLVendor.GetVendorByName(_objVendor);
        //    }

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        hdnVendorID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
        //    }
        //}

        //try
        //{
        //    _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
        //}
        //catch (Exception)
        //{
        //    //ClientScript.RegisterStartupScript(Page.GetType(), "vendorWarning", "noty({text: 'Can't find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        //    return false;
        //}
        //_objPJ.ConnConfig = Session["config"].ToString();
        //_objPJ.Ref = txtRef.Text;
        ////_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);

        //if (Request.QueryString["t"] != null)
        //{
        //    if (Request.QueryString["t"].ToString() == "c")
        //    {
        //        isCopy = true;
        //    }

        //}
        //if (isCopy)
        //{
        //    //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
        //    IsExist = false;
        //    GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

        //    Flag = (bool)ViewState["FlagPeriodClose"];

        //}
        //else if ((Request.QueryString["id"] != null))
        //{
        //    _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);
        //    //IsExist = _objBLBills.IsBillExistForEdit(_objPJ);
        //    IsExist = false;
        //    GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
        //    Flag = (bool)ViewState["FlagPeriodClose"];
        //}
        //else
        //{
        //    //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
        //    IsExist = false;
        //    GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

        //    Flag = (bool)ViewState["FlagPeriodClose"];
        //}

        //if (IsExist.Equals(false))
        //{
        //    if (Flag)
        //    {
        //        string strMessage = "";
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            if (dr["JobName"].ToString() != "")
        //            {
        //                if (dr["Phase"].ToString() == "")
        //                {

        //                    IsValid = false;
        //                    strMessage = "Please enter a code for the Project." + dr["JobName"].ToString();
        //                }

        //            }
        //            if ((dr["Amount"].ToString() == "") || (dr["fDesc"].ToString() == "") || (dr["AcctID"].ToString() == ""))
        //            {
        //                IsValid = false;
        //                strMessage = "Item description, acct no. and amount are required.";
        //            }
        //        }
        //        if (dt.Rows.Count.Equals(0))
        //        {
        //            IsValid = false;
        //            strMessage = "You must have at least one item on the purchase order.";
        //        }

        //        if (IsValid.Equals(false))
        //        {
        //            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + strMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        //        }
        //    }
        //    else
        //    {
        //        IsValid = false;
        //        divSuccess.Visible = true;
        //    }
        //}
        //else
        //{
        //    IsValid = false;
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number with this vendor already exists, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        //}


        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            if (string.IsNullOrEmpty(hdnVendorID.Value) && !string.IsNullOrEmpty(txtVendor.Text))
            {

                DataSet ds = new DataSet();

                _GetVendorByName.SearchValue = txtVendor.Text;
                _GetVendorByName.ConnConfig = HttpContext.Current.Session["config"].ToString();

                if (Session["CmpChkDefault"].ToString() == "1")
                {

                    _GetVendorByName.EN = 1;
                }
                else
                {

                    _GetVendorByName.EN = 0;
                }


                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();


                string APINAME = "BillAPI/AddBills_GetVendorByName";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetVendorByName);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnVendorID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                }
            }

            try
            {
                _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "vendorWarning", "noty({text: 'Can't find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.Ref = txtRef.Text;
            //_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);

            if (Request.QueryString["t"] != null)
            {
                if (Request.QueryString["t"].ToString() == "c")
                {
                    isCopy = true;
                }

            }
            if (isCopy)
            {
                //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

                Flag = (bool)ViewState["FlagPeriodClose"];

            }
            else if ((Request.QueryString["id"] != null))
            {
                _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);
                _UpdateBills.ID = Convert.ToInt32(Request.QueryString["id"]);
                _UpdateRecurrBills.ID = Convert.ToInt32(Request.QueryString["id"]);
                //IsExist = _objBLBills.IsBillExistForEdit(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
                Flag = (bool)ViewState["FlagPeriodClose"];
            }
            else
            {
                //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

                Flag = (bool)ViewState["FlagPeriodClose"];
            }

            if (IsExist.Equals(false))
            {
                if (Flag)
                {
                    string strMessage = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["JobName"].ToString() != "")
                        {
                            if (dr["Phase"].ToString() == "")
                            {

                                IsValid = false;
                                strMessage = "Please enter a code for the Project." + dr["JobName"].ToString();
                            }

                        }
                        if ((dr["Amount"].ToString() == "") || (dr["fDesc"].ToString() == "") || (dr["AcctID"].ToString() == ""))
                        {
                            IsValid = false;
                            strMessage = "Item description, acct no. and amount are required.";
                        }
                    }
                    if (dt.Rows.Count.Equals(0))
                    {
                        IsValid = false;
                        strMessage = "You must have at least one item on the purchase order.";
                    }

                    if (IsValid.Equals(false))
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + strMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    IsValid = false;
                    divSuccess.Visible = true;
                }
            }
            else
            {
                IsValid = false;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number with this vendor already exists, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            if (string.IsNullOrEmpty(hdnVendorID.Value) && !string.IsNullOrEmpty(txtVendor.Text))
            {

                DataSet ds = new DataSet();
                _objVendor.SearchValue = txtVendor.Text;
                _objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();

                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objVendor.EN = 1;

                }
                else
                {
                    _objVendor.EN = 0;

                }

                ds = _objBLVendor.GetVendorByName(_objVendor);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    hdnVendorID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                }
            }

            try
            {
                _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "vendorWarning", "noty({text: 'Can't find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.Ref = txtRef.Text;
            //_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);

            if (Request.QueryString["t"] != null)
            {
                if (Request.QueryString["t"].ToString() == "c")
                {
                    isCopy = true;
                }

            }
            if (isCopy)
            {
                //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

                Flag = (bool)ViewState["FlagPeriodClose"];

            }
            else if ((Request.QueryString["id"] != null))
            {
                _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);
                //IsExist = _objBLBills.IsBillExistForEdit(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
                Flag = (bool)ViewState["FlagPeriodClose"];
            }
            else
            {
                //IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
                IsExist = false;
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));

                Flag = (bool)ViewState["FlagPeriodClose"];
            }

            if (IsExist.Equals(false))
            {
                if (Flag)
                {
                    string strMessage = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["JobName"].ToString() != "")
                        {
                            if (dr["Phase"].ToString() == "")
                            {

                                IsValid = false;
                                strMessage = "Please enter a code for the Project." + dr["JobName"].ToString();
                            }

                        }
                        if ((dr["Amount"].ToString() == "") || (dr["fDesc"].ToString() == "") || (dr["AcctID"].ToString() == ""))
                        {
                            IsValid = false;
                            strMessage = "Item description, acct no. and amount are required.";
                        }
                    }
                    if (dt.Rows.Count.Equals(0))
                    {
                        IsValid = false;
                        strMessage = "You must have at least one item on the purchase order.";
                    }

                    if (IsValid.Equals(false))
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + strMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    IsValid = false;
                    divSuccess.Visible = true;
                }
            }
            else
            {
                IsValid = false;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number with this vendor already exists, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }

        }

        return IsValid;
    }

    private bool ValidateGridUpdPrj(DataTable dt)
    {
        bool IsValid = true;

        {
            string strMessage = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["JobName"].ToString() != "")
                {
                    if (dr["Phase"].ToString() == "")
                    {

                        IsValid = false;
                        strMessage = "Please enter a code for the Project." + dr["JobName"].ToString();
                    }

                }
                if ((dr["Amount"].ToString() == "") || (dr["fDesc"].ToString() == "") || (dr["AcctID"].ToString() == ""))
                {
                    IsValid = false;
                    strMessage = "Item description, acct no. and amount are required.";
                }
            }
            if (dt.Rows.Count.Equals(0))
            {
                IsValid = false;
                strMessage = "You must have at least one item on the purchase order.";
            }

            if (IsValid.Equals(false))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + strMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }

            return IsValid;
        }
    }
    private string ValidateOnHand(int ItemId, string WarehouseID, int locationID, double BillQty)
    {
        string strErrorMessage = string.Empty;
        //IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
        //invWarehouseLoc.InvID = ItemId;
        //invWarehouseLoc.WarehouseID = WarehouseID;
        //invWarehouseLoc.locationID = locationID;

        //_GetAutoFillOnHandBalance.InvID = ItemId;
        //_GetAutoFillOnHandBalance.WarehouseID = WarehouseID;
        //_GetAutoFillOnHandBalance.locationID = locationID;

        //// TODO: Get onHand value from database
        //DataSet ds = new DataSet();


        //List<GetAutoFillOnHandBalanceViewModel> _lstGetAutoFillOnHandBalance = new List<GetAutoFillOnHandBalanceViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetAutoFillOnHandBalance";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAutoFillOnHandBalance);

        //    _lstGetAutoFillOnHandBalance = (new JavaScriptSerializer()).Deserialize<List<GetAutoFillOnHandBalanceViewModel>>(_APIResponse.ResponseData);
        //    ds = CommonMethods.ToDataSet<GetAutoFillOnHandBalanceViewModel>(_lstGetAutoFillOnHandBalance);
        //}
        //else
        //{
        //    ds = _objInventory.GetAutoFillOnHandBalance(invWarehouseLoc);
        //}

        //var table = ds.Tables[0];
        //if (table != null && table.Rows.Count > 0)
        //{
        //    var onHandVal = Convert.ToDouble(table.Rows[0]["Hand"].ToString());
        //    if (onHandVal + BillQty < 0)
        //    {
        //        strErrorMessage = "On hand value is not enough.";                
        //    }
        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _GetAutoFillOnHandBalance.InvID = ItemId;
            _GetAutoFillOnHandBalance.WarehouseID = WarehouseID;
            _GetAutoFillOnHandBalance.locationID = locationID;

            // TODO: Get onHand value from database
            DataSet ds = new DataSet();


            List<GetAutoFillOnHandBalanceViewModel> _lstGetAutoFillOnHandBalance = new List<GetAutoFillOnHandBalanceViewModel>();


            string APINAME = "BillAPI/AddBills_GetAutoFillOnHandBalance";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAutoFillOnHandBalance);

            _lstGetAutoFillOnHandBalance = (new JavaScriptSerializer()).Deserialize<List<GetAutoFillOnHandBalanceViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetAutoFillOnHandBalanceViewModel>(_lstGetAutoFillOnHandBalance);


            var table = ds.Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                var onHandVal = Convert.ToDouble(table.Rows[0]["Hand"].ToString());
                if (onHandVal + BillQty < 0)
                {
                    strErrorMessage = "On hand value is not enough.";
                }
            }
        }
        else
        {
            IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
            invWarehouseLoc.InvID = ItemId;
            invWarehouseLoc.WarehouseID = WarehouseID;
            invWarehouseLoc.locationID = locationID;

            // TODO: Get onHand value from database
            DataSet ds = new DataSet();

            ds = _objInventory.GetAutoFillOnHandBalance(invWarehouseLoc);

            var table = ds.Tables[0];
            if (table != null && table.Rows.Count > 0)
            {
                var onHandVal = Convert.ToDouble(table.Rows[0]["Hand"].ToString());
                if (onHandVal + BillQty < 0)
                {
                    strErrorMessage = "On hand value is not enough.";
                }
            }
        }

        return strErrorMessage;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValid())
            {
                DataTable dt = GetTransaction();
                bool checktqty = false;


                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    if (ValidateGrid(dt))
                    {
                        dt.Columns.Remove("AcctNo");
                        dt.Columns.Remove("JobName");
                        //dt.Columns.Remove("Phase");
                        dt.Columns.Remove("UName");
                        //     dt.Columns.Remove("UtaxGL");
                        //dt.Columns.Remove("Item");
                        //dt.Columns.Remove("TypeID");
                        dt.Columns.Remove("Loc");
                        dt.Columns.Remove("Line");
                        dt.Columns.Remove("PrvInQuan");
                        dt.Columns.Remove("PrvIn");
                        dt.Columns.Remove("OutstandQuan");
                        dt.Columns.Remove("OutstandBalance");
                        dt.Columns.Remove("AmountTot");

                        dt.Columns.Remove("Warehousefdesc");
                        dt.Columns.Remove("Locationfdesc");

                        dt.Columns.Remove("RPOItemId");
                        dt.Columns.Remove("POItemId");
                        //dt.Columns.Remove("IsPO");

                        dt.Select("JobID = 0")
                              .AsEnumerable().ToList()
                              .ForEach(t => t["JobID"] = DBNull.Value);
                        //dt.Select("PhaseID = 0")
                        //     .AsEnumerable().ToList()
                        //     .ForEach(t => t["PhaseID"] = DBNull.Value);
                        dt.Select("ItemID = 0")
                            .AsEnumerable().ToList()
                            .ForEach(t => t["ItemID"] = DBNull.Value);

                        //dt.Select("ItemDesc = '' or ItemDesc is null")
                        //    .AsEnumerable().ToList()
                        //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);

                        dt.AcceptChanges();

                        foreach (DataRow dtrow in dt.Rows)
                        {
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


                            string phase = Convert.ToString(dtrow["Phase"]);
                            if (phase == "Inventory")
                            {
                                int Itemcode = 0;
                                if (Convert.ToString(dtrow["ItemID"]) != "")
                                {
                                    Itemcode = Convert.ToInt32(dtrow["ItemID"]);
                                }
                                string sWarehouseID = "";
                                if (Convert.ToString(dtrow["Warehouse"]) != "")
                                {
                                    sWarehouseID = Convert.ToString(dtrow["Warehouse"]);
                                }
                                if (Itemcode == 0)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter item.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                if (sWarehouseID.Trim() == "")
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter warehouse.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                string errMessage = ValidateOnHand(Itemcode, sWarehouseID, 0, tquant);
                                if (!string.IsNullOrEmpty(errMessage))
                                {
                                    if (string.IsNullOrEmpty(txtReceptionId.Text))
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: '" + errMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }

                            }



                            double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                            int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                            int acctid = Convert.ToInt32(dtrow["AcctID"]);

                            if (gstamt > 0)
                            {
                                if (gsttaxgl == 0)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                            }
                            ///////// Check Account Active/Inactive in bill ///////
                            DataSet _dsChart = new DataSet();

                            _GetChart.ConnConfig = Session["config"].ToString();
                            _GetChart.ID = Convert.ToInt32(acctid);

                            List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

                            string APINAME = "BillAPI/AddBills_GetChart";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChart);

                            _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                            _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);

                            int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                            if (acctstatus == 1)
                            {
                                if (phase == "Inventory")
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                else
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                            }
                            ///////////////////////////////////////////////////////
                            if (Convert.ToString(dtrow["ItemID"]) != "")
                            {
                                int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                string sName = Convert.ToString(dtrow["fDesc"]);
                                ///////// ES-3793 Check Active/Inactive Item ///////
                                if (Inv > 0 && phase == "Inventory")
                                {
                                    DataSet _dsInv = new DataSet();

                                    _GetInventoryItemStatus.ConnConfig = Session["config"].ToString();
                                    _GetInventoryItemStatus.ID = Inv;
                                    _GetInventoryItemStatus.UserID = Convert.ToInt32(Session["UserID"].ToString());

                                    DataSet _dsInv1 = new DataSet();
                                    DataSet _dsInv2 = new DataSet();
                                    ListGetInventoryItemStatus _lstInventory = new ListGetInventoryItemStatus();


                                    string APINAME1 = "BillAPI/AddBills_GetInventoryItemStatus";

                                    APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetInventoryItemStatus);

                                    _lstInventory = (new JavaScriptSerializer()).Deserialize<ListGetInventoryItemStatus>(_APIResponse1.ResponseData);
                                    _dsInv1 = _lstInventory.lstTable1.ToDataSet();
                                    _dsInv2 = _lstInventory.lstTable2.ToDataSet();

                                    DataTable dt1 = new DataTable();
                                    DataTable dt2 = new DataTable();

                                    dt1 = _dsInv1.Tables[0];
                                    dt2 = _dsInv2.Tables[0];

                                    dt1.TableName = "Table1";
                                    dt2.TableName = "Table2";

                                    _dsInv.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });


                                    int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());

                                    if (Invstatuss == 1)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;

                                    }

                                    if (phase == "Inventory")
                                    {
                                        if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                        {
                                            DataTable _ddts = GetCurrentTransaction();
                                            BINDGRID(_ddts);
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                            return;
                                        }
                                    }
                                }
                            }
                            ///////// ES-3793 Check Active/Inactive Item ///////
                            ///////// Check Account Active/Inactive in bill ///////


                        }


                        _AddBills.ConnConfig = Session["config"].ToString();
                        _UpdateRecurrBills.ConnConfig = Session["config"].ToString();
                        _UpdateBills.ConnConfig = Session["config"].ToString();

                        try
                        {

                            _AddBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            _UpdateRecurrBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            _UpdateBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                        }
                        catch (Exception)
                        {
                            DataTable _ddts = GetCurrentTransaction();
                            BINDGRID(_ddts);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }

                        _AddBills.fDate = Convert.ToDateTime(txtDate.Text);
                        _AddBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                        _AddBills.Due = Convert.ToDateTime(txtDueDate.Text);
                        _AddBills.Ref = txtRef.Text;
                        _AddBills.fDesc = txtMemo.Text;
                        _AddBills.Terms = Convert.ToInt16(txtDueIn.Text);
                        _AddBills.Spec = Convert.ToInt16(ddlStatus.SelectedValue);

                        _UpdateRecurrBills.fDate = Convert.ToDateTime(txtDate.Text);
                        _UpdateRecurrBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                        _UpdateRecurrBills.Due = Convert.ToDateTime(txtDueDate.Text);
                        _UpdateRecurrBills.Ref = txtRef.Text;
                        _UpdateRecurrBills.fDesc = txtMemo.Text;
                        _UpdateRecurrBills.Terms = Convert.ToInt16(txtDueIn.Text);
                        _UpdateRecurrBills.Spec = Convert.ToInt16(ddlStatus.SelectedValue);

                        _UpdateBills.fDate = Convert.ToDateTime(txtDate.Text);
                        _UpdateBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                        _UpdateBills.Due = Convert.ToDateTime(txtDueDate.Text);
                        _UpdateBills.Ref = txtRef.Text;
                        _UpdateBills.fDesc = txtMemo.Text;
                        _UpdateBills.Terms = Convert.ToInt16(txtDueIn.Text);


                        if (!string.IsNullOrEmpty(txtPaid.Text))
                        {

                            _AddBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                            _UpdateRecurrBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                            _UpdateBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                        }
                        if (!string.IsNullOrEmpty(txtPO.Text))
                        {

                            _AddBills.PO = Convert.ToInt32(txtPO.Text);
                            _UpdateRecurrBills.PO = Convert.ToInt32(txtPO.Text);
                            _UpdateBills.PO = Convert.ToInt32(txtPO.Text);
                        }
                        if (!string.IsNullOrEmpty(txtReceptionId.Text))
                        {

                            _AddBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                            _UpdateRecurrBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                            _UpdateBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                        }

                        _AddBills.Disc = Convert.ToDouble(txtDisc.Text);
                        _UpdateRecurrBills.Disc = Convert.ToDouble(txtDisc.Text);
                        _UpdateBills.Disc = Convert.ToDouble(txtDisc.Text);

                        if (!string.IsNullOrEmpty(txtCustom1.Text))
                        {

                            _AddBills.Custom1 = txtCustom1.Text;
                            _UpdateRecurrBills.Custom1 = txtCustom1.Text;
                            _UpdateBills.Custom1 = txtCustom1.Text;
                        }
                        if (!string.IsNullOrEmpty(txtCustom2.Text))
                        {

                            _AddBills.Custom2 = txtCustom2.Text;
                            _UpdateRecurrBills.Custom2 = txtCustom2.Text;
                            _UpdateBills.Custom2 = txtCustom2.Text;
                        }

                        _AddBills.MOMUSer = Session["User"].ToString();
                        _UpdateRecurrBills.MOMUSer = Session["User"].ToString();
                        _UpdateBills.MOMUSer = Session["User"].ToString();

                        _GetMaxReceivePOId.ConnConfig = Session["config"].ToString();

                        if (string.IsNullOrEmpty(txtReceptionId.Text) & !string.IsNullOrEmpty(txtPO.Text))
                        {
                            int ID_NEW = 0;

                            string APINAME = "BillAPI/AddBills_GetMaxReceivePOId";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMaxReceivePOId);
                            ID_NEW = Convert.ToInt32(_APIResponse.ResponseData);

                            ID_NEW = AddRPOItem(ID_NEW);

                            _AddBills.ReceivePo = ID_NEW;
                            _UpdateRecurrBills.ReceivePo = ID_NEW;
                            _UpdateBills.ReceivePo = ID_NEW;
                            txtReceptionId.Text = Convert.ToString(ID_NEW);
                        }
                        //UpdatePoStatus();


                        _AddBills.Dt = dt;
                        _UpdateRecurrBills.Dt = dt;
                        _UpdateBills.Dt = dt;
                        ///////////// Start - ES-3274 Data need to save at bill level only ////////


                        _AddBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                        _UpdateRecurrBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                        _UpdateBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));

                        if (!string.IsNullOrEmpty(hdnQST.Value))
                        {

                            _AddBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                            _UpdateRecurrBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                            _UpdateBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                        }
                        else
                        {

                            _AddBills.STaxRate = 0;
                            _UpdateRecurrBills.STaxRate = 0;
                            _UpdateBills.STaxRate = 0;
                        }
                        if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                        {

                            _AddBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                            _UpdateRecurrBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                            _UpdateBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                        }
                        else
                        {

                            _AddBills.STaxGL = 0;
                            _UpdateRecurrBills.STaxGL = 0;
                            _UpdateBills.STaxGL = 0;
                        }


                        _AddBills.STaxName = ddlSTax.SelectedValue.ToString();
                        _AddBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _AddBills.UTaxName = husetaxName.Value.ToString();

                        _UpdateRecurrBills.STaxName = ddlSTax.SelectedValue.ToString();
                        _UpdateRecurrBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _UpdateRecurrBills.UTaxName = husetaxName.Value.ToString();

                        _UpdateBills.STaxName = ddlSTax.SelectedValue.ToString();
                        _UpdateBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _UpdateBills.UTaxName = husetaxName.Value.ToString();

                        if (!string.IsNullOrEmpty(husetaxRate.Value))
                        {

                            _AddBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                            _UpdateRecurrBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                            _UpdateBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                        }
                        else
                        {

                            _AddBills.UTaxRate = 0;
                            _UpdateRecurrBills.UTaxRate = 0;
                            _UpdateBills.UTaxRate = 0;
                        }
                        if (!string.IsNullOrEmpty(husetaxGL.Value))
                        {

                            _AddBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                            _UpdateRecurrBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                            _UpdateBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                        }
                        else
                        {

                            _AddBills.UTaxGL = 0;
                            _UpdateRecurrBills.UTaxGL = 0;
                            _UpdateBills.UTaxGL = 0;
                        }

                        if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                        {

                            _AddBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                            _UpdateRecurrBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                            _UpdateBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                        }
                        else
                        {

                            _AddBills.GSTGL = 0;
                            _UpdateRecurrBills.GSTGL = 0;
                            _UpdateBills.GSTGL = 0;
                        }
                        if (!string.IsNullOrEmpty(hdnGST.Value))
                        {

                            _AddBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                            _UpdateRecurrBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                            _UpdateBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                        }
                        else
                        {

                            _AddBills.GSTRate = 0;
                            _UpdateRecurrBills.GSTRate = 0;
                            _UpdateBills.GSTRate = 0;
                        }

                        _AddBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _UpdateRecurrBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _UpdateBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));

                        //////////////// End - ES-3274 Data need to save at bill level only ////////

                        if (Request.QueryString["t"] != null)
                        {
                            if (Request.QueryString["t"].ToString() == "c")
                            {
                                isCopy = true;

                            }
                        }
                        if (isCopy)
                        {

                            _AddBills.IsRecurring = chkIsRecurr.Checked;
                            _UpdateRecurrBills.IsRecurring = chkIsRecurr.Checked;

                            //if (chkIsRecurr.Checked)
                            //{
                            //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                            //}
                            //_objPJ.Status = 0;
                            //_objBLBills.AddBills(_objPJ);
                            ////Response.Redirect(Request.RawUrl, false);
                            //ResetFormControlValues(this);
                            //SetBillForm();
                            if (chkIsRecurr.Checked)
                            {
                                _AddBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //_AddBills.Status = 0;
                                _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string APINAME = "BillAPI/AddBills_AddBills";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
                            else
                            {
                                //_objPJ.Status = 0;
                                //_AddBills.Status = 0;

                                _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string APINAME = "BillAPI/AddBills_AddBills";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
                        }

                        else if (Request.QueryString["id"] != null)
                        {
                            if (Request.QueryString["r"] != null)
                            {
                                if (Request.QueryString["r"].ToString() == "1")
                                {

                                    _UpdateRecurrBills.IsRecurring = chkIsRecurr.Checked;

                                    if (chkIsRecurr.Checked)
                                    {

                                        _UpdateRecurrBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    }

                                    //_UpdateRecurrBills.Status = 0;
                                    _UpdateRecurrBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                    _UpdateRecurrBills.ID = Convert.ToInt32(Request.QueryString["id"]);

                                    _UpdateBills.ID = Convert.ToInt32(Request.QueryString["id"]);

                                    string APINAME = "BillAPI/AddBills_UpdateRecurrBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateRecurrBills);

                                    RadGrid_gvLogs.Rebind();
                                    //Response.Redirect("managebills.aspx", false);


                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateRecurrBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateRecurrBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }

                                }
                            }
                            else
                            {

                                //_UpdateBills.Status = Convert.ToInt16(hdnStatus.Value);
                                _UpdateBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                _UpdateBills.Batch = Convert.ToInt32(hdnBatch.Value);
                                _UpdateBills.TRID = Convert.ToInt32(hdnTransID.Value);

                                string APINAME = "BillAPI/AddBills_UpdateBills";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateBills);

                                RadGrid_gvLogs.Rebind();

                                DataTable _ddts = GetCurrentTransaction();

                                BINDGRID(_ddts);

                                //Response.Redirect("managebills.aspx", false);

                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                }

                            }

                        }
                        else
                        {


                            _AddBills.IsRecurring = chkIsRecurr.Checked;

                            //if (chkIsRecurr.Checked)
                            //{
                            //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                            //}
                            //_objPJ.Status = 0;
                            //_objBLBills.AddBills(_objPJ);
                            ////Response.Redirect(Request.RawUrl, false);
                            //ResetFormControlValues(this);
                            //SetBillForm();
                            if (chkIsRecurr.Checked)
                            {

                                _AddBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //_AddBills.Status = 0;
                                _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string APINAME = "BillAPI/AddBills_AddBills";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }


                            }
                            else
                            {
                                //_objPJ.Status = 0;
                                //_AddBills.Status = 0;

                                _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string strpjid;

                                string APINAME = "BillAPI/AddBills_AddBills";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                strpjid = _APIResponse.ResponseData;
                                object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                                strpjid = JsonData.ToString();

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ValidateGrid(dt))
                    {
                        dt.Columns.Remove("AcctNo");
                        dt.Columns.Remove("JobName");
                        //dt.Columns.Remove("Phase");
                        dt.Columns.Remove("UName");
                        //     dt.Columns.Remove("UtaxGL");
                        //dt.Columns.Remove("Item");
                        //dt.Columns.Remove("TypeID");
                        dt.Columns.Remove("Loc");
                        dt.Columns.Remove("Line");
                        dt.Columns.Remove("PrvInQuan");
                        dt.Columns.Remove("PrvIn");
                        dt.Columns.Remove("OutstandQuan");
                        dt.Columns.Remove("OutstandBalance");
                        dt.Columns.Remove("AmountTot");

                        dt.Columns.Remove("Warehousefdesc");
                        dt.Columns.Remove("Locationfdesc");
                        dt.Columns.Remove("OrderedQuan");
                        dt.Columns.Remove("Ordered");
                        dt.Columns.Remove("RPOItemId");
                        dt.Columns.Remove("POItemId");
                        //dt.Columns.Remove("IsPO");

                        dt.Select("JobID = 0")
                              .AsEnumerable().ToList()
                              .ForEach(t => t["JobID"] = DBNull.Value);
                        //dt.Select("PhaseID = 0")
                        //     .AsEnumerable().ToList()
                        //     .ForEach(t => t["PhaseID"] = DBNull.Value);
                        dt.Select("ItemID = 0")
                            .AsEnumerable().ToList()
                            .ForEach(t => t["ItemID"] = DBNull.Value);

                        //dt.Select("ItemDesc = '' or ItemDesc is null")
                        //    .AsEnumerable().ToList()
                        //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);

                        dt.AcceptChanges();

                        foreach (DataRow dtrow in dt.Rows)
                        {
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


                            string phase = Convert.ToString(dtrow["Phase"]);
                            if (phase == "Inventory")
                            {
                                int Itemcode = 0;
                                if (Convert.ToString(dtrow["ItemID"]) != "")
                                {
                                    Itemcode = Convert.ToInt32(dtrow["ItemID"]);
                                }
                                string sWarehouseID = "";
                                if (Convert.ToString(dtrow["Warehouse"]) != "")
                                {
                                    sWarehouseID = Convert.ToString(dtrow["Warehouse"]);
                                }
                                if (dtrow["JobID"] != DBNull.Value)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Inventory not allowed with Project.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                

                                if (Itemcode == 0)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter item.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                if (sWarehouseID.Trim() == "")
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter warehouse.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                string errMessage = ValidateOnHand(Itemcode, sWarehouseID, 0, tquant);
                                if (!string.IsNullOrEmpty(errMessage))
                                {
                                    if (string.IsNullOrEmpty(txtReceptionId.Text))
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: '" + errMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }

                            }



                            double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                            int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                            int acctid = Convert.ToInt32(dtrow["AcctID"]);

                            int _jobidAK = 0;

                            if (!(dtrow["JobID"].Equals(System.DBNull.Value)))
                            {
                                _jobidAK = Convert.ToInt32(dtrow["JobID"]);

                            }

                            if (gstamt > 0)
                            {
                                if (gsttaxgl == 0)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                            }
                            ///////// Check Job Closed in bill ///////

                            if (_jobidAK != 0)
                            {
                                DataSet _dsJobs = new DataSet();
                                objJob.ConnConfig = Session["config"].ToString();
                                objJob.ID = Convert.ToInt32(_jobidAK);
                                _dsJobs = objBL_Job.spGetJobStatus(objJob);

                                int jobstatus = Convert.ToInt32(_dsJobs.Tables[0].Rows[0]["STATUS"].ToString());
                                if (jobstatus == 1)
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Project# " + _jobidAK.ToString() + "  is closed. Please change the project status before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                            }
                            ///////// Check Account Active/Inactive in bill ///////
                            DataSet _dsChart = new DataSet();
                            _objChart.ConnConfig = Session["config"].ToString();
                            _objChart.ID = Convert.ToInt32(acctid);

                            _dsChart = _objBLChart.GetChart(_objChart);


                            int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                            if (acctstatus == 1)
                            {
                                if (phase == "Inventory")
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                                else
                                {
                                    DataTable _ddts = GetCurrentTransaction();
                                    BINDGRID(_ddts);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                    return;
                                }
                            }
                            ///////////////////////////////////////////////////////
                            if (Convert.ToString(dtrow["ItemID"]) != "")
                            {
                                int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                string sName = Convert.ToString(dtrow["fDesc"]);
                                ///////// ES-3793 Check Active/Inactive Item ///////
                                if (Inv > 0 && phase == "Inventory")
                                {
                                    DataSet _dsInv = new DataSet();
                                    _objInv.ConnConfig = Session["config"].ToString();
                                    _objInv.ID = Inv;
                                    _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());

                                    _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);

                                    int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());

                                    if (Invstatuss == 1)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;

                                    }

                                    if (phase == "Inventory")
                                    {
                                        if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                        {
                                            DataTable _ddts = GetCurrentTransaction();
                                            BINDGRID(_ddts);
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                            return;
                                        }
                                    }
                                }
                            }
                            ///////// ES-3793 Check Active/Inactive Item ///////
                            ///////// Check Account Active/Inactive in bill ///////


                        }



                        _objPJ.ConnConfig = Session["config"].ToString();
                        try
                        {
                            _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                        }
                        catch (Exception)
                        {
                            DataTable _ddts = GetCurrentTransaction();
                            BINDGRID(_ddts);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }
                        //_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                        _objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                        _objPJ.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                        //_objPJ.IDate = Convert.ToDateTime(txtDueDate.Text);
                        _objPJ.Due = Convert.ToDateTime(txtDueDate.Text);
                        _objPJ.Ref = txtRef.Text;
                        _objPJ.fDesc = txtMemo.Text;
                        _objPJ.Terms = Convert.ToInt16(txtDueIn.Text);
                        _objPJ.Spec = Convert.ToInt16(ddlStatus.SelectedValue);

                        if (!string.IsNullOrEmpty(txtPaid.Text))
                        {
                            _objPJ.IfPaid = Convert.ToInt32(txtPaid.Text);
                        }
                        if (!string.IsNullOrEmpty(txtPO.Text))
                        {
                            _objPJ.PO = Convert.ToInt32(txtPO.Text);
                        }
                        if (!string.IsNullOrEmpty(txtReceptionId.Text))
                        {
                            _objPJ.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                        }
                        _objPJ.Disc = Convert.ToDouble(txtDisc.Text);

                        if (!string.IsNullOrEmpty(txtCustom1.Text))
                        {
                            _objPJ.Custom1 = txtCustom1.Text;
                        }
                        if (!string.IsNullOrEmpty(txtCustom2.Text))
                        {
                            _objPJ.Custom2 = txtCustom2.Text;
                        }
                        _objPJ.MOMUSer = Session["User"].ToString();

                        if (string.IsNullOrEmpty(txtReceptionId.Text) & !string.IsNullOrEmpty(txtPO.Text))
                        {
                            int ID_NEW = 0;
                            //ID_NEW = _objBLBills.GetMaxReceivePOId(objPO);

                            ID_NEW = AddRPOItem(ID_NEW);
                            _objPJ.ReceivePo = ID_NEW;
                            txtReceptionId.Text = Convert.ToString(ID_NEW);
                        }
                        //UpdatePoStatus();

                        _objPJ.Dt = dt;
                        ///////////// Start - ES-3274 Data need to save at bill level only ////////

                        _objPJ.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));

                        if (!string.IsNullOrEmpty(hdnQST.Value))
                        {
                            _objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                        }
                        else
                        {
                            _objPJ.STaxRate = 0;
                        }
                        if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                        {
                            _objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                        }
                        else
                        {
                            _objPJ.STaxGL = 0;
                        }
                        //_objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                        _objPJ.STaxName = ddlSTax.SelectedValue.ToString();
                        //_objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                        _objPJ.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        //_objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                        _objPJ.UTaxName = husetaxName.Value.ToString();
                        //_objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);

                        if (!string.IsNullOrEmpty(husetaxRate.Value))
                        {
                            _objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                        }
                        else
                        {
                            _objPJ.UTaxRate = 0;
                        }
                        if (!string.IsNullOrEmpty(husetaxGL.Value))
                        {
                            _objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                        }
                        else
                        {
                            _objPJ.UTaxGL = 0;
                        }

                        if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                        {
                            _objPJ.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                        }
                        else
                        {
                            _objPJ.GSTGL = 0;
                        }
                        if (!string.IsNullOrEmpty(hdnGST.Value))
                        {
                            _objPJ.GSTRate = Convert.ToDouble(hdnGST.Value);
                        }
                        else
                        {
                            _objPJ.GSTRate = 0;
                        }
                        _objPJ.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                        _objPJ.IsPOClose = chkPOClose.Checked;

                        //////////////// End - ES-3274 Data need to save at bill level only ////////

                        if (Request.QueryString["t"] != null)
                        {
                            if (Request.QueryString["t"].ToString() == "c")
                            {
                                isCopy = true;

                            }
                        }
                        if (isCopy)
                        {
                            _objPJ.IsRecurring = chkIsRecurr.Checked;

                            //if (chkIsRecurr.Checked)
                            //{
                            //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                            //}
                            //_objPJ.Status = 0;
                            //_objBLBills.AddBills(_objPJ);
                            ////Response.Redirect(Request.RawUrl, false);
                            //ResetFormControlValues(this);
                            //SetBillForm();
                            if (chkIsRecurr.Checked)
                            {
                                _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //_objPJ.Status = 0;
                                _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string strpjid;

                                strpjid = _objBLBills.AddBills(_objPJ);

                                //Update  Attachment Doc INFO                 
                                UpdateTempDateWhenCreatingNewAPBill(strpjid);
                                UpdateDocInfo();

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
                            else
                            {
                                //_objPJ.Status = 0;
                                //_AddBills.Status = 0;
                                _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string strpjid;

                                strpjid = _objBLBills.AddBills(_objPJ);

                                //Update  Attachment Doc INFO                 
                                UpdateTempDateWhenCreatingNewAPBill(strpjid);
                                UpdateDocInfo();

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
                        }

                        else if (Request.QueryString["id"] != null)
                        {
                            if (Request.QueryString["r"] != null)
                            {
                                if (Request.QueryString["r"].ToString() == "1")
                                {
                                    _objPJ.IsRecurring = chkIsRecurr.Checked;

                                    if (chkIsRecurr.Checked)
                                    {
                                        _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);

                                    }
                                    //_objPJ.Status = 0;
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                    _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                                    _objBLBills.UpdateRecurrBills(_objPJ);

                                    UpdateDocInfo();

                                    RadGrid_gvLogs.Rebind();
                                    //Response.Redirect("managebills.aspx", false);


                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }
                                }
                            }
                            else
                            {
                                //_objPJ.Status = Convert.ToInt16(hdnStatus.Value);
                                _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
                                _objPJ.TRID = Convert.ToInt32(hdnTransID.Value);


                                _objBLBills.UpdateBills(_objPJ);

                                UpdateDocInfo();

                                RadGrid_gvLogs.Rebind();

                                DataTable _ddts = GetCurrentTransaction();

                                BINDGRID(_ddts);

                                //Response.Redirect("managebills.aspx", false);


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                }

                            }

                        }
                        else
                        {

                            _objPJ.IsRecurring = chkIsRecurr.Checked;

                            //if (chkIsRecurr.Checked)
                            //{
                            //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                            //}
                            //_objPJ.Status = 0;
                            //_objBLBills.AddBills(_objPJ);
                            ////Response.Redirect(Request.RawUrl, false);
                            //ResetFormControlValues(this);
                            //SetBillForm();
                            if (chkIsRecurr.Checked)
                            {
                                _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //_objPJ.Status = 0;
                                _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                string strpjid;

                                strpjid = _objBLBills.AddBills(_objPJ);

                                //Update  Attachment Doc INFO                 
                                UpdateTempDateWhenCreatingNewAPBill(strpjid);
                                UpdateDocInfo();

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)


                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
                            else
                            {
                                //_objPJ.Status = 0;
                                //_AddBills.Status = 0;
                                _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                string strpjid;

                                strpjid = _objBLBills.AddBills(_objPJ);

                                //Update  Attachment Doc INFO                 
                                UpdateTempDateWhenCreatingNewAPBill(strpjid);
                                UpdateDocInfo();

                                //Response.Redirect(Request.RawUrl, false);
                                ResetFormControlValues(this);
                                SetBillForm();
                                SetTax();
                                GetInvDefaultAcct();
                                txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)

                                if (checktqty == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }

                            }
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
    protected void btnQuickcheck_Click(object sender, EventArgs e)
    {
        try
        {
            bool FlagPeriodClose = false;

            GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
            FlagPeriodClose = (bool)ViewState["FlagPeriodClose"];
            if (FlagPeriodClose == true)
            {
                if (IsValid())
                {
                    DataTable dt = GetTransaction();
                    bool checktqty = false;


                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        if (ValidateGrid(dt))
                        {
                            dt.Columns.Remove("AcctNo");
                            dt.Columns.Remove("JobName");
                            //dt.Columns.Remove("Phase");
                            dt.Columns.Remove("UName");
                            //     dt.Columns.Remove("UtaxGL");
                            //dt.Columns.Remove("Item");
                            //dt.Columns.Remove("TypeID");
                            dt.Columns.Remove("Loc");
                            dt.Columns.Remove("Line");
                            dt.Columns.Remove("PrvInQuan");
                            dt.Columns.Remove("PrvIn");
                            dt.Columns.Remove("OutstandQuan");
                            dt.Columns.Remove("OutstandBalance");
                            dt.Columns.Remove("AmountTot");

                            dt.Columns.Remove("Warehousefdesc");
                            dt.Columns.Remove("Locationfdesc");
                            dt.Columns.Remove("RPOItemId");
                            dt.Columns.Remove("POItemId");
                            //dt.Columns.Remove("IsPO");

                            dt.Select("JobID = 0")
                                  .AsEnumerable().ToList()
                                  .ForEach(t => t["JobID"] = DBNull.Value);
                            //dt.Select("PhaseID = 0")
                            //     .AsEnumerable().ToList()
                            //     .ForEach(t => t["PhaseID"] = DBNull.Value);
                            dt.Select("ItemID = 0")
                                .AsEnumerable().ToList()
                                .ForEach(t => t["ItemID"] = DBNull.Value);

                            //dt.Select("ItemDesc = '' or ItemDesc is null")
                            //    .AsEnumerable().ToList()
                            //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);

                            dt.AcceptChanges();

                            foreach (DataRow dtrow in dt.Rows)
                            {
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

                                string phase = Convert.ToString(dtrow["Phase"]);
                                if (phase == "Inventory")
                                {
                                    int Itemcode = 0;
                                    if (Convert.ToString(dtrow["ItemID"]) != "")
                                    {
                                        Itemcode = Convert.ToInt32(dtrow["ItemID"]);
                                    }
                                    string sWarehouseID = "";
                                    if (Convert.ToString(dtrow["Warehouse"]) != "")
                                    {
                                        sWarehouseID = Convert.ToString(dtrow["Warehouse"]);
                                    }
                                    if (Itemcode == 0)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter item.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                    if (sWarehouseID.Trim() == "")
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter warehouse.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }



                                double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                                int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                                int acctid = Convert.ToInt32(dtrow["AcctID"]);

                                if (gstamt > 0)
                                {
                                    if (gsttaxgl == 0)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }
                                ///////// Check Account Active/Inactive in bill ///////
                                DataSet _dsChart = new DataSet();

                                _GetChart.ConnConfig = Session["config"].ToString();
                                _GetChart.ID = Convert.ToInt32(acctid);

                                List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();


                                string APINAME = "BillAPI/AddBills_GetChart";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChart);

                                _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                                _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);


                                int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                                if (acctstatus == 1)
                                {
                                    if (phase == "Inventory")
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                    else
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }
                                ///////////////////////////////////////////////////////
                                if (Convert.ToString(dtrow["ItemID"]) != "")
                                {
                                    int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                    string sName = Convert.ToString(dtrow["fDesc"]);
                                    ///////// ES-3793 Check Active/Inactive Item ///////
                                    if (Inv > 0 && phase == "Inventory")
                                    {
                                        DataSet _dsInv = new DataSet();

                                        _GetInventoryItemStatus.ConnConfig = Session["config"].ToString();
                                        _GetInventoryItemStatus.ID = Inv;
                                        _GetInventoryItemStatus.UserID = Convert.ToInt32(Session["UserID"].ToString());

                                        DataSet _dsInv1 = new DataSet();
                                        DataSet _dsInv2 = new DataSet();
                                        ListGetInventoryItemStatus _lstInventory = new ListGetInventoryItemStatus();


                                        string APINAME1 = "BillAPI/AddBills_GetInventoryItemStatus";

                                        APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetInventoryItemStatus);

                                        _lstInventory = (new JavaScriptSerializer()).Deserialize<ListGetInventoryItemStatus>(_APIResponse1.ResponseData);
                                        _dsInv1 = _lstInventory.lstTable1.ToDataSet();
                                        _dsInv2 = _lstInventory.lstTable2.ToDataSet();

                                        DataTable dt1 = new DataTable();
                                        DataTable dt2 = new DataTable();

                                        dt1 = _dsInv1.Tables[0];
                                        dt2 = _dsInv2.Tables[0];

                                        dt1.TableName = "Table1";
                                        dt2.TableName = "Table2";

                                        _dsInv.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

                                        int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());

                                        if (Invstatuss == 1)
                                        {
                                            DataTable _ddts = GetCurrentTransaction();
                                            BINDGRID(_ddts);
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                            return;

                                        }
                                        if (phase == "Inventory")
                                        {
                                            if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                            {
                                                DataTable _ddts = GetCurrentTransaction();
                                                BINDGRID(_ddts);
                                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                                ///////// ES-3793 Check Active/Inactive Item ///////
                                ///////// Check Account Active/Inactive in bill ///////


                                //double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                                //int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                                //int acctid = Convert.ToInt32(dtrow["AcctID"]);
                                //string phase = Convert.ToString(dtrow["Phase"]);
                                //if (gstamt > 0)
                                //{
                                //    if (gsttaxgl == 0)
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //}
                                /////////// Check Account Active/Inactive in bill ///////
                                //DataSet _dsChart = new DataSet();
                                //_objChart.ConnConfig = Session["config"].ToString();
                                //_objChart.ID = Convert.ToInt32(acctid);
                                //_dsChart = _objBLChart.GetChart(_objChart);
                                //int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                                //if (acctstatus == 1)
                                //{
                                //    if (phase == "Inventory")
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //    else
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //}
                                /////////////////////////////////////////////////////////
                                //if (Convert.ToString(dtrow["ItemID"]) != "")
                                //{
                                //    int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                //    string sName = Convert.ToString(dtrow["fDesc"]);
                                //    ///////// ES-3793 Check Active/Inactive Item ///////
                                //    if (Inv > 0 && phase == "Inventory")
                                //    {
                                //        DataSet _dsInv = new DataSet();
                                //        _objInv.ConnConfig = Session["config"].ToString();
                                //        _objInv.ID = Inv;
                                //        _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());
                                //        _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);
                                //        int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());
                                //        if (Invstatuss == 1)
                                //        {
                                //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //            return;

                                //        }
                                //        if (phase == "Inventory")
                                //        {
                                //            if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                //            {
                                //                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //                return;
                                //            }
                                //        }
                                //    }
                                //}
                                /////////// ES-3793 Check Active/Inactive Item ///////
                                /////////// Check Account Active/Inactive in bill ///////


                            }





                            _AddBills.ConnConfig = Session["config"].ToString();
                            _UpdateRecurrBills.ConnConfig = Session["config"].ToString();
                            _UpdateBills.ConnConfig = Session["config"].ToString();

                            try
                            {

                                _AddBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                _UpdateRecurrBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                _UpdateBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            }
                            catch (Exception)
                            {
                                DataTable _ddts = GetCurrentTransaction();
                                BINDGRID(_ddts);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                return;
                            }


                            _AddBills.fDate = Convert.ToDateTime(txtDate.Text);
                            _AddBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                            _AddBills.Due = Convert.ToDateTime(txtDueDate.Text);
                            _AddBills.Ref = txtRef.Text;
                            _AddBills.fDesc = txtMemo.Text;
                            _AddBills.Terms = Convert.ToInt16(txtDueIn.Text);
                            _AddBills.Spec = Convert.ToInt16(ddlStatus.SelectedValue);

                            _UpdateRecurrBills.fDate = Convert.ToDateTime(txtDate.Text);
                            _UpdateRecurrBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                            _UpdateRecurrBills.Due = Convert.ToDateTime(txtDueDate.Text);
                            _UpdateRecurrBills.Ref = txtRef.Text;
                            _UpdateRecurrBills.fDesc = txtMemo.Text;
                            _UpdateRecurrBills.Terms = Convert.ToInt16(txtDueIn.Text);
                            _UpdateRecurrBills.Spec = Convert.ToInt16(ddlStatus.SelectedValue);

                            _UpdateBills.fDate = Convert.ToDateTime(txtDate.Text);
                            _UpdateBills.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                            _UpdateBills.Due = Convert.ToDateTime(txtDueDate.Text);
                            _UpdateBills.Ref = txtRef.Text;
                            _UpdateBills.fDesc = txtMemo.Text;
                            _UpdateBills.Terms = Convert.ToInt16(txtDueIn.Text);


                            if (!string.IsNullOrEmpty(txtPaid.Text))
                            {

                                _AddBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                                _UpdateRecurrBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                                _UpdateBills.IfPaid = Convert.ToInt32(txtPaid.Text);
                            }
                            if (!string.IsNullOrEmpty(txtPO.Text))
                            {

                                _AddBills.PO = Convert.ToInt32(txtPO.Text);
                                _UpdateRecurrBills.PO = Convert.ToInt32(txtPO.Text);
                                _UpdateBills.PO = Convert.ToInt32(txtPO.Text);
                            }
                            if (!string.IsNullOrEmpty(txtReceptionId.Text))
                            {

                                _AddBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                                _UpdateRecurrBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                                _UpdateBills.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                            }

                            _AddBills.Disc = Convert.ToDouble(txtDisc.Text);
                            _UpdateRecurrBills.Disc = Convert.ToDouble(txtDisc.Text);
                            _UpdateBills.Disc = Convert.ToDouble(txtDisc.Text);

                            if (!string.IsNullOrEmpty(txtCustom1.Text))
                            {

                                _AddBills.Custom1 = txtCustom1.Text;
                                _UpdateRecurrBills.Custom1 = txtCustom1.Text;
                                _UpdateBills.Custom1 = txtCustom1.Text;
                            }
                            if (!string.IsNullOrEmpty(txtCustom2.Text))
                            {

                                _AddBills.Custom2 = txtCustom2.Text;
                                _UpdateRecurrBills.Custom2 = txtCustom2.Text;
                                _UpdateBills.Custom2 = txtCustom2.Text;
                            }


                            _AddBills.MOMUSer = Session["User"].ToString();
                            _UpdateRecurrBills.MOMUSer = Session["User"].ToString();
                            _UpdateBills.MOMUSer = Session["User"].ToString();

                            _GetMaxReceivePOId.ConnConfig = Session["config"].ToString();

                            if (string.IsNullOrEmpty(txtReceptionId.Text) & !string.IsNullOrEmpty(txtPO.Text))
                            {
                                int ID_NEW = 0;


                                string APINAME = "BillAPI/AddBills_GetMaxReceivePOId";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetMaxReceivePOId);
                                ID_NEW = Convert.ToInt32(_APIResponse.ResponseData);


                                ID_NEW = AddRPOItem(ID_NEW);

                                _AddBills.ReceivePo = ID_NEW;
                                _UpdateRecurrBills.ReceivePo = ID_NEW;
                                _UpdateBills.ReceivePo = ID_NEW;
                                txtReceptionId.Text = Convert.ToString(ID_NEW);
                            }
                            //UpdatePoStatus();

                            ///////////// Start - ES-3274 Data need to save at bill level only ////////


                            _AddBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                            _UpdateRecurrBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                            _UpdateBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));

                            if (!string.IsNullOrEmpty(hdnQST.Value))
                            {

                                _AddBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                                _UpdateRecurrBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                                _UpdateBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                            }
                            else
                            {

                                _AddBills.STaxRate = 0;
                                _UpdateRecurrBills.STaxRate = 0;
                                _UpdateBills.STaxRate = 0;
                            }
                            if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                            {

                                _AddBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                _UpdateRecurrBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                _UpdateBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                            }
                            else
                            {

                                _AddBills.STaxGL = 0;
                                _UpdateRecurrBills.STaxGL = 0;
                                _UpdateBills.STaxGL = 0;
                            }


                            _AddBills.STaxName = ddlSTax.SelectedValue.ToString();
                            _AddBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _AddBills.UTaxName = husetaxName.Value.ToString();

                            _UpdateRecurrBills.STaxName = ddlSTax.SelectedValue.ToString();
                            _UpdateRecurrBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _UpdateRecurrBills.UTaxName = husetaxName.Value.ToString();

                            _UpdateBills.STaxName = ddlSTax.SelectedValue.ToString();
                            _UpdateBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _UpdateBills.UTaxName = husetaxName.Value.ToString();

                            if (!string.IsNullOrEmpty(husetaxRate.Value))
                            {

                                _AddBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                                _UpdateRecurrBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                                _UpdateBills.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                            }
                            else
                            {

                                _AddBills.UTaxRate = 0;
                                _UpdateRecurrBills.UTaxRate = 0;
                                _UpdateBills.UTaxRate = 0;
                            }
                            if (!string.IsNullOrEmpty(husetaxGL.Value))
                            {

                                _AddBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                                _UpdateRecurrBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                                _UpdateBills.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                            }
                            else
                            {

                                _AddBills.UTaxGL = 0;
                                _UpdateRecurrBills.UTaxGL = 0;
                                _UpdateBills.UTaxGL = 0;
                            }

                            if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                            {

                                _AddBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                _UpdateRecurrBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                _UpdateBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                            }
                            else
                            {

                                _AddBills.GSTGL = 0;
                                _UpdateRecurrBills.GSTGL = 0;
                                _UpdateBills.GSTGL = 0;
                            }
                            if (!string.IsNullOrEmpty(hdnGST.Value))
                            {

                                _AddBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                                _UpdateRecurrBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                                _UpdateBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                            }
                            else
                            {

                                _AddBills.GSTRate = 0;
                                _UpdateRecurrBills.GSTRate = 0;
                                _UpdateBills.GSTRate = 0;
                            }

                            _AddBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _UpdateRecurrBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _UpdateBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _AddBills.IsPOClose = chkPOClose.Checked;
                            _UpdateBills.IsPOClose = chkPOClose.Checked;

                            //////////////// End - ES-3274 Data need to save at bill level only ////////



                            _UpdateRecurrBills.Dt = dt;
                            _AddBills.Dt = dt;
                            _UpdateBills.Dt = dt;

                            if (Request.QueryString["t"] != null)
                            {
                                if (Request.QueryString["t"].ToString() == "c")
                                {
                                    isCopy = true;

                                }
                            }
                            if (isCopy)
                            {

                                _AddBills.IsRecurring = chkIsRecurr.Checked;

                                //if (chkIsRecurr.Checked)
                                //{
                                //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //}
                                //_objPJ.Status = 0;
                                //_objBLBills.AddBills(_objPJ);
                                ////Response.Redirect(Request.RawUrl, false);
                                //ResetFormControlValues(this);
                                //SetBillForm();
                                if (chkIsRecurr.Checked)
                                {

                                    _AddBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    //_AddBills.Status = 0;
                                    _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                    string APINAME = "BillAPI/AddBills_AddBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();
                                    GetInvDefaultAcct();

                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }

                                }
                                else
                                {
                                    // _objPJ.Status = 0;
                                    //_AddBills.Status = 0;

                                    _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                    string APINAME = "BillAPI/AddBills_AddBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);


                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();


                                    if (checktqty == true)
                                    {

                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _AddBills.Vendor + "&ref=" + _AddBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive1", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _AddBills.Vendor + "&ref=" + _AddBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive2", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }
                            }

                            else if (Request.QueryString["id"] != null)
                            {
                                if (Request.QueryString["r"] != null)
                                {
                                    if (Request.QueryString["r"].ToString() == "1")
                                    {


                                        _UpdateRecurrBills.IsRecurring = chkIsRecurr.Checked;

                                        if (chkIsRecurr.Checked)
                                        {

                                            _UpdateRecurrBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                        }

                                        //_UpdateRecurrBills.Status = 0;
                                        _UpdateRecurrBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                        _UpdateRecurrBills.ID = Convert.ToInt32(Request.QueryString["id"]);

                                        _UpdateBills.ID = Convert.ToInt32(Request.QueryString["id"]);

                                        string APINAME = "BillAPI/AddBills_UpdateRecurrBills";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateRecurrBills);


                                        RadGrid_gvLogs.Rebind();
                                        //Response.Redirect("managebills.aspx", false);


                                        if (checktqty == true)
                                        {

                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Recurring Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateRecurrBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateRecurrBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                        }
                                    }
                                }
                                else
                                {

                                    //_UpdateBills.Status = Convert.ToInt16(hdnStatus.Value);
                                    _UpdateBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                    _UpdateBills.Batch = Convert.ToInt32(hdnBatch.Value);
                                    _UpdateBills.TRID = Convert.ToInt32(hdnTransID.Value);

                                    string APINAME = "BillAPI/AddBills_UpdateBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateBills);


                                    RadGrid_gvLogs.Rebind();
                                    //Response.Redirect("managebills.aspx", false);


                                    if (checktqty == true)
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _UpdateBills.Vendor + "&ref=" + _UpdateBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive33", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _UpdateBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _UpdateBills.Vendor + "&ref=" + _UpdateBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive41", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }

                            }
                            else
                            {


                                _AddBills.IsRecurring = chkIsRecurr.Checked;
                                //if (chkIsRecurr.Checked)
                                //{
                                //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //}
                                //_objPJ.Status = 0;
                                //_objBLBills.AddBills(_objPJ);
                                ////Response.Redirect(Request.RawUrl, false);
                                //ResetFormControlValues(this);
                                //SetBillForm();
                                if (chkIsRecurr.Checked)
                                {

                                    _AddBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    //_AddBills.Status = 0;
                                    _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                    string APINAME = "BillAPI/AddBills_AddBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);


                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();
                                    GetInvDefaultAcct();

                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }

                                }
                                else
                                {
                                    //_objPJ.Status = 0;
                                    // _AddBills.Status = 0;

                                    _AddBills.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                    string APINAME = "BillAPI/AddBills_AddBills";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddBills);

                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();


                                    if (checktqty == true)
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _AddBills.Vendor + "&ref=" + _AddBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive35", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _AddBills.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _AddBills.Vendor + "&ref=" + _AddBills.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive43", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (ValidateGrid(dt))
                        {
                            dt.Columns.Remove("AcctNo");
                            dt.Columns.Remove("JobName");
                            //dt.Columns.Remove("Phase");
                            dt.Columns.Remove("UName");
                            //     dt.Columns.Remove("UtaxGL");
                            //dt.Columns.Remove("Item");
                            //dt.Columns.Remove("TypeID");
                            dt.Columns.Remove("Loc");
                            dt.Columns.Remove("Line");
                            dt.Columns.Remove("PrvInQuan");
                            dt.Columns.Remove("PrvIn");
                            dt.Columns.Remove("OutstandQuan");
                            dt.Columns.Remove("OutstandBalance");
                            dt.Columns.Remove("AmountTot");

                            dt.Columns.Remove("Warehousefdesc");
                            dt.Columns.Remove("Locationfdesc");
                            dt.Columns.Remove("OrderedQuan");
                            dt.Columns.Remove("Ordered");
                            dt.Columns.Remove("RPOItemId");
                            dt.Columns.Remove("POItemId");
                            //dt.Columns.Remove("IsPO");

                            dt.Select("JobID = 0")
                                  .AsEnumerable().ToList()
                                  .ForEach(t => t["JobID"] = DBNull.Value);
                            //dt.Select("PhaseID = 0")
                            //     .AsEnumerable().ToList()
                            //     .ForEach(t => t["PhaseID"] = DBNull.Value);
                            dt.Select("ItemID = 0")
                                .AsEnumerable().ToList()
                                .ForEach(t => t["ItemID"] = DBNull.Value);

                            //dt.Select("ItemDesc = '' or ItemDesc is null")
                            //    .AsEnumerable().ToList()
                            //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);

                            dt.AcceptChanges();

                            foreach (DataRow dtrow in dt.Rows)
                            {
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

                                string phase = Convert.ToString(dtrow["Phase"]);
                                if (phase == "Inventory")
                                {
                                    int Itemcode = 0;
                                    if (Convert.ToString(dtrow["ItemID"]) != "")
                                    {
                                        Itemcode = Convert.ToInt32(dtrow["ItemID"]);
                                    }
                                    string sWarehouseID = "";
                                    if (Convert.ToString(dtrow["Warehouse"]) != "")
                                    {
                                        sWarehouseID = Convert.ToString(dtrow["Warehouse"]);
                                    }
                                    if (dtrow["JobID"] != DBNull.Value)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Inventory not allowed with Project.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                    if (Itemcode == 0)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter item.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                    if (sWarehouseID.Trim() == "")
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Please enter warehouse.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }



                                double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                                int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                                int acctid = Convert.ToInt32(dtrow["AcctID"]);
                                //int _jobidAK = Convert.ToInt32(dtrow["JobID"]);
                                int _jobidAK = 0;

                                if (!(dtrow["JobID"].Equals(System.DBNull.Value)))
                                {
                                    _jobidAK = Convert.ToInt32(dtrow["JobID"]);
                                }

                                if (gstamt > 0)
                                {
                                    if (gsttaxgl == 0)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }
                                ///////// Check Job Closed in bill ///////

                                if (_jobidAK != 0)
                                {
                                    DataSet _dsJobs = new DataSet();
                                    objJob.ConnConfig = Session["config"].ToString();
                                    objJob.ID = Convert.ToInt32(_jobidAK);
                                    _dsJobs = objBL_Job.spGetJobStatus(objJob);

                                    int jobstatus = Convert.ToInt32(_dsJobs.Tables[0].Rows[0]["STATUS"].ToString());
                                    if (jobstatus == 1)
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Project# " + _jobidAK.ToString() + "  is closed. Please change the project status before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }
                                ///////// Check Account Active/Inactive in bill ///////
                                DataSet _dsChart = new DataSet();
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = Convert.ToInt32(acctid);

                                _dsChart = _objBLChart.GetChart(_objChart);


                                int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                                if (acctstatus == 1)
                                {
                                    if (phase == "Inventory")
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                    else
                                    {
                                        DataTable _ddts = GetCurrentTransaction();
                                        BINDGRID(_ddts);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                        return;
                                    }
                                }
                                ///////////////////////////////////////////////////////
                                if (Convert.ToString(dtrow["ItemID"]) != "")
                                {
                                    int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                    string sName = Convert.ToString(dtrow["fDesc"]);
                                    ///////// ES-3793 Check Active/Inactive Item ///////
                                    if (Inv > 0 && phase == "Inventory")
                                    {
                                        DataSet _dsInv = new DataSet();
                                        _objInv.ConnConfig = Session["config"].ToString();
                                        _objInv.ID = Inv;
                                        _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());

                                        _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);


                                        int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());

                                        if (Invstatuss == 1)
                                        {
                                            DataTable _ddts = GetCurrentTransaction();
                                            BINDGRID(_ddts);
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                            return;

                                        }

                                        if (phase == "Inventory")
                                        {
                                            if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                            {
                                                DataTable _ddts = GetCurrentTransaction();
                                                BINDGRID(_ddts);
                                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                                return;
                                            }
                                        }
                                    }
                                }
                                ///////// ES-3793 Check Active/Inactive Item ///////
                                ///////// Check Account Active/Inactive in bill ///////


                                //double gstamt = Convert.ToDouble(dtrow["GTaxAmt"]);
                                //int gsttaxgl = Convert.ToInt32(dtrow["GSTTaxGL"]);
                                //int acctid = Convert.ToInt32(dtrow["AcctID"]);
                                //string phase = Convert.ToString(dtrow["Phase"]);
                                //if (gstamt > 0)
                                //{
                                //    if (gsttaxgl == 0)
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'GST GL Acct is missing , Please add the acct. in control panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //}
                                /////////// Check Account Active/Inactive in bill ///////
                                //DataSet _dsChart = new DataSet();
                                //_objChart.ConnConfig = Session["config"].ToString();
                                //_objChart.ID = Convert.ToInt32(acctid);
                                //_dsChart = _objBLChart.GetChart(_objChart);
                                //int acctstatus = Convert.ToInt32(_dsChart.Tables[0].Rows[0]["Status"].ToString());
                                //if (acctstatus == 1)
                                //{
                                //    if (phase == "Inventory")
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'This GL account is Inactive, please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //    else
                                //    {
                                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendors", "noty({text: 'Account# " + _dsChart.Tables[0].Rows[0]["Acct"].ToString() + "  is inactive. Please change the account name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //        return;
                                //    }
                                //}
                                /////////////////////////////////////////////////////////
                                //if (Convert.ToString(dtrow["ItemID"]) != "")
                                //{
                                //    int Inv = Convert.ToInt32(dtrow["ItemID"]);
                                //    string sName = Convert.ToString(dtrow["fDesc"]);
                                //    ///////// ES-3793 Check Active/Inactive Item ///////
                                //    if (Inv > 0 && phase == "Inventory")
                                //    {
                                //        DataSet _dsInv = new DataSet();
                                //        _objInv.ConnConfig = Session["config"].ToString();
                                //        _objInv.ID = Inv;
                                //        _objInv.UserID = Convert.ToInt32(Session["UserID"].ToString());
                                //        _dsInv = _objBLBills.GetInventoryItemStatus(_objInv);
                                //        int Invstatuss = Convert.ToInt32(_dsInv.Tables[0].Rows[0]["Status"].ToString());
                                //        if (Invstatuss == 1)
                                //        {
                                //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Item " + sName + " is Inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //            return;

                                //        }
                                //        if (phase == "Inventory")
                                //        {
                                //            if (acctid != Convert.ToInt32(hdnInvDefaultAcctID.Value))
                                //            {
                                //                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendorsss", "noty({text: 'Please verify the Default Inventory Account under Control Panel.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                //                return;
                                //            }
                                //        }
                                //    }
                                //}
                                /////////// ES-3793 Check Active/Inactive Item ///////
                                /////////// Check Account Active/Inactive in bill ///////


                            }




                            _objPJ.ConnConfig = Session["config"].ToString();

                            try
                            {
                                _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            }
                            catch (Exception)
                            {
                                DataTable _ddts = GetCurrentTransaction();
                                BINDGRID(_ddts);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                                return;
                            }
                            //_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                            _objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                            _objPJ.PostDate = Convert.ToDateTime(txtPostingDate.Text);
                            //_objPJ.IDate = Convert.ToDateTime(txtDueDate.Text);
                            _objPJ.Due = Convert.ToDateTime(txtDueDate.Text);
                            _objPJ.Ref = txtRef.Text;
                            _objPJ.fDesc = txtMemo.Text;
                            _objPJ.Terms = Convert.ToInt16(txtDueIn.Text);
                            _objPJ.Spec = Convert.ToInt16(ddlStatus.SelectedValue);


                            if (!string.IsNullOrEmpty(txtPaid.Text))
                            {
                                _objPJ.IfPaid = Convert.ToInt32(txtPaid.Text);
                            }
                            if (!string.IsNullOrEmpty(txtPO.Text))
                            {
                                _objPJ.PO = Convert.ToInt32(txtPO.Text);
                            }
                            if (!string.IsNullOrEmpty(txtReceptionId.Text))
                            {
                                _objPJ.ReceivePo = Convert.ToInt32(txtReceptionId.Text);
                            }
                            _objPJ.Disc = Convert.ToDouble(txtDisc.Text);

                            if (!string.IsNullOrEmpty(txtCustom1.Text))
                            {
                                _objPJ.Custom1 = txtCustom1.Text;
                            }
                            if (!string.IsNullOrEmpty(txtCustom2.Text))
                            {
                                _objPJ.Custom2 = txtCustom2.Text;
                            }

                            _objPJ.MOMUSer = Session["User"].ToString();

                            if (string.IsNullOrEmpty(txtReceptionId.Text) & !string.IsNullOrEmpty(txtPO.Text))
                            {
                                int ID_NEW = 0;


                                //ID_NEW = _objBLBills.GetMaxReceivePOId(objPO);

                                ID_NEW = AddRPOItem(ID_NEW);
                                _objPJ.ReceivePo = ID_NEW;
                                txtReceptionId.Text = Convert.ToString(ID_NEW);
                            }
                            //UpdatePoStatus();

                            ///////////// Start - ES-3274 Data need to save at bill level only ////////

                            _objPJ.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));

                            if (!string.IsNullOrEmpty(hdnQST.Value))
                            {
                                _objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                            }
                            else
                            {
                                _objPJ.STaxRate = 0;
                            }
                            if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                            {
                                _objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                            }
                            else
                            {
                                _objPJ.STaxGL = 0;
                            }
                            //_objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                            _objPJ.STaxName = ddlSTax.SelectedValue.ToString();
                            //_objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                            _objPJ.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            //_objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                            _objPJ.UTaxName = husetaxName.Value.ToString();
                            //_objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);

                            if (!string.IsNullOrEmpty(husetaxRate.Value))
                            {
                                _objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                            }
                            else
                            {
                                _objPJ.UTaxRate = 0;
                            }
                            if (!string.IsNullOrEmpty(husetaxGL.Value))
                            {
                                _objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);
                            }
                            else
                            {
                                _objPJ.UTaxGL = 0;
                            }

                            if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                            {
                                _objPJ.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                            }
                            else
                            {
                                _objPJ.GSTGL = 0;
                            }
                            if (!string.IsNullOrEmpty(hdnGST.Value))
                            {
                                _objPJ.GSTRate = Convert.ToDouble(hdnGST.Value);
                            }
                            else
                            {
                                _objPJ.GSTRate = 0;
                            }
                            _objPJ.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                            _objPJ.IsPOClose = chkPOClose.Checked;

                            //////////////// End - ES-3274 Data need to save at bill level only ////////


                            _objPJ.Dt = dt;

                            if (Request.QueryString["t"] != null)
                            {
                                if (Request.QueryString["t"].ToString() == "c")
                                {
                                    isCopy = true;

                                }
                            }
                            if (isCopy)
                            {
                                _objPJ.IsRecurring = chkIsRecurr.Checked;

                                //if (chkIsRecurr.Checked)
                                //{
                                //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //}
                                //_objPJ.Status = 0;
                                //_objBLBills.AddBills(_objPJ);
                                ////Response.Redirect(Request.RawUrl, false);
                                //ResetFormControlValues(this);
                                //SetBillForm();
                                if (chkIsRecurr.Checked)
                                {
                                    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    // _objPJ.Status = 0;
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);


                                    _objBLBills.AddBills(_objPJ);

                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();
                                    GetInvDefaultAcct();

                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }

                                }
                                else
                                {
                                    // _objPJ.Status = 0;
                                    //_AddBills.Status = 0;
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                    _objBLBills.AddBills(_objPJ);

                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();


                                    if (checktqty == true)
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive3", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive4", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }
                            }

                            else if (Request.QueryString["id"] != null)
                            {
                                if (Request.QueryString["r"] != null)
                                {
                                    if (Request.QueryString["r"].ToString() == "1")
                                    {
                                        _objPJ.IsRecurring = chkIsRecurr.Checked;

                                        if (chkIsRecurr.Checked)
                                        {
                                            _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);

                                        }
                                        //_objPJ.Status = 0;
                                        _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                        _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                                        _objBLBills.UpdateRecurrBills(_objPJ);

                                        RadGrid_gvLogs.Rebind();
                                        //Response.Redirect("managebills.aspx", false);


                                        if (checktqty == true)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Recurring Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Recurring bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                        }
                                    }
                                }
                                else
                                {
                                    //_objPJ.Status = Convert.ToInt16(hdnStatus.Value);
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                                    _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
                                    _objPJ.TRID = Convert.ToInt32(hdnTransID.Value);

                                    _objBLBills.UpdateBills(_objPJ);

                                    RadGrid_gvLogs.Rebind();
                                    //Response.Redirect("managebills.aspx", false);


                                    if (checktqty == true)
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive34", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive42", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }

                            }
                            else
                            {

                                _objPJ.IsRecurring = chkIsRecurr.Checked;

                                //if (chkIsRecurr.Checked)
                                //{
                                //    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                //}
                                //_objPJ.Status = 0;
                                //_objBLBills.AddBills(_objPJ);
                                ////Response.Redirect(Request.RawUrl, false);
                                //ResetFormControlValues(this);
                                //SetBillForm();
                                if (chkIsRecurr.Checked)
                                {
                                    _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    //_objPJ.Status = 0;
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                    _objBLBills.AddBills(_objPJ);


                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();
                                    GetInvDefaultAcct();

                                    if (checktqty == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }
                                    else
                                    {
                                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Recurring bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                    }

                                }
                                else
                                {
                                    //_objPJ.Status = 0;
                                    // _AddBills.Status = 0;
                                    _objPJ.Status = Convert.ToInt16(ddlStatus.SelectedValue);

                                    _objBLBills.AddBills(_objPJ);

                                    //Response.Redirect(Request.RawUrl, false);
                                    ResetFormControlValues(this);
                                    SetBillForm();


                                    if (checktqty == true)
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive", "noty({text: 'Bill will be created with zero quantity.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive36", "noty({text: 'Bill will be created with zero quantity. <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt16(ddlStatus.SelectedValue) != 1 && Convert.ToInt16(ddlStatus.SelectedValue) != 2 && Convert.ToInt16(ddlStatus.SelectedValue) != 3)
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningInvInactive44", "noty({text: 'Bill created successfully! <BR/> <b> This Bill is on status " + ddlStatus.SelectedItem.Text + " and cannot be paid. </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 2000,theme : 'noty_theme_default',  closable : true});", true);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string str = "This month/year period is closed out. You do not have permission to add/update this record.";
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    /// <summary>
    /// Created by AZHAR for ES-3256 Harmonized Tax on AP: We need to add a new changes on bill screen if tax type is selected harmonized DT 31-12-2019 
    /// </summary>
    private void SetHarmonizedTax()
    {
        try
        {
            //if (hdnSTaxType.Value == "2")
            //{
            //    hdnGST.Value = "0";
            //    hdnGSTGL.Value = "0";
            //    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;

            //    if (txtgstgv.Visible == true)
            //    {
            //        if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
            //        {
            //            txtgstgv.Visible = false;
            //            RadGrid_gvJobCostItems.Columns[13].Display = false;
            //            RadGrid_gvJobCostItems.Columns[14].Display = false;
            //        }

            //    }
            //}
            //else if (hdnSTaxType.Value != "2")
            //{
            //    //hdnGST.Value = "0";
            //    //hdnGSTGL.Value = "0";
            //    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;
            //    //SetTax();
            //    _objPropGeneral.ConnConfig = Session["config"].ToString();
            //    _objPropGeneral.CustomName = "Country";


            //    _getCustomFields.ConnConfig = Session["config"].ToString();
            //    _getCustomFields.CustomName = "Country";

            //    DataSet dsCustom = new DataSet();
            //    List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        string APINAME = "BillAPI/AddBills_GetCustomFields";

            //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            //        _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            //        dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
            //    }
            //    else
            //    {
            //        dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);
            //    }

            //    if (dsCustom.Tables[0].Rows.Count > 0)
            //    {
            //        if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            //        {
            //            //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
            //            //RadGrid_gvJobCostItems.Columns[13].Visible = true;
            //            RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
            //            RadGrid_gvJobCostItems.Columns[14].Display = true;
            //            RadGrid_gvJobCostItems.Columns[13].Display = true;
            //            txtgstgv.Visible = true;
            //            spansalestax.InnerText = "PST Tax";

            //            ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
            //            string gst_gstgl = "";
            //            string gst_gstrate = "";
            //            _objPropGeneral.ConnConfig = Session["config"].ToString();
            //            _getCustomFieldsControl.ConnConfig = Session["config"].ToString();
            //            DataSet _dsCustom = new DataSet();
            //            List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();

            //            if (IsAPIIntegrationEnable == "YES")
            //            //if (Session["APAPIEnable"].ToString() == "YES")
            //            {
            //                string APINAME = "BillAPI/AddBills_GetCustomFieldsControl";

            //                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

            //                _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            //                _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);
            //            }
            //            else
            //            {
            //                _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
            //            }

            //            if (_dsCustom.Tables[0].Rows.Count > 0)
            //            {
            //                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            //                {


            //                    if (_dr["Name"].ToString().Equals("GSTGL"))
            //                    {
            //                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
            //                        {
            //                            _objChart.ConnConfig = Session["config"].ToString();
            //                            _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

            //                            _GetChart.ConnConfig = Session["config"].ToString();
            //                            _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

            //                            DataSet _dsChart = new DataSet();

            //                            List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

            //                            if (IsAPIIntegrationEnable == "YES")
            //                            //if (Session["APAPIEnable"].ToString() == "YES")
            //                            {
            //                                string APINAME = "BillAPI/AddBills_GetChart";

            //                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChart);

            //                                _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
            //                                _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);
            //                            }
            //                            else
            //                            {
            //                                _dsChart = _objBLChart.GetChart(_objChart);
            //                            }


            //                            if (_dsChart.Tables[0].Rows.Count > 0)
            //                            {
            //                                //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
            //                                gst_gstgl = _dr["Label"].ToString();
            //                                hdnGSTGL.Value = _dr["Label"].ToString();
            //                            }

            //                        }
            //                        else
            //                        {
            //                            gst_gstgl = "0";
            //                            hdnGSTGL.Value = "0";
            //                        }
            //                    }
            //                    else if (_dr["Name"].ToString().Equals("GSTRate"))
            //                    {
            //                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
            //                        {
            //                            gst_gstrate = _dr["Label"].ToString();
            //                            hdnGST.Value = _dr["Label"].ToString();
            //                            if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
            //                            {
            //                                hdnGST.Value = _dr["Label"].ToString();
            //                            }
            //                            else
            //                            {
            //                                hdnGST.Value = "0";
            //                            }

            //                        }
            //                        else
            //                        {
            //                            gst_gstrate = "0";
            //                            hdnGST.Value = "0";
            //                        }
            //                    }

            //                }

            //            }

            //            if (gst_gstrate == "")
            //            {
            //                gst_gstrate = "0";
            //                hdnGST.Value = "0";
            //            }

            //            if (gst_gstrate == "0" || gst_gstrate == "0.0000")
            //            {
            //                spansalestax.InnerText = "Sales Tax";
            //                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

            //                txtgstgv.Visible = false;
            //            }
            //            if (Convert.ToDouble(gst_gstrate) <= 0)
            //            {
            //                spansalestax.InnerText = "Sales Tax";
            //                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

            //                txtgstgv.Visible = false;
            //            }
            //            ////////////////////////////////////////////////////////

            //            //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
            //        }
            //        else
            //        {
            //            RadGrid_gvJobCostItems.Columns[13].Display = false;
            //            RadGrid_gvJobCostItems.Columns[14].Display = false;
            //            RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
            //            txtgstgv.Visible = false;
            //            spansalestax.InnerText = "Sales Tax";
            //        }
            //    }
            //    else
            //    {
            //        RadGrid_gvJobCostItems.Columns[13].Display = false;
            //        RadGrid_gvJobCostItems.Columns[14].Display = false;
            //        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
            //        txtgstgv.Visible = false;
            //        spansalestax.InnerText = "Sales Tax";
            //    }

            //}

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                if (hdnSTaxType.Value == "2")
                {
                    hdnGST.Value = "0";
                    hdnGSTGL.Value = "0";
                    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;

                    if (txtgstgv.Visible == true)
                    {
                        if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                        {
                            txtgstgv.Visible = false;
                            RadGrid_gvJobCostItems.Columns[13].Display = false;
                            RadGrid_gvJobCostItems.Columns[14].Display = false;
                        }

                    }
                }
                else if (hdnSTaxType.Value != "2")
                {
                    //hdnGST.Value = "0";
                    //hdnGSTGL.Value = "0";
                    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;
                    //SetTax();

                    _getCustomFields.ConnConfig = Session["config"].ToString();
                    _getCustomFields.CustomName = "Country";

                    DataSet dsCustom = new DataSet();
                    List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

                    string APINAME = "BillAPI/AddBills_GetCustomFields";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

                    _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                    dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);

                    if (dsCustom.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                        {
                            //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
                            //RadGrid_gvJobCostItems.Columns[13].Visible = true;
                            RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
                            RadGrid_gvJobCostItems.Columns[14].Display = true;
                            RadGrid_gvJobCostItems.Columns[13].Display = true;
                            txtgstgv.Visible = true;
                            spansalestax.InnerText = "PST Tax";

                            ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                            string gst_gstgl = "";
                            string gst_gstrate = "";

                            _getCustomFieldsControl.ConnConfig = Session["config"].ToString();
                            DataSet _dsCustom = new DataSet();
                            List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();


                            string APINAME1 = "BillAPI/AddBills_GetCustomFieldsControl";

                            APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _getCustomFieldsControl);

                            _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse1.ResponseData);
                            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);


                            if (_dsCustom.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                                {


                                    if (_dr["Name"].ToString().Equals("GSTGL"))
                                    {
                                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                        {

                                            _GetChart.ConnConfig = Session["config"].ToString();
                                            _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                            DataSet _dsChart = new DataSet();

                                            List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();


                                            string APINAME2 = "BillAPI/AddBills_GetChart";

                                            APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _GetChart);

                                            _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse2.ResponseData);
                                            _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);


                                            if (_dsChart.Tables[0].Rows.Count > 0)
                                            {
                                                //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                                gst_gstgl = _dr["Label"].ToString();
                                                hdnGSTGL.Value = _dr["Label"].ToString();
                                            }

                                        }
                                        else
                                        {
                                            gst_gstgl = "0";
                                            hdnGSTGL.Value = "0";
                                        }
                                    }
                                    else if (_dr["Name"].ToString().Equals("GSTRate"))
                                    {
                                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                        {
                                            gst_gstrate = _dr["Label"].ToString();
                                            hdnGST.Value = _dr["Label"].ToString();
                                            if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
                                            {
                                                hdnGST.Value = _dr["Label"].ToString();
                                            }
                                            else
                                            {
                                                hdnGST.Value = "0";
                                            }

                                        }
                                        else
                                        {
                                            gst_gstrate = "0";
                                            hdnGST.Value = "0";
                                        }
                                    }

                                }

                            }

                            if (gst_gstrate == "")
                            {
                                gst_gstrate = "0";
                                hdnGST.Value = "0";
                            }

                            if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                            {
                                spansalestax.InnerText = "Sales Tax";
                                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

                                txtgstgv.Visible = false;
                            }
                            if (Convert.ToDouble(gst_gstrate) <= 0)
                            {
                                spansalestax.InnerText = "Sales Tax";
                                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

                                txtgstgv.Visible = false;
                            }
                            ////////////////////////////////////////////////////////

                            //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                        }
                        else
                        {
                            RadGrid_gvJobCostItems.Columns[13].Display = false;
                            RadGrid_gvJobCostItems.Columns[14].Display = false;
                            RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                            txtgstgv.Visible = false;
                            spansalestax.InnerText = "Sales Tax";
                        }
                    }
                    else
                    {
                        RadGrid_gvJobCostItems.Columns[13].Display = false;
                        RadGrid_gvJobCostItems.Columns[14].Display = false;
                        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                        txtgstgv.Visible = false;
                        spansalestax.InnerText = "Sales Tax";
                    }

                }
            }
            else
            {
                if (hdnSTaxType.Value == "2")
                {
                    hdnGST.Value = "0";
                    hdnGSTGL.Value = "0";
                    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;

                    if (txtgstgv.Visible == true)
                    {
                        if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                        {
                            txtgstgv.Visible = false;
                            RadGrid_gvJobCostItems.Columns[13].Display = false;
                            RadGrid_gvJobCostItems.Columns[14].Display = false;
                        }

                    }
                }
                else if (hdnSTaxType.Value != "2")
                {
                    //hdnGST.Value = "0";
                    //hdnGSTGL.Value = "0";
                    txtqst.Text = hdnSTaxName.Value + " " + hdnQST.Value;
                    //SetTax();
                    _objPropGeneral.ConnConfig = Session["config"].ToString();
                    _objPropGeneral.CustomName = "Country";

                    DataSet dsCustom = new DataSet();

                    dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);


                    if (dsCustom.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                        {
                            //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
                            //RadGrid_gvJobCostItems.Columns[13].Visible = true;
                            RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
                            RadGrid_gvJobCostItems.Columns[14].Display = true;
                            RadGrid_gvJobCostItems.Columns[13].Display = true;
                            txtgstgv.Visible = true;
                            spansalestax.InnerText = "PST Tax";

                            ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                            string gst_gstgl = "";
                            string gst_gstrate = "";
                            _objPropGeneral.ConnConfig = Session["config"].ToString();

                            DataSet _dsCustom = new DataSet();

                            _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);

                            if (_dsCustom.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                                {


                                    if (_dr["Name"].ToString().Equals("GSTGL"))
                                    {
                                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                        {
                                            _objChart.ConnConfig = Session["config"].ToString();
                                            _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                            DataSet _dsChart = new DataSet();

                                            _dsChart = _objBLChart.GetChart(_objChart);

                                            if (_dsChart.Tables[0].Rows.Count > 0)
                                            {
                                                //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                                gst_gstgl = _dr["Label"].ToString();
                                                hdnGSTGL.Value = _dr["Label"].ToString();
                                            }

                                        }
                                        else
                                        {
                                            gst_gstgl = "0";
                                            hdnGSTGL.Value = "0";
                                        }
                                    }
                                    else if (_dr["Name"].ToString().Equals("GSTRate"))
                                    {
                                        if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                        {
                                            gst_gstrate = _dr["Label"].ToString();
                                            hdnGST.Value = _dr["Label"].ToString();
                                            if (Convert.ToDouble(_dr["Label"].ToString()) > 0)
                                            {
                                                hdnGST.Value = _dr["Label"].ToString();
                                            }
                                            else
                                            {
                                                hdnGST.Value = "0";
                                            }

                                        }
                                        else
                                        {
                                            gst_gstrate = "0";
                                            hdnGST.Value = "0";
                                        }
                                    }

                                }

                            }

                            if (gst_gstrate == "")
                            {
                                gst_gstrate = "0";
                                hdnGST.Value = "0";
                            }

                            if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                            {
                                spansalestax.InnerText = "Sales Tax";
                                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

                                txtgstgv.Visible = false;
                            }
                            if (Convert.ToDouble(gst_gstrate) <= 0)
                            {
                                spansalestax.InnerText = "Sales Tax";
                                //RadGrid_gvJobCostItems.Columns[13].HeaderText = "Sales Tax Amount";

                                txtgstgv.Visible = false;
                            }
                            ////////////////////////////////////////////////////////

                            //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                        }
                        else
                        {
                            RadGrid_gvJobCostItems.Columns[13].Display = false;
                            RadGrid_gvJobCostItems.Columns[14].Display = false;
                            RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                            txtgstgv.Visible = false;
                            spansalestax.InnerText = "Sales Tax";
                        }
                    }
                    else
                    {
                        RadGrid_gvJobCostItems.Columns[13].Display = false;
                        RadGrid_gvJobCostItems.Columns[14].Display = false;
                        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                        txtgstgv.Visible = false;
                        spansalestax.InnerText = "Sales Tax";
                    }

                }
            }

            //if (txtgstgv.Visible == true)
            //{

            //        RadGrid_gvJobCostItems.Columns[12].Visible = true;
            //    RadGrid_gvJobCostItems.Columns[12].Display = true;
            //}

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnStaxType_Click(object sender, EventArgs e)
    {
        FillSalesTax();
        if (hdnSTaxName.Value.Trim() != "")
        {
            ddlSTax.SelectedValue = hdnSTaxName.Value;
        }
        else
        {
            ddlSTax.SelectedIndex = 0;
        }
        SetHarmonizedTax();
        DataTable dtgt = GetTransaction();

        if (ViewState["Transactions_JobCost"] != null && dtgt.Rows.Count == 0)
        {
            //DataTable dtgt = (DataTable)ViewState["Transactions_JobCost"];
            dtgt = (DataTable)ViewState["Transactions_JobCost"];
        }
        double rtsrate = 0;
        int rtsgl = 0;
        int rtgstgl = 0;
        double rtgstamt = 0;
        double rtgstrate = 0;
        double dramt = 0;
        string rtsname = "";
        string rtstype = hdnSTaxType.Value;
        if (hdnQST.Value != "")
        {
            rtsrate = Convert.ToDouble(hdnQST.Value);
        }
        if (hdnQSTGL.Value != "")
        {
            rtsgl = Convert.ToInt32(hdnQSTGL.Value);
        }
        if (hdnGST.Value != "")
        {
            rtgstrate = Convert.ToDouble(hdnGST.Value);
        }
        if (hdnGSTGL.Value != "")
        {
            rtgstgl = Convert.ToInt32(hdnGSTGL.Value);
        }
        if (hdnSTaxName.Value != "")
        {
            rtsname = hdnSTaxName.Value;
        }

        DataRow[] drows = dtgt.Select();
        for (int i = 0; i < drows.Length; i++)
        {
            dtgt.Rows[i]["STaxName"] = rtsname;
            dtgt.Rows[i]["STaxRate"] = rtsrate;
            dtgt.Rows[i]["STaxGL"] = Convert.ToInt32(rtsgl);
            if (RadGrid_gvJobCostItems.Columns[12].Display == true)
            {
                dtgt.Rows[i]["GSTRate"] = rtgstrate;
                dtgt.Rows[i]["GSTTaxGL"] = rtgstgl;
                if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                {
                    dtgt.Rows[i]["GTaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * Convert.ToDouble(rtgstrate) / 100 * 100) / 100;
                }
                else
                {
                    dtgt.Rows[i]["GTaxAmt"] = 0;
                }
            }
            else
            {
                dtgt.Rows[i]["GSTRate"] = 0;
                dtgt.Rows[i]["GSTTaxGL"] = 0;
                dtgt.Rows[i]["GTaxAmt"] = 0;
            }

            if (Convert.ToInt32(dtgt.Rows[i]["STax"]) == 1)
            {

                if (rtstype == "0" || rtstype == "2")
                {
                    if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                    {
                        if (Convert.ToDouble(dtgt.Rows[i]["Amount"]) < 0)
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * (-1) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;
                            dtgt.Rows[i]["StaxAmt"] = Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]) * (-1);

                        }
                        else
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;

                        }
                    }
                }
                else if (rtstype == "1")
                {
                    if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                    {
                        dramt = Convert.ToDouble(dtgt.Rows[i]["Amount"]) + Convert.ToDouble(dtgt.Rows[i]["GTaxAmt"]);
                        if (Convert.ToDouble(dtgt.Rows[i]["Amount"]) < 0)
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dramt) * (-1) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;
                            dtgt.Rows[i]["StaxAmt"] = Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]) * (-1);

                        }
                        else
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dramt) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;

                        }
                    }
                }
                dtgt.Rows[i]["AmountTot"] = Convert.ToDouble(dtgt.Rows[i]["Amount"]) + Convert.ToDouble(dtgt.Rows[i]["GTaxAmt"]) + Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]);
            }

        }
        dtgt.AcceptChanges();
        ViewState["Transactions_JobCost"] = dtgt;
        BINDGRID(dtgt);

    }
    protected void btnUpdtStax_Click(object sender, EventArgs e)
    {
        if (hdnUpdateStax.Value.ToString() == "1")
        {

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "BillAPI/AddBills_UpdateVendorSTax";

                UpdateVendorSTaxParam _UpdateVendorSTaxParam = new UpdateVendorSTaxParam();
                _UpdateVendorSTaxParam.sTax = ddlSTax.SelectedValue;
                _UpdateVendorSTaxParam.VendorId = Convert.ToInt32(hdnVendorID.Value);
                _UpdateVendorSTaxParam.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateVendorSTaxParam);
            }
            else
            {
                _objBLVendor.UpdateVendorSTax(Session["config"].ToString(), ddlSTax.SelectedValue, Convert.ToInt32(hdnVendorID.Value));
            }
        }
        FillSalesTax();
        if (hdnSTaxName.Value.Trim() != "")
        {
            ddlSTax.SelectedValue = hdnSTaxName.Value;
        }
        else
        {
            ddlSTax.SelectedIndex = 0;
        }
        SetHarmonizedTax();
        DataTable dtgt = GetTransaction();

        if (ViewState["Transactions_JobCost"] != null && dtgt.Rows.Count == 0)
        {
            //DataTable dtgt = (DataTable)ViewState["Transactions_JobCost"];
            dtgt = (DataTable)ViewState["Transactions_JobCost"];
        }


        double rtsrate = 0;
        int rtsgl = 0;
        int rtgstgl = 0;
        double rtgstamt = 0;
        double rtgstrate = 0;
        double dramt = 0;
        string rtsname = "";
        string rtstype = hdnSTaxType.Value;
        if (hdnQST.Value != "")
        {
            rtsrate = Convert.ToDouble(hdnQST.Value);
        }
        if (hdnQSTGL.Value != "")
        {
            rtsgl = Convert.ToInt32(hdnQSTGL.Value);
        }
        if (hdnSTaxName.Value != "")
        {
            rtsname = hdnSTaxName.Value;
        }
        if (hdnGST.Value != "")
        {
            rtgstrate = Convert.ToDouble(hdnGST.Value);
        }
        if (hdnGSTGL.Value != "")
        {
            rtgstgl = Convert.ToInt32(hdnGSTGL.Value);
        }
        DataRow[] drows = dtgt.Select();
        for (int i = 0; i < drows.Length; i++)
        {
            dtgt.Rows[i]["STaxName"] = rtsname;
            dtgt.Rows[i]["STaxRate"] = rtsrate;
            dtgt.Rows[i]["STaxGL"] = Convert.ToInt32(rtsgl);

            //if (RadGrid_gvJobCostItems.Columns[12].Display == true)
            //{
            if (Convert.ToInt32(dtgt.Rows[i]["GTax"]) == 1)
            {

                dtgt.Rows[i]["GSTRate"] = rtgstrate;
                dtgt.Rows[i]["GSTTaxGL"] = rtgstgl;
                if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                {
                    dtgt.Rows[i]["GTaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * Convert.ToDouble(rtgstrate) / 100 * 100) / 100;
                }
                else
                {
                    dtgt.Rows[i]["GTaxAmt"] = 0;
                }
            }
            else
            {
                dtgt.Rows[i]["GSTRate"] = 0;
                dtgt.Rows[i]["GSTTaxGL"] = 0;
                dtgt.Rows[i]["GTaxAmt"] = 0;
            }


            if (Convert.ToInt32(dtgt.Rows[i]["STax"]) == 1)
            {

                if (rtstype == "0" || rtstype == "2")
                {
                    if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                    {
                        if (Convert.ToDouble(dtgt.Rows[i]["Amount"]) < 0)
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * (-1) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;
                            dtgt.Rows[i]["StaxAmt"] = Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]) * (-1);

                        }
                        else
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dtgt.Rows[i]["Amount"]) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;

                        }
                    }
                }
                else if (rtstype == "1")
                {
                    if (dtgt.Rows[i]["Amount"] != DBNull.Value)
                    {
                        dramt = Convert.ToDouble(dtgt.Rows[i]["Amount"]) + Convert.ToDouble(dtgt.Rows[i]["GTaxAmt"]);
                        if (Convert.ToDouble(dtgt.Rows[i]["Amount"]) < 0)
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dramt) * (-1) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;
                            dtgt.Rows[i]["StaxAmt"] = Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]) * (-1);

                        }
                        else
                        {
                            dtgt.Rows[i]["StaxAmt"] = Math.Round(Convert.ToDouble(dramt) * Convert.ToDouble(rtsrate) / 100 * 100) / 100;

                        }
                    }
                }
                dtgt.Rows[i]["AmountTot"] = Convert.ToDouble(dtgt.Rows[i]["Amount"]) + Convert.ToDouble(dtgt.Rows[i]["GTaxAmt"]) + Convert.ToDouble(dtgt.Rows[i]["StaxAmt"]);
            }
            else
            {
                dtgt.Rows[i]["AmountTot"] = dtgt.Rows[i]["Amount"];
            }
        }
        dtgt.AcceptChanges();
        ViewState["Transactions_JobCost"] = dtgt;
        BINDGRID(dtgt);

    }
    protected void btnSelectPo_Click(object sender, EventArgs e)
    {
        try
        {
            //objPO.ConnConfig = Session["config"].ToString();
            //_getOutStandingPOById.ConnConfig = Session["config"].ToString();
            //if (!string.IsNullOrEmpty(txtPO.Text))
            //{
            //    txtVendor.Enabled = false;
            //    objPO.POID = Convert.ToInt32(txtPO.Text);
            //    _getOutStandingPOById.POID = Convert.ToInt32(txtPO.Text);
            //    //  DataSet ds = _objBLBills.GetPOById(objPO);

            //    DataSet ds = new DataSet();
            //    DataSet ds1 = new DataSet();
            //    DataSet ds2 = new DataSet();
            //    ListGetOutStandingPOById _lstGetOutStandingPOById = new ListGetOutStandingPOById();

            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        string APINAME = "BillAPI/AddBills_GetOutStandingPOById";

            //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOutStandingPOById);

            //        _lstGetOutStandingPOById = (new JavaScriptSerializer()).Deserialize<ListGetOutStandingPOById>(_APIResponse.ResponseData);

            //        ds1 = _lstGetOutStandingPOById.lstTable1.ToDataSet();
            //        ds2 = _lstGetOutStandingPOById.lstTable2.ToDataSet();

            //        DataTable dt1 = new DataTable();
            //        DataTable dt2 = new DataTable();

            //        dt1 = ds1.Tables[0];
            //        dt2 = ds2.Tables[0];

            //        dt1.TableName = "Table1";
            //        dt2.TableName = "Table2";

            //        ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            //    }
            //    else
            //    {
            //        ds = _objBLBills.GetOutStandingPOById(objPO);
            //    }

            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        if (ds1.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow dr = ds1.Tables[0].Rows[0];
            //            txtVendor.Text = dr["VendorName"].ToString();
            //            hdnVendorID.Value = dr["Vendor"].ToString();
            //            //txtRef.Text = dr["Ref"].ToString();
            //            txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            //txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
            //            txtMemo.Text = dr["fDesc"].ToString();
            //            //hdnTotalAmount.Value = dr["Amount"].ToString();
            //            hdnTotalAmount.Value = dr["Amount"].ToString();
            //            txtDueIn.Text = dr["Term"].ToString();
            //            txtPaid.Text = dr["Days"].ToString();
            //            txtCustom1.Text = dr["Custom1"].ToString();
            //            txtCustom2.Text = dr["Custom2"].ToString();
            //            hdnSTaxState.Value = dr["State"].ToString();
            //            FillSalesTax();
            //            if (!string.IsNullOrEmpty(txtDueIn.Text))
            //            {
            //                int dueIn = Convert.ToInt32(txtDueIn.Text);
            //                txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
            //            }

            //            //ds.Tables[1].Columns.Add(new DataColumn("AmountTot"));
            //            //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            //            //{
            //            //    double _sTaxAmt = 0;
            //            //    double _gTaxAmt = 0;
            //            //    double _sAmt = 0;
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["STaxAmt"].ToString()))
            //            //    {
            //            //        _sTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["STaxAmt"].ToString());
            //            //    }
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["GTaxAmt"].ToString()))
            //            //    {
            //            //        _gTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["GTaxAmt"].ToString());
            //            //    }
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["Amount"].ToString()))
            //            //    {
            //            //        _sAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["Amount"].ToString());
            //            //    }
            //            //    ds.Tables[1].Rows[i]["AmountTot"] = "new value here";
            //            //}

            //            DataColumn dc = new DataColumn("IsPO");
            //            dc.DataType = typeof(int);
            //            ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
            //            ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
            //            dc.DefaultValue = 1;
            //            ds2.Tables[0].Columns.Add(dc);

            //            DataColumn colGtax = new DataColumn("GTax");
            //            colGtax.DataType = typeof(int);
            //            ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
            //            ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
            //            colGtax.DefaultValue = 0;
            //            ds2.Tables[0].Columns.Add(colGtax);
            //            ViewState["Transactions_JobCost"] = ds2.Tables[0];
            //            BINDGRID(ds2.Tables[0]);

            //            if (ds1.Tables[0].Rows.Count > 0)
            //            {
            //                DataSet dset = new DataSet();
            //                _objVendor.SearchValue = txtVendor.Text;
            //                _objVendor.ConnConfig = Session["config"].ToString();
            //                _getVendorSearch.SearchValue = txtVendor.Text;
            //                _getVendorSearch.ConnConfig = Session["config"].ToString();
            //                if (Session["CmpChkDefault"].ToString() == "1")
            //                {
            //                    _objVendor.EN = 1;
            //                    _getVendorSearch.EN = 1;
            //                }
            //                else
            //                {
            //                    _objVendor.EN = 0;
            //                    _getVendorSearch.EN = 0;
            //                }

            //                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            //                if (IsAPIIntegrationEnable == "YES")
            //                //if (Session["APAPIEnable"].ToString() == "YES")
            //                {
            //                    string APINAME = "BillAPI/AddBills_GetVendorSearch";

            //                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorSearch);

            //                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            //                    dset = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            //                }
            //                else
            //                {
            //                    dset = _objBLVendor.GetVendorSearch(_objVendor);
            //                }


            //                txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
            //                hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
            //                hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
            //                hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
            //                hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

            //                txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
            //                hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
            //                hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
            //                hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
            //                hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

            //                if (hdnSTaxName.Value.Trim() != "")
            //                {
            //                    if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
            //                    {
            //                        ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                    }
            //                    else
            //                    {
            //                        ddlSTax.SelectedIndex = 0;
            //                    }
            //                    //ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                }
            //                else
            //                {
            //                    ddlSTax.SelectedIndex = 0;
            //                }

            //            }

            //            lblTotalAmount.Text = ds1.Tables[0].Rows[0]["Amount"].ToString();

            //        }
            //    }
            //    else
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow dr = ds.Tables[0].Rows[0];
            //            txtVendor.Text = dr["VendorName"].ToString();
            //            hdnVendorID.Value = dr["Vendor"].ToString();
            //            //txtRef.Text = dr["Ref"].ToString();
            //            txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            //txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
            //            txtMemo.Text = dr["fDesc"].ToString();
            //            //hdnTotalAmount.Value = dr["Amount"].ToString();
            //            hdnTotalAmount.Value = dr["Amount"].ToString();
            //            txtDueIn.Text = dr["Term"].ToString();
            //            txtPaid.Text = dr["Days"].ToString();
            //            txtCustom1.Text = dr["Custom1"].ToString();
            //            txtCustom2.Text = dr["Custom2"].ToString();
            //            hdnSTaxState.Value = dr["State"].ToString();
            //            FillSalesTax();
            //            if (!string.IsNullOrEmpty(txtDueIn.Text))
            //            {
            //                int dueIn = Convert.ToInt32(txtDueIn.Text);
            //                txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
            //            }

            //            //ds.Tables[1].Columns.Add(new DataColumn("AmountTot"));
            //            //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
            //            //{
            //            //    double _sTaxAmt = 0;
            //            //    double _gTaxAmt = 0;
            //            //    double _sAmt = 0;
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["STaxAmt"].ToString()))
            //            //    {
            //            //        _sTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["STaxAmt"].ToString());
            //            //    }
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["GTaxAmt"].ToString()))
            //            //    {
            //            //        _gTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["GTaxAmt"].ToString());
            //            //    }
            //            //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["Amount"].ToString()))
            //            //    {
            //            //        _sAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["Amount"].ToString());
            //            //    }
            //            //    ds.Tables[1].Rows[i]["AmountTot"] = "new value here";
            //            //}

            //            DataColumn dc = new DataColumn("IsPO");
            //            dc.DataType = typeof(int);
            //            ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
            //            ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
            //            dc.DefaultValue = 1;
            //            ds.Tables[1].Columns.Add(dc);

            //            DataColumn colGtax = new DataColumn("GTax");
            //            colGtax.DataType = typeof(int);
            //            ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
            //            ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
            //            colGtax.DefaultValue = 0;
            //            ds.Tables[1].Columns.Add(colGtax);
            //            ViewState["Transactions_JobCost"] = ds.Tables[1];
            //            BINDGRID(ds.Tables[1]);

            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                DataSet dset = new DataSet();
            //                _objVendor.SearchValue = txtVendor.Text;
            //                _objVendor.ConnConfig = Session["config"].ToString();
            //                _getVendorSearch.SearchValue = txtVendor.Text;
            //                _getVendorSearch.ConnConfig = Session["config"].ToString();
            //                if (Session["CmpChkDefault"].ToString() == "1")
            //                {
            //                    _objVendor.EN = 1;
            //                    _getVendorSearch.EN = 1;
            //                }
            //                else
            //                {
            //                    _objVendor.EN = 0;
            //                    _getVendorSearch.EN = 0;
            //                }

            //                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            //                if (IsAPIIntegrationEnable == "YES")
            //                //if (Session["APAPIEnable"].ToString() == "YES")
            //                {
            //                    string APINAME = "BillAPI/AddBills_GetVendorSearch";

            //                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorSearch);

            //                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            //                    dset = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            //                }
            //                else
            //                {
            //                    dset = _objBLVendor.GetVendorSearch(_objVendor);
            //                }


            //                txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
            //                hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
            //                hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
            //                hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
            //                hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

            //                txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
            //                hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
            //                hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
            //                hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
            //                hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

            //                if (hdnSTaxName.Value.Trim() != "")
            //                {
            //                    if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
            //                    {
            //                        ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                    }
            //                    else
            //                    {
            //                        ddlSTax.SelectedIndex = 0;
            //                    }
            //                    //ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                }
            //                else
            //                {
            //                    ddlSTax.SelectedIndex = 0;
            //                }

            //                //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
            //                //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
            //                //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

            //            }

            //            lblTotalAmount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
            //            //try
            //            //{
            //            //    objPO.Vendor = Convert.ToInt16(hdnVendorID.Value);
            //            //    DataSet dsReceptionNo = _objBLBills.GetReceivePOList(objPO);
            //            //    if (dsReceptionNo.Tables[0].Rows.Count > 0)
            //            //    {
            //            //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please select reception no#.',  type : 'warrning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);

            //            //    }
            //            //    else
            //            //    {
            //            //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);                          
            //            //    }
            //            //}
            //            //catch { }
            //        }
            //    }
            //    btnUpdtStax_Click(sender, e);
            //}
            //else
            //{
            //    txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
            //}

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _getOutStandingPOById.ConnConfig = Session["config"].ToString();
                if (!string.IsNullOrEmpty(txtPO.Text))
                {
                    txtVendor.Enabled = false;

                    _getOutStandingPOById.POID = Convert.ToInt32(txtPO.Text);
                    //  DataSet ds = _objBLBills.GetPOById(objPO);

                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();
                    ListGetOutStandingPOById _lstGetOutStandingPOById = new ListGetOutStandingPOById();

                    string APINAME = "BillAPI/AddBills_GetOutStandingPOById";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOutStandingPOById);

                    _lstGetOutStandingPOById = (new JavaScriptSerializer()).Deserialize<ListGetOutStandingPOById>(_APIResponse.ResponseData);

                    ds1 = _lstGetOutStandingPOById.lstTable1.ToDataSet();
                    ds2 = _lstGetOutStandingPOById.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtVendor.Text = dr["VendorName"].ToString();
                        hdnVendorID.Value = dr["Vendor"].ToString();
                        //txtRef.Text = dr["Ref"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        //txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        txtMemo.Text = dr["fDesc"].ToString();
                        //hdnTotalAmount.Value = dr["Amount"].ToString();
                        hdnTotalAmount.Value = dr["Amount"].ToString();
                        txtDueIn.Text = dr["Term"].ToString();
                        txtPaid.Text = dr["Days"].ToString();
                        txtCustom1.Text = dr["Custom1"].ToString();
                        txtCustom2.Text = dr["Custom2"].ToString();
                        hdnSTaxState.Value = dr["State"].ToString();
                        FillSalesTax();
                        if (!string.IsNullOrEmpty(txtDueIn.Text))
                        {
                            int dueIn = Convert.ToInt32(txtDueIn.Text);
                            txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
                        }

                        //ds.Tables[1].Columns.Add(new DataColumn("AmountTot"));
                        //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                        //{
                        //    double _sTaxAmt = 0;
                        //    double _gTaxAmt = 0;
                        //    double _sAmt = 0;
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["STaxAmt"].ToString()))
                        //    {
                        //        _sTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["STaxAmt"].ToString());
                        //    }
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["GTaxAmt"].ToString()))
                        //    {
                        //        _gTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["GTaxAmt"].ToString());
                        //    }
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["Amount"].ToString()))
                        //    {
                        //        _sAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["Amount"].ToString());
                        //    }
                        //    ds.Tables[1].Rows[i]["AmountTot"] = "new value here";
                        //}

                        DataColumn dc = new DataColumn("IsPO");
                        dc.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        dc.DefaultValue = 2;
                        ds.Tables[1].Columns.Add(dc);

                        DataColumn colGtax = new DataColumn("GTax");
                        colGtax.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        colGtax.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(colGtax);
                        ViewState["Transactions_JobCost"] = ds.Tables[1];
                        BINDGRID(ds.Tables[1]);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataSet dset = new DataSet();
                            _getVendorSearch.SearchValue = txtVendor.Text;
                            _getVendorSearch.ConnConfig = Session["config"].ToString();
                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                _getVendorSearch.EN = 1;
                            }
                            else
                            {
                                _getVendorSearch.EN = 0;
                            }

                            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();


                            string APINAME1 = "BillAPI/AddBills_GetVendorSearch";

                            APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _getVendorSearch);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse2.ResponseData);
                            dset = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);


                            txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
                            hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
                            hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
                            hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
                            hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

                            txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
                            hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
                            hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
                            hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
                            hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

                            if (hdnSTaxName.Value.Trim() != "")
                            {
                                if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
                                {
                                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                                }
                                else
                                {
                                    ddlSTax.SelectedIndex = 0;
                                }
                                //ddlSTax.SelectedValue = hdnSTaxName.Value;
                            }
                            else
                            {
                                ddlSTax.SelectedIndex = 0;
                            }

                            //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
                            //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                            //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

                        }

                        lblTotalAmount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                        //try
                        //{
                        //    objPO.Vendor = Convert.ToInt16(hdnVendorID.Value);
                        //    DataSet dsReceptionNo = _objBLBills.GetReceivePOList(objPO);
                        //    if (dsReceptionNo.Tables[0].Rows.Count > 0)
                        //    {
                        //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please select reception no#.',  type : 'warrning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);

                        //    }
                        //    else
                        //    {
                        //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);                          
                        //    }
                        //}
                        //catch { }
                    }

                    btnUpdtStax_Click(sender, e);
                }
                else
                {
                    txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
                }
                chkPOClose.Visible = true;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
            else
            {
                objPO.ConnConfig = Session["config"].ToString();

                if (!string.IsNullOrEmpty(txtPO.Text))
                {
                    txtVendor.Enabled = false;
                    objPO.POID = Convert.ToInt32(txtPO.Text);

                    //  DataSet ds = _objBLBills.GetPOById(objPO);

                    DataSet ds = new DataSet();

                    ds = _objBLBills.GetOutStandingPOById(objPO);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtVendor.Text = dr["VendorName"].ToString();


                        hdnVendorID.Value = dr["Vendor"].ToString();
                        //txtRef.Text = dr["Ref"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        //txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        txtMemo.Text = dr["fDesc"].ToString();
                        //hdnTotalAmount.Value = dr["Amount"].ToString();
                        hdnTotalAmount.Value = dr["Amount"].ToString();
                        txtDueIn.Text = dr["Term"].ToString();
                        txtPaid.Text = dr["Days"].ToString();
                        txtCustom1.Text = dr["Custom1"].ToString();
                        txtCustom2.Text = dr["Custom2"].ToString();
                        hdnSTaxState.Value = dr["State"].ToString();
                        FillSalesTax();
                        if (!string.IsNullOrEmpty(txtDueIn.Text))
                        {
                            int dueIn = Convert.ToInt32(txtDueIn.Text);
                            txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
                        }

                        //ds.Tables[1].Columns.Add(new DataColumn("AmountTot"));
                        //for (int i = 0; i <= ds.Tables[1].Rows.Count - 1; i++)
                        //{
                        //    double _sTaxAmt = 0;
                        //    double _gTaxAmt = 0;
                        //    double _sAmt = 0;
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["STaxAmt"].ToString()))
                        //    {
                        //        _sTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["STaxAmt"].ToString());
                        //    }
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["GTaxAmt"].ToString()))
                        //    {
                        //        _gTaxAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["GTaxAmt"].ToString());
                        //    }
                        //    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[i]["Amount"].ToString()))
                        //    {
                        //        _sAmt = Convert.ToDouble(ds.Tables[1].Rows[i]["Amount"].ToString());
                        //    }
                        //    ds.Tables[1].Rows[i]["AmountTot"] = "new value here";
                        //}

                        DataColumn dc = new DataColumn("IsPO");
                        dc.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        dc.DefaultValue = 2;
                        ds.Tables[1].Columns.Add(dc);

                        DataColumn colGtax = new DataColumn("GTax");
                        colGtax.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        colGtax.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(colGtax);
                        ViewState["Transactions_JobCost"] = ds.Tables[1];
                        BINDGRID(ds.Tables[1]);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataSet dset = new DataSet();
                            _objVendor.SearchValue = txtVendor.Text;
                            _objVendor.ConnConfig = Session["config"].ToString();

                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                _objVendor.EN = 1;

                            }
                            else
                            {
                                _objVendor.EN = 0;
                            }

                            dset = _objBLVendor.GetVendorSearch(_objVendor);

                            txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
                            hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
                            hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
                            hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
                            hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

                            txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
                            hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
                            hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
                            hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
                            hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

                            if (hdnSTaxName.Value.Trim() != "")
                            {
                                if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
                                {
                                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                                }
                                else
                                {
                                    ddlSTax.SelectedIndex = 0;
                                }
                                //ddlSTax.SelectedValue = hdnSTaxName.Value;
                            }
                            else
                            {
                                ddlSTax.SelectedIndex = 0;
                            }

                            //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
                            //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                            //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

                        }

                        lblTotalAmount.Text = ds.Tables[0].Rows[0]["Amount"].ToString();
                        //try
                        //{
                        //    objPO.Vendor = Convert.ToInt16(hdnVendorID.Value);
                        //    DataSet dsReceptionNo = _objBLBills.GetReceivePOList(objPO);
                        //    if (dsReceptionNo.Tables[0].Rows.Count > 0)
                        //    {
                        //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please select reception no#.',  type : 'warrning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);

                        //    }
                        //    else
                        //    {
                        //        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);                          
                        //    }
                        //}
                        //catch { }
                    }

                    btnUpdtStax_Click(sender, e);
                }
                else
                {
                    txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
                }
                chkPOClose.Visible = true;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetRPOItem()
    {


        //save in IwarehouseLocAdj
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();

        _objPropGeneral.ConnConfig = Session["config"].ToString();

        DataSet _dsCustom = new DataSet();

        _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");

        DataSet _dsDefaultAccount = new DataSet();

        _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);

        Boolean TrackingInventory = false;
        int DefaultAcctID = 0;
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

        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
        {
            DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
        }


        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("POLine", typeof(Int32)));
        DataColumn dcq = new DataColumn("Quan", typeof(string));
        dcq.AllowDBNull = true;
        DataColumn dPrice = new DataColumn("Price", typeof(string));
        dPrice.AllowDBNull = true;
        DataColumn dAmount = new DataColumn("Amount", typeof(string));
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


        foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
        {
            HiddenField lblOutstand = (HiddenField)gr.FindControl("hdnOutstandBalance");
            TextBox lblPrice = (TextBox)gr.FindControl("txtGvPrice");
            TextBox txtReceive = (TextBox)gr.FindControl("txtGvAmount");
            TextBox txtReceiveQty = (TextBox)gr.FindControl("txtGvQuan");
            HiddenField hdnReceive = (HiddenField)gr.FindControl("hdnReceive");
            HiddenField hdnReceiveQty = (HiddenField)gr.FindControl("hdnQuantity");
            HiddenField lblLine = (HiddenField)gr.FindControl("hdnLine");
            HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
            HiddenField hdnPrvInQuan = (HiddenField)gr.FindControl("hdnPrvInQuan");
            HiddenField hdnPrvIn = (HiddenField)gr.FindControl("hdnPrvIn");

            TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
            HiddenField hdnItemID = (HiddenField)gr.FindControl("hdnItemID");
            HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
            HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");
            HiddenField hdnOutstandQuan = (HiddenField)gr.FindControl("hdnOutstandQuan");

            HiddenField lblOrderedQuan = (HiddenField)gr.FindControl("HdnOrderedQuan");
            HiddenField lblOrdered = (HiddenField)gr.FindControl("HdnOrdered");
            TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
            HiddenField hdnRPOItemId = (HiddenField)gr.FindControl("hdnRPOItemId");
            HiddenField hdnPOItemId = (HiddenField)gr.FindControl("hdnPOItemId");
            HiddenField hdnTypeId = (HiddenField)gr.FindControl("hdnTypeId");



            //if (!string.IsNullOrEmpty(lblLine.Value) && lblLine.Value != "0")
            if (!string.IsNullOrEmpty(hdnPOItemId.Value) && hdnPOItemId.Value != "0")
            {

                double dblOutstand = 0.00;
                double dblReceive = 0.00;
                double dblPrvIn = 0.00;
                double dblReceiveQty = 0;
                double dblPrvInQuan = Convert.ToDouble(hdnPrvInQuan.Value);

                if (!string.IsNullOrEmpty(Request.Form[txtReceive.UniqueID]) & !string.IsNullOrEmpty(lblOutstand.Value))
                {
                    if (!Convert.ToDouble(Request.Form[txtReceive.UniqueID]).Equals(0))
                    {
                        dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Value);
                        dblReceive = ConvertCurrentCurrencyFormatToDbl(Request.Form[txtReceive.UniqueID]);
                        dblPrvIn = ConvertCurrentCurrencyFormatToDbl(hdnPrvIn.Value);
                    }

                    if (Request.Form[txtReceiveQty.UniqueID].Trim() != "")
                    {
                        dblReceiveQty = Convert.ToDouble(Request.Form[txtReceiveQty.UniqueID]);
                    }
                    if (dblReceive >= ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn)
                    {
                        dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn, 4);
                        txtReceive.Text = Convert.ToString(dblReceive);

                        dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan, 4);
                        txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                        dblOutstand = dblReceive;
                        hdnOutstandQuan.Value = Convert.ToString(dblReceiveQty);
                    }
                    if (dblReceiveQty >= Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan)
                    {
                        dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan, 4);
                        txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

                        dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn, 4);
                        txtReceive.Text = Convert.ToString(dblReceive);

                        dblOutstand = dblReceive;
                        hdnOutstandQuan.Value = Convert.ToString(dblReceiveQty);
                    }

                    // Start-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)
                    if (dblOutstand == 0 && dblReceive == 0)
                    {
                        dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Value);
                        dblPrvIn = ConvertCurrentCurrencyFormatToDbl(hdnPrvIn.Value);
                    }
                    // End-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)

                    DataRow dr = dt.NewRow();
                    dr["ID"] = Convert.ToInt32(hdnRPOItemId.Value);
                    dr["POLine"] = Convert.ToInt32(lblLine.Value);
                    dr["Quan"] = Convert.ToString(dblReceiveQty);
                    dr["Price"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(lblPrice.Text));
                    dr["Amount"] = Convert.ToString(dblReceive);
                    dr["fDesc"] = Convert.ToString(txtGvDesc.Text);
                    if (Convert.ToString(hdnItemID.Value) != "")
                    {
                        dr["ItemID"] = Convert.ToInt32(hdnItemID.Value);
                    }
                    else
                    {
                        dr["ItemID"] = 0;
                    }
                    dr["POItemId"] = Convert.ToInt32(hdnPOItemId.Value);
                    dr["Selected"] = Convert.ToString(dblReceive + dblPrvIn);
                    dr["SelectedQuan"] = Convert.ToString(Math.Round((dblPrvInQuan + dblReceiveQty), 4));
                    dr["Balance"] = Convert.ToString(dblOutstand - dblReceive);
                    dr["BalanceQuan"] = Convert.ToString(ConvertCurrentCurrencyFormatToDbl(lblOrderedQuan.Value) - Math.Round((dblPrvInQuan + dblReceiveQty), 4));

                    int _jobID = 0;
                    int.TryParse(hdnJobID.Value, out _jobID);
                    if (_jobID > 0)
                    {
                        if (hdnTypeId.Value == "8")
                        {
                            dr["IsReceiveIssued"] = Convert.ToInt32(2);
                        }
                        else
                        {
                            dr["IsReceiveIssued"] = Convert.ToInt32(1);
                        }
                    }
                    else
                    {
                        if (TrackingInventory == true)
                        {
                            if (hdnTypeId.Value == "8")
                            {
                                dr["IsReceiveIssued"] = Convert.ToInt32(2);
                            }
                            else
                            {
                                dr["IsReceiveIssued"] = Convert.ToInt32(0);
                            }
                        }
                        else
                        {
                            dr["IsReceiveIssued"] = Convert.ToInt32(0);
                        }
                    }
                    //dr["IsReceiveIssued"] = Convert.ToInt32(0);
                    if (Convert.ToString(hdnJobID.Value) != "")
                    {
                        dr["JobID"] = Convert.ToInt32(hdnJobID.Value);
                    }
                    else
                    {
                        dr["JobID"] = "0";
                    }


                    dr["Warehouse"] = Convert.ToString(hdnWarehouse.Value);

                    if (Convert.ToString(hdnWarehouseLocationID.Value) != "")
                    {
                        dr["LocationID"] = Convert.ToInt32(hdnWarehouseLocationID.Value);
                    }
                    else
                    {
                        dr["LocationID"] = "0";
                    }
                    dr["BOQty"] = Convert.ToString(hdnOutstandQuan.Value);
                    dt.Rows.Add(dr);



                }


            }

        }

        return dt;
    }

    /// <summary>
    /// Created by Rustam to make a entry with RPOITEM table 
    /// </summary>
    private int AddRPOItem(int ID_NEW)
    {
        double amount = 0;
        double poAmount = 0;
        double poQty = 0;
        double qty = 0;
        foreach (GridDataItem gv in RadGrid_gvJobCostItems.Items)
        {
            TextBox lblBOQty = (TextBox)gv.FindControl("txtGvQuan");
            TextBox lblOutstand = (TextBox)gv.FindControl("txtGvAmount");
            HiddenField hdnLine = (HiddenField)gv.FindControl("hdnLine");
            if (!string.IsNullOrEmpty(lblBOQty.Text) & !string.IsNullOrEmpty(lblOutstand.Text))
            {
                if (!string.IsNullOrEmpty(hdnLine.Value) && hdnLine.Value != "0")
                {
                    poQty = poQty + Convert.ToDouble(lblBOQty.Text);
                    poAmount = poAmount + ConvertCurrentCurrencyFormatToDbl(lblOutstand.Text);
                }
            }
        }
        //bool IsAmount = false;
        int countItem = 0;
        int BatchID = 0;
        double Inv_Total = 0;


        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            #region Update PO balance

            //save in IwarehouseLocAdj
            General _objPropGeneral = new General();
            BL_General _objBLGeneral = new BL_General();

            _getCustomField.ConnConfig = Session["config"].ToString();
            _GetInvDefaultAcct.ConnConfig = Session["config"].ToString();

            DataSet _dsCustom = new DataSet();
            List<CustomViewModel> _lstCustom = new List<CustomViewModel>();


            _getCustomField.fieldName = "InvGL";

            string APINAME = "BillAPI/AddBills_GetCustomField";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomField);

            _lstCustom = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustom);



            DataSet _dsDefaultAccount = new DataSet();
            List<GeneralViewModel> _lstGeneralViewModel = new List<GeneralViewModel>();

            string APINAME1 = "BillAPI/AddBills_GetInvDefaultAcct";

            APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetInvDefaultAcct);

            _lstGeneralViewModel = (new JavaScriptSerializer()).Deserialize<List<GeneralViewModel>>(_APIResponse1.ResponseData);
            _dsDefaultAccount = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneralViewModel);


            Boolean TrackingInventory = false;
            int DefaultAcctID = 0;
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

            if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
            {
                DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            }

            foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            {
                HiddenField lblOutstand = (HiddenField)gr.FindControl("hdnOutstandBalance");
                TextBox lblPrice = (TextBox)gr.FindControl("txtGvPrice");
                TextBox txtReceive = (TextBox)gr.FindControl("txtGvAmount");
                TextBox txtReceiveQty = (TextBox)gr.FindControl("txtGvQuan");
                HiddenField hdnReceive = (HiddenField)gr.FindControl("hdnReceive");
                HiddenField hdnReceiveQty = (HiddenField)gr.FindControl("hdnQuantity");
                HiddenField lblLine = (HiddenField)gr.FindControl("hdnLine");
                HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                HiddenField hdnPrvInQuan = (HiddenField)gr.FindControl("hdnPrvInQuan");
                HiddenField hdnPrvIn = (HiddenField)gr.FindControl("hdnPrvIn");

                TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
                HiddenField hdnItemID = (HiddenField)gr.FindControl("hdnItemID");
                HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
                HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");


                //_objPO.ConnConfig = Session["config"].ToString();
                if (!string.IsNullOrEmpty(lblLine.Value) && lblLine.Value != "0")
                {
                    _updatePOItemBalance.POID = Convert.ToInt32(txtPO.Text);
                    _updatePOItemBalance.Line = Convert.ToInt16(lblLine.Value);

                    _updatePOItemQuan.POID = Convert.ToInt32(txtPO.Text);
                    _updatePOItemQuan.Line = Convert.ToInt16(lblLine.Value);

                    _addReceivePOItem.POID = Convert.ToInt32(txtPO.Text);
                    _addReceivePOItem.Line = Convert.ToInt16(lblLine.Value);

                    _updatePOItemWarehouseLocation.POID = Convert.ToInt32(txtPO.Text);
                    _updatePOItemWarehouseLocation.Line = Convert.ToInt16(lblLine.Value);

                    if (!string.IsNullOrEmpty(txtReceive.Text) & !string.IsNullOrEmpty(lblOutstand.Value))
                    {
                        if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                        {

                            var dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Value);
                            var dblReceive = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                            var dblPrvIn = ConvertCurrentCurrencyFormatToDbl(hdnPrvIn.Value);

                            _updatePOItemBalance.Balance = dblOutstand - dblReceive;

                            _updatePOItemBalance.Selected = dblReceive + dblPrvIn;


                            string APINAME2 = "BillAPI/AddBills_UpdatePOItemBalance";

                            APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _updatePOItemBalance);

                            amount = amount + dblReceive;
                        }

                        HiddenField hdnOutstandQuan = (HiddenField)gr.FindControl("hdnOutstandQuan");
                        double dblReceiveQty = 0;
                        if (txtReceiveQty.Text.Trim() != "")
                        {
                            dblReceiveQty = Convert.ToDouble(txtReceiveQty.Text);
                        }


                        var dblPrvInQuan = Convert.ToDouble(hdnPrvInQuan.Value);

                        _updatePOItemQuan.BalanceQuan = Convert.ToDouble(hdnOutstandQuan.Value) - dblReceiveQty;
                        _updatePOItemQuan.SelectedQuan = dblPrvInQuan + dblReceiveQty;


                        string APINAME3 = "BillAPI/AddBills_UpdatePOItemQuan";

                        APIResponse _APIResponse3 = new MOMWebUtility().CallMOMWebAPI(APINAME3, _updatePOItemQuan);


                        qty = qty + dblReceiveQty;

                        _addReceivePOItem.Quan = dblReceiveQty;
                        _addReceivePOItem.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                        _addReceivePOItem.Line = Convert.ToInt16(lblLine.Value);
                        _addReceivePOItem.ReceivePOId = Convert.ToInt32(ID_NEW.ToString());
                        _addReceivePOItem.IsReceiveIssued = 0;//Convert.ToInt32(drpRecievepoIssued.SelectedValue);


                        string APINAME4 = "BillAPI/AddBills_AddReceivePOItem";

                        APIResponse _APIResponse4 = new MOMWebUtility().CallMOMWebAPI(APINAME4, _addReceivePOItem);


                        ////save in IwarehouseLocAdj
                        //General _objPropGeneral = new General();
                        //BL_General _objBLGeneral = new BL_General();

                        //_objPropGeneral.ConnConfig = Session["config"].ToString();
                        //DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
                        //DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
                        //Boolean TrackingInventory = false;
                        //int DefaultAcctID = 0;
                        //if (_dsCustom.Tables[0].Rows.Count > 0)
                        //{
                        //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        //    {
                        //        if (_dr["Name"].ToString().Equals("InvGL"))
                        //        {
                        //            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                        //            {
                        //                TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                        //            }
                        //        }
                        //    }
                        //}

                        //if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
                        //{
                        //    DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                        //}
                        //countItem++;

                        #region Inventory Case
                        if (TrackingInventory == true)
                        {
                            if (txtGvPhase.Text == "Inventory" && hdnItemID.Value != "")
                            {
                                IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();

                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_InvID = int.Parse(hdnItemID.Value);

                                if (string.IsNullOrEmpty(hdnJobID.Value) || hdnJobID.Value == "0")
                                {
                                    _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_WarehouseID = hdnWarehouse.Value;
                                    _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_locationID = int.Parse(hdnWarehouseLocationID.Value == "" ? "0" : hdnWarehouseLocationID.Value);
                                }
                                else
                                {
                                    _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_WarehouseID = hdnWarehouse.Value;
                                    _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_locationID = int.Parse(hdnWarehouseLocationID.Value);

                                    _updatePOItemWarehouseLocation.WarehouseID = hdnWarehouse.Value;
                                    _updatePOItemWarehouseLocation.LocationID = int.Parse(hdnWarehouseLocationID.Value);

                                    string APINAME5 = "BillAPI/AddBills_UpdatePOItemWarehouseLocation";

                                    APIResponse _APIResponse5 = new MOMWebUtility().CallMOMWebAPI(APINAME5, _updatePOItemWarehouseLocation);
                                }

                                //API 
                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Hand = Convert.ToDecimal(txtReceiveQty.Text);
                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Balance = (Decimal)ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDecimal(txtReceive.Text);
                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_fOrder = Convert.ToDecimal(txtReceiveQty.Text) - Convert.ToDecimal(txtReceiveQty.Text);

                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Committed = Convert.ToDecimal(0);
                                //invWarehouseLoc.Committed = Convert.ToDecimal(txtReceiveQty.Text);
                                ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO
                                //invWarehouseLoc.Available = Convert.ToDecimal(0);
                                _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Available = Convert.ToDecimal(txtReceiveQty.Text);
                                ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO

                                //  Trnsaction Hitting for Inventory

                                //API
                                _CreateReceivePOInvWarehouseTrans.ID = 0;
                                _CreateReceivePOInvWarehouseTrans.BatchID = BatchID;
                                _CreateReceivePOInvWarehouseTrans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                                _CreateReceivePOInvWarehouseTrans.AcctSub = int.Parse(hdnItemID.Value);
                                _CreateReceivePOInvWarehouseTrans.Acct = DefaultAcctID;
                                _CreateReceivePOInvWarehouseTrans.Type = 41;
                                _CreateReceivePOInvWarehouseTrans.Line = Convert.ToInt16(countItem);
                                _CreateReceivePOInvWarehouseTrans.strRef = txtRef.Text;
                                _CreateReceivePOInvWarehouseTrans.Status = txtReceiveQty.Text == "" ? "0" : txtReceiveQty.Text;
                                _CreateReceivePOInvWarehouseTrans.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                                _CreateReceivePOInvWarehouseTrans.fDate = Convert.ToDateTime(txtDate.Text);
                                _CreateReceivePOInvWarehouseTrans.fDesc = "Inventory Recieve PO";


                                string APINAME6 = "BillAPI/AddBills_CreateReceivePOInvWarehouse";

                                APIResponse _APIResponse6 = new MOMWebUtility().CallMOMWebAPI(APINAME6, _CreateReceivePOInvWarehouseTrans);

                                BatchID = _CreateReceivePOInvWarehouseTrans.BatchID;

                                Inv_Total = Inv_Total + ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
                                //Inv_Qty = Inv_Qty + Convert.ToInt32(trans.Status);

                                /* ** START REVERT ENTRY FOR PO QTY **  */

                                //API
                                _AddReceiveInventoryWHTrans.ConnConfig = Session["config"].ToString();
                                _AddReceiveInventoryWHTrans.InvID = Convert.ToInt32(hdnItemID.Value);
                                _AddReceiveInventoryWHTrans.WarehouseID = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_WarehouseID;
                                _AddReceiveInventoryWHTrans.LocationID = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_locationID;
                                _AddReceiveInventoryWHTrans.Hand = 0;
                                _AddReceiveInventoryWHTrans.Balance = 0;
                                _AddReceiveInventoryWHTrans.fOrder = Convert.ToDecimal(txtReceiveQty.Text) * -1;
                                _AddReceiveInventoryWHTrans.Committed = 0;
                                _AddReceiveInventoryWHTrans.Available = 0;
                                _AddReceiveInventoryWHTrans.Screen = "APBILL";
                                _AddReceiveInventoryWHTrans.ScreenID = Convert.ToInt32(ID_NEW.ToString());
                                _AddReceiveInventoryWHTrans.Mode = "Add";
                                _AddReceiveInventoryWHTrans.TransType = "Revert";
                                _AddReceiveInventoryWHTrans.Batch = BatchID;


                                string APINAME7 = "BillAPI/AddBills_AddReceiveInventoryWHTrans";

                                APIResponse _APIResponse7 = new MOMWebUtility().CallMOMWebAPI(APINAME7, _AddReceiveInventoryWHTrans);


                                /* ** CLOSE REVERT ENTRY FOR PO QTY **  */

                                /* ** START INVENTORY ENTRY FOR RPO **  */

                                //API
                                _AddReceiveInventoryWHTrans.ConnConfig = Session["config"].ToString();
                                _AddReceiveInventoryWHTrans.InvID = Convert.ToInt32(hdnItemID.Value);
                                _AddReceiveInventoryWHTrans.WarehouseID = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_WarehouseID;
                                _AddReceiveInventoryWHTrans.LocationID = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_locationID;
                                _AddReceiveInventoryWHTrans.Hand = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Hand;
                                _AddReceiveInventoryWHTrans.Balance = _CreateReceivePOInvWarehouseTrans.IWarehouseLocAdj_Balance;
                                _AddReceiveInventoryWHTrans.fOrder = 0;
                                _AddReceiveInventoryWHTrans.Committed = 0;
                                _AddReceiveInventoryWHTrans.Available = 0;
                                _AddReceiveInventoryWHTrans.Screen = "RPO";
                                _AddReceiveInventoryWHTrans.ScreenID = Convert.ToInt32(ID_NEW.ToString());
                                _AddReceiveInventoryWHTrans.Mode = "Add";
                                _AddReceiveInventoryWHTrans.TransType = "In";
                                _AddReceiveInventoryWHTrans.Batch = BatchID;


                                string APINAME8 = "BillAPI/AddBills_AddReceiveInventoryWHTrans";

                                APIResponse _APIResponse8 = new MOMWebUtility().CallMOMWebAPI(APINAME8, _AddReceiveInventoryWHTrans);


                                /* ** END INVENTORY ENTRY FOR RPO **  */
                                countItem++;
                            }
                        }
                        #endregion Inventory Case
                    }
                }
            }
            #endregion

            if (countItem > 0)
            {
                //API
                _ReceivePOInvWarehouseTrans.ID = 0;
                _ReceivePOInvWarehouseTrans.BatchID = BatchID;
                _ReceivePOInvWarehouseTrans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);
                _ReceivePOInvWarehouseTrans.strRef = txtRef.Text;
                _ReceivePOInvWarehouseTrans.Status = "0";
                _ReceivePOInvWarehouseTrans.Amount = Inv_Total;//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
                _ReceivePOInvWarehouseTrans.fDate = Convert.ToDateTime(txtDate.Text);
                _ReceivePOInvWarehouseTrans.fDesc = "Inventory Recieve PO";


                string APINAME9 = "BillAPI/AddBills_CreateReceivePOInvWarehouseTrans";

                APIResponse _APIResponse9 = new MOMWebUtility().CallMOMWebAPI(APINAME9, _ReceivePOInvWarehouseTrans);

            }

            #region Add Reception PO

            //API
            _addEditReceivePO.ConnConfig = Session["config"].ToString();
            _addEditReceivePO.RID = Convert.ToInt32(ID_NEW);
            _addEditReceivePO.POID = Convert.ToInt32(txtPO.Text);
            _addEditReceivePO.Ref = txtRef.Text;
            _addEditReceivePO.WB = null;
            _addEditReceivePO.Comments = txtMemo.Text;
            _addEditReceivePO.Amount = poAmount;
            _addEditReceivePO.fDate = Convert.ToDateTime(txtDate.Text);
            _addEditReceivePO.BatchID = BatchID;
            _addEditReceivePO.Due = Convert.ToDateTime(txtDueDate.Text);
            _addEditReceivePO.MOMUSer = Session["User"].ToString();

            _updateJobComm.ConnConfig = Session["config"].ToString();

            string APINAME10 = "BillAPI/AddBills_AddEditReceivePO";

            APIResponse _APIResponse10 = new MOMWebUtility().CallMOMWebAPI(APINAME10, _addEditReceivePO);


            //_objBLBills.UpdatePODue(_objPO);
            #endregion

            #region Update job costing
            foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            {
                HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                if (!string.IsNullOrEmpty(hdnJobID.Value) && hdnJobID.Value != "0")
                {
                    try
                    {
                        _updateJobComm.jobID = Convert.ToInt32(hdnJobID.Value);

                        string APINAME11 = "BillAPI/AddBills_updateJobComm";

                        APIResponse _APIResponse11 = new MOMWebUtility().CallMOMWebAPI(APINAME11, _updateJobComm);

                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }

                }
            }
            #endregion

            #region Update PO status
            //_objBLBills.AutoUpdatePOStatus(objPO);
            #endregion
        }
        else
        {
            //#region Update PO balance

            ////save in IwarehouseLocAdj
            //General _objPropGeneral = new General();
            //BL_General _objBLGeneral = new BL_General();

            //_objPropGeneral.ConnConfig = Session["config"].ToString();

            //DataSet _dsCustom = new DataSet();

            //_dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");

            //DataSet _dsDefaultAccount = new DataSet();

            //_dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);

            //Boolean TrackingInventory = false;
            //int DefaultAcctID = 0;
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

            //if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
            //{
            //    DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            //}


            DataTable dtRPO = GetRPOItem();
            #region Add Reception PO New
            objPO.ConnConfig = Session["config"].ToString();
            objPO.RID = 0;
            objPO.POID = Convert.ToInt32(txtPO.Text);
            objPO.Ref = txtRef.Text;
            objPO.WB = null;
            objPO.Comments = txtMemo.Text;
            objPO.Amount = amount;
            objPO.fDate = Convert.ToDateTime(txtDate.Text);
            objPO.BatchID = BatchID;
            if (rdbyAmt.Checked == true)
            {
                objPO.IsReceiveIssued = 0;
            }
            else
            {
                objPO.IsReceiveIssued = 1;
            }
            objPO.Due = Convert.ToDateTime(txtDueDate.Text);
            objPO.MOMUSer = Session["User"].ToString();
            objPO.Dt = dtRPO;
            objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);
            objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objPO.IsAddReceivePO = false;
            //int RecivePOId = _objBLBills.AddEditReceivePO(_objPO);
            ID_NEW = _objBLBills.AddRPO(objPO);
            #endregion


            //#region Add Reception PO
            //objPO.ConnConfig = Session["config"].ToString();
            //objPO.RID = 0;
            //objPO.POID = Convert.ToInt32(txtPO.Text);
            //objPO.Ref = txtRef.Text;
            //objPO.WB = null;
            //objPO.Comments = txtMemo.Text;
            //objPO.Amount = amount;
            //objPO.fDate = Convert.ToDateTime(txtDate.Text);
            //objPO.BatchID = BatchID;
            //if (rdbyAmt.Checked == true)
            //{
            //    objPO.IsReceiveIssued = 0;
            //}
            //else
            //{
            //    objPO.IsReceiveIssued = 1;
            //}
            //objPO.Due = Convert.ToDateTime(txtDueDate.Text);
            //objPO.MOMUSer = Session["User"].ToString();
            //int RecivePOId = _objBLBills.AddEditReceivePO(objPO);
            //ID_NEW = RecivePOId;
            //#endregion


            //foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            //{
            //    HiddenField lblOutstand = (HiddenField)gr.FindControl("hdnOutstandBalance");
            //    TextBox lblPrice = (TextBox)gr.FindControl("txtGvPrice");
            //    TextBox txtReceive = (TextBox)gr.FindControl("txtGvAmount");
            //    TextBox txtReceiveQty = (TextBox)gr.FindControl("txtGvQuan");
            //    HiddenField hdnReceive = (HiddenField)gr.FindControl("hdnReceive");
            //    HiddenField hdnReceiveQty = (HiddenField)gr.FindControl("hdnQuantity");
            //    HiddenField lblLine = (HiddenField)gr.FindControl("hdnLine");
            //    HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
            //    HiddenField hdnPrvInQuan = (HiddenField)gr.FindControl("hdnPrvInQuan");
            //    HiddenField hdnPrvIn = (HiddenField)gr.FindControl("hdnPrvIn");

            //    TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
            //    HiddenField hdnItemID = (HiddenField)gr.FindControl("hdnItemID");
            //    HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
            //    HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");
            //    HiddenField hdnOutstandQuan = (HiddenField)gr.FindControl("hdnOutstandQuan");

            //    HiddenField lblOrderedQuan = (HiddenField)gr.FindControl("HdnOrderedQuan");
            //    HiddenField lblOrdered = (HiddenField)gr.FindControl("HdnOrdered");




            //    //_objPO.ConnConfig = Session["config"].ToString();
            //    if (!string.IsNullOrEmpty(lblLine.Value) && lblLine.Value != "0")
            //    {
            //        objPO.POID = Convert.ToInt32(txtPO.Text);
            //        objPO.Line = Convert.ToInt16(lblLine.Value);

            //        double dblOutstand = 0.00;
            //        double dblReceive = 0.00;
            //        double dblPrvIn = 0.00;
            //        double dblReceiveQty = 0;
            //        double dblPrvInQuan = Convert.ToDouble(hdnPrvInQuan.Value);


            //        if (!string.IsNullOrEmpty(Request.Form[txtReceive.UniqueID]) & !string.IsNullOrEmpty(lblOutstand.Value))
            //        {
            //            if (!Convert.ToDouble(Request.Form[txtReceive.UniqueID]).Equals(0))
            //            {
            //                dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Value);
            //                dblReceive = ConvertCurrentCurrencyFormatToDbl(Request.Form[txtReceive.UniqueID]);
            //                dblPrvIn = ConvertCurrentCurrencyFormatToDbl(hdnPrvIn.Value);
            //            }

            //            if (Request.Form[txtReceiveQty.UniqueID].Trim() != "")
            //            {
            //                dblReceiveQty = Convert.ToDouble(Request.Form[txtReceiveQty.UniqueID]);
            //            }
            //            if (dblReceive >= ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn)
            //            {
            //                dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn, 4);
            //                txtReceive.Text = Convert.ToString(dblReceive);

            //                dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan, 4);
            //                txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

            //                dblOutstand = dblReceive;
            //                hdnOutstandQuan.Value = Convert.ToString(dblReceiveQty);
            //            }
            //            if (dblReceiveQty >= Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan)
            //            {
            //                dblReceiveQty = Math.Round(Convert.ToDouble(lblOrderedQuan.Value) - dblPrvInQuan, 4);
            //                txtReceiveQty.Text = Convert.ToString(dblReceiveQty);

            //                dblReceive = Math.Round(ConvertCurrentCurrencyFormatToDbl(lblOrdered.Value) - dblPrvIn, 4);
            //                txtReceive.Text = Convert.ToString(dblReceive);

            //                dblOutstand = dblReceive;
            //                hdnOutstandQuan.Value = Convert.ToString(dblReceiveQty);
            //            }

            //            // Start-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)
            //            if (dblOutstand == 0 && dblReceive == 0)
            //            {
            //                dblOutstand = ConvertCurrentCurrencyFormatToDbl(lblOutstand.Value);
            //                dblPrvIn = ConvertCurrentCurrencyFormatToDbl(hdnPrvIn.Value);
            //            }
            //            // End-- ES-6411 QAE- Major issue on Received PO (As per Laxmi want)

            //            objPO.Balance = dblOutstand - dblReceive;
            //            objPO.Selected = dblReceive + dblPrvIn;
            //            _objBLBills.UpdatePOItemBalance(objPO);
            //            amount = amount + dblReceive;


            //            objPO.BalanceQuan = Convert.ToDouble(hdnOutstandQuan.Value) - dblReceiveQty;
            //            objPO.SelectedQuan = dblPrvInQuan + dblReceiveQty;
            //            _objBLBills.UpdatePOItemQuan(objPO);
            //            qty = qty + dblReceiveQty;

            //            //_objPO.ConnConfig = Session["config"].ToString();
            //            objPO.Quan = dblReceiveQty;
            //            ///objPO.Amount = ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);
            //            //objPO.Amount = ConvertCurrentCurrencyFormatToDbl(Request.Form[txtReceive.UniqueID]);
            //            objPO.Amount = dblReceive;

            //            objPO.Line = Convert.ToInt16(lblLine.Value);
            //            objPO.ReceivePOId = Convert.ToInt32(ID_NEW.ToString());
            //            objPO.IsReceiveIssued = 0;//Convert.ToInt32(drpRecievepoIssued.SelectedValue);

            //            _objBLBills.AddReceivePOItem(objPO);


            //            ////save in IwarehouseLocAdj
            //            //General _objPropGeneral = new General();
            //            //BL_General _objBLGeneral = new BL_General();

            //            //_objPropGeneral.ConnConfig = Session["config"].ToString();
            //            //DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
            //            //DataSet _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
            //            //Boolean TrackingInventory = false;
            //            //int DefaultAcctID = 0;
            //            //if (_dsCustom.Tables[0].Rows.Count > 0)
            //            //{
            //            //    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            //            //    {
            //            //        if (_dr["Name"].ToString().Equals("InvGL"))
            //            //        {
            //            //            if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
            //            //            {
            //            //                TrackingInventory = Convert.ToBoolean(_dr["Label"]);
            //            //            }
            //            //        }
            //            //    }
            //            //}

            //            //if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
            //            //{
            //            //    DefaultAcctID = Convert.ToInt32(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            //            //}
            //            //countItem++;

            //            #region Inventory Case
            //            if (TrackingInventory == true)
            //            {
            //                if (txtGvPhase.Text == "Inventory" && hdnItemID.Value != "")
            //                {
            //                    IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
            //                    invWarehouseLoc.InvID = int.Parse(hdnItemID.Value);
            //                    if (string.IsNullOrEmpty(hdnJobID.Value) || hdnJobID.Value == "0")
            //                    {
            //                        invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
            //                        invWarehouseLoc.locationID = int.Parse(hdnWarehouseLocationID.Value == "" ? "0" : hdnWarehouseLocationID.Value);
            //                    }
            //                    else
            //                    {
            //                        invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
            //                        invWarehouseLoc.locationID = int.Parse(hdnWarehouseLocationID.Value);
            //                        objPO.WarehouseID = hdnWarehouse.Value;
            //                        objPO.LocationID = int.Parse(hdnWarehouseLocationID.Value);

            //                        _objBLBills.UpdatePOItemWarehouseLocation(objPO);

            //                    }

            //                    //invWarehouseLoc.Hand = Convert.ToDecimal(txtReceiveQty.Text);
            //                    //invWarehouseLoc.Balance = (Decimal)ConvertCurrentCurrencyFormatToDbl(txtReceive.Text);//Convert.ToDecimal(txtReceive.Text);
            //                    //invWarehouseLoc.fOrder = Convert.ToDecimal(txtReceiveQty.Text) - Convert.ToDecimal(txtReceiveQty.Text);
            //                    //double dblReceiveQtynew = 0;
            //                    //if (Request.Form[txtReceiveQty.UniqueID].Trim() != "")
            //                    //{
            //                    //    dblReceiveQtynew = Convert.ToDouble(Request.Form[txtReceiveQty.UniqueID]);
            //                    //}
            //                    //var dblReceivenew = ConvertCurrentCurrencyFormatToDbl(Request.Form[txtReceive.UniqueID]);

            //                    invWarehouseLoc.Hand = Convert.ToDecimal(dblReceiveQty);
            //                    invWarehouseLoc.Balance = (Decimal)dblReceive;//Convert.ToDecimal(txtReceive.Text);
            //                    invWarehouseLoc.fOrder = Convert.ToDecimal(dblReceiveQty) - Convert.ToDecimal(dblReceiveQty);

            //                    invWarehouseLoc.Committed = Convert.ToDecimal(0);
            //                    //invWarehouseLoc.Committed = Convert.ToDecimal(txtReceiveQty.Text);
            //                    ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO
            //                    //invWarehouseLoc.Available = Convert.ToDecimal(0);
            //                    //invWarehouseLoc.Available = Convert.ToDecimal(txtReceiveQty.Text);
            //                    invWarehouseLoc.Available = Convert.ToDecimal(dblReceiveQty);
            //                    ///// Commented By Azhar on 08-Mar-2020 Due to ES-3718 Available quantity is not getting increased when user perform Receive PO

            //                    //  Trnsaction Hitting for Inventory
            //                    Transaction trans = new Transaction();
            //                    trans.ID = 0;
            //                    //trans.BatchID = 0;
            //                    trans.BatchID = BatchID;
            //                    trans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtDate.Text);


            //                    trans.AcctSub = int.Parse(hdnItemID.Value);
            //                    trans.Acct = DefaultAcctID;
            //                    trans.Type = 41;
            //                    trans.Line = Convert.ToInt16(countItem);
            //                    trans.strRef = txtRef.Text;
            //                    trans.Status = dblReceiveQty.ToString();
            //                    trans.Amount = dblReceive;//Convert.ToDouble(txtReceive.Text == "" ? "0" : txtReceive.Text);
            //                    trans.fDate = Convert.ToDateTime(txtDate.Text);
            //                    trans.fDesc = "Inventory Recieve PO";

            //                    _objInventory.CreateReceivePOInvWarehouse(invWarehouseLoc, trans);



            //                    BatchID = trans.BatchID;

            //                    Inv_Total = Inv_Total + dblReceive;
            //                    //Inv_Qty = Inv_Qty + Convert.ToInt32(trans.Status);

            //                    /* ** START REVERT ENTRY FOR PO QTY **  */
            //                    InventoryWHTrans _objInventoryWHTrans = new InventoryWHTrans();
            //                    _objInventoryWHTrans.ConnConfig = Session["config"].ToString();
            //                    _objInventoryWHTrans.InvID = Convert.ToInt32(hdnItemID.Value);
            //                    _objInventoryWHTrans.WarehouseID = invWarehouseLoc.WarehouseID;
            //                    _objInventoryWHTrans.LocationID = invWarehouseLoc.locationID;
            //                    _objInventoryWHTrans.Hand = 0;
            //                    _objInventoryWHTrans.Balance = 0;
            //                    _objInventoryWHTrans.fOrder = Convert.ToDecimal(dblReceiveQty) * -1;
            //                    _objInventoryWHTrans.Committed = 0;
            //                    _objInventoryWHTrans.Available = 0;
            //                    //_objInventoryWHTrans.Screen = "APBILL";
            //                    _objInventoryWHTrans.Screen = "RPO";
            //                    _objInventoryWHTrans.ScreenID = Convert.ToInt32(ID_NEW.ToString());
            //                    _objInventoryWHTrans.Mode = "Add";
            //                    _objInventoryWHTrans.TransType = "Revert";
            //                    _objInventoryWHTrans.Batch = BatchID;



            //                    _objBLBills.AddReceiveInventoryWHTrans(_objInventoryWHTrans);


            //                    /* ** CLOSE REVERT ENTRY FOR PO QTY **  */

            //                    /* ** START INVENTORY ENTRY FOR RPO **  */
            //                    InventoryWHTrans obj = new InventoryWHTrans();
            //                    obj.ConnConfig = Session["config"].ToString();
            //                    obj.InvID = Convert.ToInt32(hdnItemID.Value);
            //                    obj.WarehouseID = invWarehouseLoc.WarehouseID;
            //                    obj.LocationID = invWarehouseLoc.locationID;
            //                    obj.Hand = invWarehouseLoc.Hand;
            //                    obj.Balance = invWarehouseLoc.Balance;
            //                    obj.fOrder = 0;
            //                    obj.Committed = 0;
            //                    obj.Available = 0;
            //                    obj.Screen = "RPO";
            //                    obj.ScreenID = Convert.ToInt32(ID_NEW.ToString());
            //                    obj.Mode = "Add";
            //                    obj.TransType = "In";
            //                    obj.Batch = BatchID;


            //                    _objBLBills.AddReceiveInventoryWHTrans(obj);


            //                    /* ** END INVENTORY ENTRY FOR RPO **  */
            //                    countItem++;
            //                }
            //            }
            //            #endregion Inventory Case
            //        }
            //    }
            //}
            //#endregion

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



            //#region Update Reception PO Amount & Batch
            //objPO.ConnConfig = Session["config"].ToString();
            //objPO.RID = Convert.ToInt32(ID_NEW);
            //objPO.POID = Convert.ToInt32(txtPO.Text);
            //objPO.Ref = txtRef.Text;
            //objPO.WB = null;
            //objPO.Comments = txtMemo.Text;
            //objPO.Amount = amount;
            //objPO.fDate = Convert.ToDateTime(txtDate.Text);
            //objPO.BatchID = BatchID;
            //objPO.IsReceiveIssued = 1;
            //objPO.Due = Convert.ToDateTime(txtDueDate.Text);
            //objPO.MOMUSer = Session["User"].ToString();
            //_objBLBills.UpdtReceivePOAmnt(objPO);
            //#endregion







        }
        return ID_NEW;
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
    protected void btnSelectRPo_Click(object sender, EventArgs e)
    {
        try
        {
            //objPO.ConnConfig = Session["config"].ToString();
            //_getPOReceivePOById.ConnConfig = Session["config"].ToString();
            //if (!string.IsNullOrEmpty(txtReceptionId.Text))
            //{
            //    txtVendor.Enabled = false;
            //    //objPO.POID = Convert.ToInt32(txtPO.Text);
            //    //DataSet ds = _objBLBills.GetPOById(objPO);
            //    objPO.RID = Convert.ToInt32(txtReceptionId.Text);
            //    _getPOReceivePOById.RID = Convert.ToInt32(txtReceptionId.Text);

            //    DataSet ds = new DataSet();
            //    DataSet ds1 = new DataSet();
            //    DataSet ds2 = new DataSet();

            //    ListGetPOReceivePOById _lstPOReceivePOById = new ListGetPOReceivePOById();

            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        string APINAME = "BillAPI/AddBills_GetPOReceivePOById";

            //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPOReceivePOById);

            //        _lstPOReceivePOById = (new JavaScriptSerializer()).Deserialize<ListGetPOReceivePOById>(_APIResponse.ResponseData);
            //        ds1 = _lstPOReceivePOById.lstTable1.ToDataSet();
            //        ds2 = _lstPOReceivePOById.lstTable2.ToDataSet();

            //        DataTable dt1 = new DataTable();
            //        DataTable dt2 = new DataTable();

            //        dt1 = ds1.Tables[0];
            //        dt2 = ds2.Tables[0];

            //        dt1.TableName = "Table1";
            //        dt2.TableName = "Table2";

            //        ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            //    }
            //    else
            //    {
            //        ds = _objBLBills.GetPOReceivePOById(objPO);
            //    }

            //    if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow dr = ds.Tables[0].Rows[0];
            //            txtPO.Text = dr["PO"].ToString();
            //            txtVendor.Text = dr["VendorName"].ToString();
            //            hdnVendorID.Value = dr["Vendor"].ToString();
            //            //txtRef.Text = dr["Ref"].ToString();
            //            txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
            //            txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
            //            txtMemo.Text = dr["fDesc"].ToString();
            //            lblTotalAmount.Text = ds.Tables[0].Rows[0]["ReceivedAmount"].ToString();
            //            hdnTotalAmount.Value = dr["Amount"].ToString();
            //            txtDueIn.Text = dr["Term"].ToString();
            //            txtPaid.Text = dr["Days"].ToString();
            //            hdnSTaxState.Value = dr["State"].ToString();
            //            FillSalesTax();

            //            if (!string.IsNullOrEmpty(txtDueIn.Text))
            //            {
            //                int dueIn = Convert.ToInt32(txtDueIn.Text);
            //                txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
            //            }

            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                DataSet dset = new DataSet();
            //                _objVendor.SearchValue = txtVendor.Text;
            //                _objVendor.ConnConfig = Session["config"].ToString();
            //                if (Session["CmpChkDefault"].ToString() == "1")
            //                {
            //                    _objVendor.EN = 1;
            //                }
            //                else
            //                {
            //                    _objVendor.EN = 0;
            //                }

            //                dset = _objBLVendor.GetVendorSearch(_objVendor);

            //                txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
            //                hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
            //                hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
            //                hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
            //                hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

            //                txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
            //                hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
            //                hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
            //                hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
            //                hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

            //                if (hdnSTaxName.Value.Trim() != "")
            //                {
            //                    //ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                    if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
            //                    {
            //                        ddlSTax.SelectedValue = hdnSTaxName.Value;
            //                    }
            //                    else
            //                    {
            //                        ddlSTax.SelectedIndex = 0;
            //                    }
            //                }
            //                else
            //                {
            //                    ddlSTax.SelectedIndex = 0;
            //                }

            //                //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
            //                //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
            //                //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

            //            }

            //            // hdnReceivedAmount.Value = Convert.ToDouble(dr["ReceivedAmount"]).ToString("0.00", CultureInfo.InvariantCulture); 
            //            DataColumn dc = new DataColumn("IsPO");
            //            dc.DataType = typeof(int);
            //            dc.DefaultValue = 0;
            //            ds.Tables[1].Columns.Add(dc);

            //            DataColumn colGtax = new DataColumn("GTax");
            //            colGtax.DataType = typeof(int);
            //            ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
            //            ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
            //            colGtax.DefaultValue = 0;
            //            ds.Tables[1].Columns.Add(colGtax);
            //            ViewState["Transactions_JobCost"] = ds.Tables[1];
            //            BINDGRID(ds.Tables[1]);

            //            //
            //        }

            //}

            //btnUpdtStax_Click(sender, e);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "checkPORPO", "checkPORPO();", true);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);


            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _getPOReceivePOById.ConnConfig = Session["config"].ToString();
                if (!string.IsNullOrEmpty(txtReceptionId.Text))
                {
                    txtVendor.Enabled = false;
                    //objPO.POID = Convert.ToInt32(txtPO.Text);
                    //DataSet ds = _objBLBills.GetPOById(objPO);

                    _getPOReceivePOById.RID = Convert.ToInt32(txtReceptionId.Text);

                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();

                    ListGetPOReceivePOById _lstPOReceivePOById = new ListGetPOReceivePOById();

                    string APINAME = "BillAPI/AddBills_GetPOReceivePOById";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getPOReceivePOById);

                    _lstPOReceivePOById = (new JavaScriptSerializer()).Deserialize<ListGetPOReceivePOById>(_APIResponse.ResponseData);
                    ds1 = _lstPOReceivePOById.lstTable1.ToDataSet();
                    ds2 = _lstPOReceivePOById.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtPO.Text = dr["PO"].ToString();
                        txtVendor.Text = dr["VendorName"].ToString();
                        hdnVendorID.Value = dr["Vendor"].ToString();
                        //txtRef.Text = dr["Ref"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        txtMemo.Text = dr["fDesc"].ToString();
                        lblTotalAmount.Text = ds.Tables[0].Rows[0]["ReceivedAmount"].ToString();
                        hdnTotalAmount.Value = dr["Amount"].ToString();
                        txtDueIn.Text = dr["Term"].ToString();
                        txtPaid.Text = dr["Days"].ToString();
                        hdnSTaxState.Value = dr["State"].ToString();
                        FillSalesTax();

                        if (!string.IsNullOrEmpty(txtDueIn.Text))
                        {
                            int dueIn = Convert.ToInt32(txtDueIn.Text);
                            txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataSet dset = new DataSet();

                            _getVendorSearch.SearchValue = txtVendor.Text;
                            _getVendorSearch.ConnConfig = Session["config"].ToString();
                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                _getVendorSearch.EN = 1;
                            }
                            else
                            {
                                _getVendorSearch.EN = 0;
                            }

                            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();


                            string APINAME1 = "BillAPI/AddBills_GetVendorSearch";

                            APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _getVendorSearch);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse1.ResponseData);
                            dset = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);

                            txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
                            hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
                            hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
                            hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
                            hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

                            txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
                            hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
                            hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
                            hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
                            hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

                            if (hdnSTaxName.Value.Trim() != "")
                            {
                                //ddlSTax.SelectedValue = hdnSTaxName.Value;
                                if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
                                {
                                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                                }
                                else
                                {
                                    ddlSTax.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                ddlSTax.SelectedIndex = 0;
                            }

                            //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
                            //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                            //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

                        }

                        // hdnReceivedAmount.Value = Convert.ToDouble(dr["ReceivedAmount"]).ToString("0.00", CultureInfo.InvariantCulture); 
                        DataColumn dc = new DataColumn("IsPO");
                        dc.DataType = typeof(int);
                        dc.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(dc);

                        DataColumn colGtax = new DataColumn("GTax");
                        colGtax.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        colGtax.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(colGtax);
                        ViewState["Transactions_JobCost"] = ds.Tables[1];
                        BINDGRID(ds.Tables[1]);

                        //
                    }

                }
                chkPOClose.Visible = true;
                btnUpdtStax_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "checkPORPO", "checkPORPO();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
            else
            {
                objPO.ConnConfig = Session["config"].ToString();
                if (!string.IsNullOrEmpty(txtReceptionId.Text))
                {
                    txtVendor.Enabled = false;
                    //objPO.POID = Convert.ToInt32(txtPO.Text);
                    //DataSet ds = _objBLBills.GetPOById(objPO);
                    objPO.RID = Convert.ToInt32(txtReceptionId.Text);

                    DataSet ds = new DataSet();

                    ds = _objBLBills.GetPOReceivePOById(objPO);


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        txtPO.Text = dr["PO"].ToString();
                        txtVendor.Text = dr["VendorName"].ToString();
                        hdnVendorID.Value = dr["Vendor"].ToString();
                        //txtRef.Text = dr["Ref"].ToString();
                        txtDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtPostingDate.Text = Convert.ToDateTime(dr["fDate"]).ToShortDateString();
                        txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToShortDateString();
                        txtMemo.Text = dr["fDesc"].ToString();
                        lblTotalAmount.Text = ds.Tables[0].Rows[0]["ReceivedAmount"].ToString();
                        hdnTotalAmount.Value = dr["Amount"].ToString();
                        txtDueIn.Text = dr["Term"].ToString();
                        txtPaid.Text = dr["Days"].ToString();
                        hdnSTaxState.Value = dr["State"].ToString();
                        FillSalesTax();

                        if (!string.IsNullOrEmpty(txtDueIn.Text))
                        {
                            int dueIn = Convert.ToInt32(txtDueIn.Text);
                            txtDueDate.Text = DateTime.Now.AddDays(dueIn).ToString("MM/dd/yyyy");
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataSet dset = new DataSet();
                            _objVendor.SearchValue = txtVendor.Text;
                            _objVendor.ConnConfig = Session["config"].ToString();
                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                _objVendor.EN = 1;
                            }
                            else
                            {
                                _objVendor.EN = 0;
                            }

                            dset = _objBLVendor.GetVendorSearch(_objVendor);

                            txtqst.Text = dset.Tables[0].Rows[0]["STax"].ToString();
                            hdnQST.Value = dset.Tables[0].Rows[0]["STaxRate"].ToString();
                            hdnQSTGL.Value = dset.Tables[0].Rows[0]["STaxGL"].ToString();
                            hdnSTaxType.Value = dset.Tables[0].Rows[0]["STaxType"].ToString();
                            hdnSTaxName.Value = dset.Tables[0].Rows[0]["STaxName"].ToString();

                            txtusetaxc.Text = dset.Tables[0].Rows[0]["UTax"].ToString();
                            hdnusetaxc.Value = dset.Tables[0].Rows[0]["UTaxRate"].ToString();
                            hdnusetaxcGL.Value = dset.Tables[0].Rows[0]["SUaxGL"].ToString();
                            hdnUTaxType.Value = dset.Tables[0].Rows[0]["UTaxType"].ToString();
                            hdnUTaxName.Value = dset.Tables[0].Rows[0]["UtaxName"].ToString();

                            if (hdnSTaxName.Value.Trim() != "")
                            {
                                //ddlSTax.SelectedValue = hdnSTaxName.Value;
                                if (ddlSTax.Items.FindByValue(hdnSTaxName.Value) != null)
                                {
                                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                                }
                                else
                                {
                                    ddlSTax.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                ddlSTax.SelectedIndex = 0;
                            }

                            //txtgst.Text = dset.Tables[0].Rows[0]["GSTRate"].ToString();
                            //hdnGSTGL.Value = dset.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                            //hdnGST.Value = dset.Tables[0].Rows[0]["GSTRate"].ToString();

                        }

                        // hdnReceivedAmount.Value = Convert.ToDouble(dr["ReceivedAmount"]).ToString("0.00", CultureInfo.InvariantCulture); 
                        DataColumn dc = new DataColumn("IsPO");
                        dc.DataType = typeof(int);
                        dc.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(dc);

                        DataColumn colGtax = new DataColumn("GTax");
                        colGtax.DataType = typeof(int);
                        ////// Before here dc.DefaultValue = 0; for disbale Qty and Delete when inventory implemetaion changes for TEI
                        ////// now for ES-4203 AP Bill: Quantity should be disable when it is coming from RPO and not from PO on bill screen
                        colGtax.DefaultValue = 0;
                        ds.Tables[1].Columns.Add(colGtax);
                        ViewState["Transactions_JobCost"] = ds.Tables[1];
                        BINDGRID(ds.Tables[1]);

                        //
                    }

                }
                chkPOClose.Visible = true;
                btnUpdtStax_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "checkPORPO", "checkPORPO();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region Add New lines
    protected void lbtnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnVendorID.Value != "")
            {
                int rowIndex;
                //Job cost grid view
                rowIndex = RadGrid_gvJobCostItems.Items.Count - 1;
                if (RadGrid_gvJobCostItems.Items.Count > 0)
                {
                    GridDataItem row = RadGrid_gvJobCostItems.Items[rowIndex];
                    Label lblJId = row.FindControl("lblId") as Label;
                }
                DataTable dt = GetCurrentTransaction();
                //if (dt.Rows.Count > 0)
                //{
                DataRow dr = dt.NewRow();


                dr["UseTax"] = txtTotalUseTax.Text;
                if (!string.IsNullOrEmpty(husetaxGL.Value))
                {
                    dr["UtaxGL"] = husetaxGL.Value;
                }
                if (!string.IsNullOrEmpty(husetaxName.Value))
                {
                    dr["UtaxName"] = husetaxName.Value;
                    dr["UName"] = husetaxName.Value;
                }
                //dr["STaxName"] = "";
                //dr["STaxRate"] = "0";
                dr["StaxAmt"] = "0";
                //dr["STaxGL"] = "0";
                dr["GSTRate"] = "0";
                dr["GTaxAmt"] = "0";
                dr["GSTTaxGL"] = "0";
                dr["AmountTot"] = "0";

                if (hdnQST.Value != "")
                {
                    dr["STaxRate"] = Convert.ToDouble(hdnQST.Value);
                }
                if (hdnQSTGL.Value != "")
                {
                    dr["STaxGL"] = Convert.ToInt32(hdnQSTGL.Value);
                }
                if (hdnSTaxName.Value != "")
                {
                    dr["STaxName"] = hdnSTaxName.Value;
                }



                dr["STax"] = 0;
                // dr["AmountTot"] = 0;
                dr["IsPO"] = 1;
                dr["GTax"] = 0;

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                //}
                //else
                //{
                //    dt = (DataTable)ViewState["Transactions_JobCost"];
                //}

                ViewState["Transactions_JobCost"] = dt;

                //DataColumnCollection columns = dt.Columns;
                //if (!columns.Contains("AmountTot"))
                //{
                //    //dt.Columns.Add("AmountTot", typeof(double), "STaxAmt + GTaxAmt + Amount");
                //    //dt.AcceptChanges();

                //    dt.Columns.Remove("AmountTot");
                //}

                BINDGRID(dt);

                //Focus last row
                GridDataItem lastRow = RadGrid_gvJobCostItems.Items[RadGrid_gvJobCostItems.Items.Count - 1];
                TextBox txtGvJob = (TextBox)lastRow.FindControl("txtGvJob");
                if (txtGvJob != null)
                {
                    txtGvJob.Focus();
                }

                //TextBox txtGvQuan = (TextBox)lastRow.FindControl("txtGvQuan");
                //TextBox txtGvAmount = (TextBox)lastRow.FindControl("txtGvAmount");
                //TextBox txtGvPrice = (TextBox)lastRow.FindControl("txtGvPrice");
                //txtGvQuan.ReadOnly = false;
                //txtGvAmount.ReadOnly = false;
                //txtGvPrice.ReadOnly = false;

            }
            else
            {
                string str = "Please select vendor first";
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Please select vendor first.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

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
                var selectItem = RadGrid_gvJobCostItems.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }

            var dt = GetCurrentTransaction();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];

                dr["ID"] = copyRow["ID"];
                dr["AcctID"] = copyRow["AcctID"];
                dr["fDesc"] = copyRow["fDesc"];
                dr["Amount"] = copyRow["Amount"];
                dr["Usetax"] = copyRow["Usetax"];
                dr["UtaxName"] = copyRow["UtaxName"];
                dr["JobID"] = copyRow["JobID"];
                dr["PhaseID"] = copyRow["PhaseID"];
                dr["ItemID"] = copyRow["ItemID"];

                dr["AcctNo"] = copyRow["AcctNo"];
                dr["JobName"] = copyRow["JobName"];
                dr["Phase"] = copyRow["Phase"];
                dr["UName"] = copyRow["UName"];
                dr["UtaxGL"] = copyRow["UtaxGL"];
                dr["ItemDesc"] = copyRow["ItemDesc"];
                dr["TypeID"] = copyRow["TypeID"];
                dr["Loc"] = copyRow["Loc"];
                dr["TypeDesc"] = copyRow["TypeDesc"];
                dr["Quan"] = copyRow["Quan"];
                dr["Ticket"] = copyRow["Ticket"];
                dr["OpSq"] = copyRow["OpSq"];

                dr["Warehouse"] = copyRow["Warehouse"];
                dr["WHLocID"] = copyRow["WHLocID"];

                dr["Line"] = copyRow["Line"];
                dr["PrvInQuan"] = copyRow["PrvInQuan"];
                dr["PrvIn"] = copyRow["PrvIn"];
                dr["OutstandQuan"] = copyRow["OutstandQuan"];
                dr["OutstandBalance"] = copyRow["OutstandBalance"];

                dr["STax"] = copyRow["STax"];
                dr["STaxName"] = copyRow["STaxName"];
                dr["STaxName"] = copyRow["STaxName"];
                dr["STaxRate"] = copyRow["STaxRate"];
                dr["StaxAmt"] = copyRow["StaxAmt"];
                dr["STaxGL"] = copyRow["STaxGL"];
                dr["GSTRate"] = copyRow["GSTRate"];
                dr["GTaxAmt"] = copyRow["GTaxAmt"];
                dr["GSTTaxGL"] = copyRow["GSTTaxGL"];
                dr["AmountTot"] = copyRow["AmountTot"];

                dr["Warehousefdesc"] = copyRow["Warehousefdesc"];
                dr["Locationfdesc"] = copyRow["Locationfdesc"];
                //dr["IsPO"] = copyRow["IsPO"];
                dr["IsPO"] = "1";
                dr["GTax"] = copyRow["GTax"];
                dr["Price"] = copyRow["Price"];

                dr["RPOItemId"] = "0";
                dr["POItemId"] = "0";
                dt.AcceptChanges();

                ViewState["Transactions_JobCost"] = dt;
                BINDGRID(dt);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalUseTaxExpense", "CalculateTotalUseTaxExpense();", true);

                //Focus row
                GridDataItem focusRow = RadGrid_gvJobCostItems.Items[selectIndex];
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
    #endregion

    #endregion

    #region Custom Functions
    private void GetDate()
    {
        if (Request.QueryString["t"] != null)
        {
            if (Request.QueryString["t"].ToString() == "c")
            {
                btnSubmitJob.Visible = false;
                lblHeader.Text = "Enter Bills";
                pnlNext.Visible = false;
                //lnkQuickCheck.Visible = true;
            }

        }
        else
        {
            lblHeader.Text = "Edit Bills";
            pnlNext.Visible = true;
            //btnSubmitJob.Visible = true;
            //lnkQuickCheck.Visible = false;
            chkIsRecurr.Enabled = false;

        }


        //pnlNext.Style["display"] = "none";
        txtQty.Text = "0.00";
        txtBudgetUnit.Text = "0.00";
        lblBudgetExt.Text = "0.00";

        lblTotalAmount.Text = "0.00";
        lblTotalUseTax.Text = "0.00";
        //FillVendor();
        FillAPStatus();

        //_objPJ.ConnConfig = Session["config"].ToString();
        //_objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

        //_GetPJDetailByID.ConnConfig = Session["config"].ToString();
        //_GetPJDetailByID.ID = Convert.ToInt32(Request.QueryString["id"]);

        //DataSet _dsPJ = new DataSet();
        //List<GetPJDetailByIDViewModel> _lstGetPJDetailByID = new List<GetPJDetailByIDViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetPJDetailByID";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetPJDetailByID);

        //    _lstGetPJDetailByID = (new JavaScriptSerializer()).Deserialize<List<GetPJDetailByIDViewModel>>(_APIResponse.ResponseData);
        //    _dsPJ = CommonMethods.ToDataSet<GetPJDetailByIDViewModel>(_lstGetPJDetailByID);
        //}
        //else
        //{
        //    _dsPJ = _objBLBills.GetPJDetailByID(_objPJ);
        //    //Get AP bill details from PJ table By ID.
        //}

        //if (_dsPJ.Tables[0].Rows.Count > 0)
        //{
        //    DataRow _drPJ = _dsPJ.Tables[0].Rows[0];

        //    txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
        //    txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
        //    txtRef.Text = _drPJ["Ref"].ToString();
        //    txtMemo.Text = _drPJ["fDesc"].ToString();
        //    lblTotalAmount.Text = _drPJ["Amount"].ToString();
        //    //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
        //    txtVendor.Text = _drPJ["VendorName"].ToString();
        //    hdnVendorID.Value = _drPJ["VendorID"].ToString();

        //    txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
        //    hdnQST.Value = _drPJ["STaxRate"].ToString();
        //    hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
        //    hdnSTaxType.Value = _drPJ["STaxType"].ToString();
        //    hdnSTaxName.Value = _drPJ["STaxName"].ToString();

        //    hdnSTaxState.Value = _drPJ["State"].ToString();
        //    FillSalesTax();
        //    if (hdnSTaxName.Value.Trim() != "")
        //    {
        //        ddlSTax.SelectedValue = hdnSTaxName.Value;
        //    }
        //    else
        //    {
        //        ddlSTax.SelectedIndex = 0;
        //    }
        //    SetHarmonizedTax();
        //    ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

        //    ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
        //    txtDisc.Text = _drPJ["Disc"].ToString();
        //    txtDueIn.Text = _drPJ["Terms"].ToString();
        //    txtPO.Text = _drPJ["PO"].ToString();
        //    if (txtPO.Text.Trim() !="")
        //    {
        //        txtVendor.Enabled = false;
        //    }
        //    else
        //    {
        //        txtVendor.Enabled = true;
        //    }

        //    txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
        //    //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
        //    txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
        //    txtCustom1.Text = _drPJ["Custom1"].ToString();
        //    txtCustom2.Text = _drPJ["Custom2"].ToString();
        //    hdnBatch.Value = _drPJ["Batch"].ToString();
        //    hdnTransID.Value = _drPJ["TRID"].ToString();
        //    hdnStatus.Value = _drPJ["Status"].ToString();
        //    _objPJ.ConnConfig = Session["config"].ToString();
        //    _objPJ.Batch = Convert.ToInt32(_drPJ["Batch"]);
        //    hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
        //    hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
        //    txtPaid.Text = _drPJ["IfPaid"].ToString();
        //    //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

        //    _GetBillTransDetails.ConnConfig = Session["config"].ToString();
        //    _GetBillTransDetails.Batch = Convert.ToInt32(_drPJ["Batch"]);

        //    DataSet _dsTrans = new DataSet();

        //    List<GetBillTransDetailsViewModel> _lstGetBillTrans = new List<GetBillTransDetailsViewModel>();

        //    if (IsAPIIntegrationEnable == "YES")
        //    //if (Session["APAPIEnable"].ToString() == "YES")
        //    {
        //        string APINAME = "BillAPI/AddBills_GetBillTransDetails";

        //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillTransDetails);

        //        _lstGetBillTrans = (new JavaScriptSerializer()).Deserialize<List<GetBillTransDetailsViewModel>>(_APIResponse.ResponseData);
        //        _dsTrans = CommonMethods.ToDataSet<GetBillTransDetailsViewModel>(_lstGetBillTrans);
        //    }
        //    else
        //    {
        //        _dsTrans = _objBLBills.GetBillTransDetails(_objPJ);
        //    }


        //    if (_dsTrans.Tables[0].Rows.Count > 0)
        //    {
        //        //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
        //        //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
        //        //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
        //        //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
        //        //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

        //        txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
        //        hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
        //        hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
        //        hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
        //        hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

        //        //txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
        //        //hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
        //        //hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

        //    }
        //    SetDatatableData(_dsTrans);
        //    SetHarmonizedTax();
        //    //  one can also check paid status from trans table trans.sel where Type = 40. 
        //    if (Request.QueryString["t"] != null)
        //    {
        //        if (Request.QueryString["t"].ToString() == "c")
        //        {
        //            isCopy = true;
        //        }
        //    }

        //    if (Convert.ToInt16(_drPJ["Status"]).Equals(1) || Convert.ToInt16(_drPJ["Status"]).Equals(3)) //  check if bill is paid or not
        //    {
        //        if (isCopy == true) //  check if bill is paid or not
        //        {                                               //  Status can be "Status : Void/Open/Closed"
        //            imgPaid.Visible = false;
        //            //btnSubmit.Visible = true;
        //        }//  Status can be "Status : Void/Open/Closed"
        //        else
        //        {
        //            if (!Convert.IsDBNull(_drPJ["Paid"]))
        //            {
        //                if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
        //                {
        //                    imgPaid.Visible = false;
        //                    btnSubmitJob.Visible = false;
        //                }
        //                else
        //                {
        //                    imgPaid.Visible = true;
        //                    btnSubmitJob.Visible = true;
        //                    lnkQuickCheck.Visible = false;
        //                    //liHistoryPayment.Style["display"] = "inline-block";
        //                    //tblPayment.Style["display"] = "block";
        //                    if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
        //                    {
        //                        imgPaid.ImageUrl = "~/images/icons/ppaid.png";
        //                    }
        //                }
        //            }
        //            else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
        //            {
        //                if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
        //                {
        //                    imgPaid.Visible = false;
        //                    btnSubmitJob.Visible = false;
        //                }
        //                else
        //                {
        //                    imgPaid.Visible = true;
        //                    btnSubmitJob.Visible = true;
        //                    lnkQuickCheck.Visible = false;
        //                    //liHistoryPayment.Style["display"] = "inline-block";
        //                    //tblPayment.Style["display"] = "block";
        //                    if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
        //                    {
        //                        imgPaid.ImageUrl = "~/images/icons/ppaid.png";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                imgPaid.Visible = true;
        //                btnSubmitJob.Visible = true;
        //                lnkQuickCheck.Visible = false;
        //                //liHistoryPayment.Style["display"] = "inline-block";
        //                //tblPayment.Style["display"] = "block";
        //                if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
        //                {
        //                    imgPaid.ImageUrl = "~/images/icons/ppaid.png";
        //                }

        //            }
        //            btnSubmit.Visible = false;
        //        }
        //    }
        //    else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
        //    {
        //        if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
        //        {                                               //  Status can be "Status : Void/Open/Closed"
        //            imgVoid.Visible = false;
        //            btnSubmit.Visible = true;
        //            btnSubmitJob.Visible = false;
        //        }//  Status can be "Status : Void/Open/Closed"
        //        else
        //        {
        //            imgVoid.Visible = true;
        //            btnSubmit.Visible = false;
        //            btnSubmitJob.Visible = true;
        //        }

        //    }

        //}
        //else
        //{
        //    SetBillForm();
        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {

            _GetPJDetailByID.ConnConfig = Session["config"].ToString();
            _GetPJDetailByID.ID = Convert.ToInt32(Request.QueryString["id"]);

            DataSet _dsPJ = new DataSet();
            List<GetPJDetailByIDViewModel> _lstGetPJDetailByID = new List<GetPJDetailByIDViewModel>();

            string APINAME = "BillAPI/AddBills_GetPJDetailByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetPJDetailByID);

            _lstGetPJDetailByID = (new JavaScriptSerializer()).Deserialize<List<GetPJDetailByIDViewModel>>(_APIResponse.ResponseData);
            _dsPJ = CommonMethods.ToDataSet<GetPJDetailByIDViewModel>(_lstGetPJDetailByID);

            if (_dsPJ.Tables[0].Rows.Count > 0)
            {
                DataRow _drPJ = _dsPJ.Tables[0].Rows[0];

                txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
                txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
                txtRef.Text = _drPJ["Ref"].ToString();
                txtMemo.Text = _drPJ["fDesc"].ToString();
                lblTotalAmount.Text = _drPJ["Amount"].ToString();
                //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
                txtVendor.Text = _drPJ["VendorName"].ToString();
                hdnVendorID.Value = _drPJ["VendorID"].ToString();

                txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
                hdnQST.Value = _drPJ["STaxRate"].ToString();
                hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
                hdnSTaxType.Value = _drPJ["STaxType"].ToString();
                hdnSTaxName.Value = _drPJ["STaxName"].ToString();

                hdnSTaxState.Value = _drPJ["State"].ToString();
                FillSalesTax();
                if (hdnSTaxName.Value.Trim() != "")
                {
                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                }
                else
                {
                    ddlSTax.SelectedIndex = 0;
                }
                SetHarmonizedTax();
                ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

                ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
                txtDisc.Text = _drPJ["Disc"].ToString();
                txtDueIn.Text = _drPJ["Terms"].ToString();
                txtPO.Text = _drPJ["PO"].ToString();
                lblVendorName.Text = _drPJ["Ref"].ToString() + " | " + _drPJ["VendorName"].ToString();
                lblVendorName.Visible = true;
                if (txtPO.Text.Trim() != "")
                {
                    if (Convert.ToInt32(txtPO.Text.Trim()) > 0)
                    {
                        txtVendor.Enabled = false;
                        chkPOClose.Visible = true;
                    }
                    else
                    {
                        txtVendor.Enabled = true;
                        chkPOClose.Visible = false;
                    }
                }
                else
                {
                    txtVendor.Enabled = true;
                    chkPOClose.Visible = false;
                }

                txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
                //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
                txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
                txtCustom1.Text = _drPJ["Custom1"].ToString();
                txtCustom2.Text = _drPJ["Custom2"].ToString();
                hdnBatch.Value = _drPJ["Batch"].ToString();
                hdnTransID.Value = _drPJ["TRID"].ToString();
                hdnStatus.Value = _drPJ["Status"].ToString();

                hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
                hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
                txtPaid.Text = _drPJ["IfPaid"].ToString();
                //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

                _GetBillTransDetails.ConnConfig = Session["config"].ToString();
                _GetBillTransDetails.Batch = Convert.ToInt32(_drPJ["Batch"]);

                DataSet _dsTrans = new DataSet();

                List<GetBillTransDetailsViewModel> _lstGetBillTrans = new List<GetBillTransDetailsViewModel>();

                string APINAME1 = "BillAPI/AddBills_GetBillTransDetails";

                APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetBillTransDetails);

                _lstGetBillTrans = (new JavaScriptSerializer()).Deserialize<List<GetBillTransDetailsViewModel>>(_APIResponse1.ResponseData);
                _dsTrans = CommonMethods.ToDataSet<GetBillTransDetailsViewModel>(_lstGetBillTrans);

                if (_dsTrans.Tables[0].Rows.Count > 0)
                {
                    //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
                    //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
                    //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

                    txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
                    hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
                    hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

                    //txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
                    //hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                    //hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

                }
                SetDatatableData(_dsTrans);
                SetHarmonizedTax();
                //  one can also check paid status from trans table trans.sel where Type = 40. 
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        isCopy = true;
                    }
                }

                if (Convert.ToInt16(_drPJ["Status"]).Equals(1) || Convert.ToInt16(_drPJ["Status"]).Equals(3)) //  check if bill is paid or not
                {
                    if (isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgPaid.Visible = false;
                        //btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        if (!Convert.IsDBNull(_drPJ["Paid"]))
                        {
                            if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                                btnSubmitJob.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                btnSubmitJob.Visible = true;
                                lnkQuickCheck.Visible = false;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";
                                if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                                {
                                    imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                                }
                            }
                        }
                        else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
                        {
                            if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                                btnSubmitJob.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                btnSubmitJob.Visible = true;
                                lnkQuickCheck.Visible = false;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";
                                if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                                {
                                    imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                                }
                            }
                        }
                        else
                        {
                            imgPaid.Visible = true;
                            btnSubmitJob.Visible = true;
                            lnkQuickCheck.Visible = false;
                            //liHistoryPayment.Style["display"] = "inline-block";
                            //tblPayment.Style["display"] = "block";
                            if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                            {
                                imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                            }

                        }
                        btnSubmit.Visible = false;
                    }
                }
                else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
                {
                    if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgVoid.Visible = false;
                        btnSubmit.Visible = true;
                        btnSubmitJob.Visible = false;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        imgVoid.Visible = true;
                        btnSubmit.Visible = false;
                        btnSubmitJob.Visible = true;
                    }

                }

            }
            else
            {
                SetBillForm();
            }
        }
        else
        {
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

            DataSet _dsPJ = new DataSet();

            _dsPJ = _objBLBills.GetPJDetailByID(_objPJ);
            //Get AP bill details from PJ table By ID.

            if (_dsPJ.Tables[0].Rows.Count > 0)
            {
                DataRow _drPJ = _dsPJ.Tables[0].Rows[0];

                txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
                txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
                txtRef.Text = _drPJ["Ref"].ToString();
                txtMemo.Text = _drPJ["fDesc"].ToString();
                lblTotalAmount.Text = _drPJ["Amount"].ToString();
                //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
                txtVendor.Text = _drPJ["VendorName"].ToString();
                txtVendorType.Text = _drPJ["VendorType"].ToString();
                hdnVendorID.Value = _drPJ["VendorID"].ToString();

                txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
                hdnQST.Value = _drPJ["STaxRate"].ToString();
                hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
                hdnSTaxType.Value = _drPJ["STaxType"].ToString();
                hdnSTaxName.Value = _drPJ["STaxName"].ToString();

                hdnSTaxState.Value = _drPJ["State"].ToString();
                FillSalesTax();
                if (hdnSTaxName.Value.Trim() != "")
                {
                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                }
                else
                {
                    ddlSTax.SelectedIndex = 0;
                }
                SetHarmonizedTax();
                ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

                ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
                txtDisc.Text = _drPJ["Disc"].ToString();
                txtDueIn.Text = _drPJ["Terms"].ToString();
                txtPO.Text = _drPJ["PO"].ToString();
                lblVendorName.Text = _drPJ["Ref"].ToString() + " | " + _drPJ["VendorName"].ToString();
                lblVendorName.Visible = true;
                if (txtPO.Text.Trim() != "")
                {
                    if (Convert.ToInt32(txtPO.Text.Trim()) > 0)
                    {
                        txtVendor.Enabled = false;
                        chkPOClose.Visible = true;
                    }
                    else
                    {
                        txtVendor.Enabled = true;
                        chkPOClose.Visible = false;
                    }
                }
                else
                {
                    txtVendor.Enabled = true;
                    chkPOClose.Visible = false;
                }

                txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
                //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
                txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
                txtCustom1.Text = _drPJ["Custom1"].ToString();
                txtCustom2.Text = _drPJ["Custom2"].ToString();
                hdnBatch.Value = _drPJ["Batch"].ToString();
                hdnTransID.Value = _drPJ["TRID"].ToString();
                hdnStatus.Value = _drPJ["Status"].ToString();
                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.Batch = Convert.ToInt32(_drPJ["Batch"]);
                hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
                hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
                txtPaid.Text = _drPJ["IfPaid"].ToString();
                //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

                DataSet _dsTrans = new DataSet();

                _dsTrans = _objBLBills.GetBillTransDetails(_objPJ);

                if (_dsTrans.Tables[0].Rows.Count > 0)
                {
                    //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
                    //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
                    //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

                    txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
                    hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
                    hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

                    //txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
                    //hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                    //hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

                }
                SetDatatableData(_dsTrans);
                SetHarmonizedTax();
                //  one can also check paid status from trans table trans.sel where Type = 40. 
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        isCopy = true;
                    }
                }

                if (Convert.ToInt16(_drPJ["Status"]).Equals(1) || Convert.ToInt16(_drPJ["Status"]).Equals(3)) //  check if bill is paid or not
                {
                    if (isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgPaid.Visible = false;
                        //btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        if (!Convert.IsDBNull(_drPJ["Paid"]))
                        {
                            if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                                btnSubmitJob.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                btnSubmitJob.Visible = true;
                                lnkQuickCheck.Visible = false;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";
                                if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                                {
                                    imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                                }
                            }
                        }
                        else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
                        {
                            if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                                btnSubmitJob.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                btnSubmitJob.Visible = true;
                                lnkQuickCheck.Visible = false;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";
                                if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                                {
                                    imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                                }
                            }
                        }
                        else
                        {
                            imgPaid.Visible = true;
                            btnSubmitJob.Visible = true;
                            lnkQuickCheck.Visible = false;
                            //liHistoryPayment.Style["display"] = "inline-block";
                            //tblPayment.Style["display"] = "block";
                            if (Convert.ToDouble(_drPJ["Status"]).Equals(3))
                            {
                                imgPaid.ImageUrl = "~/images/icons/ppaid.png";
                            }

                        }
                        btnSubmit.Visible = false;
                    }
                }
                else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
                {
                    if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgVoid.Visible = false;
                        btnSubmit.Visible = true;
                        btnSubmitJob.Visible = false;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        imgVoid.Visible = true;
                        btnSubmit.Visible = false;
                        btnSubmitJob.Visible = true;
                    }

                }

            }
            else
            {
                SetBillForm();
            }
        }
    }
    private void GetDateRecurr()
    {
        if (Request.QueryString["t"] != null)
        {
            if (Request.QueryString["t"].ToString() == "c")
            {
                btnSubmitJob.Visible = false;
                lblHeader.Text = "Enter Recurring";
                pnlNext.Visible = false;
            }

        }
        else
        {
            lblHeader.Text = "Edit Recurring Bills";
            pnlNext.Visible = true;
            //btnSubmitJob.Visible = true;
        }


        //pnlNext.Style["display"] = "none";
        txtQty.Text = "0.00";
        txtBudgetUnit.Text = "0.00";
        lblBudgetExt.Text = "0.00";

        lblTotalAmount.Text = "0.00";
        lblTotalUseTax.Text = "0.00";
        //FillVendor();
        FillAPStatus();


        //_objPJ.ConnConfig = Session["config"].ToString();
        //_objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

        //_GetPJRecurrDetailByID.ConnConfig = Session["config"].ToString();
        //_GetPJRecurrDetailByID.ID = Convert.ToInt32(Request.QueryString["id"]);

        //DataSet _dsPJ = new DataSet();
        //List <PJViewModel> _lstPJViewModel = new List<PJViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetPJRecurrDetailByID";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetPJRecurrDetailByID);

        //    _lstPJViewModel = (new JavaScriptSerializer()).Deserialize<List<PJViewModel>>(_APIResponse.ResponseData);
        //     _dsPJ = CommonMethods.ToDataSet<PJViewModel>(_lstPJViewModel);
        //}
        //else
        //{
        //    _dsPJ = _objBLBills.GetPJRecurrDetailByID(_objPJ);        //Get AP bill details from PJ table By ID.
        //}

        //if (_dsPJ.Tables[0].Rows.Count > 0)
        //{
        //    DataRow _drPJ = _dsPJ.Tables[0].Rows[0];
        //    lnkQuickCheck.Visible = false;
        //    txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
        //    txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
        //    txtRef.Text = _drPJ["Ref"].ToString();
        //    txtMemo.Text = _drPJ["fDesc"].ToString();
        //    lblTotalAmount.Text = _drPJ["Amount"].ToString();
        //    //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
        //    txtVendor.Text = _drPJ["VendorName"].ToString();
        //    hdnVendorID.Value = _drPJ["VendorID"].ToString();

        //    txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
        //    hdnQST.Value = _drPJ["STaxRate"].ToString();
        //    hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
        //    hdnSTaxType.Value = _drPJ["STaxType"].ToString();
        //    hdnSTaxName.Value = _drPJ["STaxName"].ToString();

        //    hdnSTaxState.Value = _drPJ["State"].ToString();
        //    FillSalesTax();
        //    if (hdnSTaxName.Value.Trim() != "")
        //    {
        //        ddlSTax.SelectedValue = hdnSTaxName.Value;
        //    }
        //    else
        //    {
        //        ddlSTax.SelectedIndex = 0;
        //    }
        //    SetHarmonizedTax();
        //    ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

        //    ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
        //    txtDisc.Text = _drPJ["Disc"].ToString();
        //    txtDueIn.Text = _drPJ["Terms"].ToString();
        //    txtPO.Text = _drPJ["PO"].ToString();
        //    txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
        //    //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
        //    txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
        //    txtCustom1.Text = _drPJ["Custom1"].ToString();
        //    txtCustom2.Text = _drPJ["Custom2"].ToString();
        //    hdnBatch.Value = _drPJ["Batch"].ToString();
        //    hdnTransID.Value = _drPJ["TRID"].ToString();
        //    hdnStatus.Value = _drPJ["Status"].ToString();
        //    _objPJ.ConnConfig = Session["config"].ToString();
        //    _objPJ.Batch = Convert.ToInt32(Request.QueryString["id"]);
        //    hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
        //    hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
        //    txtPaid.Text = _drPJ["IfPaid"].ToString();
        //    chkIsRecurr.Checked = true;
        //    ddlFrequency.SelectedValue = _drPJ["Batch"].ToString(); 
        //    //chkIsRecurr.Attributes.Add("onclick", "return false;");
        //    if (Convert.ToInt32(_drPJ["ReqBy"]) >0)
        //    {
        //        chkIsRecurr.Enabled = false;                
        //    }

        //    //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

        //    _GetBillRecurrTransactions.ConnConfig = Session["config"].ToString();
        //    _GetBillRecurrTransactions.Batch = Convert.ToInt32(Request.QueryString["id"]);

        //    DataSet _dsTrans = new DataSet();
        //    List<GetBillRecurrTransactionsViewModel> _lstGetBillRecurrTrans = new List<GetBillRecurrTransactionsViewModel>();

        //    if (IsAPIIntegrationEnable == "YES")
        //    //if (Session["APAPIEnable"].ToString() == "YES")
        //    {
        //        string APINAME = "BillAPI/AddBills_GetBillRecurrTransactions";

        //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillRecurrTransactions);

        //        _lstGetBillRecurrTrans = (new JavaScriptSerializer()).Deserialize<List<GetBillRecurrTransactionsViewModel>>(_APIResponse.ResponseData);
        //        _dsTrans = CommonMethods.ToDataSet<GetBillRecurrTransactionsViewModel>(_lstGetBillRecurrTrans);
        //    }
        //    else
        //    {
        //        _dsTrans = _objBLBills.GetBillRecurrTransactions(_objPJ);
        //    }

        //    if (_dsTrans.Tables[0].Rows.Count > 0)
        //    {
        //        //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
        //        //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
        //        //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
        //        //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
        //        //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

        //        txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
        //        hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
        //        hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
        //        hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
        //        hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

        //        txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
        //        hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
        //        hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

        //    }


        //    SetDatatableData(_dsTrans);
        //    //  one can also check paid status from trans table trans.sel where Type = 40. 
        //    if (Request.QueryString["t"] != null)
        //    {
        //        if (Request.QueryString["t"].ToString() == "c")
        //        {
        //            isCopy = true;
        //        }
        //    }

        //    if (Convert.ToInt16(_drPJ["Status"]).Equals(1)) //  check if bill is paid or not
        //    {
        //        if (isCopy == true) //  check if bill is paid or not
        //        {                                               //  Status can be "Status : Void/Open/Closed"
        //            imgPaid.Visible = false;
        //            btnSubmit.Visible = true;
        //        }//  Status can be "Status : Void/Open/Closed"
        //        else
        //        {
        //            if (!Convert.IsDBNull(_drPJ["Paid"]))
        //            {
        //                if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
        //                {
        //                    imgPaid.Visible = false;
        //                }
        //                else
        //                {
        //                    imgPaid.Visible = true;
        //                    //liHistoryPayment.Style["display"] = "inline-block";
        //                    //tblPayment.Style["display"] = "block";

        //                }
        //            }
        //            else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
        //            {
        //                if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
        //                {
        //                    imgPaid.Visible = false;
        //                }
        //                else
        //                {
        //                    imgPaid.Visible = true;
        //                    //liHistoryPayment.Style["display"] = "inline-block";
        //                    //tblPayment.Style["display"] = "block";

        //                }
        //            }
        //            else
        //            {
        //                imgPaid.Visible = true;
        //                //liHistoryPayment.Style["display"] = "inline-block";
        //                //tblPayment.Style["display"] = "block";

        //            }
        //            btnSubmit.Visible = false;
        //        }
        //    }
        //    else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
        //    {
        //        if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
        //        {                                               //  Status can be "Status : Void/Open/Closed"
        //            imgVoid.Visible = false;
        //            btnSubmit.Visible = true;
        //        }//  Status can be "Status : Void/Open/Closed"
        //        else
        //        {
        //            imgVoid.Visible = true;
        //            btnSubmit.Visible = false;
        //        }

        //    }

        //}
        //else
        //{
        //    SetBillForm();
        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _GetPJRecurrDetailByID.ConnConfig = Session["config"].ToString();
            _GetPJRecurrDetailByID.ID = Convert.ToInt32(Request.QueryString["id"]);

            DataSet _dsPJ = new DataSet();
            List<GetPJRecurrDetailByIDViewModel> _lstGetPJRecurrDetailByID = new List<GetPJRecurrDetailByIDViewModel>();

            string APINAME = "BillAPI/AddBills_GetPJRecurrDetailByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetPJRecurrDetailByID);

            _lstGetPJRecurrDetailByID = (new JavaScriptSerializer()).Deserialize<List<GetPJRecurrDetailByIDViewModel>>(_APIResponse.ResponseData);
            _dsPJ = CommonMethods.ToDataSet<GetPJRecurrDetailByIDViewModel>(_lstGetPJRecurrDetailByID);


            if (_dsPJ.Tables[0].Rows.Count > 0)
            {
                DataRow _drPJ = _dsPJ.Tables[0].Rows[0];
                lnkQuickCheck.Visible = false;
                txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
                txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
                txtRef.Text = _drPJ["Ref"].ToString();
                txtMemo.Text = _drPJ["fDesc"].ToString();
                lblTotalAmount.Text = _drPJ["Amount"].ToString();
                //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
                txtVendor.Text = _drPJ["VendorName"].ToString();
                hdnVendorID.Value = _drPJ["VendorID"].ToString();

                txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
                hdnQST.Value = _drPJ["STaxRate"].ToString();
                hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
                hdnSTaxType.Value = _drPJ["STaxType"].ToString();
                hdnSTaxName.Value = _drPJ["STaxName"].ToString();

                hdnSTaxState.Value = _drPJ["State"].ToString();
                FillSalesTax();
                if (hdnSTaxName.Value.Trim() != "")
                {
                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                }
                else
                {
                    ddlSTax.SelectedIndex = 0;
                }
                SetHarmonizedTax();
                ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

                ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
                txtDisc.Text = _drPJ["Disc"].ToString();
                txtDueIn.Text = _drPJ["Terms"].ToString();
                txtPO.Text = _drPJ["PO"].ToString();
                txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
                //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
                txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
                txtCustom1.Text = _drPJ["Custom1"].ToString();
                txtCustom2.Text = _drPJ["Custom2"].ToString();
                hdnBatch.Value = _drPJ["Batch"].ToString();
                hdnTransID.Value = _drPJ["TRID"].ToString();
                hdnStatus.Value = _drPJ["Status"].ToString();

                hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
                hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
                txtPaid.Text = _drPJ["IfPaid"].ToString();
                chkIsRecurr.Checked = true;
                ddlFrequency.SelectedValue = _drPJ["Batch"].ToString();
                //chkIsRecurr.Attributes.Add("onclick", "return false;");
                if (Convert.ToInt32(_drPJ["ReqBy"]) > 0)
                {
                    chkIsRecurr.Enabled = false;
                }

                //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

                _GetBillRecurrTransactions.ConnConfig = Session["config"].ToString();
                _GetBillRecurrTransactions.Batch = Convert.ToInt32(Request.QueryString["id"]);

                DataSet _dsTrans = new DataSet();
                List<GetBillRecurrTransactionsViewModel> _lstGetBillRecurrTrans = new List<GetBillRecurrTransactionsViewModel>();

                string APINAME1 = "BillAPI/AddBills_GetBillRecurrTransactions";

                APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _GetBillRecurrTransactions);

                _lstGetBillRecurrTrans = (new JavaScriptSerializer()).Deserialize<List<GetBillRecurrTransactionsViewModel>>(_APIResponse1.ResponseData);
                _dsTrans = CommonMethods.ToDataSet<GetBillRecurrTransactionsViewModel>(_lstGetBillRecurrTrans);


                if (_dsTrans.Tables[0].Rows.Count > 0)
                {
                    //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
                    //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
                    //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

                    txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
                    hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
                    hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

                    txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
                    hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                    hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

                }


                SetDatatableData(_dsTrans);
                //  one can also check paid status from trans table trans.sel where Type = 40. 
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        isCopy = true;
                    }
                }

                if (Convert.ToInt16(_drPJ["Status"]).Equals(1)) //  check if bill is paid or not
                {
                    if (isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgPaid.Visible = false;
                        btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        if (!Convert.IsDBNull(_drPJ["Paid"]))
                        {
                            if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";

                            }
                        }
                        else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
                        {
                            if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";

                            }
                        }
                        else
                        {
                            imgPaid.Visible = true;
                            //liHistoryPayment.Style["display"] = "inline-block";
                            //tblPayment.Style["display"] = "block";

                        }
                        btnSubmit.Visible = false;
                    }
                }
                else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
                {
                    if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgVoid.Visible = false;
                        btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        imgVoid.Visible = true;
                        btnSubmit.Visible = false;
                    }

                }

            }
            else
            {
                SetBillForm();
            }
        }
        else
        {
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

            DataSet _dsPJ = new DataSet();

            _dsPJ = _objBLBills.GetPJRecurrDetailByID(_objPJ);        //Get AP bill details from PJ table By ID.

            if (_dsPJ.Tables[0].Rows.Count > 0)
            {
                DataRow _drPJ = _dsPJ.Tables[0].Rows[0];
                lnkQuickCheck.Visible = false;
                txtReceptionId.Text = _drPJ["ReceivePO"].ToString();
                txtPostingDate.Text = Convert.ToDateTime(_drPJ["fDate"]).ToString("MM/dd/yyyy");
                txtRef.Text = _drPJ["Ref"].ToString();
                txtMemo.Text = _drPJ["fDesc"].ToString();
                lblTotalAmount.Text = _drPJ["Amount"].ToString();
                //ddlVendor.SelectedValue = _drPJ["Vendor"].ToString();
                txtVendor.Text = _drPJ["VendorName"].ToString();
                txtVendorType.Text = _drPJ["VendorType"].ToString();
                hdnVendorID.Value = _drPJ["VendorID"].ToString();

                txtqst.Text = _drPJ["STaxName"].ToString() + " " + _drPJ["STaxRate"].ToString();
                hdnQST.Value = _drPJ["STaxRate"].ToString();
                hdnQSTGL.Value = _drPJ["STaxGL"].ToString();
                hdnSTaxType.Value = _drPJ["STaxType"].ToString();
                hdnSTaxName.Value = _drPJ["STaxName"].ToString();

                hdnSTaxState.Value = _drPJ["State"].ToString();
                FillSalesTax();
                if (hdnSTaxName.Value.Trim() != "")
                {
                    ddlSTax.SelectedValue = hdnSTaxName.Value;
                }
                else
                {
                    ddlSTax.SelectedIndex = 0;
                }
                SetHarmonizedTax();
                ddlSTax.SelectedValue = _drPJ["STaxName"].ToString();

                ddlStatus.SelectedValue = _drPJ["Spec"].ToString();
                txtDisc.Text = _drPJ["Disc"].ToString();
                txtDueIn.Text = _drPJ["Terms"].ToString();
                txtPO.Text = _drPJ["PO"].ToString();
                txtDate.Text = Convert.ToDateTime(_drPJ["IDate"]).ToString("MM/dd/yyyy");
                //txtDueDate.Text = DateTime.Now.AddDays(Convert.ToInt16(_drPJ["Terms"])).ToString("MM/dd/yyyy");
                txtDueDate.Text = Convert.ToDateTime(_drPJ["Due"]).ToShortDateString();
                txtCustom1.Text = _drPJ["Custom1"].ToString();
                txtCustom2.Text = _drPJ["Custom2"].ToString();
                hdnBatch.Value = _drPJ["Batch"].ToString();
                hdnTransID.Value = _drPJ["TRID"].ToString();
                hdnStatus.Value = _drPJ["Status"].ToString();
                _objPJ.ConnConfig = Session["config"].ToString();
                _objPJ.Batch = Convert.ToInt32(Request.QueryString["id"]);
                hdnReceivedAmount.Value = _drPJ["ReceivedAmount"].ToString();
                hdnTotalAmount.Value = _drPJ["POAmount"].ToString();
                txtPaid.Text = _drPJ["IfPaid"].ToString();
                chkIsRecurr.Checked = true;
                ddlFrequency.SelectedValue = _drPJ["Batch"].ToString();
                //chkIsRecurr.Attributes.Add("onclick", "return false;");
                if (Convert.ToInt32(_drPJ["ReqBy"]) > 0)
                {
                    chkIsRecurr.Enabled = false;
                }

                //DataSet _dsTrans = _objBLJournal.GetBillsTransByBatch(_objTrans);       //Get bill transactions details by Type = 41 from trans table

                DataSet _dsTrans = new DataSet();

                _dsTrans = _objBLBills.GetBillRecurrTransactions(_objPJ);

                if (_dsTrans.Tables[0].Rows.Count > 0)
                {
                    //txtqst.Text = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQST.Value = _dsTrans.Tables[0].Rows[0]["STaxRate"].ToString();
                    //hdnQSTGL.Value = _dsTrans.Tables[0].Rows[0]["STaxGL"].ToString();
                    //hdnSTaxType.Value = _dsTrans.Tables[0].Rows[0]["STaxType"].ToString();
                    //hdnSTaxName.Value = _dsTrans.Tables[0].Rows[0]["STaxName"].ToString();

                    txtusetaxc.Text = _dsTrans.Tables[0].Rows[0]["UName"].ToString() + " " + _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxc.Value = _dsTrans.Tables[0].Rows[0]["UseTax"].ToString();
                    hdnusetaxcGL.Value = _dsTrans.Tables[0].Rows[0]["UtaxGL"].ToString();
                    hdnUTaxType.Value = _dsTrans.Tables[0].Rows[0]["UTaxType"].ToString();
                    hdnUTaxName.Value = _dsTrans.Tables[0].Rows[0]["UName"].ToString();

                    txtgst.Text = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();
                    hdnGSTGL.Value = _dsTrans.Tables[0].Rows[0]["GSTTaxGL"].ToString();
                    hdnGST.Value = _dsTrans.Tables[0].Rows[0]["GSTRate"].ToString();

                }


                SetDatatableData(_dsTrans);
                //  one can also check paid status from trans table trans.sel where Type = 40. 
                if (Request.QueryString["t"] != null)
                {
                    if (Request.QueryString["t"].ToString() == "c")
                    {
                        isCopy = true;
                    }
                }

                if (Convert.ToInt16(_drPJ["Status"]).Equals(1)) //  check if bill is paid or not
                {
                    if (isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgPaid.Visible = false;
                        btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        if (!Convert.IsDBNull(_drPJ["Paid"]))
                        {
                            if (Convert.ToDouble(_drPJ["Paid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";

                            }
                        }
                        else if (!Convert.IsDBNull(_drPJ["CrPaid"]))
                        {
                            if (Convert.ToDouble(_drPJ["CrPaid"]).Equals(0))
                            {
                                imgPaid.Visible = false;
                            }
                            else
                            {
                                imgPaid.Visible = true;
                                //liHistoryPayment.Style["display"] = "inline-block";
                                //tblPayment.Style["display"] = "block";

                            }
                        }
                        else
                        {
                            imgPaid.Visible = true;
                            //liHistoryPayment.Style["display"] = "inline-block";
                            //tblPayment.Style["display"] = "block";

                        }
                        btnSubmit.Visible = false;
                    }
                }
                else if (Convert.ToInt16(_drPJ["Status"]).Equals(2)) // check if bill is voided or not
                {
                    if (Convert.ToInt16(_drPJ["Status"]).Equals(2) && isCopy == true) //  check if bill is paid or not
                    {                                               //  Status can be "Status : Void/Open/Closed"
                        imgVoid.Visible = false;
                        btnSubmit.Visible = true;
                    }//  Status can be "Status : Void/Open/Closed"
                    else
                    {
                        imgVoid.Visible = true;
                        btnSubmit.Visible = false;
                    }

                }

            }
            else
            {
                SetBillForm();
            }
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

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }

    private DataTable GetTransaction()
    {
        DataTable dt = GetTable();

        try
        {
            string strItems = hdnGLItem.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnAcctID"].ToString().Trim() == string.Empty || dict["hdnAcctID"].ToString() == "0")
                    {
                        continue;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    //dr["Line"] = i;
                    dr["AcctID"] = dict["hdnAcctID"].ToString().Trim();
                    //if (dict["hdnQuantity"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["Quan"] = dict["hdnQuantity"].ToString();
                    //}

                    if (dict["txtGvQuan"].ToString().Trim() != string.Empty)
                    {
                        dr["Quan"] = dict["txtGvQuan"].ToString();
                    }

                    dr["Ticket"] = !string.IsNullOrEmpty(dict["txtGvTicket"].ToString()) ? dict["txtGvTicket"].ToString() : "0";
                    if (dict["hdnTID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = dict["hdnTID"].ToString();
                    }
                    if (dict["txtGvDesc"].ToString().Trim() != string.Empty)
                    {
                        dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    }
                    if (dict["txtGvAmount"].ToString().Trim() != string.Empty)
                    {
                        dr["Amount"] = dict["txtGvAmount"].ToString();
                    }
                    //if (dict["txtGvUseTax"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["Usetax"] = dict["txtGvUseTax"].ToString();
                    //}
                    //if (dict["hdnUtax"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["UtaxName"] = dict["hdnUtax"].ToString();
                    //}
                    //dr["UName"] = dict["hdnUtax"].ToString();
                    //if (dict["hdnUtaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["UtaxGL"] = dict["hdnUtaxGL"].ToString();
                    //}
                    //else
                    //{
                    //    dr["UtaxGL"] = DBNull.Value;
                    //}

                    if (dict.ContainsKey("txtGvUseTax"))
                    {
                        if (dict["txtGvUseTax"].ToString().Trim() != string.Empty)
                        {
                            dr["Usetax"] = dict["txtGvUseTax"].ToString();
                        }
                        else
                        {
                            dr["Usetax"] = 0;
                        }
                    }
                    else
                    {
                        dr["Usetax"] = 0;
                    }

                    if (dict.ContainsKey("hdnUtax"))
                    {
                        if (dict["hdnUtax"].ToString().Trim() != string.Empty)
                        {
                            dr["UtaxName"] = dict["hdnUtax"].ToString();
                            dr["UName"] = dict["hdnUtax"].ToString();
                        }
                        else
                        {
                            dr["UtaxName"] = DBNull.Value;
                            dr["UName"] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dr["UtaxName"] = DBNull.Value;
                        dr["UName"] = DBNull.Value;
                    }
                    if (dict.ContainsKey("hdnUtaxGL"))
                    {
                        if (dict["hdnUtaxGL"].ToString().Trim() != string.Empty)
                        {
                            dr["UtaxGL"] = dict["hdnUtaxGL"].ToString();
                        }
                        else
                        {
                            dr["UtaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["UtaxGL"] = 0;
                    }


                    if (dict["hdnJobID"].ToString().Trim() != string.Empty)
                    {
                        dr["JobID"] = dict["hdnJobID"].ToString();
                    }
                    dr["JobName"] = dict["txtGvJob"].ToString();
                    if (dict["hdnPID"].ToString().Trim() != string.Empty)
                    {
                        double temp = Convert.ToDouble(dict["hdnPID"]);
                        dr["PhaseID"] = Convert.ToDouble(dict["hdnPID"]);
                    }
                    else
                    {
                        dr["PhaseID"] = 0;
                    }
                    if (dict["hdnItemID"].ToString().Trim() != string.Empty)
                    {
                        dr["ItemID"] = dict["hdnItemID"].ToString();
                    }
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString();

                    dr["Loc"] = dict["txtGvLoc"].ToString();
                    dr["Phase"] = dict["txtGvPhase"].ToString();
                    if (dict["hdnTypeId"].ToString().Trim() != string.Empty)
                    {
                        dr["TypeID"] = dict["hdnTypeId"].ToString();
                    }
                    dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    if (!(dict["hdOpSq"].ToString().Trim() == string.Empty))
                    {
                        dr["OpSq"] = dict["hdOpSq"].ToString();
                    }
                    else
                    {
                        dr["OpSq"] = "100";
                    }
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                        dr["Line"] = dict["hdnLine"].ToString();
                    if (dict["hdnPrvInQuan"].ToString().Trim() != string.Empty)
                        dr["PrvInQuan"] = Convert.ToDouble(dict["hdnPrvInQuan"]);
                    if (dict["hdnPrvIn"].ToString().Trim() != string.Empty)
                        dr["PrvIn"] = Convert.ToDouble(dict["hdnPrvIn"]);
                    if (dict["hdnOutstandQuan"].ToString().Trim() != string.Empty)
                        dr["OutstandQuan"] = Convert.ToDouble(dict["hdnOutstandQuan"]);
                    if (dict["hdnOutstandBalance"].ToString().Trim() != string.Empty)
                        dr["OutstandBalance"] = Convert.ToDouble(dict["hdnOutstandBalance"]);

                    //BL_Inventory DL_INV = new BL_Inventory();
                    //if (DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString())) { dr["Warehouse"] = "22"; }  //Temporally for A4 Access We set default warehouse 22 , Then later we will need to allow user to select warehouse and location
                    if (dict.ContainsKey("hdnWarehouse") && dict.ContainsKey("txtGvWarehouse"))
                    {
                        if (dict["hdnWarehouse"].ToString().Trim() != string.Empty && dict["txtGvWarehouse"].ToString().Trim() != string.Empty)
                        {
                            dr["Warehouse"] = dict["hdnWarehouse"].ToString();
                        }
                        else
                        {
                            dr["Warehouse"] = "";
                        }
                    }
                    else
                    {
                        dr["Warehouse"] = "";
                    }
                    if (dict.ContainsKey("hdnWarehousefdesc"))
                    {
                        if (dict["hdnWarehousefdesc"].ToString().Trim() != string.Empty)
                        {
                            dr["Warehousefdesc"] = Convert.ToString(dict["hdnWarehousefdesc"]);
                        }
                        else
                        {
                            dr["Warehousefdesc"] = "";
                        }
                    }
                    else
                    {
                        dr["Warehousefdesc"] = "";
                    }

                    //if (dict["hdnWarehousefdesc"].ToString().Trim() != string.Empty)
                    //    dr["Warehousefdesc"] = Convert.ToString(dict["hdnWarehousefdesc"]);
                    if (dict.ContainsKey("hdnWarehouseLocationID"))
                    {
                        if (dict["hdnWarehouseLocationID"].ToString().Trim() != string.Empty)
                        {
                            dr["WHLocID"] = dict["hdnWarehouseLocationID"].ToString();
                        }
                        else
                        {
                            dr["WHLocID"] = "0";
                        }
                    }
                    else
                    {
                        dr["WHLocID"] = "0";
                    }



                    if (dict.ContainsKey("hdnchkTaxable"))
                    {
                        if (dict["hdnchkTaxable"].ToString().Trim() != string.Empty)
                        {

                            dr["stax"] = Convert.ToInt32(dict["hdnchkTaxable"]);
                        }

                    }
                    else
                    {
                        dr["stax"] = "0";
                    }
                    if (dict.ContainsKey("txtGvStaxAmount"))
                    {
                        dr["STaxName"] = hdnSTaxName.Value.ToString();
                        dr["STaxRate"] = Convert.ToDecimal(hdnQST.Value);
                        ////dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                        //if (dict["hdnSTaxAm"].ToString().Trim() != string.Empty)
                        //{
                        //    dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                        //}
                        //else
                        //{
                        //    dr["STaxAmt"] = 0;
                        //}
                        if (dict["txtGvStaxAmount"].ToString().Trim() != string.Empty)
                        {
                            dr["STaxAmt"] = dict["txtGvStaxAmount"].ToString();
                        }
                        else
                        {
                            dr["STaxAmt"] = 0;
                        }
                    }
                    else
                    {
                        dr["STaxName"] = "";
                        dr["STaxRate"] = 0;
                        dr["STaxAmt"] = 0;
                    }
                    if (dict.ContainsKey("hdnSTaxGL"))
                    {
                        //dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                        if (dict["hdnSTaxGL"].ToString().Trim() != string.Empty)
                        {
                            dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                        }
                        else
                        {
                            dr["STaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["STaxGL"] = 0;
                    }

                    if (dict.ContainsKey("hdnGSTTaxAm"))
                    {
                        dr["GSTRate"] = Convert.ToDecimal(hdnGST.Value);
                        //dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                        if (dict["hdnGSTTaxAm"].ToString().Trim() != string.Empty)
                        {
                            dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                        }
                        else
                        {
                            dr["GTaxAmt"] = 0;
                        }
                    }
                    else
                    {
                        dr["GSTRate"] = 0;
                        dr["GTaxAmt"] = 0;
                    }
                    if (dict.ContainsKey("hdnGSTTaxGL"))
                    {
                        //dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                        if (dict["hdnGSTTaxGL"].ToString().Trim() != string.Empty && dict["hdnGSTTaxGL"].ToString().Trim() != "NaN")
                        {
                            dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                        }
                        else
                        {
                            dr["GSTTaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["GSTTaxGL"] = 0;
                    }
                    //return test[myKey];
                    //if (dict["hdnSTaxAm"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                    //}
                    //if (dict["hdnSTaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                    //}
                    //if (dict["hdnGSTTaxAm"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                    //}
                    //if (dict["hdnGSTTaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                    //}

                    if (dict.ContainsKey("hdnIsPO"))
                    {
                        //dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                        if (dict["hdnIsPO"].ToString().Trim() != string.Empty && dict["hdnIsPO"].ToString().Trim() != "NaN")
                        {
                            dr["IsPO"] = dict["hdnIsPO"].ToString();
                        }
                        else
                        {
                            dr["IsPO"] = 1;
                        }
                    }
                    else
                    {
                        dr["IsPO"] = 1;
                    }
                    if (dict.ContainsKey("hdnchkGTaxable"))
                    {
                        if (dict["hdnchkGTaxable"].ToString().Trim() != string.Empty)
                        {

                            dr["GTax"] = Convert.ToInt32(dict["hdnchkGTaxable"]);
                        }

                    }
                    else
                    {
                        dr["GTax"] = "0";
                    }
                    if (dict["txtGvPrice"].ToString().Trim() != string.Empty)
                    {
                        dr["Price"] = dict["txtGvPrice"].ToString();
                    }
                    if (dict["hdnOrderedQuan"].ToString().Trim() != string.Empty)
                        dr["OrderedQuan"] = Convert.ToDouble(dict["hdnOrderedQuan"]);
                    if (dict["hdnOrdered"].ToString().Trim() != string.Empty)
                        dr["Ordered"] = Convert.ToDouble(dict["hdnOrdered"]);
                    if (!(dict["hdnRPOItemId"].ToString().Trim() == string.Empty))
                    {
                        dr["RPOItemId"] = dict["hdnRPOItemId"].ToString();
                    }
                    else
                    {
                        dr["RPOItemId"] = "0";
                    }
                    if (!(dict["hdnPOItemId"].ToString().Trim() == string.Empty))
                    {
                        dr["POItemId"] = dict["hdnPOItemId"].ToString();
                    }
                    else
                    {
                        dr["POItemId"] = "0";
                    }
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

    private DataTable GetCurrentTransaction()
    {
        DataTable dt = GetTable();

        try
        {
            BL_Inventory DL_INV = new BL_Inventory();


            bool inv = false;

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "BillAPI/AddBills_ISINVENTORYTRACKINGISON";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, Session["config"].ToString());
                inv = Convert.ToBoolean(_APIResponse.ResponseData);
            }
            else
            {
                inv = DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString());
            }

            foreach (GridDataItem row in RadGrid_gvJobCostItems.Items)
            {
                HiddenField hdnTID = (HiddenField)row.FindControl("hdnTID");
                HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                TextBox txtGvUseTax = (TextBox)row.FindControl("txtGvUseTax");
                HiddenField hdnUtax = (HiddenField)row.FindControl("hdnUtax");
                Label lblTID = (Label)row.FindControl("lblTID");

                TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                TextBox txtGvTicket = (TextBox)row.FindControl("txtGvTicket");
                HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");
                HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdnQuantity = (HiddenField)row.FindControl("hdnQuantity");
                TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                HiddenField hdnLine = (HiddenField)row.FindControl("hdnLine");
                HiddenField hdnPrvInQuan = (HiddenField)row.FindControl("hdnPrvInQuan");
                HiddenField hdnPrvIn = (HiddenField)row.FindControl("hdnPrvIn");
                HiddenField hdnOutstandQuan = (HiddenField)row.FindControl("hdnOutstandQuan");
                HiddenField hdnOutstandBalance = (HiddenField)row.FindControl("hdnOutstandBalance");
                HiddenField hdnUtaxGL = (HiddenField)row.FindControl("hdnUtaxGL");
                HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                HiddenField hdOpSq = (HiddenField)row.FindControl("hdOpSq");

                HiddenField hdnchkTaxable = (HiddenField)row.FindControl("hdnchkTaxable");
                HiddenField hdnSTaxAm = (HiddenField)row.FindControl("hdnSTaxAm");
                HiddenField hdnSTaxGL = (HiddenField)row.FindControl("hdnSTaxGL");
                HiddenField hdnGSTTaxAm = (HiddenField)row.FindControl("hdnGSTTaxAm");
                HiddenField hdnGSTTaxGL = (HiddenField)row.FindControl("hdnGSTTaxGL");
                Label lblAmountWithTax = (Label)row.FindControl("lblAmountWithTax");
                TextBox lblSalesTax = (TextBox)row.FindControl("lblSalesTax");
                TextBox lblGstTax = (TextBox)row.FindControl("lblGstTax");
                CheckBox chkTaxable = (CheckBox)row.FindControl("chkTaxable");
                HiddenField hdnAmountWithTax = (HiddenField)row.FindControl("hdnAmountWithTax");

                TextBox txtGvWarehouse = (TextBox)row.FindControl("txtGvWarehouse");
                HiddenField hdnWarehouse = (HiddenField)row.FindControl("hdnWarehouse");
                TextBox txtGvWarehouseLocation = (TextBox)row.FindControl("txtGvWarehouseLocation");
                HiddenField hdnWarehouseLocationID = (HiddenField)row.FindControl("hdnWarehouseLocationID");

                HiddenField hdnWarehousefdesc = (HiddenField)row.FindControl("hdnWarehousefdesc");
                HiddenField hdnLocationfdesc = (HiddenField)row.FindControl("hdnLocationfdesc");
                HiddenField hdnIsPO = (HiddenField)row.FindControl("hdnIsPO");
                HiddenField hdnchkGTaxable = (HiddenField)row.FindControl("hdnchkGTaxable");
                CheckBox chkGTaxable = (CheckBox)row.FindControl("chkGTaxable");
                TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");

                HiddenField hdnOrderedQuan = (HiddenField)row.FindControl("hdnOrderedQuan");
                HiddenField hdnOrdered = (HiddenField)row.FindControl("hdnOrdered");

                HiddenField hdnRPOItemId = (HiddenField)row.FindControl("hdnRPOItemId");
                HiddenField hdnPOItemId = (HiddenField)row.FindControl("hdnPOItemId");

                var dr = dt.NewRow();
                if (!string.IsNullOrEmpty(hdnTID.Value))
                {
                    dr["ID"] = hdnTID.Value;
                }
                if (!string.IsNullOrEmpty(hdnAcctID.Value))
                {
                    dr["AcctID"] = hdnAcctID.Value;
                }
                if (!string.IsNullOrEmpty(txtGvAmount.Text))
                {
                    //dr["Amount"] = txtGvAmount.Text;
                    dr["Amount"] = Request.Form[txtGvAmount.UniqueID];
                }
                if (!string.IsNullOrEmpty(txtGvUseTax.Text))
                {
                    dr["Usetax"] = txtGvUseTax.Text;
                }
                if (!string.IsNullOrEmpty(hdnJobID.Value))
                {
                    dr["JobID"] = hdnJobID.Value;
                }
                if (!string.IsNullOrEmpty(hdnPID.Value))
                {
                    dr["PhaseID"] = hdnPID.Value;
                }
                if (!string.IsNullOrEmpty(hdnItemID.Value))
                {
                    dr["ItemID"] = hdnItemID.Value;
                }
                if (!string.IsNullOrEmpty(hdnUtaxGL.Value))
                {
                    dr["UtaxGL"] = hdnUtaxGL.Value;
                }
                if (!string.IsNullOrEmpty(hdnTypeId.Value))
                {
                    dr["TypeID"] = hdnTypeId.Value;
                }
                if (!string.IsNullOrEmpty(txtGvQuan.Text))
                {
                    //dr["Quan"] = txtGvQuan.Text;
                    dr["Quan"] = Request.Form[txtGvQuan.UniqueID];
                }
                if (!string.IsNullOrEmpty(txtGvTicket.Text))
                {
                    dr["Ticket"] = txtGvTicket.Text;
                }

                dr["fDesc"] = txtGvDesc.Text;
                dr["UtaxName"] = hdnUtax.Value;
                dr["AcctNo"] = txtGvAcctNo.Text;
                dr["JobName"] = txtGvJob.Text;
                dr["Phase"] = txtGvPhase.Text;
                dr["UName"] = hdnUtax.Value;
                dr["ItemDesc"] = txtGvItem.Text;
                dr["Loc"] = txtGvLoc.Text;
                dr["TypeDesc"] = "";
                dr["OpSq"] = hdOpSq.Value;

                if (!string.IsNullOrEmpty(hdnLine.Value))
                {
                    dr["Line"] = hdnLine.Value;
                }
                if (!string.IsNullOrEmpty(hdnPrvInQuan.Value))
                {
                    dr["PrvInQuan"] = hdnPrvInQuan.Value;
                }
                if (!string.IsNullOrEmpty(hdnPrvIn.Value))
                {
                    dr["PrvIn"] = hdnPrvIn.Value;
                }
                if (!string.IsNullOrEmpty(hdnOutstandQuan.Value))
                {
                    dr["OutstandQuan"] = hdnOutstandQuan.Value;
                }
                if (!string.IsNullOrEmpty(hdnOutstandBalance.Value))
                {
                    dr["OutstandBalance"] = hdnOutstandBalance.Value;
                }


                //if (inv)
                //{
                //    dr["Warehouse"] = "22";
                //}
                //else
                //{
                //    dr["Warehouse"] = "";
                //}
                //dr["WHLocID"] = "0";
                if (inv)
                {
                    dr["Warehousefdesc"] = hdnWarehousefdesc.Value;
                    dr["Warehouse"] = hdnWarehouse.Value;
                    dr["Locationfdesc"] = hdnLocationfdesc.Value;
                    if (!string.IsNullOrEmpty(hdnWarehouseLocationID.Value))
                    {
                        dr["WHLocID"] = hdnWarehouseLocationID.Value;
                    }
                    else
                    {
                        dr["WHLocID"] = "0";
                    }
                }
                else
                {
                    dr["Warehousefdesc"] = "";
                    dr["Warehouse"] = "";
                    dr["Locationfdesc"] = "";
                    dr["WHLocID"] = "0";
                }


                if (chkTaxable.Checked == true)
                {
                    dr["stax"] = "1";
                }
                else
                {
                    dr["stax"] = "0";
                }
                //dr["stax"] = Convert.ToInt32(hdnchkTaxable.Value);


                if (!string.IsNullOrEmpty(hdnSTaxAm.Value))
                {
                    dr["STaxName"] = hdnSTaxName.Value.ToString();
                    dr["STaxRate"] = Convert.ToDecimal(hdnQST.Value);
                    dr["STaxAmt"] = hdnSTaxAm.Value.ToString();
                }
                else
                {
                    dr["STaxName"] = "";
                    dr["STaxRate"] = DBNull.Value;
                    dr["STaxAmt"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnSTaxGL.Value))
                {
                    dr["STaxGL"] = hdnSTaxGL.Value;
                }
                else
                {
                    dr["STaxGL"] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(hdnGSTTaxAm.Value))
                {
                    dr["GSTRate"] = Convert.ToDecimal(hdnGST.Value);
                    dr["GTaxAmt"] = hdnGSTTaxAm.Value;
                }
                else
                {
                    dr["GSTRate"] = DBNull.Value;
                    dr["GTaxAmt"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnGSTTaxGL.Value))
                {
                    dr["GSTTaxGL"] = hdnGSTTaxGL.Value;
                }
                else
                {
                    dr["GSTTaxGL"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnAmountWithTax.Value))
                {
                    dr["AmountTot"] = hdnAmountWithTax.Value;
                }
                else
                {
                    dr["AmountTot"] = "0.00";
                }

                if (!string.IsNullOrEmpty(hdnIsPO.Value))
                {
                    dr["IsPO"] = hdnIsPO.Value;
                }
                else
                {
                    dr["IsPO"] = "1";
                }
                if (chkGTaxable.Checked == true)
                {
                    dr["GTax"] = "1";
                }
                else
                {
                    dr["GTax"] = "0";
                }
                if (!string.IsNullOrEmpty(txtGvPrice.Text))
                {
                    //dr["Price"] = txtGvPrice.Text;
                    dr["Price"] = Request.Form[txtGvPrice.UniqueID];
                }

                if (!string.IsNullOrEmpty(hdnOrderedQuan.Value))
                {
                    dr["OrderedQuan"] = hdnOrderedQuan.Value;
                }
                if (!string.IsNullOrEmpty(hdnOrdered.Value))
                {
                    dr["Ordered"] = hdnOrdered.Value;
                }
                if (!string.IsNullOrEmpty(hdnRPOItemId.Value))
                {
                    dr["RPOItemId"] = hdnRPOItemId.Value;
                }
                if (!string.IsNullOrEmpty(hdnPOItemId.Value))
                {
                    dr["POItemId"] = hdnPOItemId.Value;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void SetDatatableData(DataSet _dsTrans)
    {
        ViewState["Transactions_JobCost"] = _dsTrans.Tables[0];


        BINDGRID(_dsTrans.Tables[0]);
    }

    private void GetPeriodDetails(DateTime invoicedt, DateTime postdt)
    {
        bool flag = CommonHelper.GetPeriodDetails(invoicedt);
        if (flag)
        {
            flag = CommonHelper.GetPeriodDetails(postdt);
        }
        //bool flag = true;
        ViewState["FlagPeriodClose"] = flag;
        if (!flag)
        {
            divSuccess.Visible = true;
            btnSubmit.Visible = false;
        }
    }

    private void FillAPStatus()             //Fill AP Status
    {
        List<APStatus> _lstStatus = new List<APStatus>();
        _lstStatus = APStatusHelper.GetAll();

        ddlStatus.DataSource = _lstStatus;
        ddlStatus.DataValueField = "ID";
        ddlStatus.DataTextField = "Name";
        ddlStatus.DataBind();
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

    private void SetBillForm()
    {
        //FillVendor();
        divSuccess.Visible = false;
        FillAPStatus();
        SetInitialRow();
        txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtDueDate.Text = DateTime.Now.AddDays(30).ToString("MM/dd/yyyy");
        txtPostingDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
        //txtPO.Text = "0";
        txtDueIn.Text = "30";       // on Due in change, Due Date dates will be added on today's date.
        lblTotalAmount.Text = "0.00";
        lblTotalUseTax.Text = "0.00";
        txtDisc.Text = "0.00";
        txtMemo.Text = "Invoice";
        txtPaid.Text = "0";       // Default If Paid 0 For Changeset ES2303
    }

    private List<Transaction> GetGLTransactions()   //  Get Expense Gridview data
    {
        List<Transaction> _lstTrans = new List<Transaction>();
        try
        {
            foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)//Project Expense grid 
            {
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                HiddenField hdnAcctID = (HiddenField)gr.FindControl("hdnAcctID");
                TextBox txtGvAcctName = (TextBox)gr.FindControl("txtGvAcctName");
                TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                TextBox txtGvUseTax = (TextBox)gr.FindControl("txtGvUseTax");
                Label lblTID = (Label)gr.FindControl("lblTID");

                HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                TextBox txtGvJob = (TextBox)gr.FindControl("txtGvJob");
                TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)gr.FindControl("hdnPID");
                HiddenField hdnLine = (HiddenField)gr.FindControl("hdnLine");
                HiddenField hdnPrvInQuan = (HiddenField)gr.FindControl("hdnPrvInQuan");
                HiddenField hdnPrvIn = (HiddenField)gr.FindControl("hdnPrvIn");
                HiddenField hdnOutstandQuan = (HiddenField)gr.FindControl("hdnOutstandQuan");
                HiddenField hdnOutstandBalance = (HiddenField)gr.FindControl("hdnOutstandBalance");

                String _acctName = txtGvAcctName.Text;
                String _acct = hdnAcctID.Value;
                String _amount = txtGvAmount.Text;
                String _jobName = txtGvJob.Text;
                String _jobID = hdnJobID.Value;
                String _phase = txtGvPhase.Text;
                String _phaseId = hdnPID.Value;
                String _useTax = txtGvUseTax.Text;
                String _Line = hdnLine.Value;
                String _PrvInQuan = hdnPrvInQuan.Value;
                String _PrvIn = hdnPrvIn.Value;
                String _OutstandQuan = hdnOutstandQuan.Value;
                String _OutstandBalance = hdnOutstandBalance.Value;

                if (!string.IsNullOrEmpty(_acct) && !string.IsNullOrEmpty(_acctName) && !string.IsNullOrEmpty(_amount)) //!string.IsNullOrEmpty(_usetax)
                {
                    Transaction _objT = new Transaction();
                    _objT.ID = Convert.ToInt32(lblTID.Text);
                    _objT.TransDescription = _acctName; //Accout name
                    _objT.Acct = Convert.ToInt32(_acct);
                    _objT.Amount = Convert.ToDouble(_amount);
                    if (!string.IsNullOrEmpty(_jobID))
                    {
                        _objT.JobInt = Convert.ToInt32(_jobID);
                    }
                    if (!string.IsNullOrEmpty(_phaseId))
                    {
                        _objT.PhaseDoub = Convert.ToDouble(_phaseId);
                    }
                    bool IsTax = false;
                    if (!string.IsNullOrEmpty(_useTax))
                    {
                        if (Convert.ToDouble(_useTax) > 0)
                        {
                            IsTax = true;
                        }
                    }
                    _objT.IsUseTax = IsTax;
                    _objT.UseTax = string.IsNullOrEmpty(_useTax) ? 0 : Convert.ToDouble(_useTax);
                    _lstTrans.Add(_objT);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _lstTrans;
    }

    private void SetInitialRow()            //Initialization of Datatable.
    {
        try
        {
            int rowIndex = 0;
            DataTable dtJob = new DataTable();

            dtJob.Columns.Add(new DataColumn("RowID", typeof(string)));
            dtJob.Columns.Add(new DataColumn("ID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AcctNo", typeof(string)));
            //dtJob.Columns.Add(new DataColumn("Account", typeof(string)));
            dtJob.Columns.Add(new DataColumn("fDesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtJob.Columns.Add(new DataColumn("UseTax", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Loc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("JobName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("JobID", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Phase", typeof(string)));
            dtJob.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("UName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("UtaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("ItemID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("Quan", typeof(String)));
            dtJob.Columns.Add(new DataColumn("Ticket", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("OpSq", typeof(string)));// Ticket 

            dtJob.Columns.Add(new DataColumn("Warehouse", typeof(string)));
            dtJob.Columns.Add(new DataColumn("WHLocID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Line", typeof(int)));
            dtJob.Columns.Add(new DataColumn("PrvInQuan", typeof(int)));
            dtJob.Columns.Add(new DataColumn("PrvIn", typeof(int)));
            dtJob.Columns.Add(new DataColumn("OutstandQuan", typeof(int)));
            dtJob.Columns.Add(new DataColumn("OutstandBalance", typeof(int)));

            dtJob.Columns.Add(new DataColumn("STax", typeof(int)));
            dtJob.Columns.Add(new DataColumn("STaxName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("STaxRate", typeof(double)));
            dtJob.Columns.Add(new DataColumn("StaxAmt", typeof(double)));
            dtJob.Columns.Add(new DataColumn("STaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("GSTRate", typeof(double)));
            dtJob.Columns.Add(new DataColumn("GTaxAmt", typeof(double)));
            dtJob.Columns.Add(new DataColumn("GSTTaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AmountTot", typeof(string)));

            dtJob.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("IsPO", typeof(int)));
            dtJob.Columns.Add(new DataColumn("GTax", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Price", typeof(double)));

            dtJob.Columns.Add(new DataColumn("OrderedQuan", typeof(double)));
            dtJob.Columns.Add(new DataColumn("Ordered", typeof(double)));

            dtJob.Columns.Add(new DataColumn("RPOItemId", typeof(int)));
            dtJob.Columns.Add(new DataColumn("POItemId", typeof(int)));

            rowIndex = 0;

            DataRow drJob = dtJob.NewRow();
            drJob["STax"] = 0;
            drJob["GTax"] = 0;
            drJob["RPOItemId"] = 0;
            drJob["POItemId"] = 0;
            dtJob.Rows.Add(drJob);

            ViewState["Transactions_JobCost"] = dtJob;

            BINDGRID(dtJob);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetGridViewData()          //Set Added Data to DataTable
    {
        try
        {
            int i = 0;
            DataTable dtJob = (DataTable)ViewState["Transactions_JobCost"];
            i = 0;
            foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            {
                HiddenField hdnAcctID = (HiddenField)gr.FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");
                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                TextBox txtGvUseTax = (TextBox)gr.FindControl("txtGvUseTax");
                Label lblTID = (Label)gr.FindControl("lblTID");

                TextBox txtGvLoc = (TextBox)gr.FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)gr.FindControl("txtGvJob");
                HiddenField hdnJobID = (HiddenField)gr.FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)gr.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)gr.FindControl("hdnPID");
                TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");
                HiddenField hdnItemID = (HiddenField)gr.FindControl("hdnItemID");
                //  HiddenField hdnQuantity = (HiddenField)gr.FindControl("hdnQuantity");
                TextBox txtGvQuan = (TextBox)gr.FindControl("txtGvQuan");
                HiddenField hdnLine = (HiddenField)gr.FindControl("hdnLine");
                HiddenField hdnPrvInQuan = (HiddenField)gr.FindControl("hdnPrvInQuan");
                HiddenField hdnPrvIn = (HiddenField)gr.FindControl("hdnPrvIn");
                HiddenField hdnOutstandQuan = (HiddenField)gr.FindControl("hdnOutstandQuan");
                HiddenField hdnOutstandBalance = (HiddenField)gr.FindControl("hdnOutstandBalance");

                HiddenField hdnchkTaxable = (HiddenField)gr.FindControl("hdnchkTaxable");
                HiddenField STaxAmt = (HiddenField)gr.FindControl("STaxAmt");
                HiddenField hdnSTaxGL = (HiddenField)gr.FindControl("hdnSTaxGL");
                HiddenField hdnGSTTaxAm = (HiddenField)gr.FindControl("hdnGSTTaxAm");
                HiddenField hdnGSTTaxGL = (HiddenField)gr.FindControl("hdnGSTTaxGL");
                Label lblAmountWithTax = (Label)gr.FindControl("lblAmountWithTax");
                HiddenField hdnchkGTaxable = (HiddenField)gr.FindControl("hdnchkGTaxable");
                TextBox txtGvPrice = (TextBox)gr.FindControl("txtGvPrice");


                dtJob.Rows[i]["AcctNo"] = txtGvAcctNo.Text;
                dtJob.Rows[i]["AcctID"] = hdnAcctID.Value;
                dtJob.Rows[i]["fDesc"] = txtGvDesc.Text;
                dtJob.Rows[i]["Amount"] = txtGvAmount.Text;
                dtJob.Rows[i]["Quan"] = txtGvQuan.Text;
                dtJob.Rows[i]["UseTax"] = txtGvUseTax.Text;
                dtJob.Rows[i]["Loc"] = txtGvLoc.Text;
                dtJob.Rows[i]["JobName"] = txtGvJob.Text;
                dtJob.Rows[i]["JobID"] = hdnJobID.Value;
                dtJob.Rows[i]["Phase"] = txtGvPhase.Text;

                if (!string.IsNullOrEmpty(hdnAcctID.Value.ToString()))
                    dtJob.Rows[i]["PhaseID"] = "0";
                else
                    dtJob.Rows[i]["PhaseID"] = hdnPID.Value;
                if (!string.IsNullOrEmpty(txtGvItem.Text.ToString()))
                    dtJob.Rows[i]["ItemDesc"] = txtGvItem.Text;
                else
                    dtJob.Rows[i]["ItemDesc"] = "";

                if (!string.IsNullOrEmpty(hdnItemID.Value))
                {
                    dtJob.Rows[i]["ItemID"] = Convert.ToInt32(hdnItemID.Value);
                }
                if (!string.IsNullOrEmpty(txtGvItem.Text))
                {
                    dtJob.Rows[i]["ItemDesc"] = txtGvItem.Text;
                }
                dtJob.Rows[i]["Line"] = hdnLine.Value;
                dtJob.Rows[i]["PrvInQuan"] = hdnPrvInQuan.Value;
                dtJob.Rows[i]["PrvIn"] = hdnPrvIn.Value;
                dtJob.Rows[i]["OutstandQuan"] = hdnOutstandQuan.Value;
                dtJob.Rows[i]["OutstandBalance"] = hdnOutstandBalance.Value;
                dtJob.Rows[i]["Price"] = txtGvPrice.Text;

                if (!string.IsNullOrEmpty(hdnchkTaxable.Value.ToString()))
                    dtJob.Rows[i]["STax"] = "0";
                else
                    dtJob.Rows[i]["STax"] = hdnchkTaxable.Value;
                if (!string.IsNullOrEmpty(hdnSTaxName.Value.ToString()))
                    dtJob.Rows[i]["STaxName"] = "";
                else
                    dtJob.Rows[i]["STaxName"] = hdnSTaxName.Value;
                if (!string.IsNullOrEmpty(STaxAmt.Value.ToString()))
                    dtJob.Rows[i]["StaxAmt"] = 0;
                else
                    dtJob.Rows[i]["StaxAmt"] = STaxAmt.Value;
                if (!string.IsNullOrEmpty(hdnQST.Value.ToString()))
                    dtJob.Rows[i]["STaxRate"] = 0;
                else
                    dtJob.Rows[i]["STaxRate"] = hdnQST.Value;
                if (!string.IsNullOrEmpty(hdnSTaxGL.Value.ToString()))
                    dtJob.Rows[i]["STaxGL"] = 0;
                else
                    dtJob.Rows[i]["STaxGL"] = hdnSTaxGL.Value;

                if (!string.IsNullOrEmpty(hdnGST.Value.ToString()))
                    dtJob.Rows[i]["GSTRate"] = "";
                else
                    dtJob.Rows[i]["GSTRate"] = hdnGST.Value;
                if (!string.IsNullOrEmpty(hdnGSTTaxAm.Value.ToString()))
                    dtJob.Rows[i]["GTaxAmt"] = 0;
                else
                    dtJob.Rows[i]["GTaxAmt"] = hdnGSTTaxAm.Value;
                if (!string.IsNullOrEmpty(hdnGSTTaxGL.Value.ToString()))
                    dtJob.Rows[i]["GSTTaxGL"] = 0;
                else
                    dtJob.Rows[i]["GSTTaxGL"] = hdnGSTTaxGL.Value;
                if (!string.IsNullOrEmpty(lblAmountWithTax.Text.ToString()))
                    dtJob.Rows[i]["AmountTot"] = 0;
                else
                    dtJob.Rows[i]["AmountTot"] = lblAmountWithTax.Text;
                if (!string.IsNullOrEmpty(hdnchkGTaxable.Value.ToString()))
                    dtJob.Rows[i]["GTax"] = "0";
                else
                    dtJob.Rows[i]["GTax"] = hdnchkGTaxable.Value;

                i++;
            }

            ViewState["Transactions_JobCost"] = dtJob;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
    private void userpermissions()
    {
        //if (Session["type"].ToString() != "c")
        //{
        //    if (Session["type"].ToString() != "am")
        //    {
        //        _objPropUser.ConnConfig = Session["config"].ToString();
        //        _objPropUser.Username = Session["username"].ToString();
        //        _objPropUser.PageName = "addbills.aspx";
        //        DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
        //        if (dspage.Tables[0].Rows.Count > 0)
        //        {
        //            if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
        //            {
        //                Response.Redirect("home.aspx");
        //            }
        //        }
        //        else
        //        {
        //            Response.Redirect("home.aspx");
        //        }
        //    }
        //}

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable dtUserPermission = new DataTable();
            dtUserPermission = GetUserById();
            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// Bill ///////////////////------->

            string BillPermission = dtUserPermission.Rows[0]["Bill"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["Bill"].ToString();
            string ADD = BillPermission.Length < 1 ? "Y" : BillPermission.Substring(0, 1);
            string Edit = BillPermission.Length < 2 ? "Y" : BillPermission.Substring(1, 1);
            string Delete = BillPermission.Length < 2 ? "Y" : BillPermission.Substring(2, 1);
            string View = BillPermission.Length < 4 ? "Y" : BillPermission.Substring(3, 1);

            if (Request.QueryString["id"] != null)
            {
                aImport.Visible = false;
            }
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
                    btnSubmit.Visible = btnAddNewLines.Visible = false;
                    btnSubmitJob.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
        }
    }

    private void UpdatePoStatus()
    {
        try
        {
            //objPO.ConnConfig = Session["config"].ToString();
            //_IsExistPO.ConnConfig = Session["config"].ToString();
            //_UpdatePOStatus.ConnConfig = Session["config"].ToString();
            //_UpdateReceivePOStatusByPOID.ConnConfig = Session["config"].ToString();
            //_GetClosePOCheck.ConnConfig = Session["config"].ToString();
            //_UpdateReceivePOStatus.ConnConfig = Session["config"].ToString();

            //if (!string.IsNullOrEmpty(txtPO.Text) && string.IsNullOrEmpty(txtReceptionId.Text))
            //{
            //    objPO.POID = Convert.ToInt32(txtPO.Text);
            //    _IsExistPO.POID = Convert.ToInt32(txtPO.Text);

            //    bool IsExistPo = false;

            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        string APINAME = "BillAPI/AddBills_IsExistPO";

            //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistPO);
            //        IsExistPo = Convert.ToBoolean(_APIResponse.ResponseData);
            //    }
            //    else
            //    {
            //        IsExistPo = _objBLBills.IsExistPO(objPO);
            //    }

            //    if (IsExistPo)
            //    {
            //        if (Convert.ToInt32(txtPO.Text) > 0)
            //        {
            //            if (string.IsNullOrEmpty(hdnReceivedAmount.Value))
            //                hdnReceivedAmount.Value = "0";
            //            if (string.IsNullOrEmpty(hdnTotalAmount.Value))
            //                hdnTotalAmount.Value = "0";
            //            double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value) + Convert.ToDouble(lblTotalAmount.Text);
            //            //  double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value);
            //            double totalAmt = Convert.ToDouble(hdnTotalAmount.Value); //po amount
            //            if (receivedAmt >= totalAmt)
            //            {
            //                objPO.POID = Convert.ToInt32(txtPO.Text);
            //                objPO.Status = 1; // Final close (closed at bill level)

            //                _UpdatePOStatus.POID = Convert.ToInt32(txtPO.Text);
            //                _UpdatePOStatus.Status = 1; // Final close (closed at bill level)

            //                _UpdateReceivePOStatusByPOID.POID = Convert.ToInt32(txtPO.Text);
            //                _UpdateReceivePOStatusByPOID.Status = 1; // Final close (closed at bill level)

            //                if (IsAPIIntegrationEnable == "YES")
            //                //if (Session["APAPIEnable"].ToString() == "YES")
            //                {
            //                    string APINAME = "BillAPI/AddBills_UpdatePOStatus";

            //                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdatePOStatus);
            //                }
            //                else
            //                {
            //                    _objBLBills.UpdatePOStatus(objPO);
            //                }

            //                if (IsAPIIntegrationEnable == "YES")
            //                //if (Session["APAPIEnable"].ToString() == "YES")
            //                {
            //                    string APINAME = "BillAPI/AddBills_UpdateReceivePOStatusByPOID";

            //                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivePOStatusByPOID);
            //                }
            //                else
            //                {
            //                    _objBLBills.UpdateReceivePOStatusByPOID(objPO);//For close all reception ID
            //                }
            //            }
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(txtReceptionId.Text) && !string.IsNullOrEmpty(txtPO.Text))
            //{
            //    String Res = "";
            //    objPO.RID = Convert.ToInt32(txtReceptionId.Text);
            //    objPO.POID = Convert.ToInt32(txtPO.Text);

            //    _GetClosePOCheck.RID = Convert.ToInt32(txtReceptionId.Text);
            //    _GetClosePOCheck.POID = Convert.ToInt32(txtPO.Text);


            //    if (IsAPIIntegrationEnable == "YES")
            //    //if (Session["APAPIEnable"].ToString() == "YES")
            //    {
            //        string APINAME = "BillAPI/AddBills_GetClosePOCheck";

            //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetClosePOCheck);
            //        Res = Convert.ToString(_APIResponse.ResponseData);
            //    }
            //    else
            //    {
            //        Res = _objBLBills.GetClosePOCheck(objPO);//For close all reception ID
            //    }


            //    //write code to fetch the data.

            //    if (Res == "0")
            //    {
            //        objPO.RID = Convert.ToInt32(txtReceptionId.Text);
            //        objPO.POID = Convert.ToInt32(txtPO.Text);
            //        objPO.Status = 1;

            //        _UpdateReceivePOStatus.RID = Convert.ToInt32(txtReceptionId.Text);
            //        _UpdateReceivePOStatus.POID = Convert.ToInt32(txtPO.Text);
            //        _UpdateReceivePOStatus.Status = 1;

            //        if (IsAPIIntegrationEnable == "YES")
            //        //if (Session["APAPIEnable"].ToString() == "YES")
            //        {
            //            string APINAME = "BillAPI/AddBills_UpdateReceivePOStatus";

            //            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivePOStatus);
            //        }
            //        else
            //        {
            //            _objBLBills.UpdateReceivePOStatus(objPO);
            //        }
            //    }
            //    else
            //    {
            //        objPO.POID = Convert.ToInt32(txtPO.Text);
            //        objPO.Status = 1; // Final close (closed at bill level)

            //        _UpdatePOStatus.POID = Convert.ToInt32(txtPO.Text);
            //        _UpdatePOStatus.Status = 1; // Final close (closed at bill level)

            //        _UpdateReceivePOStatusByPOID.POID = Convert.ToInt32(txtPO.Text);
            //        _UpdateReceivePOStatusByPOID.Status = 1; // Final close (closed at bill level)

            //        if (IsAPIIntegrationEnable == "YES")
            //        //if (Session["APAPIEnable"].ToString() == "YES")
            //        {
            //            string APINAME = "BillAPI/AddBills_UpdatePOStatus";

            //            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdatePOStatus);
            //        }
            //        else
            //        {
            //            _objBLBills.UpdatePOStatus(objPO);
            //        }

            //        if (IsAPIIntegrationEnable == "YES")
            //        //if (Session["APAPIEnable"].ToString() == "YES")
            //        {
            //            string APINAME = "BillAPI/AddBills_UpdateReceivePOStatusByPOID";

            //            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivePOStatusByPOID);
            //        }
            //        else
            //        {
            //            _objBLBills.UpdateReceivePOStatusByPOID(objPO);//For close all reception ID
            //        }
            //    }
            //}


            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _IsExistPO.ConnConfig = Session["config"].ToString();
                _UpdatePOStatus.ConnConfig = Session["config"].ToString();
                _UpdateReceivePOStatusByPOID.ConnConfig = Session["config"].ToString();
                _GetClosePOCheck.ConnConfig = Session["config"].ToString();
                _UpdateReceivePOStatus.ConnConfig = Session["config"].ToString();

                if (!string.IsNullOrEmpty(txtPO.Text) && string.IsNullOrEmpty(txtReceptionId.Text))
                {
                    _IsExistPO.POID = Convert.ToInt32(txtPO.Text);

                    bool IsExistPo = false;

                    string APINAME = "BillAPI/AddBills_IsExistPO";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistPO);
                    IsExistPo = Convert.ToBoolean(_APIResponse.ResponseData);

                    if (IsExistPo)
                    {
                        if (Convert.ToInt32(txtPO.Text) > 0)
                        {
                            if (string.IsNullOrEmpty(hdnReceivedAmount.Value))
                                hdnReceivedAmount.Value = "0";
                            if (string.IsNullOrEmpty(hdnTotalAmount.Value))
                                hdnTotalAmount.Value = "0";
                            double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value) + Convert.ToDouble(lblTotalAmount.Text);
                            //  double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value);
                            double totalAmt = Convert.ToDouble(hdnTotalAmount.Value); //po amount
                            if (receivedAmt >= totalAmt)
                            {
                                _UpdatePOStatus.POID = Convert.ToInt32(txtPO.Text);
                                _UpdatePOStatus.Status = 1; // Final close (closed at bill level)

                                _UpdateReceivePOStatusByPOID.POID = Convert.ToInt32(txtPO.Text);
                                _UpdateReceivePOStatusByPOID.Status = 1; // Final close (closed at bill level)

                                string APINAME1 = "BillAPI/AddBills_UpdatePOStatus";

                                APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _UpdatePOStatus);


                                string APINAME2 = "BillAPI/AddBills_UpdateReceivePOStatusByPOID";

                                APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _UpdateReceivePOStatusByPOID);

                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(txtReceptionId.Text) && !string.IsNullOrEmpty(txtPO.Text))
                {
                    String Res = "";

                    _GetClosePOCheck.RID = Convert.ToInt32(txtReceptionId.Text);
                    _GetClosePOCheck.POID = Convert.ToInt32(txtPO.Text);

                    string APINAME3 = "BillAPI/AddBills_GetClosePOCheck";

                    APIResponse _APIResponse3 = new MOMWebUtility().CallMOMWebAPI(APINAME3, _GetClosePOCheck);
                    Res = Convert.ToString(_APIResponse3.ResponseData);

                    //write code to fetch the data.

                    if (Res == "0")
                    {
                        _UpdateReceivePOStatus.RID = Convert.ToInt32(txtReceptionId.Text);
                        _UpdateReceivePOStatus.POID = Convert.ToInt32(txtPO.Text);
                        _UpdateReceivePOStatus.Status = 1;

                        string APINAME4 = "BillAPI/AddBills_UpdateReceivePOStatus";

                        APIResponse _APIResponse4 = new MOMWebUtility().CallMOMWebAPI(APINAME4, _UpdateReceivePOStatus);

                    }
                    else
                    {

                        _UpdatePOStatus.POID = Convert.ToInt32(txtPO.Text);
                        _UpdatePOStatus.Status = 1; // Final close (closed at bill level)

                        _UpdateReceivePOStatusByPOID.POID = Convert.ToInt32(txtPO.Text);
                        _UpdateReceivePOStatusByPOID.Status = 1; // Final close (closed at bill level)

                        string APINAME = "BillAPI/AddBills_UpdatePOStatus";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdatePOStatus);

                        string APINAME5 = "BillAPI/AddBills_UpdateReceivePOStatusByPOID";

                        APIResponse _APIResponse5 = new MOMWebUtility().CallMOMWebAPI(APINAME5, _UpdateReceivePOStatusByPOID);

                    }
                }

            }
            else
            {
                objPO.ConnConfig = Session["config"].ToString();

                if (!string.IsNullOrEmpty(txtPO.Text) && string.IsNullOrEmpty(txtReceptionId.Text))
                {
                    objPO.POID = Convert.ToInt32(txtPO.Text);
                    bool IsExistPo = false;

                    IsExistPo = _objBLBills.IsExistPO(objPO);

                    if (IsExistPo)
                    {
                        if (Convert.ToInt32(txtPO.Text) > 0)
                        {
                            if (string.IsNullOrEmpty(hdnReceivedAmount.Value))
                                hdnReceivedAmount.Value = "0";
                            if (string.IsNullOrEmpty(hdnTotalAmount.Value))
                                hdnTotalAmount.Value = "0";
                            double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value) + Convert.ToDouble(lblTotalAmount.Text);
                            //  double receivedAmt = Convert.ToDouble(hdnReceivedAmount.Value);
                            double totalAmt = Convert.ToDouble(hdnTotalAmount.Value); //po amount
                            if (receivedAmt >= totalAmt)
                            {
                                objPO.POID = Convert.ToInt32(txtPO.Text);
                                objPO.Status = 1; // Final close (closed at bill level)

                                _objBLBills.UpdatePOStatus(objPO);

                                _objBLBills.UpdateReceivePOStatusByPOID(objPO);//For close all reception ID

                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(txtReceptionId.Text) && !string.IsNullOrEmpty(txtPO.Text))
                {
                    String Res = "";
                    objPO.RID = Convert.ToInt32(txtReceptionId.Text);
                    objPO.POID = Convert.ToInt32(txtPO.Text);

                    Res = _objBLBills.GetClosePOCheck(objPO);//For close all reception ID

                    //write code to fetch the data.

                    if (Res == "0")
                    {
                        objPO.RID = Convert.ToInt32(txtReceptionId.Text);
                        objPO.POID = Convert.ToInt32(txtPO.Text);
                        objPO.Status = 1;

                        _objBLBills.UpdateReceivePOStatus(objPO);

                    }
                    else
                    {
                        objPO.POID = Convert.ToInt32(txtPO.Text);
                        objPO.Status = 1; // Final close (closed at bill level)

                        _objBLBills.UpdatePOStatus(objPO);

                        _objBLBills.UpdateReceivePOStatusByPOID(objPO);//For close all reception ID

                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool IsValid()
    {
        if (txtPO.Text != "")
        {
            try
            {
                //objPO.POID = Convert.ToInt32(txtPO.Text);
                //objPO.Vendor = Convert.ToInt16(hdnVendorID.Value);
                //objPO.ConnConfig = Session["config"].ToString();

                //_getReceivePOList.POID = Convert.ToInt32(txtPO.Text);
                //_getReceivePOList.Vendor = Convert.ToInt16(hdnVendorID.Value);
                //_getReceivePOList.ConnConfig = Session["config"].ToString();

                //DataSet dsReceptionNo = new DataSet();
                //List <GetReceivePOListViewModel> _lstGetReceivePOList = new List<GetReceivePOListViewModel>();

                //if (IsAPIIntegrationEnable == "YES")
                ////if (Session["APAPIEnable"].ToString() == "YES")
                //{
                //    string APINAME = "BillAPI/AddBills_GetReceivePOList";

                //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getReceivePOList);

                //    _lstGetReceivePOList = (new JavaScriptSerializer()).Deserialize<List<GetReceivePOListViewModel>>(_APIResponse.ResponseData);
                //    dsReceptionNo = CommonMethods.ToDataSet<GetReceivePOListViewModel>(_lstGetReceivePOList);
                //}
                //else
                //{
                //    dsReceptionNo = _objBLBills.GetReceivePOList(objPO);
                //}

                //if (dsReceptionNo.Tables[0].Rows.Count > 0)
                //{
                //    //if (txtReceptionId.Text == "")
                //    //{
                //    //    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction11", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                //    //    return false;
                //    //}
                //}
                //else
                //{
                //    // if (Request.QueryString["id"] == null)
                //    // {
                //    //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction22", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                //    //     return false;
                //    // }
                //    //else if (txtReceptionId.Text == "")
                //    // {
                //    //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction33", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                //    //     return false;
                //    // }
                //}

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    _getReceivePOList.POID = Convert.ToInt32(txtPO.Text);
                    _getReceivePOList.Vendor = Convert.ToInt16(hdnVendorID.Value);
                    _getReceivePOList.ConnConfig = Session["config"].ToString();

                    DataSet dsReceptionNo = new DataSet();
                    List<GetReceivePOListViewModel> _lstGetReceivePOList = new List<GetReceivePOListViewModel>();


                    string APINAME = "BillAPI/AddBills_GetReceivePOList";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getReceivePOList);

                    _lstGetReceivePOList = (new JavaScriptSerializer()).Deserialize<List<GetReceivePOListViewModel>>(_APIResponse.ResponseData);
                    dsReceptionNo = CommonMethods.ToDataSet<GetReceivePOListViewModel>(_lstGetReceivePOList);


                    if (dsReceptionNo.Tables[0].Rows.Count > 0)
                    {
                        //if (txtReceptionId.Text == "")
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction11", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                        //    return false;
                        //}
                    }
                    else
                    {
                        // if (Request.QueryString["id"] == null)
                        // {
                        //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction22", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //     return false;
                        // }
                        //else if (txtReceptionId.Text == "")
                        // {
                        //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction33", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                        //     return false;
                        // }
                    }
                }
                else
                {
                    objPO.POID = Convert.ToInt32(txtPO.Text);
                    objPO.Vendor = Convert.ToInt16(hdnVendorID.Value);
                    objPO.ConnConfig = Session["config"].ToString();

                    DataSet dsReceptionNo = new DataSet();

                    dsReceptionNo = _objBLBills.GetReceivePOList(objPO);

                    if (dsReceptionNo.Tables[0].Rows.Count > 0)
                    {
                        //if (txtReceptionId.Text == "")
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction11", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                        //    return false;
                        //}
                    }
                    else
                    {
                        // if (Request.QueryString["id"] == null)
                        // {
                        //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction22", "noty({text: 'Please receive PO before entering bill.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        //     return false;
                        // }
                        //else if (txtReceptionId.Text == "")
                        // {
                        //     ScriptManager.RegisterStartupScript(this, GetType(), "myFunction33", "noty({text: 'Please select reception no#.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                        //     return false;
                        // }
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction44", "noty({text: '" + ex.Message + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
        }



        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);
        var due = DateTime.TryParseExact(txtDueDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);
        var post = DateTime.TryParseExact(txtPostingDate.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);

        if (dt & due & post)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }


    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            //objJob.ConnConfig = Session["config"].ToString();
            //_GetBomType.ConnConfig = Session["config"].ToString();

            //List<BOMTViewModel> _lstBOMTViewModel = new List<BOMTViewModel>();

            //if (IsAPIIntegrationEnable == "YES")
            ////if (Session["APAPIEnable"].ToString() == "YES")
            //{
            //    string APINAME = "BillAPI/AddBills_GetBomType";

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBomType);

            //    _lstBOMTViewModel = (new JavaScriptSerializer()).Deserialize<List<BOMTViewModel>>(_APIResponse.ResponseData);
            //    ds = CommonMethods.ToDataSet<BOMTViewModel>(_lstBOMTViewModel);
            //}
            //else
            //{
            //    ds = objBL_Job.GetBomType(objJob);
            //}



            //DataRow dr = ds.Tables[0].NewRow();
            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    dr["ID"] = 0;
            //    dr["Type"] = "Select Type";
            //    ds.Tables[0].Rows.InsertAt(dr, 0);
            //}
            //else
            //{
            //    dr["ID"] = 0;
            //    dr["Type"] = "No data found";
            //    ds.Tables[0].Rows.InsertAt(dr, 0);
            //}
            //ddlBomType.DataSource = ds.Tables[0];
            //ddlBomType.DataTextField = "Type";
            //ddlBomType.DataValueField = "ID";
            //ddlBomType.DataBind();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _GetBomType.ConnConfig = Session["config"].ToString();

                List<BOMTViewModel> _lstBOMTViewModel = new List<BOMTViewModel>();

                string APINAME = "BillAPI/AddBills_GetBomType";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBomType);

                _lstBOMTViewModel = (new JavaScriptSerializer()).Deserialize<List<BOMTViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<BOMTViewModel>(_lstBOMTViewModel);


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
            else
            {
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
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion  
    protected void btnSubmitJob_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValid())
            {
                DataTable dt = GetTransaction();

                if (ValidateGridUpdPrj(dt))
                {
                    dt.Columns.Remove("AcctNo");
                    dt.Columns.Remove("JobName");
                    //dt.Columns.Remove("Phase");
                    dt.Columns.Remove("UName");
                    //dt.Columns.Remove("UtaxGL");
                    //dt.Columns.Remove("Item");
                    //dt.Columns.Remove("TypeID");
                    dt.Columns.Remove("Loc");
                    //dt.Columns.Remove("Warehouse");
                    //dt.Columns.Remove("WHLocID");
                    dt.Columns.Remove("Line");
                    dt.Columns.Remove("PrvInQuan");
                    dt.Columns.Remove("PrvIn");
                    dt.Columns.Remove("OutstandQuan");
                    dt.Columns.Remove("OutstandBalance");

                    //dt.Columns.Remove("STax");
                    //dt.Columns.Remove("STaxName");
                    //dt.Columns.Remove("STaxRate");
                    //dt.Columns.Remove("StaxAmt");
                    //dt.Columns.Remove("STaxGL");
                    //dt.Columns.Remove("GSTRate");
                    //dt.Columns.Remove("GTaxAmt");
                    //dt.Columns.Remove("GSTTaxGL");
                    dt.Columns.Remove("AmountTot");

                    dt.Columns.Remove("Warehousefdesc");
                    dt.Columns.Remove("Locationfdesc");
                    //dt.Columns.Remove("IsPO");
                    //dt.Columns.Remove("GTax");
                    dt.Columns.Remove("OrderedQuan");
                    dt.Columns.Remove("Ordered");
                    dt.Columns.Remove("RPOItemId");
                    dt.Columns.Remove("POItemId");
                    dt.Select("JobID = 0")
                          .AsEnumerable().ToList()
                          .ForEach(t => t["JobID"] = DBNull.Value);
                    //dt.Select("PhaseID = 0")
                    //     .AsEnumerable().ToList()
                    //     .ForEach(t => t["PhaseID"] = DBNull.Value);
                    dt.Select("ItemID = 0")
                        .AsEnumerable().ToList()
                        .ForEach(t => t["ItemID"] = DBNull.Value);
                    // DT 02-Nov-2020 as per ES-5836 and discussion in MOMDevTeam Group and Laxmi Suggested.
                    //dt.Select("ItemDesc = '' or ItemDesc is null")
                    //    .AsEnumerable().ToList()
                    //    .ForEach(t => t["ItemDesc"] = t["fDesc"]);



                    bool chkperiod = Convert.ToBoolean(ViewState["FlagPeriodClose"]);
                    if (chkperiod == false)
                    {

                        dt.Select()
                        .AsEnumerable().ToList()
                        .ForEach(t => t["AcctID"] = DBNull.Value);

                    }

                    dt.AcceptChanges();

                    //_objPJ.ConnConfig = Session["config"].ToString();
                    //_objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                    //_objPJ.Ref = txtRef.Text;

                    //_UpdateBillsJobDetails.ConnConfig = Session["config"].ToString();
                    //_UpdateBillsJobDetails.fDate = Convert.ToDateTime(txtDate.Text);
                    //_UpdateBillsJobDetails.Ref = txtRef.Text;


                    //_objPJ.Dt = dt;
                    //_UpdateBillsJobDetails.Dt = dt;
                    //if (Request.QueryString["id"] != null)
                    //{
                    //    _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);
                    //    _UpdateBillsJobDetails.Batch = Convert.ToInt32(hdnBatch.Value);


                    //    if (IsAPIIntegrationEnable == "YES")
                    //    //if (Session["APAPIEnable"].ToString() == "YES")
                    //    {
                    //        string APINAME = "BillAPI/AddBills_UpdateBillsJobDetails";

                    //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateBillsJobDetails);
                    //    }
                    //    else
                    //    {
                    //        _objBLBills.UpdateBillsJobDetails(_objPJ);
                    //    }

                    //    RadGrid_gvLogs.Rebind();
                    //    //Response.Redirect("managebills.aspx", false);
                    //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill# : " + txtRef.Text + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //}

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        _UpdateBillsJobDetails.ConnConfig = Session["config"].ToString();
                        _UpdateBillsJobDetails.fDate = Convert.ToDateTime(txtDate.Text);
                        _UpdateBillsJobDetails.Ref = txtRef.Text;

                        _UpdateBillsJobDetails.Dt = dt;
                        if (Request.QueryString["id"] != null)
                        {
                            _UpdateBillsJobDetails.Batch = Convert.ToInt32(hdnBatch.Value);

                            string APINAME = "BillAPI/AddBills_UpdateBillsJobDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateBillsJobDetails);


                            RadGrid_gvLogs.Rebind();
                            //Response.Redirect("managebills.aspx", false);
                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill# : " + txtRef.Text + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        }
                    }
                    else
                    {
                        _objPJ.ConnConfig = Session["config"].ToString();
                        _objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                        _objPJ.Ref = txtRef.Text;
                        _objPJ.fDesc = txtMemo.Text;
                        _objPJ.MOMUSer = Session["User"].ToString();
                        _objPJ.Dt = dt;

                        if (Request.QueryString["id"] != null)
                        {
                            _objPJ.Batch = Convert.ToInt32(hdnBatch.Value);

                            _objBLBills.UpdateBillsJobDetails(_objPJ);

                            RadGrid_gvLogs.Rebind();
                            UpdateDocInfo();
                            //Response.Redirect("managebills.aspx", false);
                            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Bill updated successfully! <BR/> <b> Bill# : " + txtRef.Text + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
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

    protected void RadGrid_gvJobCostItems_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            CheckBox chkGTaxable = (CheckBox)gr.FindControl("chkGTaxable");
            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + RadGrid_gvJobCostItems.ClientID + "',event);";
            chkTaxable.Attributes["onclick"] = "CalTotalValStax(this);";
            chkGTaxable.Attributes["onclick"] = "CalTotalValGtax(this);";
        }
    }

    protected void RadGrid_gvJobCostItems_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DataTable dt = GetCurrentTransaction();

            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                dt.Rows.RemoveAt(index);
                dt.AcceptChanges();

                BINDGRID(dt);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallCalAmountTax", "CallCalAmountTax();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
                DataTable dt = new DataTable();
                if (FileUploadControl.HasFile)
                {
                    if (ext == ".csv")
                        dt = ReadCsvFile();
                    if (ext == ".xls" || ext == ".xlsx")
                        dt = ReadExcelFile();

                    //_objPJ.Dt = dt;
                    //_objPJ.ConnConfig = Session["config"].ToString();

                    //_GetBillingItems.Dt = dt;
                    //_GetBillingItems.ConnConfig = Session["config"].ToString();

                    //if (Session["CmpChkDefault"].ToString() == "1")
                    //{
                    //    _objPJ.EN = 1;
                    //    _GetBillingItems.EN = 1;
                    //}
                    //else
                    //{
                    //    _objPJ.EN = 0;
                    //    _GetBillingItems.EN = 0;
                    //}

                    //_objPJ.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
                    //_GetBillingItems.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());

                    //DataSet ds = new DataSet();
                    //DataSet ds1 = new DataSet();
                    //DataSet ds2 = new DataSet();

                    //ListGetBillingItems _lstBillingItems = new ListGetBillingItems();

                    //if (IsAPIIntegrationEnable == "YES")
                    ////if (Session["APAPIEnable"].ToString() == "YES")
                    //{
                    //    string APINAME = "BillAPI/AddBills_GetBillingItems";

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillingItems);

                    //    _lstBillingItems = (new JavaScriptSerializer()).Deserialize<ListGetBillingItems>(_APIResponse.ResponseData);
                    //    ds1 = _lstBillingItems.lstTable1.ToDataSet();
                    //    ds2 = _lstBillingItems.lstTable2.ToDataSet();

                    //    DataTable dt1 = new DataTable();
                    //    DataTable dt2 = new DataTable();

                    //    dt1 = ds1.Tables[0];
                    //    dt2 = ds2.Tables[0];

                    //    dt1.TableName = "Table1";
                    //    dt2.TableName = "Table2";

                    //    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

                    //}
                    //else
                    //{
                    //    ds = _objBLBills.GetBillingItems(_objPJ);
                    //}

                    //ViewState["ImportExcelData"] = ds.Tables[0];

                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    //    gv_Errorrows.DataSource = ds.Tables[1];
                    //    gv_Errorrows.Rebind();

                    //    btnContinue.Visible = true;
                    //    btnCancel.Visible = true;

                    //    lblTotalRows.Text = Convert.ToString(ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count);
                    //    lblValidRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                    //    lblInvalidRows.Text = Convert.ToString(ds.Tables[1].Rows.Count);
                    //    string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    //}
                    //else
                    //{

                    //    BINDGRID(ds.Tables[0]);
                    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
                    //}

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {

                        _GetBillingItems.Dt = dt;
                        _GetBillingItems.ConnConfig = Session["config"].ToString();

                        if (Session["CmpChkDefault"].ToString() == "1")
                        {

                            _GetBillingItems.EN = 1;
                        }
                        else
                        {

                            _GetBillingItems.EN = 0;
                        }


                        _GetBillingItems.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());

                        DataSet ds = new DataSet();
                        DataSet ds1 = new DataSet();
                        DataSet ds2 = new DataSet();

                        ListGetBillingItems _lstBillingItems = new ListGetBillingItems();

                        string APINAME = "BillAPI/AddBills_GetBillingItems";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillingItems);

                        _lstBillingItems = (new JavaScriptSerializer()).Deserialize<ListGetBillingItems>(_APIResponse.ResponseData);
                        ds1 = _lstBillingItems.lstTable1.ToDataSet();
                        ds2 = _lstBillingItems.lstTable2.ToDataSet();

                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();

                        dt1 = ds1.Tables[0];
                        dt2 = ds2.Tables[0];

                        dt1.TableName = "Table1";
                        dt2.TableName = "Table2";

                        ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });


                        ViewState["ImportExcelData"] = ds.Tables[0];

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            gv_Errorrows.DataSource = ds.Tables[1];
                            gv_Errorrows.Rebind();

                            btnContinue.Visible = true;
                            btnCancel.Visible = true;

                            lblTotalRows.Text = Convert.ToString(ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count);
                            lblValidRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                            lblInvalidRows.Text = Convert.ToString(ds.Tables[1].Rows.Count);
                            string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        }
                        else
                        {

                            BINDGRID(ds.Tables[0]);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
                        }

                    }
                    else
                    {
                        _objPJ.Dt = dt;
                        _objPJ.ConnConfig = Session["config"].ToString();


                        if (Session["CmpChkDefault"].ToString() == "1")
                        {
                            _objPJ.EN = 1;

                        }
                        else
                        {
                            _objPJ.EN = 0;

                        }

                        _objPJ.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());


                        DataSet ds = new DataSet();

                        ds = _objBLBills.GetBillingItems(_objPJ);


                        ViewState["ImportExcelData"] = ds.Tables[0];

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            gv_Errorrows.DataSource = ds.Tables[1];
                            gv_Errorrows.Rebind();

                            btnContinue.Visible = true;
                            btnCancel.Visible = true;

                            lblTotalRows.Text = Convert.ToString(ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count);
                            lblValidRows.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                            lblInvalidRows.Text = Convert.ToString(ds.Tables[1].Rows.Count);
                            string script = "function f(){OpenErrorModal(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        }
                        else
                        {

                            BINDGRID(ds.Tables[0]);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
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

    public DataTable ReadExcelFile()
    {
        try
        {
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();

            string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportBills\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ext);
            FileUploadControl.SaveAs(FileSaveWithPath);

            OleDbConnection oledbConn = new OleDbConnection();
            if (ext == ".xls")
                oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileSaveWithPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            else if (ext == ".xlsx")
                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileSaveWithPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

            oledbConn.Open();
            OleDbCommand ocmd = new OleDbCommand("select * from [Sheet1$]", oledbConn);
            OleDbDataAdapter oleda = new OleDbDataAdapter(ocmd);
            DataSet ds = new DataSet();
            oleda.Fill(ds);
            oledbConn.Close();

            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);

            DataTable dtExcel = ds.Tables[0].Copy();
            dtExcel.Columns.Add("RowNo", typeof(int));

            for (int i = 0; dtExcel.Rows.Count > i; i++)
            {
                dtExcel.Rows[i]["RowNo"] = i + 1;
            }
            return dtExcel;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable ReadCsvFile()
    {
        try
        {
            DataTable dtCsv = new DataTable();
            dtCsv.Columns.Add("AccNo", typeof(string));
            dtCsv.Columns.Add("ProjNo", typeof(string));
            dtCsv.Columns.Add("Code", typeof(string));
            dtCsv.Columns.Add("ItemDis", typeof(string));
            dtCsv.Columns.Add("Amount", typeof(string));
            dtCsv.Columns.Add("RowNo", typeof(int));

            string Fulltext;
            string FileSaveWithPath = Server.MapPath("~\\TempPDF\\ImportBills\\Import" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".csv");
            FileUploadControl.SaveAs(FileSaveWithPath);
            using (StreamReader sr = new StreamReader(FileSaveWithPath))
            {
                while (!sr.EndOfStream)
                {
                    Fulltext = sr.ReadToEnd().ToString();
                    string[] rows = Fulltext.Split('\n');

                    for (int i = 1; i < rows.Count() - 1; i++)
                    {
                        System.Text.RegularExpressions.Regex CSVParser = new System.Text.RegularExpressions.Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        String[] rowValues = CSVParser.Split(rows[i]);

                        DataRow dr = dtCsv.NewRow();
                        for (int k = 0; k < rowValues.Count(); k++)
                        {
                            if (string.IsNullOrEmpty(rowValues[k]))
                                dr[k] = DBNull.Value;
                            else
                                dr[k] = rowValues[k].ToString();
                        }
                        dr[rowValues.Count()] = i;
                        dtCsv.Rows.Add(dr);
                    }
                }
            }
            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);
            return dtCsv;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            btnContinue.Visible = false;
            btnCancel.Visible = false;
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CloseModal", "CloseModal()", true);

            DataTable dt = (DataTable)ViewState["ImportExcelData"];
            if (dt != null && dt.Rows.Count > 0)
            {

                BINDGRID(dt);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CallMyFunction", "CalculateTotalAmt()", true);
            }
            else
            {
                dt = GetTable();
                DataRow drJob = dt.NewRow();
                drJob["Stax"] = "0";
                drJob["GTax"] = 0;
                dt.Rows.Add(drJob);

                ViewState["Transactions_JobCost"] = dt;
                BINDGRID(dt);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Allrows", "noty({text: 'All rows are invalid.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnContinue.Visible = false;
        btnCancel.Visible = false;
        DataTable dt = GetTable();
        DataRow drJob = dt.NewRow();
        drJob["STax"] = "0";
        dt.Rows.Add(drJob);

        ViewState["Transactions_JobCost"] = dt;
        BINDGRID(dt);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "CloseModal", "CloseModal()", true);
    }

    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("AcctID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("Usetax", typeof(string));
        dt.Columns.Add("UtaxName", typeof(string));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("PhaseID", typeof(int));
        dt.Columns.Add("ItemID", typeof(int));

        dt.Columns.Add("AcctNo", typeof(string));
        dt.Columns.Add("JobName", typeof(string));
        dt.Columns.Add("Phase", typeof(string));
        dt.Columns.Add("UName", typeof(string));
        dt.Columns.Add("UtaxGL", typeof(Int32));
        dt.Columns.Add("ItemDesc", typeof(string));
        dt.Columns.Add("TypeID", typeof(Int32));
        dt.Columns.Add("Loc", typeof(string));
        dt.Columns.Add("TypeDesc", typeof(string));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("Ticket", typeof(Int32));
        dt.Columns.Add("OpSq", typeof(string));

        dt.Columns.Add("Warehouse", typeof(string));
        dt.Columns.Add("WHLocID", typeof(Int32));

        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("PrvInQuan", typeof(double));
        dt.Columns.Add("PrvIn", typeof(double));
        dt.Columns.Add("OutstandQuan", typeof(double));
        dt.Columns.Add("OutstandBalance", typeof(double));

        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("STaxName", typeof(string));
        dt.Columns.Add("STaxRate", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("STaxGL", typeof(Int32));
        dt.Columns.Add("GSTRate", typeof(double));
        dt.Columns.Add("GTaxAmt", typeof(double));
        dt.Columns.Add("GSTTaxGL", typeof(Int32));
        dt.Columns.Add("AmountTot", typeof(double));


        dt.Columns.Add("Warehousefdesc", typeof(string));
        dt.Columns.Add("Locationfdesc", typeof(string));
        dt.Columns.Add("IsPO", typeof(int));
        dt.Columns.Add("GTax", typeof(int));
        dt.Columns.Add("Price", typeof(double));

        dt.Columns.Add("OrderedQuan", typeof(double));
        dt.Columns.Add("Ordered", typeof(double));

        dt.Columns.Add("RPOItemId", typeof(int));
        dt.Columns.Add("POItemId", typeof(int));

        return dt;
    }

    protected void btnDownloadCSV_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportBills/Sample.csv");
    }

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/TempPDF/ImportBills/Sample.xls");
    }


    private void BINDGRID(DataTable dt)
    {
        DataColumnCollection columns = dt.Columns;
        if (!columns.Contains("AmountTot"))
        {
            //dt.Columns.Add("AmountTot", typeof(double), "STaxAmt + GTaxAmt + Amount");
            //dt.AcceptChanges();

            dt.Columns.Add(new DataColumn("AmountTot"));
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                double _sTaxAmt = 0;
                double _gTaxAmt = 0;
                double _sAmt = 0;
                if (!string.IsNullOrEmpty(dt.Rows[i]["STaxAmt"].ToString()))
                {
                    _sTaxAmt = Convert.ToDouble(dt.Rows[i]["STaxAmt"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["GTaxAmt"].ToString()))
                {
                    _gTaxAmt = Convert.ToDouble(dt.Rows[i]["GTaxAmt"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["Amount"].ToString()))
                {
                    _sAmt = Convert.ToDouble(dt.Rows[i]["Amount"].ToString());
                }
                dt.Rows[i]["AmountTot"] = _sAmt + _sTaxAmt + _gTaxAmt;
            }
            dt.AcceptChanges();



        }


        RadGrid_gvJobCostItems.DataSource = dt;
        //_objPropGeneral.ConnConfig = Session["config"].ToString();
        //_objPropGeneral.CustomName = "Country";

        //_getCustomFields.ConnConfig = Session["config"].ToString();
        //_getCustomFields.CustomName = "Country";

        //DataSet dsCustom = new DataSet();
        //List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();


        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetCustomFields";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

        //    _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
        //    dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        //}
        //else
        //{
        //    dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);
        //}

        //if (dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
        //    {
        //        //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
        //        //RadGrid_gvJobCostItems.Columns[13].Visible = true;
        //        RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
        //        RadGrid_gvJobCostItems.Columns[14].Visible = true;
        //        RadGrid_gvJobCostItems.Columns[13].Visible = true;
        //        txtgstgv.Visible = true;
        //        spansalestax.InnerText = "PST Tax";

        //        ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
        //        string gst_gstgl = "";
        //        string gst_gstrate = "";
        //        _objPropGeneral.ConnConfig = Session["config"].ToString();

        //        _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

        //        DataSet _dsCustom = new DataSet();
        //        List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();

        //        if (IsAPIIntegrationEnable == "YES")
        //        //if (Session["APAPIEnable"].ToString() == "YES")
        //        {
        //            string APINAME = "BillAPI/AddBills_GetCustomFieldsControl";

        //            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

        //            _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
        //            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);
        //        }
        //        else
        //        {
        //            _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
        //        }

        //        if (_dsCustom.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
        //            {
        //                if (_dr["Name"].ToString().Equals("GSTGL"))
        //                {
        //                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //                    {
        //                        _objChart.ConnConfig = Session["config"].ToString();
        //                        _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

        //                        _GetChart.ConnConfig = Session["config"].ToString();
        //                        _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

        //                        DataSet _dsChart = new DataSet();

        //                        List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

        //                        if (IsAPIIntegrationEnable == "YES")
        //                        //if (Session["APAPIEnable"].ToString() == "YES")
        //                        {
        //                            string APINAME = "BillAPI/AddBills_GetChart";

        //                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChart);

        //                            _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
        //                            _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);
        //                        }
        //                        else
        //                        {
        //                            _dsChart = _objBLChart.GetChart(_objChart);
        //                        }

        //                        if (_dsChart.Tables[0].Rows.Count > 0)
        //                        {
        //                            //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
        //                            gst_gstgl = _dr["Label"].ToString();
        //                        }

        //                    }
        //                    else
        //                    {
        //                        gst_gstgl = "0";
        //                    }
        //                }
        //                else if (_dr["Name"].ToString().Equals("GSTRate"))
        //                {
        //                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
        //                    {
        //                        gst_gstrate = _dr["Label"].ToString();

        //                    }
        //                    else
        //                    {
        //                        gst_gstrate = "0";
        //                    }
        //                }

        //            }

        //        }

        //        if (gst_gstrate == "")
        //        {
        //            gst_gstrate = "0";
        //        }
        //        if (gst_gstrate == "0" || gst_gstrate == "0.0000")
        //        {
        //            spansalestax.InnerText = "Sales Tax";
        //            RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
        //        }
        //        ////////////////////////////////////////////////////////

        //        //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
        //    }
        //    else
        //    {
        //        RadGrid_gvJobCostItems.Columns[13].Visible = false;
        //        RadGrid_gvJobCostItems.Columns[14].Visible = false;
        //        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
        //        txtgstgv.Visible = false;
        //        spansalestax.InnerText = "Sales Tax";
        //    }
        //}
        //else
        //{
        //    RadGrid_gvJobCostItems.Columns[13].Visible = false;
        //    RadGrid_gvJobCostItems.Columns[14].Visible = false;
        //    RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
        //    txtgstgv.Visible = false;
        //    spansalestax.InnerText = "Sales Tax";
        //}

        //BusinessEntity.User objProp_User = new BusinessEntity.User();
        //DataSet ds = new DataSet();
        //objProp_User.ConnConfig = Session["config"].ToString();
        //_getConnectionConfig.ConnConfig = Session["config"].ToString();

        //List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetControl";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

        //    _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
        //    ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        //}
        //else
        //{
        //    ds = _objBLUser.getControl(objProp_User);
        //}

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
        //    {
        //        RadGrid_gvJobCostItems.Columns[12].Visible = false;
        //    }
        //    else
        //    {
        //        RadGrid_gvJobCostItems.Columns[12].Visible = true;
        //    }

        //    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSalesTaxAPBills"].ToString()) ==false)
        //    {
        //        RadGrid_gvJobCostItems.Columns[15].Visible = false;
        //        RadGrid_gvJobCostItems.Columns[16].Visible = false;
        //        txtgstgv.Visible = false;
        //        RadGrid_gvJobCostItems.Columns[14].Visible = false;
        //        RadGrid_gvJobCostItems.Columns[13].Visible = false;
        //        hdnQST.Value = "0";
        //        hdnQSTGL.Value = "0";
        //        hdnSTaxType.Value = "";
        //        hdnSTaxName.Value = "";

        //    }
        //    else
        //    {

        //        RadGrid_gvJobCostItems.Columns[16].Visible = true;
        //        RadGrid_gvJobCostItems.Columns[15].Visible = true;
        //        if (txtgstgv.Visible == true)
        //        {
        //            txtgstgv.Visible = true;
        //            RadGrid_gvJobCostItems.Columns[13].Visible = true;
        //            RadGrid_gvJobCostItems.Columns[14].Visible = true;
        //        }


        //        if (txtgstgv.Visible == true)
        //        {

        //            if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
        //            {
        //                txtgstgv.Visible = false;
        //                RadGrid_gvJobCostItems.Columns[13].Visible = false;
        //                RadGrid_gvJobCostItems.Columns[14].Visible = false;
        //            }
        //        }



        //    }

        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _getCustomFields.ConnConfig = Session["config"].ToString();
            _getCustomFields.CustomName = "Country";

            DataSet dsCustom = new DataSet();
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            string APINAME = "BillAPI/AddBills_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);


            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
                    //RadGrid_gvJobCostItems.Columns[13].Visible = true;
                    RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
                    RadGrid_gvJobCostItems.Columns[14].Visible = true;
                    RadGrid_gvJobCostItems.Columns[13].Visible = true;
                    txtgstgv.Visible = true;
                    spansalestax.InnerText = "PST Tax";

                    ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                    string gst_gstgl = "";
                    string gst_gstrate = "";


                    _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

                    DataSet _dsCustom = new DataSet();
                    List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();

                    string APINAME1 = "BillAPI/AddBills_GetCustomFieldsControl";

                    APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _getCustomFieldsControl);

                    _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse1.ResponseData);
                    _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);


                    if (_dsCustom.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        {
                            if (_dr["Name"].ToString().Equals("GSTGL"))
                            {
                                if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                {
                                    _GetChart.ConnConfig = Session["config"].ToString();
                                    _GetChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                    DataSet _dsChart = new DataSet();

                                    List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();


                                    string APINAME2 = "BillAPI/AddBills_GetChart";

                                    APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _GetChart);

                                    _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse2.ResponseData);
                                    _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);


                                    if (_dsChart.Tables[0].Rows.Count > 0)
                                    {
                                        //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                        gst_gstgl = _dr["Label"].ToString();
                                    }

                                }
                                else
                                {
                                    gst_gstgl = "0";
                                }
                            }
                            else if (_dr["Name"].ToString().Equals("GSTRate"))
                            {
                                if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                {
                                    gst_gstrate = _dr["Label"].ToString();

                                }
                                else
                                {
                                    gst_gstrate = "0";
                                }
                            }

                        }

                    }

                    if (gst_gstrate == "")
                    {
                        gst_gstrate = "0";
                    }
                    if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                    {
                        spansalestax.InnerText = "Sales Tax";
                        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                    }
                    ////////////////////////////////////////////////////////

                    //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                }
                else
                {
                    RadGrid_gvJobCostItems.Columns[13].Visible = false;
                    RadGrid_gvJobCostItems.Columns[14].Visible = false;
                    RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                    txtgstgv.Visible = false;
                    spansalestax.InnerText = "Sales Tax";
                }
            }
            else
            {
                RadGrid_gvJobCostItems.Columns[13].Visible = false;
                RadGrid_gvJobCostItems.Columns[14].Visible = false;
                RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                txtgstgv.Visible = false;
                spansalestax.InnerText = "Sales Tax";
            }

            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet ds = new DataSet();

            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();


            string APINAME3 = "BillAPI/AddBills_GetControl";

            APIResponse _APIResponse3 = new MOMWebUtility().CallMOMWebAPI(APINAME3, _getConnectionConfig);

            _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse3.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                {
                    RadGrid_gvJobCostItems.Columns[12].Visible = false;
                }
                else
                {
                    RadGrid_gvJobCostItems.Columns[12].Visible = true;
                }

                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSalesTaxAPBills"].ToString()) == false)
                {
                    RadGrid_gvJobCostItems.Columns[15].Visible = false;
                    RadGrid_gvJobCostItems.Columns[16].Visible = false;
                    txtgstgv.Visible = false;
                    RadGrid_gvJobCostItems.Columns[14].Visible = false;
                    RadGrid_gvJobCostItems.Columns[13].Visible = false;
                    hdnQST.Value = "0";
                    hdnQSTGL.Value = "0";
                    hdnSTaxType.Value = "";
                    hdnSTaxName.Value = "";

                }
                else
                {

                    RadGrid_gvJobCostItems.Columns[16].Visible = true;
                    RadGrid_gvJobCostItems.Columns[15].Visible = true;
                    if (txtgstgv.Visible == true)
                    {
                        txtgstgv.Visible = true;
                        RadGrid_gvJobCostItems.Columns[13].Visible = true;
                        RadGrid_gvJobCostItems.Columns[14].Visible = true;
                    }


                    if (txtgstgv.Visible == true)
                    {

                        if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                        {
                            txtgstgv.Visible = false;
                            RadGrid_gvJobCostItems.Columns[13].Visible = false;
                            RadGrid_gvJobCostItems.Columns[14].Visible = false;
                        }
                    }



                }

            }
        }
        else
        {
            _objPropGeneral.ConnConfig = Session["config"].ToString();
            _objPropGeneral.CustomName = "Country";

            DataSet dsCustom = new DataSet();

            dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);


            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
                    //RadGrid_gvJobCostItems.Columns[13].Visible = true;
                    RadGrid_gvJobCostItems.Columns[16].HeaderText = "PST Tax";
                    RadGrid_gvJobCostItems.Columns[14].Visible = true;
                    RadGrid_gvJobCostItems.Columns[13].Visible = true;
                    txtgstgv.Visible = true;
                    spansalestax.InnerText = "PST Tax";

                    ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                    string gst_gstgl = "";
                    string gst_gstrate = "";
                    _objPropGeneral.ConnConfig = Session["config"].ToString();

                    DataSet _dsCustom = new DataSet();

                    _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);


                    if (_dsCustom.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                        {
                            if (_dr["Name"].ToString().Equals("GSTGL"))
                            {
                                if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                {
                                    _objChart.ConnConfig = Session["config"].ToString();
                                    _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                    DataSet _dsChart = new DataSet();
                                    _dsChart = _objBLChart.GetChart(_objChart);


                                    if (_dsChart.Tables[0].Rows.Count > 0)
                                    {
                                        //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                        gst_gstgl = _dr["Label"].ToString();
                                    }

                                }
                                else
                                {
                                    gst_gstgl = "0";
                                }
                            }
                            else if (_dr["Name"].ToString().Equals("GSTRate"))
                            {
                                if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                                {
                                    gst_gstrate = _dr["Label"].ToString();

                                }
                                else
                                {
                                    gst_gstrate = "0";
                                }
                            }

                        }

                    }

                    if (gst_gstrate == "")
                    {
                        gst_gstrate = "0";
                    }
                    if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                    {
                        spansalestax.InnerText = "Sales Tax";
                        RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                    }
                    ////////////////////////////////////////////////////////

                    //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                }
                else
                {
                    RadGrid_gvJobCostItems.Columns[13].Visible = false;
                    RadGrid_gvJobCostItems.Columns[14].Visible = false;
                    RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                    txtgstgv.Visible = false;
                    spansalestax.InnerText = "Sales Tax";
                }
            }
            else
            {
                RadGrid_gvJobCostItems.Columns[13].Visible = false;
                RadGrid_gvJobCostItems.Columns[14].Visible = false;
                RadGrid_gvJobCostItems.Columns[16].HeaderText = "Sales Tax Amount";
                txtgstgv.Visible = false;
                spansalestax.InnerText = "Sales Tax";
            }

            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            ds = _objBLUser.getControl(objProp_User);


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
                {
                    RadGrid_gvJobCostItems.Columns[12].Visible = false;
                }
                else
                {
                    RadGrid_gvJobCostItems.Columns[12].Visible = true;
                }

                if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSalesTaxAPBills"].ToString()) == false)
                {
                    RadGrid_gvJobCostItems.Columns[15].Visible = false;
                    RadGrid_gvJobCostItems.Columns[16].Visible = false;
                    txtgstgv.Visible = false;
                    RadGrid_gvJobCostItems.Columns[14].Visible = false;
                    RadGrid_gvJobCostItems.Columns[13].Visible = false;
                    hdnQST.Value = "0";
                    hdnQSTGL.Value = "0";
                    hdnSTaxType.Value = "";
                    hdnSTaxName.Value = "";

                }
                else
                {

                    RadGrid_gvJobCostItems.Columns[16].Visible = true;
                    RadGrid_gvJobCostItems.Columns[15].Visible = true;
                    if (txtgstgv.Visible == true)
                    {
                        txtgstgv.Visible = true;
                        RadGrid_gvJobCostItems.Columns[13].Visible = true;
                        RadGrid_gvJobCostItems.Columns[14].Visible = true;
                    }


                    if (txtgstgv.Visible == true)
                    {

                        if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                        {
                            txtgstgv.Visible = false;
                            RadGrid_gvJobCostItems.Columns[13].Visible = false;
                            RadGrid_gvJobCostItems.Columns[14].Visible = false;
                        }
                    }



                }

            }

        }

        RadGrid_gvJobCostItems.DataBind();

        if (Request.QueryString["t"] != null)
        {
            if (Request.QueryString["t"].ToString() == "c")
            {
                isCopy = true;
            }
        }

        foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)//Project Expense grid 
        {

            TextBox txtGvQuan = (TextBox)gr.FindControl("txtGvQuan");
            TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
            TextBox txtGvPrice = (TextBox)gr.FindControl("txtGvPrice");

            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");
            TextBox txtGvUseTax = (TextBox)gr.FindControl("txtGvUseTax");

            HiddenField hdnIsPO = (HiddenField)gr.FindControl("hdnIsPO");

            string sdr = Convert.ToString(gr["Amount"].Text.ToString());

            ImageButton ibDelete = (ImageButton)gr.FindControl("ibDelete");
            if (txtGvQuan.Text != "" && txtGvAmount.Text != "")
            {
                double Qty = 0; double Amount = 0; double Price = 0;

                double.TryParse(txtGvQuan.Text, out Qty);
                double.TryParse(txtGvAmount.Text, out Amount);
                double.TryParse(txtGvPrice.Text, out Price);


                if (Qty == 0)
                {
                    txtGvPrice.Text = string.Empty;
                }
                else
                {
                    if (Price == 0)
                    {
                        Price = (Amount) / (Qty);

                        txtGvPrice.Text = Price.ToString("0.00");
                        if (hdnIsPO.Value == "0")
                        {
                            txtGvPrice.Text = Convert.ToString(Math.Truncate(100 * Price) / 100);
                        }
                        if (hdnIsPO.Value == "2" || hdnIsPO.Value == "0")
                        {
                            txtGvPrice.Text = DataBinder.Eval(gr.DataItem, "Price").ToString();
                        }
                    }
                }
            }
            if (hdnIsPO.Value == "0")
            //if (txtReceptionId.Text != "" && txtPO.Text != "")
            {
                txtGvQuan.ReadOnly = true;
                //txtGvAmount.ReadOnly = true;
                //txtGvPrice.ReadOnly = true;
                ibDelete.Visible = false;
            }
            if (hdnIsPO.Value == "2")
            {
                if (rdbyAmt.Checked == true)
                {
                    txtGvQuan.ReadOnly = true;
                    txtGvPrice.ReadOnly = true;
                }
                if (rdbyQty.Checked == true)
                {
                    txtGvPrice.ReadOnly = true;
                    txtGvAmount.ReadOnly = true;
                }

            }
            if (hdnStatus.Value.Trim() == "1")
            {
                if (isCopy != true) //  check if bill is paid or not
                {
                    txtGvQuan.ReadOnly = true;
                    txtGvPrice.ReadOnly = true;
                    txtGvAmount.ReadOnly = true;

                    //bool chkperiod = Convert.ToBoolean(ViewState["FlagPeriodClose"]);
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
                    bool chkperiod = (bool)ViewState["FlagPeriodClose"];

                    if (chkperiod == false)
                    {
                        txtGvAcctNo.ReadOnly = true;
                        txtGvUseTax.ReadOnly = true;
                        txtTotalUseTax.Enabled = false;
                    }
                }

            }


        }

        //if (txtReceptionId.Text != "" && txtPO.Text != "")
        //{
        //    RadGrid_gvJobCostItems.Columns[19].Visible = false;
        //}


    }

    private void GetInvDefaultAcct()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        //_objPropGeneral.ConnConfig = Session["config"].ToString();
        //_GetInvDefaultAcct.ConnConfig = Session["config"].ToString();
        //DataSet _dsDefaultAccount = new DataSet();
        //List<GeneralViewModel> _lstGeneralViewModel = new List<GeneralViewModel>();

        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetInvDefaultAcct";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvDefaultAcct);

        //    _lstGeneralViewModel = (new JavaScriptSerializer()).Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
        //    _dsDefaultAccount = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneralViewModel);
        //}
        //else
        //{
        //    _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
        //}

        //if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
        //{
        //    hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
        //    hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _GetInvDefaultAcct.ConnConfig = Session["config"].ToString();
            DataSet _dsDefaultAccount = new DataSet();
            List<GeneralViewModel> _lstGeneralViewModel = new List<GeneralViewModel>();


            string APINAME = "BillAPI/AddBills_GetInvDefaultAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvDefaultAcct);

            _lstGeneralViewModel = (new JavaScriptSerializer()).Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
            _dsDefaultAccount = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneralViewModel);


            if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
            {
                hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
            }
        }
        else
        {
            _objPropGeneral.ConnConfig = Session["config"].ToString();

            DataSet _dsDefaultAccount = new DataSet();

            _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);

            if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
            {
                hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
                hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
            }
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
                //_objPJ.ConnConfig = Session["config"].ToString();
                //_objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                //_GetBillsLogs.ConnConfig = Session["config"].ToString();
                //_GetBillsLogs.ID = Convert.ToInt32(Request.QueryString["id"]);

                //List<LogViewModel> _lstLogViewModel = new List<LogViewModel>();

                //if (IsAPIIntegrationEnable == "YES")
                ////if (Session["APAPIEnable"].ToString() == "YES")
                //{
                //    string APINAME = "BillAPI/AddBills_GetBillsLogs";

                //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillsLogs);

                //    _lstLogViewModel = (new JavaScriptSerializer()).Deserialize<List<LogViewModel>>(_APIResponse.ResponseData);
                //    dsLog = CommonMethods.ToDataSet<LogViewModel>(_lstLogViewModel);
                //}
                //else
                //{
                //    dsLog = _objBLBills.GetBillsLogs(_objPJ);
                //}

                //if (dsLog.Tables[0].Rows.Count > 0)
                //{
                //    RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                //    RadGrid_gvLogs.DataSource = dsLog.Tables[0];
                //}
                //else
                //{
                //    RadGrid_gvLogs.DataSource = string.Empty;
                //}

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    _GetBillsLogs.ConnConfig = Session["config"].ToString();
                    _GetBillsLogs.ID = Convert.ToInt32(Request.QueryString["id"]);

                    List<LogViewModel> _lstLogViewModel = new List<LogViewModel>();

                    string APINAME = "BillAPI/AddBills_GetBillsLogs";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillsLogs);

                    _lstLogViewModel = (new JavaScriptSerializer()).Deserialize<List<LogViewModel>>(_APIResponse.ResponseData);
                    dsLog = CommonMethods.ToDataSet<LogViewModel>(_lstLogViewModel);


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
                    _objPJ.ConnConfig = Session["config"].ToString();
                    _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                    dsLog = _objBLBills.GetBillsLogs(_objPJ);

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

    private void fillPOCustom()
    {
        try
        {
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("PO1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lb_txtCustom1.InnerHtml = dscstm.Tables[0].Rows[0]["label"].ToString() == "" ? "Custom 1" : dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            dscstm = GetCustomFields("PO2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lb_txtCustom2.InnerHtml = dscstm.Tables[0].Rows[0]["label"].ToString() == "" ? "Custom 2" : dscstm.Tables[0].Rows[0]["label"].ToString();
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

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            _getCustomFields.CustomName = name;
            _getCustomFields.ConnConfig = Session["config"].ToString();

            string APINAME = "BillAPI/AddBills_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            objGeneral.CustomName = name;
            objGeneral.ConnConfig = Session["config"].ToString();

            ds = objBL_General.getCustomFields(objGeneral);
        }

        return ds;
    }
    private void BindBills()
    {
        DataSet dsPaymentLog = new DataSet();
        //_objPJ.ConnConfig = Session["config"].ToString();
        //_objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

        //_GetBillHistoryPayment.ConnConfig = Session["config"].ToString();
        //_GetBillHistoryPayment.ID = Convert.ToInt32(Request.QueryString["id"]);

        //List<GetBillHistoryPaymentViewModel> _lstGetBillHistoryPayment = new List<GetBillHistoryPaymentViewModel>();


        //if (IsAPIIntegrationEnable == "YES")
        ////if (Session["APAPIEnable"].ToString() == "YES")
        //{
        //    string APINAME = "BillAPI/AddBills_GetBillHistoryPayment";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillHistoryPayment);

        //    _lstGetBillHistoryPayment = (new JavaScriptSerializer()).Deserialize<List<GetBillHistoryPaymentViewModel>>(_APIResponse.ResponseData);
        //    dsPaymentLog = CommonMethods.ToDataSet<GetBillHistoryPaymentViewModel>(_lstGetBillHistoryPayment);
        //}
        //else
        //{
        //    dsPaymentLog = _objBLBills.GetBillHistoryPayment(_objPJ);
        //}

        //if (dsPaymentLog.Tables[0].Rows.Count > 0)
        //{
        //    sum = Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString());
        //   // RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];
        //    //GridFooterItem footerItem = (GridFooterItem)RadGrid_gvPayment.MasterTableView.GetItems(GridItemType.Footer)[0];
        //    //Label lblTotalPaymentHistory = (Label)footerItem.FindControl("lblTo");
        //    //lblTotalPaymentHistory.Text = string.Format("{0:c}", Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString()));
        //}

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            _GetBillHistoryPayment.ConnConfig = Session["config"].ToString();
            _GetBillHistoryPayment.ID = Convert.ToInt32(Request.QueryString["id"]);

            List<GetBillHistoryPaymentViewModel> _lstGetBillHistoryPayment = new List<GetBillHistoryPaymentViewModel>();

            string APINAME = "BillAPI/AddBills_GetBillHistoryPayment";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillHistoryPayment);

            _lstGetBillHistoryPayment = (new JavaScriptSerializer()).Deserialize<List<GetBillHistoryPaymentViewModel>>(_APIResponse.ResponseData);
            dsPaymentLog = CommonMethods.ToDataSet<GetBillHistoryPaymentViewModel>(_lstGetBillHistoryPayment);


            if (dsPaymentLog.Tables[0].Rows.Count > 0)
            {
                sum = Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString());
                // RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];
                //GridFooterItem footerItem = (GridFooterItem)RadGrid_gvPayment.MasterTableView.GetItems(GridItemType.Footer)[0];
                //Label lblTotalPaymentHistory = (Label)footerItem.FindControl("lblTo");
                //lblTotalPaymentHistory.Text = string.Format("{0:c}", Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString()));
            }
        }
        else
        {
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

            dsPaymentLog = _objBLBills.GetBillHistoryPayment(_objPJ);

            if (dsPaymentLog.Tables[0].Rows.Count > 0)
            {
                sum = Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString());
                // RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];
                //GridFooterItem footerItem = (GridFooterItem)RadGrid_gvPayment.MasterTableView.GetItems(GridItemType.Footer)[0];
                //Label lblTotalPaymentHistory = (Label)footerItem.FindControl("lblTo");
                //lblTotalPaymentHistory.Text = string.Format("{0:c}", Convert.ToDouble(dsPaymentLog.Tables[0].Compute("sum(Amount)", "").ToString()));
            }
        }
    }
    protected void RadGrid_gvPayment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        BindBills();
        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            (footer["Amount"].FindControl("lblTo") as Label).Text = string.Format("{0:c}", sum);
            if (sum < 0)
            {
                (footer["Amount"].FindControl("lblTo") as Label).ForeColor = System.Drawing.Color.Red;
            }
            //clientID = (footer["Template1"].FindControl("TextBox2") as TextBox).ClientID;
        }
    }
    protected void RadGrid_gvPayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            if (Request.QueryString["id"] != null)
            {
                DataSet dsPaymentLog = new DataSet();
                //_objPJ.ConnConfig = Session["config"].ToString();
                //_objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                //_GetBillHistoryPayment.ConnConfig = Session["config"].ToString();
                //_GetBillHistoryPayment.ID = Convert.ToInt32(Request.QueryString["id"]);

                //List<GetBillHistoryPaymentViewModel> _lstGetBillHistoryPayment = new List<GetBillHistoryPaymentViewModel>();

                //if (IsAPIIntegrationEnable == "YES")
                ////if (Session["APAPIEnable"].ToString() == "YES")
                //{
                //    string APINAME = "BillAPI/AddBills_GetBillHistoryPayment";

                //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillHistoryPayment);

                //    _lstGetBillHistoryPayment = (new JavaScriptSerializer()).Deserialize<List<GetBillHistoryPaymentViewModel>>(_APIResponse.ResponseData);
                //    dsPaymentLog = CommonMethods.ToDataSet<GetBillHistoryPaymentViewModel>(_lstGetBillHistoryPayment);
                //}
                //else
                //{
                //    dsPaymentLog = _objBLBills.GetBillHistoryPayment(_objPJ);
                //}

                //if (dsPaymentLog.Tables[0].Rows.Count > 0)
                //{
                //    RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];

                //}
                //else
                //{
                //    RadGrid_gvPayment.DataSource = string.Empty;
                //}

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    _GetBillHistoryPayment.ConnConfig = Session["config"].ToString();
                    _GetBillHistoryPayment.ID = Convert.ToInt32(Request.QueryString["id"]);

                    List<GetBillHistoryPaymentViewModel> _lstGetBillHistoryPayment = new List<GetBillHistoryPaymentViewModel>();

                    string APINAME = "BillAPI/AddBills_GetBillHistoryPayment";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBillHistoryPayment);

                    _lstGetBillHistoryPayment = (new JavaScriptSerializer()).Deserialize<List<GetBillHistoryPaymentViewModel>>(_APIResponse.ResponseData);
                    dsPaymentLog = CommonMethods.ToDataSet<GetBillHistoryPaymentViewModel>(_lstGetBillHistoryPayment);

                    if (dsPaymentLog.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];

                    }
                    else
                    {
                        RadGrid_gvPayment.DataSource = string.Empty;
                    }
                }
                else
                {
                    _objPJ.ConnConfig = Session["config"].ToString();
                    _objPJ.ID = Convert.ToInt32(Request.QueryString["id"]);

                    dsPaymentLog = _objBLBills.GetBillHistoryPayment(_objPJ);

                    if (dsPaymentLog.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];

                    }
                    else
                    {
                        RadGrid_gvPayment.DataSource = string.Empty;
                    }

                }
            }
        }
        catch (Exception ex) { }
    }
    private void FillFrequency()
    {
        try
        {
            List<Frequency> _lstFrequency = new List<Frequency>();
            _lstFrequency = FrequencyHelper.GetAll();
            ddlFrequency.DataSource = _lstFrequency;
            ddlFrequency.DataValueField = "ID";
            ddlFrequency.DataTextField = "Name";
            ddlFrequency.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnApplyCredit_Click(object sender, EventArgs e)
    {
        try
        {
            int rpjid = 0;
            if (Request.QueryString["id"] != null)
            {
                rpjid = Convert.ToInt32(Request.QueryString["id"]);
            }

            bool Flag = false;

            GetPeriodDetails(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtPostingDate.Text));
            Flag = (bool)ViewState["FlagPeriodClose"];
            Flag = true; //Credit Apply Should be Allow in Back Date.
            if (Flag == true)
            {
                //_objCD.ConnConfig = Session["config"].ToString();
                //_objCD.fDate = Convert.ToDateTime(txtaplyDate.Text);
                //_objCD.EndDate = Convert.ToDateTime(hdnolddate.Value);
                //_objCD.TransID = Convert.ToInt32(rpjid);

                //_UpdateApplyCreditDate.ConnConfig = Session["config"].ToString();
                //_UpdateApplyCreditDate.fDate = Convert.ToDateTime(txtaplyDate.Text);
                //_UpdateApplyCreditDate.EndDate = Convert.ToDateTime(hdnolddate.Value);
                //_UpdateApplyCreditDate.TransID = Convert.ToInt32(rpjid);


                //if (IsAPIIntegrationEnable == "YES")
                ////if (Session["APAPIEnable"].ToString() == "YES")
                //{
                //    string APINAME = "BillAPI/AddBills_UpdateApplyCreditDate";

                //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateApplyCreditDate);
                //}
                //else
                //{
                //    _objBLBills.UpdateApplyCreditDate(_objCD);
                //}

                //ResetFormControlValues(this);
                //SetBillForm();
                //SetTax();
                //txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
                //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Apply credit date updated successfully! <BR/> <b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                //Page.Response.Redirect(Page.Request.Url.ToString(), true);

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    _UpdateApplyCreditDate.ConnConfig = Session["config"].ToString();
                    _UpdateApplyCreditDate.fDate = Convert.ToDateTime(txtaplyDate.Text);
                    _UpdateApplyCreditDate.EndDate = Convert.ToDateTime(hdnolddate.Value);
                    _UpdateApplyCreditDate.TransID = Convert.ToInt32(rpjid);

                    string APINAME = "BillAPI/AddBills_UpdateApplyCreditDate";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateApplyCreditDate);

                    ResetFormControlValues(this);
                    SetBillForm();
                    SetTax();
                    txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Apply credit date updated successfully! <BR/> <b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                else
                {
                    _objCD.ConnConfig = Session["config"].ToString();
                    _objCD.fDate = Convert.ToDateTime(txtaplyDate.Text);
                    _objCD.EndDate = Convert.ToDateTime(hdnolddate.Value);
                    _objCD.TransID = Convert.ToInt32(rpjid);

                    _objBLBills.UpdateApplyCreditDate(_objCD);

                    ResetFormControlValues(this);
                    SetBillForm();
                    SetTax();
                    txtVendor.Enabled = true; // ES-3168 Able to create bill with PO and change the vender (AZHAR-02-01-2020)
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Apply credit date updated successfully! <BR/> <b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }

            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Documents
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

            //var mainDirectory = string.Empty;

            //if (Request.QueryString["id"] != null)
            //{
            //    mainDirectory = Request.QueryString["id"].ToString();
            //}
            //else
            //{

            //    if (ViewState["TempUploadDirectory"] == null)
            //    {
            //        ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
            //    }

            //    mainDirectory = ViewState["TempUploadDirectory"] as string;
            //}
            var mainDirectory = "APBillsDocs";

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                mainDirectory += "\\APBill_" + Request.QueryString["id"];
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
            //savepath = GetUploadDirectory();


            if (Request.QueryString["id"] != null)
            {


                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
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


                    objMapData.Screen = "APBills";
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
                }
                UpdateDocInfo();
                //GetDocuments();
                RadGrid_Documents.Rebind();
                //RadGrid_gvLogs.Rebind();
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

                    tempTable = SaveAttachedFilesWhenAddingAPBill(filename, fullpath, mime);
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

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
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
            objMapData.Screen = "APBills";
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

    private DataTable SaveAttachedFilesWhenAddingAPBill(string fileName, string fullPath, string doctype)
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
            else
            {
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

    private void UpdateTempDateWhenCreatingNewAPBill(string strAPBillId)
    {
        var apBillId = Convert.ToInt32(strAPBillId);
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;
        var tempDirectory = "APBillsDocs\\" + ViewState["TempUploadDirectory"] as string;
        var newDirectory = "APBillsDocs\\" + "APBill_" + strAPBillId;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var sourceDirectory = GetUploadDirectory(tempDirectory);
        var destDirectory = GetUploadDirectory(newDirectory);
        Directory.Move(sourceDirectory, destDirectory);

        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            objMapData.Screen = "APBills";
            objMapData.TicketID = apBillId;
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
        objMapData.Screen = "APBills";
        objMapData.TicketID = apBillId;
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

    #endregion
}