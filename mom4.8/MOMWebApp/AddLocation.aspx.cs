using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.IO;
using System.Data.Odbc;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessLayer.Schedule;
using BusinessEntity.payroll;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;
using BusinessEntity.CustomersModel;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

public partial class AddLocation : System.Web.UI.Page
{
    DataTable dtContracts = new DataTable();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    //consult
    BusinessEntity.tblConsult objProp_Consult = new BusinessEntity.tblConsult();
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_Invoice objBL_Invoice = new BL_Invoice();

    BL_Alerts objBL_Alerts = new BL_Alerts();
    Alerts objAlerts = new Alerts();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private bool isShowAllInvoices = false;
    private double PrevRuntotal = 0.00;

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetSingleConsultantParam _GetSingleConsultant = new GetSingleConsultantParam();
    GetLocationByIDParam _GetLocationByID = new GetLocationByIDParam();
    GetCustomerByIDParam _GetCustomerByID = new GetCustomerByIDParam();
    GetGCandHowerLocIDParam _GetGCandHowerLocID = new GetGCandHowerLocIDParam();
    GetTermsParam _getTerms = new GetTermsParam();
    GetProspectByIDParam _GetProspectByID = new GetProspectByIDParam();
    GetCategoryParam _GetCategory = new GetCategoryParam();
    GetCustomersParam _GetCustomers = new GetCustomersParam();
    GetRouteParam _GetRoute = new GetRouteParam();
    getlocationTypeParam _getlocationType = new getlocationTypeParam();
    GetTerritoryParam _GetTerritory = new GetTerritoryParam();
    getSTaxParam _GetSTax = new getSTaxParam();
    getSalesTax2Param _getSalesTax2 = new getSalesTax2Param();
    getUseTaxParam _getUseTax = new getUseTaxParam();
    GetZoneParam _GetZone = new GetZoneParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    getCustomFieldsControlParam _getCustomFieldsControl = new getCustomFieldsControlParam();
    GetCompanyByCustomerParam _GetCompanyByCustomer = new GetCompanyByCustomerParam();
    IsExistContractByLocParam _IsExistContractByLoc = new IsExistContractByLocParam();
    UpdateLocationParam _UpdateLocation = new UpdateLocationParam();
    AddLocationParam _AddLocation = new AddLocationParam();
    ConvertLeadEquipmentParam _ConvertLeadEquipment = new ConvertLeadEquipmentParam();
    UpdateLocationContactRecordLogParam _UpdateLocationContactRecordLog = new UpdateLocationContactRecordLogParam();
    DeleteEquipmentParam _DeleteEquipment = new DeleteEquipmentParam();
    GetDepartmentParam _GetDepartment = new GetDepartmentParam();
    AddFileParam _AddFile = new AddFileParam();
    UpdateDocInfoParam _UpdateDocInfo = new UpdateDocInfoParam();
    GetLocationDocumentsParam _GetLocationDocuments = new GetLocationDocumentsParam();
    DeleteFileParam _DeleteFile = new DeleteFileParam();
    GetAlertTypeParam _GetAlertType = new GetAlertTypeParam();
    GetAlertsParam _GetAlerts = new GetAlertsParam();
    GetDefaultRouteTerrParam _GetDefaultRouteTerr = new GetDefaultRouteTerrParam();
    GetGCCustomerParam _GetGCCustomer = new GetGCCustomerParam();
    GetElevParam _GetElev = new GetElevParam();
    GetCallHistoryParam _GetCallHistory = new GetCallHistoryParam();
    GetARRevenueParam _GetARRevenue = new GetARRevenueParam();
    GetJobProjectParam _GetJobProject = new GetJobProjectParam();
    GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader = new GetDefaultWorkerHeaderParam();
    GetLocationLogParam _GetLocationLog = new GetLocationLogParam();
    GetContactLogByLocIDParam _GetContactLogByLocID = new GetContactLogByLocIDParam();
    GetLocContactByRolIDParam _GetLocContactByRolID = new GetLocContactByRolIDParam();
    GetBTParam _GetBT = new GetBTParam();
    DeleteOpportunityParam _DeleteOpportunity = new DeleteOpportunityParam();
    GetOpportunityNewParam _GetOpportunityNew = new GetOpportunityNewParam();
    spGetLocationServiceTypeinfoParam _GetLocationServiceTypeinfo = new spGetLocationServiceTypeinfoParam();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        PagePermission();
        FillAlertType();
        if (!IsPostBack)
        {
            HighlightSideMenu("cstmMgr", "lnkLocationsSMenu", "cstmMgrSub");
            CompanyPermission();
            GetQBInt();
            SalesTaxZonePermission();
           
            if (Session["InvToDate"] == null)
            {
                txtInvDtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                //txtInvDtTo.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtTo.Text = Session["InvToDate"].ToString();
                if (txtInvDtTo.Text.Trim() == "")
                {
                    txtInvDtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
            }
            if (Session["InvFromDate"] == null)
            {
                txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                //txtInvDtFrom.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtFrom.Text = Session["InvFromDate"].ToString();
                if (txtInvDtFrom.Text.Trim() == "")
                {
                    txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                }
            }


            objGeneral.ConnConfig = Session["config"].ToString();
            _getConnectionConfig.ConnConfig = Session["config"].ToString();
            DataSet dsLastSync = new DataSet();
            List <GeneralViewModel> _lstGeneral = new List<GeneralViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_GetSagelatsync";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGeneral = serializer.Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
                dsLastSync = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneral);
            }
            else
            {
                dsLastSync = objBL_General.getSagelatsync(objGeneral);
            }

            int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
            hdnSageIntegration.Value = intintegration.ToString();
            if (intintegration == 1)
            {
                btnSageID.Visible = true;
                //lblSageAddress.Visible = true;
                //RequiredFieldValidator20.Enabled = true;
            }
            else
            {
                btnSageID.Visible = false;
                //lblSageAddress.Visible = false;
            }
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("Loc1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            dscstm = GetCustomFields("Loc2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }

            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
            DateTime lastDay = firstDay.AddDays(DaysinMonth);

            //txtInvDtFrom.Text = firstDay.ToShortDateString();
            //txtInvDtTo.Text = lastDay.ToShortDateString();

            txtfromDate.Text = firstDay.ToShortDateString();
            txtToDate.Text = lastDay.ToShortDateString();
            liLocationHistory.Style["display"] = "none";
            adLocationHistory.Style["display"] = "none";
            liTransactions.Style["display"] = "none";
            adTransactions.Style["display"] = "none";
            liViewEquipments.Style["display"] = "none";
            adViewEquipments.Style["display"] = "none";
            liProjects.Style["display"] = "none";
            adProjects.Style["display"] = "none";
            liContacts.Style["display"] = "none";
            adContacts.Style["display"] = "none";
            liDocuments.Style["display"] = "none";
            adDocuments.Style["display"] = "none";

            liOpportunities.Style["display"] = "none";
            adOpportunities.Style["display"] = "none";

            liAlerts.Style["display"] = "none";
            adAlerts.Style["display"] = "none";

            //consultant
            DataSet dsConsult = new DataSet();
            objProp_Consult.ConnConfig = Session["config"].ToString();

            _GetSingleConsultant.ConnConfig = Session["config"].ToString();

            List<GetSingleConsultantViewModel> _lstGetSingleConsultant = new List<GetSingleConsultantViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_GetSingleConsultant";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSingleConsultant);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetSingleConsultant = serializer.Deserialize<List<GetSingleConsultantViewModel>>(_APIResponse.ResponseData);
                dsConsult = CommonMethods.ToDataSet<GetSingleConsultantViewModel>(_lstGetSingleConsultant);
            }
            else
            {
                dsConsult = objBL_User.getSingleConsultant(objProp_Consult);
            }

            ddlConsultant.DataSource = dsConsult.Tables[0];
            ddlConsultant.DataTextField = "Name";
            ddlConsultant.DataValueField = "ID";
            ddlConsultant.DataBind();
            ddlConsultant.Items.Insert(0, new ListItem("None", "0"));
            showHomeOwner();
            FillTerms();
            GetCustomerAll();
            FillRoute();
            Fillterritory();
            FillLocationType();
            FillSalesTax();
            FillSalesTax2();
            FillUseTax();
            FillZone();
            FillBusinessType();
            //FillWorker();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            ViewState["contacttableloc"] = null;
            CreateTable();
            FillCategory();
            FillDepartment();
            FillContractBill();
            lnkAddTicket.NavigateUrl = "addticket.aspx";
            makeAlertList();
            
            GetDefaultWorker();
            SetDefaultWorker();

            if (Request.QueryString["uid"] != null)
            {
                //RequiredFieldValidator20.Enabled = true;
                //GetInvoices("All", "All");
                lnkLocationTransLedger.Visible = true;
                lnkLocationHistory.Visible = true;
                ddlCompany.Visible = false;
                txtCompany.Visible = true;
                pnlNext.Visible = true;
                Page.Title = "Edit Location || MOM";
               if (Request.QueryString["t"] == null && Request.QueryString["t"] != "c")
                {
                    liLocationHistory.Style["display"] = "";
                    adLocationHistory.Style["display"] = "";
                    liTransactions.Style["display"] = "";
                    adTransactions.Style["display"] = "";
                    liViewEquipments.Style["display"] = "";
                    adViewEquipments.Style["display"] = "";
                    liProjects.Style["display"] = "";
                    adProjects.Style["display"] = "";
                    liContacts.Style["display"] = "";
                    adContacts.Style["display"] = "";
                    liDocuments.Style["display"] = "";
                    adDocuments.Style["display"] = "";
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    liOpportunities.Style["display"] = "";
                    adOpportunities.Style["display"] = "";
                    liAlerts.Style["display"] = "";
                    adAlerts.Style["display"] = "";
                }
               

                pnlDoc.Visible = true;
                lnkAddTicket.NavigateUrl = "AddTicket.aspx?locid=" + Request.QueryString["uid"].ToString();
                lnkAddProject.NavigateUrl = "AddProject.aspx?locid=" + Request.QueryString["uid"].ToString();
                
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Location";
                    //btnSageID.Visible = false;
                    //if (intintegration == 1)
                    //    txtAcctno.Enabled = false;
                }

                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetLocationByID.DBName = Session["dbname"].ToString();
                _GetLocationByID.ConnConfig = Session["config"].ToString();
                _GetLocationByID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetCustomerByID.DBName = Session["dbname"].ToString();
                _GetCustomerByID.ConnConfig = Session["config"].ToString();

                _GetGCandHowerLocID.ConnConfig = Session["config"].ToString();
                _GetGCandHowerLocID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataSet ds5 = new DataSet();

                ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetLocationByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

                    ds1 = _lstGetLocationByID.lstTable1.ToDataSet();
                    ds2 = _lstGetLocationByID.lstTable2.ToDataSet();
                    ds3 = _lstGetLocationByID.lstTable3.ToDataSet();
                    ds4 = _lstGetLocationByID.lstTable4.ToDataSet();
                    ds5 = _lstGetLocationByID.lstTable5.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    DataTable dt5 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];
                    dt4 = ds4.Tables[0];
                    dt5 = ds5.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    dt4.TableName = "Table4";
                    dt5.TableName = "Table5";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                }
                else
                {
                    ds = objBL_User.getLocationByID(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));

                    //consult
                    if(ds.Tables[0].Rows[0]["Consult"].ToString() == "" || int.Parse(ds.Tables[0].Rows[0]["Consult"].ToString()) == 0)
                    {
                        ddlConsultant.SelectedValue = "None";
                    }
                    else
                    {
                        ddlConsultant.SelectedValue = ds.Tables[0].Rows[0]["Consult"].ToString();
                    }
                    //ddlConsultant.SelectedValue = (int.Parse(ds.Tables[0].Rows[0]["Consult"].ToString()) != 0) ? ds.Tables[0].Rows[0]["Consult"].ToString() : "None";

                    if (ViewState["qbint"].ToString() == "1")
                    {
                        //if (ds.Tables[0].Rows[0]["qblocid"].ToString() == string.Empty)
                        //{
                        Label5.Text = Request.QueryString["uid"];
                        Label5.Visible = true;
                        Label4.Visible = true;
                    }
                    else
                    {
                        Label5.Text = string.Empty;
                        Label4.Visible = false;
                        Label5.Visible = false;
                    }
                    //}


                    if (ds.Tables[0].Rows[0]["PrintInvoice"].ToString() == "True")
                    {
                        chkPrintOnly.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["EmailInvoice"].ToString() == "True")
                    {
                        chkEmail.Checked = true;

                    }

                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["NoCustomerStatement"]))
                    {
                        chkNoCustStatement.Checked = true;

                    }


                    objPropUser.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[0]["owner"].ToString());

                    _GetCustomerByID.CustomerID = Convert.ToInt32(ds.Tables[0].Rows[0]["owner"].ToString());
                    DataSet dsCust = new DataSet();
                    DataSet dsCust1 = new DataSet();
                    DataSet dsCust2 = new DataSet();
                    DataSet dsCust3 = new DataSet();

                    ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "LocationsAPI/AddLocation_GetCustomerByID";

                        _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);
                        dsCust1 = _listGetCustomerByID.lstTable1.ToDataSet();
                        dsCust2 = _listGetCustomerByID.lstTable2.ToDataSet();
                        dsCust3 = _listGetCustomerByID.lstTable3.ToDataSet();

                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();
                        DataTable dt3 = new DataTable();

                        dt1 = dsCust1.Tables[0];
                        dt2 = dsCust2.Tables[0];
                        dt3 = dsCust3.Tables[0];

                        dt1.TableName = "Table1";
                        dt2.TableName = "Table2";
                        dt3.TableName = "Table3";
                        dsCust.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
                    }
                    else
                    {
                        dsCust = objBL_User.getCustomerByID(objPropUser);
                    }

                    //GetOpenCalls();
                    //GetDataEquip();
                    //FillProjects();

                    if (Convert.ToString(ds.Tables[0].Rows[0]["ID"]) != "")
                    {
                        lblLocationName.Text = "Account# " + ds.Tables[0].Rows[0]["ID"].ToString();
                    }



                    txtAcctno.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    hdnAcctID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                    txtLocName.Text = ds.Tables[0].Rows[0]["Tag"].ToString();
                    if (Convert.ToString(ds.Tables[0].Rows[0]["Tag"]) != "")
                    {
                        lblLocationNameLabel.Text = " | " + Convert.ToString(ds.Tables[0].Rows[0]["Tag"]);
                    }
                    txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();

                    hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();
                    hdnCustomerCountry.Value = dsCust.Tables[0].Rows[0]["Country"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString();

                    hdnCustomerCity.Value = dsCust.Tables[0].Rows[0]["City"].ToString();

                    //ddlState.SelectedValue = ds.Tables[0].Rows[0]["locstate"].ToString();
                    txtState.Text = ds.Tables[0].Rows[0]["locstate"].ToString();

                    hdnCustomerState.Value = dsCust.Tables[0].Rows[0]["state"].ToString();

                    txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();

                    hdnCustomerZipCode.Value = dsCust.Tables[0].Rows[0]["zip"].ToString();

                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["Route"].ToString();
                    ddlTerr.SelectedValue = ds.Tables[0].Rows[0]["terr"].ToString();
                    //Second SalesPerson
                    string Territory2 = string.Empty;

                    if (ds.Tables[0].Columns.Contains("terr2"))
                    {
                        Territory2 = ds.Tables[0].Rows[0]["terr2"] == null ? "" : ds.Tables[0].Rows[0]["terr2"].ToString();

                        if (Territory2 != "")
                        {
                            ddlTerr2.SelectedValue = Territory2;
                        }
                    }
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtMaincontact.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                    txtBillAdd.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtBillCity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                    //ddlBillState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                    txtBillState.Text = ds.Tables[0].Rows[0]["state"].ToString();
                    txtBillZip.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                    txtCountry.Text = ds.Tables[0].Rows[0]["locCountry"].ToString();
                    var item = drpBillCountry.Items.FindByText(ds.Tables[0].Rows[0]["Country"].ToString());
                    if (item != null)
                        item.Selected = true;
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    try
                    {
                        ddlCustomer.SelectedValue = ds.Tables[0].Rows[0]["owner"].ToString();
                        txtCustomer.Text = ddlCustomer.SelectedItem.Text;
                        hdnPatientId.Value = ddlCustomer.SelectedValue;
                    }
                    catch { }                   
                    //txtGoogleAddress.Text = ds.Tables[0].Rows[0]["MAPAddress"].ToString(); 
                    ViewState["rol"] = ds.Tables[0].Rows[0]["rol"].ToString();
                    lat.Value = ds.Tables[0].Rows[0]["Lat"].ToString();
                    lng.Value = ds.Tables[0].Rows[0]["Lng"].ToString();
                    txtCst1.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                    txtCst2.Text = ds.Tables[0].Rows[0]["custom2"].ToString();
                    txtEmailTo.Text = ds.Tables[0].Rows[0]["custom14"].ToString();
                    txtEmailCC.Text = ds.Tables[0].Rows[0]["custom15"].ToString();
                    txtEmailToInv.Text = ds.Tables[0].Rows[0]["custom12"].ToString();
                    txtEmailCCInv.Text = ds.Tables[0].Rows[0]["custom13"].ToString();
                    ddlLocStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    ViewState["LocStatus"] = ds.Tables[0].Rows[0]["status"].ToString();
                    ddlSTax.SelectedValue = ds.Tables[0].Rows[0]["stax"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["STax2"].ToString()))
                        ddlSalesTax2.SelectedValue = ds.Tables[0].Rows[0]["STax2"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["UTax"].ToString()))
                        ddlUseTax.SelectedValue = ds.Tables[0].Rows[0]["UTax"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Zone"].ToString()))
                        ddlZone.SelectedValue = ds.Tables[0].Rows[0]["Zone"].ToString();
                    txtCreditReason.Text = ds.Tables[0].Rows[0]["creditreason"].ToString();
                    if (Session["COPer"].ToString() == "1")
                    {
                        ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
                        txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                    }
                    chkDispAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DispAlert"]);
                    chkCreditHold.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Credit"]);
                    chkCreditFlag.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["CreditFlag"]);

                    // if the location on credit hold it needs to give a warning and prevent the user

                    imgCreditH.Visible = chkCreditHold.Checked;

                    ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString())) //added by Mayuri 24th dec, 15
                    {
                        ddlContractBill.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }
                    ddlBusinessType.SelectedValue = ds.Tables[0].Rows[0]["BusinessTypeID"].ToString();
                    //lblLocationBalance.Text = "$" + Convert.ToString(ds.Tables[0].Rows[0]["Balance"]);
                  //  lblLocationBalance.Text = String.Format("{0:C}", Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[0]["Balance"]) == "" ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["Balance"])));
                    ViewState["LocBalance"] = Convert.ToString(ds.Tables[0].Rows[0]["Balance"]);
                    // lblCustomerBalance.Text = "$" + Convert.ToString(ds.Tables[2].Rows[0][0]) == "" ? "0.00" : Convert.ToString(ds.Tables[2].Rows[0][0]);
                    //lblCustomerBalance.Text = String.Format("{0:C}", Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[0][0]) == "" ? "0.00" : Convert.ToString(ds.Tables[2].Rows[0][0])));
                    lblCustomerBalance.Text = String.Format("{0:C}", Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[0]["Balance"]) == "" ? "0.00" : Convert.ToString(ds.Tables[0].Rows[0]["Balance"])));
                    //GetDocuments(chkShowAllDocs.Checked);
                    RadGrid_Documents.Rebind();
                    GetAlerts();
                    processShowAllTrans();

                    lnkAddopp.NavigateUrl = "AddOpprt.aspx?rol=" + Convert.ToString(ViewState["rol"])
                        + "&owner=" + Convert.ToString(Request.QueryString["uid"])
                        + "&name=" + txtLocName.Text
                        + "&assignedTo=" + ddlTerr.SelectedItem.Value
                        + "&BusinessType=" + ddlBusinessType.SelectedItem.Text
                        + "&customer=" + txtCustomer.Text
                        + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
                    lnkAddTicket.Visible = true;
                    lnkEditTicket.Visible = true;

                    txtContractStatus.Text = ds.Tables[0].Rows[0]["ContractStatus"].ToString();
                    if (txtContractStatus.Text != "")
                    {
                        imgContractStatus.Visible = true;
                    }
                    getAllContractInfo();
                }

                if(ds.Tables.Count >= 5)
                {
                    if(ds.Tables[4].Rows.Count > 0)
                    {
                        hdnOpenTicketsCount.Value = ds.Tables[4].Rows.Count.ToString();
                        System.Text.StringBuilder str = new System.Text.StringBuilder();
                        foreach (DataRow item in ds.Tables[4].Rows)
                        {
                            str.AppendFormat("{0},", item["TicketID"].ToString());
                        }
                        str.Remove(str.Length - 1, 1);
                        hdnOpenTicketIDs.Value = str.ToString();
                    }
                }
                try
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {                           
                                ViewState["contacttableloc"] = ds.Tables[1];
                        }
                    }
                }
                catch { }

                DataSet dsGCandHower = new DataSet();
                DataSet dsGCandHower1 = new DataSet();
                DataSet dsGCandHower2 = new DataSet();
                ListGetGCandHowerLocID _lstGetGCandHowerLocID = new ListGetGCandHowerLocID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetGCandHowerLocID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetGCandHowerLocID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetGCandHowerLocID = serializer.Deserialize<ListGetGCandHowerLocID>(_APIResponse.ResponseData);

                    dsGCandHower1 = _lstGetGCandHowerLocID.lstTable1.ToDataSet();
                    dsGCandHower2 = _lstGetGCandHowerLocID.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt1 = dsGCandHower1.Tables[0];
                    dt2 = dsGCandHower2.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";

                    dsGCandHower.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
                }
                else
                {
                    dsGCandHower = objBL_User.GetGCandHowerLocID(objPropUser);
                }

                
                #region GC information-------------------------------------------------------->


                // Fill GC Infomation ---------------------------------------------------------->

                if (dsGCandHower.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["RolID"].ToString()))
                    {
                        if (!Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["RolID"]).Equals(0) && Convert.ToInt32(dsGCandHower.Tables[0].Rows[0]["LocContactType"]).Equals(1))
                        {
                            GCtxtName.Text = dsGCandHower.Tables[0].Rows[0]["RolName"].ToString();
                            GCtxtcity.Text = dsGCandHower.Tables[0].Rows[0]["city"].ToString();
                            GCtxtAddress.Text = dsGCandHower.Tables[0].Rows[0]["address"].ToString();
                            if (!string.IsNullOrEmpty(dsGCandHower.Tables[0].Rows[0]["state"].ToString()))
                            {
                                //GCddlState.SelectedValue = dsGCandHower.Tables[0].Rows[0]["state"].ToString();
                                GCtxtState.Text = dsGCandHower.Tables[0].Rows[0]["state"].ToString();
                            }
                            GCtxtPostalCode.Text = dsGCandHower.Tables[0].Rows[0]["zip"].ToString();
                            GCtxtCountry.Text = dsGCandHower.Tables[0].Rows[0]["country"].ToString();
                            GCtxtPhone.Text = dsGCandHower.Tables[0].Rows[0]["phone"].ToString();
                            GCtxtMobile.Text = dsGCandHower.Tables[0].Rows[0]["cellular"].ToString();
                            GCtxtFAX.Text = dsGCandHower.Tables[0].Rows[0]["fax"].ToString();
                            GCtxtEmailWeb.Text = dsGCandHower.Tables[0].Rows[0]["email"].ToString();
                            GCtxtRemarks.Text = dsGCandHower.Tables[0].Rows[0]["rolRemarks"].ToString();
                            GCtxtCName.Text = dsGCandHower.Tables[0].Rows[0]["contact"].ToString();
                            hdnGContractorID.Value = dsGCandHower.Tables[0].Rows[0]["RolID"].ToString();
                            hdnGCName.Value = dsGCandHower.Tables[0].Rows[0]["RolName"].ToString();
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
                            HotxtAddress.Text = dsGCandHower.Tables[1].Rows[0]["address"].ToString();
                            if (!string.IsNullOrEmpty(dsGCandHower.Tables[1].Rows[0]["state"].ToString()))
                            {
                                //hotddlstate.SelectedValue = dsGCandHower.Tables[1].Rows[0]["state"].ToString();
                                hottxtstate.Text = dsGCandHower.Tables[1].Rows[0]["state"].ToString();
                            }
                            hotxtZIP.Text = dsGCandHower.Tables[1].Rows[0]["zip"].ToString();
                            hotxtCountry.Text = dsGCandHower.Tables[1].Rows[0]["country"].ToString();
                            hotxtPhone.Text = dsGCandHower.Tables[1].Rows[0]["phone"].ToString();
                            hotxtMobile.Text = dsGCandHower.Tables[1].Rows[0]["cellular"].ToString();
                            HotxtFax.Text = dsGCandHower.Tables[1].Rows[0]["fax"].ToString();
                            HotxtEmailWeb.Text = dsGCandHower.Tables[1].Rows[0]["email"].ToString();
                            hotxtRemarks.Text = dsGCandHower.Tables[1].Rows[0]["rolRemarks"].ToString();
                            hotxtCName.Text = dsGCandHower.Tables[1].Rows[0]["contact"].ToString();
                            hdnHomeOwnerID.Value = dsGCandHower.Tables[1].Rows[0]["RolID"].ToString();
                            hdnHOName.Value = dsGCandHower.Tables[1].Rows[0]["RolName"].ToString();
                            hdnHONameupdate.Value = "0";
                        }
                    }
                }

                #endregion


            }
            if (Request.QueryString["uid"] == null)
            {
                ddlCompany.Visible = false;
                txtCompany.Visible = true;
                lnkLocationTransLedger.Visible = false;
                lnkLocationHistory.Visible = false;
                Page.Title = "Add Location || MOM";
            }
            PopulateCustomer();
            
            FillProspect();
            if (Request.QueryString["AddEquip"] != null)
            {
                if (Request.QueryString["AddEquip"] == "true")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc11", "notyConfirmForAddEquipment();", true);
                }
            }

            if (Request.QueryString["page"] != null)
            {
                hdnPageQueryString.Value = "page=" + Request.QueryString["page"] + "&lid=" + Request.QueryString["lid"];
            }

        }

        if (Request.QueryString["o"] == null)
        {
            Permission();
        }
        if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["filterstateAddLocationHistory"] != null)
        {
            UpdateControl();
            Session.Remove("filterstateAddLocationHistory");
        }
        if (ddlLocStatus.SelectedValue == "1")
        {
            lnkAddTicket.Visible = false;
            lnkEditTicket.Visible = false;
        }
        else
        {
            lnkAddTicket.Visible = true;
            lnkEditTicket.Visible = true;
        }
        DisableButtonWhenLocationInactive();
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
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            string CreditHoldPermission = ds.Rows[0]["CreditHold"] == DBNull.Value ? "YYYY" : ds.Rows[0]["CreditHold"].ToString();
            string CreditHold = CreditHoldPermission.Length < 1 ? "Y" : CreditHoldPermission.Substring(0, 1);
            if (CreditHold == "N")
            {
                chkCreditHold.Enabled = false;
            }

            string CreditFlagPermission = ds.Rows[0]["CreditFlag"] == DBNull.Value ? "YYYY" : ds.Rows[0]["CreditFlag"].ToString();
            string CreditFlag = CreditFlagPermission.Length < 1 ? "Y" : CreditFlagPermission.Substring(0, 1);
            if (CreditFlag == "N")
            {
                chkCreditFlag.Enabled = false;
            }
            /// Location ///////////////////------->

            //Location

            string LocationPermission = ds.Rows[0]["Location"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Location"].ToString();
            string stAddeLocation = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
            string stEditeLocation = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
            string stDeleteLocation = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
            string stViewLocation = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

           
            if (stViewLocation == "N")
            {
                result = false;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (stAddeLocation == "N")
                {
                    result = false;
                }
            }
            else if (stEditeLocation == "N")
            {
                if (stViewLocation == "Y")
                {
                    btnSubmit.Visible = false;
                }
                else
                {
                    result = false;
                }
            }
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
    private void FillTerms()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getTerms.ConnConfig = Session["config"].ToString();

        List<TermsViewModel> _TermsViewModel = new List<TermsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetTerms";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTerms);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _TermsViewModel = serializer.Deserialize<List<TermsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<TermsViewModel>(_TermsViewModel);
        }
        else
        {
            ds = objBL_User.getTerms(objPropUser);
        }

        ddlTerms.DataSource = ds.Tables[0];
        ddlTerms.DataTextField = "name";
        ddlTerms.DataValueField = "id";
        ddlTerms.DataBind();

        ddlTerms.Items.Insert(0, new ListItem(":: Select ::", "-1"));
    }
    private void GetQBInt()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();
        
        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            ds = objBL_User.getControl(objPropUser);
        }

        ViewState["qbint"] = "0";

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
                ViewState["qbint"] = "1";
            //ViewState["IsLocAddressBlank"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLocAddressBlank"]);
            ViewState["IsLocAddressBlank"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLocAddressBlank"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["IsLocAddressBlank"]);
        }
    }


    private void FillProspect()
    {
        if (Request.QueryString["cpw"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());

            _GetProspectByID.ConnConfig = Session["config"].ToString();
            _GetProspectByID.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();

            ListGetProspectByID _lstGetProspectByID = new ListGetProspectByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_GetProspectByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetProspectByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetProspectByID = serializer.Deserialize<ListGetProspectByID>(_APIResponse.ResponseData);

                ds1 = _lstGetProspectByID.lstTable1.ToDataSet();
                ds2 = _lstGetProspectByID.lstTable2.ToDataSet();
                ds3 = _lstGetProspectByID.lstTable3.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];
                dt3 = ds3.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
            }
            else
            {
                ds = objBL_Customer.getProspectByID(objProp_Customer);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                FileUpload1.Visible = false;
                lnkDeleteDoc.Visible = false;
                lnkAddnew.Visible = false;
                btnDelete.Visible = false;
                btnEdit.Visible = false;
                pnlDoc.Visible = true;
                btnSubmit.Text = "Next";
                lnkClose.Visible = false;

                lblLocationName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtAcctno.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtLocName.Text = ds.Tables[0].Rows[0]["name"].ToString();

                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString(); ;
                //ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                txtBillAdd.Text = ds.Tables[0].Rows[0]["billAddress"].ToString();
                txtBillCity.Text = ds.Tables[0].Rows[0]["billcity"].ToString();
                //ddlBillState.SelectedValue = ds.Tables[0].Rows[0]["billstate"].ToString();
                txtBillState.Text = ds.Tables[0].Rows[0]["billstate"].ToString();
                txtBillZip.Text = ds.Tables[0].Rows[0]["billzip"].ToString();
                var item = drpBillCountry.Items.FindByText(ds.Tables[0].Rows[0]["Country"].ToString());
                if (item != null)
                    item.Selected = true;
                try
                {
                    ddlCustomer.SelectedValue = Request.QueryString["customerid"].ToString();
                    txtCustomer.Text = ddlCustomer.SelectedItem.Text;
                    hdnPatientId.Value = ddlCustomer.SelectedValue;
                }
                catch { }
                ddlLocStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                // Field customer address for hdn fields
                if (!string.IsNullOrEmpty(Request.QueryString["customerid"]))
                {
                    User user = new User();
                    user.CustomerID = Convert.ToInt32(Request.QueryString["customerid"].ToString());
                    user.ConnConfig = Session["config"].ToString();

                    _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["customerid"].ToString());
                    _GetCustomerByID.ConnConfig = Session["config"].ToString();

                    DataSet dsCust = new DataSet();
                    DataSet dsCust1 = new DataSet();
                    DataSet dsCust2 = new DataSet();
                    DataSet dsCust3 = new DataSet();

                    ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "LocationsAPI/AddLocation_GetCustomerByID";

                        _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);
                        dsCust1 = _listGetCustomerByID.lstTable1.ToDataSet();
                        dsCust2 = _listGetCustomerByID.lstTable2.ToDataSet();
                        dsCust3 = _listGetCustomerByID.lstTable3.ToDataSet();

                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();
                        DataTable dt3 = new DataTable();

                        dt1 = dsCust1.Tables[0];
                        dt2 = dsCust2.Tables[0];
                        dt3 = dsCust3.Tables[0];

                        dt1.TableName = "Table1";
                        dt2.TableName = "Table2";
                        dt3.TableName = "Table3";
                        dsCust.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
                    }
                    else
                    {
                        dsCust = objBL_User.getCustomerByID(user);
                    }

                    if (dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
                    {
                        hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();
                        hdnCustomerCountry.Value = dsCust.Tables[0].Rows[0]["Country"].ToString();
                        hdnCustomerCity.Value = dsCust.Tables[0].Rows[0]["City"].ToString();
                        hdnCustomerState.Value = dsCust.Tables[0].Rows[0]["state"].ToString();
                        hdnCustomerZipCode.Value = dsCust.Tables[0].Rows[0]["zip"].ToString();
                    }
                }

                //GetDocuments(chkShowAllDocs.Checked);
                RadGrid_Documents.Rebind();
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //gvContacts.DataSource = ds.Tables[1];
                        //gvContacts.DataBind();

                        RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;
                        RadGrid_Contacts.DataSource = ds.Tables[1];
                        RadGrid_Contacts.Rebind();
                    }
                }
                ddlBusinessType.SelectedValue= ds.Tables[0].Rows[0]["BusinessTypeID"].ToString();
               
            }
        }
    }

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetCategory.ConnConfig = Session["config"].ToString();

        List<GetCategoryViewModel> _lstGetCategory = new List<GetCategoryViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetCategory";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCategory = serializer.Deserialize<List<GetCategoryViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCategoryViewModel>(_lstGetCategory);
        }
        else
        {
            ds = objBL_User.getCategory(objPropUser);
        }

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
    }
    private void GetCustomerAll()
    {
        DataSet ds = new DataSet();
        #region Company Check
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCustomers.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objPropUser.EN = 1;
            _GetCustomers.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
            _GetCustomers.EN = 0;
        }
        #endregion
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetCustomers.DBName = Session["dbname"].ToString();
        _GetCustomers.ConnConfig = Session["config"].ToString();

        List<GetCustomersViewModel> _lstGetCustomer = new List<GetCustomersViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetCustomers";

            _GetCustomers.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomers);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustomer = serializer.Deserialize<List<GetCustomersViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCustomersViewModel>(_lstGetCustomer);
        }
        else
        {
            ds = objBL_User.getCustomers(objPropUser, new GeneralFunctions().GetSalesAsigned());
        }

        ddlCustomer.DataSource = ds.Tables[0];
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
  
    private void FillRoute()
    {
        Int32 LocID = 0;
        LocID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();
        _GetRoute.ConnConfig = Session["config"].ToString();

        ListGetRouteViewModel _lstGetRoute = new ListGetRouteViewModel();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetRoute";

            _GetRoute.IsActive = 1;
            _GetRoute.LocID = LocID;
            _GetRoute.ContractID = 0;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetRoute);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetRoute = serializer.Deserialize<ListGetRouteViewModel>(_APIResponse.ResponseData);

            ds1 = _lstGetRoute.lstTable1.ToDataSet();
            ds2 = _lstGetRoute.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

            if (_lstGetRoute.lstTable3 != null)
            {
                ds3 = _lstGetRoute.lstTable3.ToDataSet();
                DataTable dt3 = new DataTable();
                dt3 = ds3.Tables[0];
                dt3.TableName = "Table3";
                ds.Tables.AddRange(new DataTable[] { dt3.Copy() });
            }
        }
        else
        {
            ds = objBL_User.getRoute(objPropUser, 1, LocID, 0);//IsActive=1 :- Get Only Active Workers
        }

        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "Label";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        if (ds.Tables[1].Rows.Count > 0)
        {

            if (ddlRoute.Items.Contains(new ListItem(ds.Tables[1].Rows[0][0].ToString())))
                ddlRoute.Items.FindByText(ds.Tables[1].Rows[0][0].ToString()).Selected = true;

        }

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));

        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));

    }
    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        _getlocationType.ConnConfig = Session["config"].ToString();

        List<GetLocationTypeViewModel> _lstgetlocationType = new List<GetLocationTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_getlocationType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getlocationType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstgetlocationType = serializer.Deserialize<List<GetLocationTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetLocationTypeViewModel>(_lstgetlocationType);
        }
        else
        {
            ds = objBL_User.getlocationType(objPropUser);
        }

        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();

        ddlType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetTerritory.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["uid"] != null)
        {
            objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
            _GetTerritory.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        }

        List<GetTerritoryViewModel> _lstGetTerritory = new List<GetTerritoryViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetTerritory";

            _GetTerritory.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTerritory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetTerritory = serializer.Deserialize<List<GetTerritoryViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetTerritoryViewModel>(_lstGetTerritory);
        }
        else
        {
            //ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());
            ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), Convert.ToInt32(Request.QueryString["uid"]), "LOCATION", "t.SDesc");
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlTerr.DataSource = ds.Tables[0];
            ddlTerr.DataTextField = "Name";
            ddlTerr.DataValueField = "ID";
            ddlTerr.DataBind();
            ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));
            // Second Salesperson  
            ddlTerr2.DataSource = ds.Tables[0];
            ddlTerr2.DataTextField = "Name";
            ddlTerr2.DataValueField = "ID";
            ddlTerr2.DataBind();
            ddlTerr2.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
    }
    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetSTax.ConnConfig = Session["config"].ToString();

        List<STaxViewModel> _lstSTax = new List<STaxViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetSTax";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSTax);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstSTax = serializer.Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<STaxViewModel>(_lstSTax);
        }
        else
        {
            ds = objBL_User.getSTax(objPropUser);                   //change by dev on 14th march
            //objBL_User.getSalesTax(objPropUser);
        }

        ddlSTax.DataSource = ds.Tables[0];
        ddlSTax.DataTextField = "Name";
        ddlSTax.DataValueField = "Name";
        ddlSTax.DataBind();
    }

    private void FillSalesTax2()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getSalesTax2.ConnConfig = Session["config"].ToString();

        List<getSalesTax2ViewModel> _lstgetSalesTax2 = new List<getSalesTax2ViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_getSalesTax2";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSalesTax2);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstgetSalesTax2 = serializer.Deserialize<List<getSalesTax2ViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<getSalesTax2ViewModel>(_lstgetSalesTax2);
        }
        else
        {
            ds = objBL_User.getSalesTax2(objPropUser);
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSalesTax2.DataSource = ds.Tables[0];
            ddlSalesTax2.DataTextField = "Name";
            ddlSalesTax2.DataValueField = "Name";
            ddlSalesTax2.DataBind();
            ddlSalesTax2.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
    }
    private void FillUseTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getUseTax.ConnConfig = Session["config"].ToString();
        List<STaxViewModel> _STaxViewModel = new List<STaxViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetUseTax";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUseTax);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _STaxViewModel = serializer.Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<STaxViewModel>(_STaxViewModel);
        }
        else
        {
            ds = objBL_User.getUseTax(objPropUser);
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlUseTax.DataSource = ds.Tables[0];
            ddlUseTax.DataTextField = "Name";
            ddlUseTax.DataValueField = "Name";
            ddlUseTax.DataBind();
            ddlUseTax.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
    }
    private void FillZone()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetZone.ConnConfig = Session["config"].ToString();

        List<GetZoneViewModel> _lstGetZone = new List<GetZoneViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetZone";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetZone);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetZone = serializer.Deserialize<List<GetZoneViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetZoneViewModel>(_lstGetZone);
        }
        else
        {
            ds = objBL_User.getZone(objPropUser);
        }

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlZone.DataSource = ds.Tables[0];
            ddlZone.DataTextField = "Name";
            ddlZone.DataValueField = "ID";
            ddlZone.DataBind();
            ddlZone.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
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
            pnlSave.Visible = false;
            lnkAddnew.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            lblHeader.Text = "Location";
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            btnSubmit.Visible = false;
            lnkAddnew.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            lnkContactSave.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }


    }

    private void PagePermission()
    {
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            // Equipment
            string ElevatorPermission = ds.Rows[0]["Elevator"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Elevator"].ToString();

            hdnAddeEquipment.Value = ElevatorPermission.Length < 1 ? "Y" : ElevatorPermission.Substring(0, 1);
            hdnEditeEquipment.Value = ElevatorPermission.Length < 2 ? "Y" : ElevatorPermission.Substring(1, 1);
            hdnDeleteEquipment.Value = ElevatorPermission.Length < 3 ? "Y" : ElevatorPermission.Substring(2, 1);
            hdnViewEquipment.Value = ElevatorPermission.Length < 4 ? "Y" : ElevatorPermission.Substring(3, 1);

            // Job
            string JobPermission = ds.Rows[0]["Job"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Job"].ToString();

            hdnAddeJob.Value = JobPermission.Length < 1 ? "Y" : JobPermission.Substring(0, 1);
            hdnEditeJob.Value = JobPermission.Length < 2 ? "Y" : JobPermission.Substring(1, 1);
            hdnDeleteJob.Value = JobPermission.Length < 3 ? "Y" : JobPermission.Substring(2, 1);
            hdnViewJob.Value = JobPermission.Length < 4 ? "Y" : JobPermission.Substring(3, 1);

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();

            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
            hdnEditeTicket.Value = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            hdnDeleteTicket.Value = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
            if (hdnDeleteTicket.Value == "N")
            {
                lnkDeleteTicket.Enabled = false;
                lnkDeleteTicket.CssClass = "disableButton";
            }

            //Contact
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

           

            if (hdnAddeContact.Value == "N")
            {
                lnkAddnew.Enabled = false;
            }

            if (hdnEditeContact.Value == "N")
            {
                btnEdit.Enabled = false;
            }

            if (hdnDeleteContact.Value == "N")
            {
                btnDelete.Enabled = false;
            }


            pnlgvConPermission.Visible = hdnViewContact.Value == "N" ? false : true;

            //Document
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            if (hdnDeleteDocument.Value == "N")
            {
                lnkDeleteDoc.Enabled = false;
            }
            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }
            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;

            // Opportunity Permission
            string ProposalPermission = ds.Rows[0]["Proposal"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Proposal"].ToString();
            string ADDOpp = ProposalPermission.Length < 1 ? "Y" : ProposalPermission.Substring(0, 1);
            string EditOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(1, 1);
            string DeleteOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(2, 1);
            string ViewOpp = ProposalPermission.Length < 4 ? "Y" : ProposalPermission.Substring(3, 1);
            string ReportOpp = ProposalPermission.Length < 6 ? "Y" : ProposalPermission.Substring(5, 1);

            if (ADDOpp == "N")
            {
                lnkAddopp.Visible = false;
                lnkCopyOpp.Visible = false;
            }
            if (EditOpp == "N")
                lnkEditOpp.Visible = false;
            if (DeleteOpp == "N")
                lnkDeleteOpp.Visible = false;
            if (ReportOpp == "N")
                lnkExcelOpp.Visible = false;
            if (ViewOpp == "N")
                RadGrid_Opportunity.Visible = false;

            /// Invoice ///////////////////------->

            string InvoicePermission = ds.Rows[0]["Invoice"] == DBNull.Value ? "YYYY" : ds.Rows[0]["Invoice"].ToString();

            hdnAddInvoice.Value = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
            hdnEditInvoice.Value = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
            hdnDeleteInvoice.Value = InvoicePermission.Length < 3 ? "Y" : InvoicePermission.Substring(2, 1);
            if (hdnAddInvoice.Value == "N")
            {
                lnkAddInvoice.Enabled = false;
                lnkAddInvoice.CssClass = "disableButton";
            }
            if (hdnEditInvoice.Value == "N")
            {
                lnkEditInvoice.Enabled = false;
                lnkEditInvoice.CssClass = "disableButton";
            }
            if (hdnDeleteInvoice.Value == "N")
            {
                lnkDeleteInvoice.Enabled = false;
                lnkDeleteInvoice.CssClass = "disableButton";
            }
        }
        else
        {
            hdnAddeDocument.Value = "Y";
            hdnEditeDocument.Value = "Y";
            hdnDeleteDocument.Value = "Y";
            hdnViewDocument.Value = "Y";

            hdnAddeContact.Value = "Y";
            hdnEditeContact.Value = "Y";
            hdnDeleteContact.Value = "Y";
            hdnViewContact.Value = "Y";

            hdnAddeJob.Value = "Y";
            hdnEditeJob.Value = "Y";
            hdnDeleteJob.Value = "Y";
            hdnViewJob.Value = "Y";

            hdnAddeEquipment.Value = "Y";
            hdnEditeEquipment.Value = "Y";
            hdnDeleteEquipment.Value = "Y";
            hdnViewEquipment.Value = "Y";

            hdnAddeTicket.Value = "Y";
            hdnEditeTicket.Value = "Y";
            hdnDeleteTicket.Value = "Y";
        }

    }
    private void ClearControls()
    {
        ResetFormControlValues(this);
        lat.Value = string.Empty;
        lng.Value = string.Empty;
        CreateTable();


    }
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();

        _getCustomFields.CustomName = name;
        _getCustomFields.ConnConfig = Session["config"].ToString();

        List<CustomViewModel> _CustomViewModel = new List<CustomViewModel>();
        
        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _CustomViewModel = serializer.Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CustomViewModel>(_CustomViewModel);
        }
        else
        {
            ds = objBL_General.getCustomFields(objGeneral);
        }

        return ds;
    }
    private void SalesTaxZonePermission()
    {
        objGeneral.ConnConfig = Session["config"].ToString();

        _getCustomFieldsControl.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = new DataSet();
        List <CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetCustomFieldsControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _lstCustomViewModel = serializer.Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            _dsCustom = objBL_General.getCustomFieldsControl(objGeneral);
        }

        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("SalesTax2"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        dvSalesTax2.Visible = true;
                    }
                    else
                        dvSalesTax2.Visible = false;
                }
                else if (_dr["Name"].ToString().Equals("UseTax"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        dvUseTax.Visible = true;
                    }
                    else
                        dvUseTax.Visible = false;
                }
                else if (_dr["Name"].ToString().Equals("Zone"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        dvZone.Visible = true;
                    }
                    else
                        dvZone.Visible = false;
                }

            }
        }
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
    private void FillCompany()
    {
        //  int inCustID = Convert.ToInt32(ddlCustomer.SelectedValue);
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _GetCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCompanyByCustomer.DBName = Session["dbname"].ToString();
        _GetCompanyByCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<CompanyOfficeViewModel> _lstCompanyOffice = new List<CompanyOfficeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetCompanyByCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyByCustomer);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCompanyOffice = serializer.Deserialize<List<CompanyOfficeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CompanyOfficeViewModel>(_lstCompanyOffice);
        }
        else
        {
            ds = objBL_Company.getCompanyByCustomer(objCompany);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=loc");
        }
        else
        {
            Response.Redirect("locations.aspx");
        }
    }
    private bool ValidateLocation()
    {
        bool _isValid = true;

        if (ddlContractBill.SelectedValue == "1")
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);

            _IsExistContractByLoc.ConnConfig = Session["config"].ToString();
            _IsExistContractByLoc.Loc = Convert.ToInt32(Request.QueryString["uid"]);

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_IsExistContractByLoc";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistContractByLoc);

                _IsExistContractByLoc.IsExistContract = Convert.ToBoolean(_APIResponse.ResponseData);
                if (!_IsExistContractByLoc.IsExistContract)
                {
                    _isValid = false;
                }
                else
                {
                    _isValid = true;
                }
            }
            else
            {
                objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                if (!objProp_Contracts.IsExistContract)
                {
                    _isValid = false;
                }
                else
                {
                    _isValid = true;
                }
            }
        }

        if (!_isValid)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
        }
        return _isValid;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateLocation())
        {
            try
            {
                Submit();
                RadGrid_Equip.Rebind();
                RadGrid_Project.Rebind();
                RadGrid_gvLogs.Rebind();
                RadGrid_Contacts.Rebind();
                DisableButtonWhenLocationInactive();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    private void Submit()
    {       

        //try
        //{
            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objPropUser.BillRate = Convert.ToDouble(txtBillRate.Text);
                _UpdateLocation.BillRate = Convert.ToDouble(txtBillRate.Text);
                _AddLocation.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objPropUser.RateOT = Convert.ToDouble(txtOt.Text);
                _UpdateLocation.RateOT = Convert.ToDouble(txtOt.Text);
                _AddLocation.RateOT = Convert.ToDouble(txtOt.Text);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objPropUser.RateNT = Convert.ToDouble(txtNt.Text);
                _UpdateLocation.RateNT = Convert.ToDouble(txtNt.Text);
                _AddLocation.RateNT = Convert.ToDouble(txtNt.Text);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objPropUser.RateDT = Convert.ToDouble(txtDt.Text);
                _UpdateLocation.RateDT = Convert.ToDouble(txtDt.Text);
                _AddLocation.RateDT = Convert.ToDouble(txtDt.Text);
            }
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objPropUser.RateTravel = Convert.ToDouble(txtTravel.Text);
                _UpdateLocation.RateTravel = Convert.ToDouble(txtTravel.Text);
                _AddLocation.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objPropUser.MileageRate = Convert.ToDouble(txtMileage.Text);
                _UpdateLocation.MileageRate = Convert.ToDouble(txtMileage.Text);
                _AddLocation.MileageRate = Convert.ToDouble(txtMileage.Text);
            }
            objPropUser.AccountNo = txtAcctno.Text;
            objPropUser.Locationname = txtLocName.Text;
            objPropUser.Address = txtAddress.Text;
            objPropUser.Status = Convert.ToInt16(ddlLocStatus.SelectedValue);
            objPropUser.City = txtCity.Text;
            objPropUser.State = txtState.Text;
            objPropUser.Zip = txtZip.Text;

            //API
            _UpdateLocation.AccountNo = txtAcctno.Text;
            _UpdateLocation.Locationname = txtLocName.Text;
            _UpdateLocation.Address = txtAddress.Text;
            _UpdateLocation.Status = Convert.ToInt16(ddlLocStatus.SelectedValue);
            _UpdateLocation.City = txtCity.Text;
            _UpdateLocation.State = txtState.Text;
            _UpdateLocation.Zip = txtZip.Text;

            _AddLocation.AccountNo = txtAcctno.Text;
            _AddLocation.Locationname = txtLocName.Text;
            _AddLocation.Address = txtAddress.Text;
            _AddLocation.Status = Convert.ToInt16(ddlLocStatus.SelectedValue);
            _AddLocation.City = txtCity.Text;
            _AddLocation.State = txtState.Text;
            _AddLocation.Zip = txtZip.Text;

            if (ddlRoute.SelectedValue != string.Empty)
            {
                objPropUser.Route = Convert.ToInt32(ddlRoute.SelectedValue);
                _UpdateLocation.Route = Convert.ToInt32(ddlRoute.SelectedValue);
                _AddLocation.Route = Convert.ToInt32(ddlRoute.SelectedValue);
            }
            // Default Salesperson
            if (ddlTerr.SelectedValue != string.Empty)
            {
                objPropUser.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
                _UpdateLocation.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
                _AddLocation.Territory = Convert.ToInt32(ddlTerr.SelectedValue);
            }
            //Second Salesperson
            if (ddlTerr2.SelectedValue != string.Empty)
            {
                objPropUser.Territory2 = Convert.ToInt32(ddlTerr2.SelectedValue);
                _UpdateLocation.Territory2 = Convert.ToInt32(ddlTerr2.SelectedValue);
                _AddLocation.Territory2 = Convert.ToInt32(ddlTerr2.SelectedValue);
            }
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.MainContact = txtMaincontact.Text;
            objPropUser.Phone = txtPhoneCust.Text;
            objPropUser.Fax = txtFax.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Website = txtWebsite.Text;
            objPropUser.RolAddress = txtBillAdd.Text;
            objPropUser.RolCity = txtBillCity.Text;
            //objPropUser.RolState = ddlBillState.SelectedValue;
            objPropUser.RolState = txtBillState.Text;
            objPropUser.RolZip = txtBillZip.Text;           
            objPropUser.Type = ddlType.SelectedValue;
            objPropUser.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            objPropUser.MAPAddress = string.Empty;
            objPropUser.Stax = ddlSTax.SelectedValue;
            objPropUser.Lat = lat.Value.Trim();
            objPropUser.Lng = lng.Value.Trim();
            objPropUser.Custom1 = txtCst1.Text;
            objPropUser.Custom2 = txtCst2.Text;
            objPropUser.ToMail = txtEmailTo.Text.Trim();
            objPropUser.CCMail = txtEmailCC.Text.Trim();
            objPropUser.MailToInv = txtEmailToInv.Text.Trim();
            objPropUser.MailCCInv = txtEmailCCInv.Text.Trim();
            objPropUser.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            objPropUser.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            objPropUser.CreditFlag = Convert.ToInt16(chkCreditFlag.Checked);
            objPropUser.CreditReason = txtCreditReason.Text.Trim();
            objPropUser.TermsID = Convert.ToInt16(ddlTerms.SelectedValue);
            objPropUser.STax2 = ddlSalesTax2.SelectedValue;
            objPropUser.UTax = ddlUseTax.SelectedValue;
            objPropUser.Country = txtCountry.Text;
            objPropUser.BusinessTypeID = Convert.ToInt16(ddlBusinessType.SelectedValue);

            //API
            _UpdateLocation.Remarks = txtRemarks.Text;
            _UpdateLocation.MainContact = txtMaincontact.Text;
            _UpdateLocation.Phone = txtPhoneCust.Text;
            _UpdateLocation.Fax = txtFax.Text;
            _UpdateLocation.Cell = txtCell.Text;
            _UpdateLocation.Email = txtEmail.Text;
            _UpdateLocation.Website = txtWebsite.Text;
            _UpdateLocation.RolAddress = txtBillAdd.Text;
            _UpdateLocation.RolCity = txtBillCity.Text;
            _UpdateLocation.RolState = txtBillState.Text;
            _UpdateLocation.RolZip = txtBillZip.Text;
            _UpdateLocation.Type = ddlType.SelectedValue;
            _UpdateLocation.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            _UpdateLocation.MAPAddress = string.Empty;
            _UpdateLocation.Stax = ddlSTax.SelectedValue;
            _UpdateLocation.Lat = lat.Value.Trim();
            _UpdateLocation.Lng = lng.Value.Trim();
            _UpdateLocation.Custom1 = txtCst1.Text;
            _UpdateLocation.Custom2 = txtCst2.Text;
            _UpdateLocation.ToMail = txtEmailTo.Text.Trim();
            _UpdateLocation.CCMail = txtEmailCC.Text.Trim();
            _UpdateLocation.MailToInv = txtEmailToInv.Text.Trim();
            _UpdateLocation.MailCCInv = txtEmailCCInv.Text.Trim();
            _UpdateLocation.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            _UpdateLocation.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            _UpdateLocation.CreditFlag = Convert.ToInt16(chkCreditFlag.Checked);
            _UpdateLocation.CreditReason = txtCreditReason.Text.Trim();
            _UpdateLocation.TermsID = Convert.ToInt16(ddlTerms.SelectedValue);
            _UpdateLocation.STax2 = ddlSalesTax2.SelectedValue;
            _UpdateLocation.UTax = ddlUseTax.SelectedValue;
            _UpdateLocation.Country = txtCountry.Text;
            _UpdateLocation.BusinessTypeID = Convert.ToInt16(ddlBusinessType.SelectedValue);

            _AddLocation.Remarks = txtRemarks.Text;
            _AddLocation.MainContact = txtMaincontact.Text;
            _AddLocation.Phone = txtPhoneCust.Text;
            _AddLocation.Fax = txtFax.Text;
            _AddLocation.Cell = txtCell.Text;
            _AddLocation.Email = txtEmail.Text;
            _AddLocation.Website = txtWebsite.Text;
            _AddLocation.RolAddress = txtBillAdd.Text;
            _AddLocation.RolCity = txtBillCity.Text;
            _AddLocation.RolState = txtBillState.Text;
            _AddLocation.RolZip = txtBillZip.Text;
            _AddLocation.Type = ddlType.SelectedValue;
            _AddLocation.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
            _AddLocation.MAPAddress = string.Empty;
            _AddLocation.Stax = ddlSTax.SelectedValue;
            _AddLocation.Lat = lat.Value.Trim();
            _AddLocation.Lng = lng.Value.Trim();
            _AddLocation.Custom1 = txtCst1.Text;
            _AddLocation.Custom2 = txtCst2.Text;
            _AddLocation.ToMail = txtEmailTo.Text.Trim();
            _AddLocation.MailToInv = txtEmailToInv.Text.Trim();
            _AddLocation.MailCCInv = txtEmailCCInv.Text.Trim();
            _AddLocation.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            _AddLocation.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            _AddLocation.CreditFlag = Convert.ToInt16(chkCreditFlag.Checked);
            _AddLocation.CreditReason = txtCreditReason.Text.Trim();
            _AddLocation.TermsID = Convert.ToInt16(ddlTerms.SelectedValue);
            _AddLocation.STax2 = ddlSalesTax2.SelectedValue;
            _AddLocation.UTax = ddlUseTax.SelectedValue;
            _AddLocation.Country = txtCountry.Text;
            _AddLocation.BusinessTypeID = Convert.ToInt16(ddlBusinessType.SelectedValue);

            if (drpBillCountry.SelectedValue != "Select")
            {
                objPropUser.RolCountry = drpBillCountry.SelectedItem.Text;
                _UpdateLocation.RolCountry = drpBillCountry.SelectedItem.Text;
                _AddLocation.RolCountry = drpBillCountry.SelectedItem.Text;
            }
            else
            {
                objPropUser.RolCountry = "";
                _UpdateLocation.RolCountry = "";
                _AddLocation.RolCountry = "";
            }
            //consult
            objPropUser.Consult = int.Parse(ddlConsultant.SelectedValue);
            _UpdateLocation.Consult = int.Parse(ddlConsultant.SelectedValue);
            _AddLocation.Consult = int.Parse(ddlConsultant.SelectedValue);

            if (ddlZone.SelectedValue != string.Empty)
            {
                objPropUser.Zone = Convert.ToInt32(ddlZone.SelectedValue);
                _UpdateLocation.Zone = Convert.ToInt32(ddlZone.SelectedValue);
                _AddLocation.Zone = Convert.ToInt32(ddlZone.SelectedValue);
            }
            if (chkPrintOnly.Checked)
            {
                objPropUser.PrintInvoice = true;
                _UpdateLocation.PrintInvoice = true;
                _AddLocation.PrintInvoice = true;
            }
            else
            {
                objPropUser.PrintInvoice = false;
                _UpdateLocation.PrintInvoice = false;
                _AddLocation.PrintInvoice = false;
            }
            if (chkEmail.Checked)
            {
                objPropUser.EmailInvoice = true;
                _UpdateLocation.EmailInvoice = true;
                _AddLocation.EmailInvoice = true;
            }
            else
            {
                objPropUser.EmailInvoice = false;
                _UpdateLocation.EmailInvoice = false;
                _AddLocation.EmailInvoice = false;
            }

            objPropUser.NoCustomerStatement = chkNoCustStatement.Checked ? true : false;
            _UpdateLocation.NoCustomerStatement = chkNoCustStatement.Checked ? true : false;
            _AddLocation.NoCustomerStatement = chkNoCustStatement.Checked ? true : false;

            if (ViewState["contacttableloc"] != null)
            {
                objPropUser.ContactData = (DataTable)ViewState["contacttableloc"];

                DataTable viewstatedata = (DataTable)ViewState["contacttableloc"];
                if (viewstatedata.Rows.Count == 0)
                {
                    DataTable returnVal = ContactDataEmptyDatatable();
                    _UpdateLocation.ContactData = returnVal;
                    _AddLocation.ContactData = returnVal;

                }
                else
                {
                    _UpdateLocation.ContactData = (DataTable)ViewState["contacttableloc"];
                    _AddLocation.ContactData = (DataTable)ViewState["contacttableloc"];
                }
            }
            else
            {
                CreateTable();
                objPropUser.ContactData = (DataTable)ViewState["contacttableloc"];
                _UpdateLocation.ContactData = (DataTable)ViewState["contacttableloc"];
                _AddLocation.ContactData = (DataTable)ViewState["contacttableloc"];
            }

            objPropUser.dtDocs = SaveDocInfo();

            DataTable _saveDocInfo = SaveDocInfo();

            if (_saveDocInfo.Rows.Count == 0)
            {
                DataTable returnVal = SaveDocInfoEmptyDatatable();
                _UpdateLocation.dtDocs = returnVal;
            }
            else
            {
                _UpdateLocation.dtDocs = SaveDocInfo();
            }

            DataSet dsAlerts = GetAlertData();

            
            objPropUser.dtAlerts = dsAlerts.Tables[0];
            objPropUser.dtAlertContacts = dsAlerts.Tables[1];
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.MOMUSer = Session["User"].ToString();

            DataTable _dtAlerts = dsAlerts.Tables[0];

            if (_dtAlerts.Rows.Count == 0)
            {
                DataTable returnVal = AlertsEmptyTable();
                _UpdateLocation.dtAlerts = returnVal;
            }
            else
            {
                _UpdateLocation.dtAlerts = dsAlerts.Tables[0];
            }

            DataTable _dtAlertContacts = dsAlerts.Tables[1];

            if (_dtAlertContacts.Rows.Count == 0)
            {
                DataTable returnVal = AlertsContactEmptyTable();
                _UpdateLocation.dtAlertContacts = returnVal;
            }
            else
            {
                _UpdateLocation.dtAlertContacts = dsAlerts.Tables[1];
            }

            //_UpdateLocation.dtAlerts = dsAlerts.Tables[0];
            //_UpdateLocation.dtAlertContacts = dsAlerts.Tables[1];
            _UpdateLocation.ConnConfig = Session["config"].ToString();
            _UpdateLocation.MOMUSer = Session["User"].ToString();

            _AddLocation.ConnConfig = Session["config"].ToString();
            _AddLocation.MOMUSer = Session["User"].ToString();

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
            if (!string.IsNullOrEmpty(GCtxtName.Text))
            {
                DataRow drGC = tblGCandHomeOwner.NewRow();
                drGC["ID"] = hdnGContractorID.Value;
                drGC["Type"] = 1;
                drGC["NAME"] = GCtxtName.Text;
                drGC["City"] = GCtxtcity.Text;
                drGC["Address"] = GCtxtAddress.Text;
                //if (!GCddlState.SelectedValue.Equals("Select State"))
                //    drGC["State"] = GCddlState.SelectedValue;
                //else drGC["State"] = "0";

                if (!GCtxtState.Text.Equals(string.Empty))
                    drGC["State"] = GCtxtState.Text;
                else drGC["State"] = "";

                drGC["Zip"] = GCtxtPostalCode.Text;
                drGC["Phone"] = GCtxtPhone.Text;
                drGC["Fax"] = GCtxtFAX.Text;
                drGC["Contact"] = GCtxtCName.Text;
                drGC["Remarks"] = GCtxtRemarks.Text;
                drGC["Country"] = GCtxtCountry.Text;
                drGC["Cellular"] = GCtxtMobile.Text;
                drGC["EMail"] = GCtxtEmailWeb.Text;

                tblGCandHomeOwner.Rows.Add(drGC);
            }
            if (!string.IsNullOrEmpty(hotxtname.Text))
            {
                DataRow drHomeOwner = tblGCandHomeOwner.NewRow();

                drHomeOwner["ID"] = hdnHomeOwnerID.Value;
                drHomeOwner["Type"] = 2;
                drHomeOwner["NAME"] = hotxtname.Text;
                drHomeOwner["City"] = hotxtcity.Text;
                drHomeOwner["Address"] = HotxtAddress.Text;
                //if (!hotddlstate.SelectedValue.Equals("Select State"))
                //    drHomeOwner["State"] = hotddlstate.SelectedValue;
                //else drHomeOwner["State"] = "0";

                if (!hottxtstate.Text.Equals(string.Empty))
                    drHomeOwner["State"] = hottxtstate.Text;
                else drHomeOwner["State"] = "0";

                drHomeOwner["Zip"] = hotxtZIP.Text;
                drHomeOwner["Phone"] = hotxtPhone.Text;
                drHomeOwner["Fax"] = HotxtFax.Text;
                drHomeOwner["Contact"] = hotxtCName.Text;
                drHomeOwner["Remarks"] = hotxtRemarks.Text;
                drHomeOwner["Country"] = hotxtCountry.Text;
                drHomeOwner["Cellular"] = hotxtMobile.Text;
                drHomeOwner["EMail"] = HotxtEmailWeb.Text;
                tblGCandHomeOwner.Rows.Add(drHomeOwner);


            }
            #endregion
            objPropUser.tblGCandHomeOwner = tblGCandHomeOwner;

            DataTable data = tblGCandHomeOwner;
            if (data.Rows.Count == 0)
            {
                DataTable returnVal = GCandHomeOwnerEmptytable();
                _UpdateLocation.tblGCandHomeOwner = returnVal;
                _AddLocation.tblGCandHomeOwner = returnVal;
            }
            else
            {
                _UpdateLocation.tblGCandHomeOwner = tblGCandHomeOwner;
                _AddLocation.tblGCandHomeOwner = tblGCandHomeOwner;
            }
            

            // if the location on credit hold it needs to give a warning and prevent the user

            imgCreditH.Visible = chkCreditHold.Checked;

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {

                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);

                _IsExistContractByLoc.ConnConfig = Session["config"].ToString();
                _IsExistContractByLoc.Loc = Convert.ToInt32(Request.QueryString["uid"]);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_IsExistContractByLoc";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistContractByLoc);

                    objProp_Contracts.IsExistContract = Convert.ToBoolean(_APIResponse.ResponseData);
                }
                else
                {
                    objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
                }

                if (objProp_Contracts.IsExistContract) //added by Mayuri 24th dec,15
                {
                    objPropUser.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                    _UpdateLocation.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                    _AddLocation.ContractBill = Convert.ToInt16(ddlContractBill.SelectedValue);
                }
                else
                {
                    objPropUser.ContractBill = 0;
                    _UpdateLocation.ContractBill = 0;
                    _AddLocation.ContractBill = 0;
                }


                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _UpdateLocation.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _AddLocation.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                if (hdnAcctID.Value.Trim() != txtAcctno.Text.Trim())
                {
                    if (SageAlert() == 1)
                    {
                        return;
                    }
                }

                int ApplyServiceTypeRule = 0;

                int ProjectPerDepartmentCount = 0;

                string ServiceTypeName = "";

                int.TryParse(hdnProjectPerDepartmentCount.Value ,     out ProjectPerDepartmentCount);

                int.TryParse(hdnApplyServiceTypeRule.Value    ,     out ApplyServiceTypeRule); 

                ServiceTypeName = hdnServiceTypeName.Value    ;


                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_UpdateLocation";

                    _UpdateLocation.CopyToLocAndJob = CopyToLocAndJob.Checked;
                    _UpdateLocation.ApplyServiceTypeRule = ApplyServiceTypeRule;
                    _UpdateLocation.ServiceTypeName = ServiceTypeName;
                    _UpdateLocation.ProjectPerDepartmentCount = ProjectPerDepartmentCount;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateLocation);
                }
                else
                {
                    objBL_User.UpdateLocation(objPropUser, CopyToLocAndJob.Checked, ApplyServiceTypeRule, ServiceTypeName, ProjectPerDepartmentCount);

                    var selectedID1 = ddlTerr.SelectedValue;
                    var selectedID2 = ddlTerr2.SelectedValue;

                    Fillterritory();

                    ddlTerr.SelectedValue = selectedID1;
                    ddlTerr2.SelectedValue = selectedID2;

                    UpnSalespersonList.Update();
                    UpnSalespersonList2.Update();
                }

                hdnAcctID.Value = txtAcctno.Text;

                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Location updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                objPropUser.ContractBill = 0;
                _AddLocation.ContractBill = 0;
                if (Request.QueryString["prospectid"] != null)
                {
                    objPropUser.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
                    _AddLocation.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
                }

                if (Request.QueryString["estimatecid"] != null)
                {
                    objPropUser.EstimateID = Convert.ToInt32(Request.QueryString["estimatecid"].ToString());
                    _AddLocation.EstimateID = Convert.ToInt32(Request.QueryString["estimatecid"].ToString());
                }
                else
                {
                    objPropUser.EstimateID = 0;
                    _AddLocation.EstimateID = 0;
                }

                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    objPropUser.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                    _AddLocation.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    objPropUser.EN = 0;
                    _AddLocation.EN = 0;
                }
                if (SageAlert() == 1)
                {
                    return;
                }

                int locid;
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_AddLocation";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddLocation);

                    object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                    locid = Convert.ToInt32(_APIResponse.ResponseData);
                }
                else
                {
                    locid = objBL_User.AddLocation(objPropUser);
                }

                //#region Update Estimate With Loc ID
                //objBL_Customer = new BL_Customer();
                //objProp_Customer.ConnConfig = Session["config"].ToString();
                //objProp_Customer.LocID = locid;
                //objBL_Customer.UpdateEstimateWithLoc(objProp_Customer);
                //#endregion

                ConvertProspectWizard(locid);

                if (Request.QueryString["cpw"] == null)
                {
                    ViewState["mode"] = 0;

                    //if (Request.QueryString["page"] != null)
                    //{
                    //    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=loc");
                    //}                    
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc1", "noty({text: 'Location added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); RedirectPage();", true);
                    // notyConfirm();                   
                    ClearControls();
                    PopulateCustomer();
                    hdnAddedLoc.Value = locid.ToString();
                }
            }
            if (ddlLocStatus.SelectedValue == "1")
            {
                lnkAddTicket.Visible = false;
                lnkEditTicket.Visible = false;
            }
            else
            {
                lnkAddTicket.Visible = true;
                lnkEditTicket.Visible = true;
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();}", true);
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
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
            CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
            //TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            Label lblScreen = (Label)item.FindControl("lblScreen");
            CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");

            if (lblScreen.Text.Equals("Location", StringComparison.InvariantCultureIgnoreCase))
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = lblID.Text;
                //dr["Portal"] = chkPortal.Checked;
                dr["Portal"] = false;
                dr["Remarks"] = "";//txtRemarks.Text;
                dr["MSVisible"] = chkMSVisible.Checked;
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
    private void ConvertProspectWizard(int locId)
    {
        if (Request.QueryString["cpw"] != null)
        {
            if (Request.QueryString["opid"] != null)
            {
                ConvertLeadEquipment(locId);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addopprt.aspx?cpw=1&uid=" + Request.QueryString["opid"].ToString() + "';", true);
            }
                
            else if (Request.QueryString["ticketid"] != null)
            {
                string ticketid = Request.QueryString["ticketid"].ToString();
                string comp = Request.QueryString["comp"].ToString();
                ConvertLeadEquipment(locId);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addticket.aspx?cpw=1&id=" + ticketid + "&comp=" + comp + "';", true);

            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addcustomer.aspx?uid=" + Request.QueryString["customerid"].ToString() + "';", true);
                if(Request.QueryString["estimatecid"] != null)
                {
                    ConvertLeadEquipment(locId);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addestimate.aspx?uid=" + Request.QueryString["estimatecid"].ToString() + "';", true);
                    //Session["estimateID"] = null;
                }
                else if(Request.QueryString["estimateid"] != null)
                {
                    ConvertLeadEquipment(locId);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addcustomer.aspx?uid=" + Request.QueryString["customerid"].ToString()+"&estimateid="+ Request.QueryString["estimateid"].ToString() + "';", true);
                }
                else
                {
                    ConvertLeadEquipment(locId);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addcustomer.aspx?uid=" + Request.QueryString["customerid"].ToString() + "';", true);
                }
                
            }
                
        }
    }

    private void ConvertLeadEquipment(int locId)
    {
        if(Request.QueryString["prospectid"] != null)
        {
            int prospectID = Int32.Parse(Request.QueryString["prospectid"].ToString());
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProspectID = prospectID;
            objProp_Customer.LocID = locId;

            _ConvertLeadEquipment.ConnConfig = Session["config"].ToString();
            _ConvertLeadEquipment.ProspectID = prospectID;

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_ConvertLeadEquipment";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ConvertLeadEquipment);
            }
            else
            {
                objBL_Customer.ConvertLeadEquipment(objProp_Customer);
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
                }
            }
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Edit Contact";
        foreach (GridDataItem item in RadGrid_Contacts.SelectedItems)
        {
            DataTable dt = (DataTable)ViewState["contacttableloc"];
            Label lblindex = (Label)item.Cells[1].FindControl("lblindex");

            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

            txtContcName.Text = dr["Name"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            chkEmailTicket.Checked = Convert.ToBoolean(dr["EmailTicket"]);           
            chkEmailInvoice.Checked = Convert.ToBoolean(dr["EmailRecInvoice"]);
            chkShutdownAlert.Checked = Convert.ToBoolean(dr["ShutdownAlert"]);
            chkTestProposals.Checked = Convert.ToBoolean(dr["EmailRecTestProp"]);
            ViewState["editcon"] = 1;
            ViewState["index"] = lblindex.Text;
        }
        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        //TogglePopup();

        DataTable dt = (DataTable)ViewState["contacttableloc"];

        //gvContacts.DataSource = dt;
        //gvContacts.DataBind();

        RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
        RadGrid_Contacts.DataSource = dt;
        RadGrid_Contacts.Rebind();
        ClearContact();
    }


    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        if (hdnAddeContact.Value == "Y")
        {
            RadWindowContact.Title = "Add Contact";
            txtContcName.Text = "";
            txtTitle.Text = "";
            txtContPhone.Text = "";
            txtContFax.Text = "";
            txtContCell.Text = "";
            txtContEmail.Text = "";
            
            chkEmailTicket.Checked = false;
            chkEmailInvoice.Checked = false;
            chkShutdownAlert.Checked = false;
            chkTestProposals.Checked = false;

            string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["contacttableloc"];

       
        if (ViewState["editcon"].ToString() == "1")
        {
            //dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            //dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Name"]= Truncate(txtContcName.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Phone"] = Truncate(txtContPhone.Text, 22);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Fax"] = Truncate(txtContFax.Text, 22);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Cell"] = Truncate(txtContCell.Text, 22);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Email"] = Truncate(txtContEmail.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Title"] = Truncate(txtTitle.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["EmailTicket"] = chkEmailTicket.Checked;

            dt.Rows[Convert.ToInt32(ViewState["index"])]["EmailRecInvoice"] = chkEmailInvoice.Checked;
            dt.Rows[Convert.ToInt32(ViewState["index"])]["ShutdownAlert"] = chkShutdownAlert.Checked;
            dt.Rows[Convert.ToInt32(ViewState["index"])]["EmailRecTestProp"] = chkTestProposals.Checked;
            ViewState["editcon"] = 0;
        }
        else
        {
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
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        ViewState["contacttableloc"] = dt;

        //gvContacts.DataSource = dt;
        //gvContacts.DataBind();

        RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
        RadGrid_Contacts.DataSource = dt;
        RadGrid_Contacts.Rebind();

        ClearContact();
        //TogglePopup();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
            updateDataEmail();
            getContact();
            RadGrid_Contacts.Rebind();
        }
        getLocationLog();
        RadGrid_gvLogs.Rebind();
    }


    private void SubmitContact()
    {
        try
        {
            if (ViewState["contacttableloc"] != null)
            {
                objPropUser.ContactData = (DataTable)ViewState["contacttableloc"];

                DataTable viewstatedata = (DataTable)ViewState["contacttableloc"];

                if (viewstatedata.Rows.Count == 0)
                {
                    DataTable returnVal = ContactDataEmptyDatatable();
                    _UpdateLocationContactRecordLog.ContactData = returnVal;

                }
                else
                {
                    _UpdateLocationContactRecordLog.ContactData = (DataTable)ViewState["contacttableloc"];
                }
            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.RolId = Convert.ToInt32(ViewState["rol"].ToString());
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.MOMUSer = Session["User"].ToString();

                _UpdateLocationContactRecordLog.RolId = Convert.ToInt32(ViewState["rol"].ToString());
                _UpdateLocationContactRecordLog.ConnConfig = Session["config"].ToString();
                _UpdateLocationContactRecordLog.MOMUSer = Session["User"].ToString();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_UpdateLocationContactRecordLog";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateLocationContactRecordLog);
                }
                else
                {
                    objBL_User.UpdateLocationContactRecordLog(objPropUser);
                }
                //RowSelect();
                //RowSelectContact();
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
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
        dt.Columns.Add("EmailTicket", typeof(bool));
        
        dt.Columns.Add("EmailRecInvoice", typeof(bool));
        dt.Columns.Add("ShutdownAlert", typeof(bool));
        dt.Columns.Add("EmailRecTestProp", typeof(bool));


        ViewState["contacttableloc"] = dt;
    }

    //API
    private DataTable ContactDataEmptyDatatable()
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

        DataRow dr = dt.NewRow();
        dr["ContactID"] = "0";
        dr["Name"] = "";
        dr["Phone"] = "";
        dr["Fax"] = "";
        dr["Cell"] = "";
        dr["Email"] = "";
        dr["Title"] = "";
        dr["EmailTicket"] = false;
        dr["EmailRecInvoice"] = false;
        dr["ShutdownAlert"] = false;
        dr["EmailRecTestProp"] = false;
        dt.Rows.Add(dr);

        return dt;
    }

    //API
    public DataTable SaveDocInfoEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["Portal"] = "0";
        dr["Remarks"] = "";
        dr["MSVisible"] = "0";

        dt.Rows.Add(dr);
        return dt;
    }

    //API
    public DataTable GCandHomeOwnerEmptytable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("NAME", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("Zip", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Contact", typeof(string));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("Country", typeof(string));
        dt.Columns.Add("Cellular", typeof(string));
        dt.Columns.Add("EMail", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Address", typeof(string));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["NAME"] = "";
        dr["City"] = "";
        dr["State"] = "";
        dr["Zip"] = "";
        dr["Phone"] = "";
        dr["Fax"] = "";
        dr["Contact"] = "";
        dr["Remarks"] = "";
        dr["Country"] = "";
        dr["Cellular"] = "";
        dr["EMail"] = "";
        dr["Type"] = "0";
        dr["Address"] = "";

        dt.Rows.Add(dr);
        return dt;
    }

    //API
    public DataTable AlertsEmptyTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("AlertID", typeof(int));
        dt.Columns.Add("AlertCode", typeof(string));
        dt.Columns.Add("AlertName", typeof(string));
        dt.Columns.Add("AlertSubject", typeof(string));
        dt.Columns.Add("AlertMessage", typeof(string));

        DataRow dr = dt.NewRow();
        dr["AlertID"] = "0";
        dr["AlertCode"] = "";
        dr["AlertName"] = "";
        dr["AlertSubject"] = "";
        dr["AlertMessage"] = "";

        dt.Rows.Add(dr);
        return dt;
    }

    //API
    public DataTable AlertsContactEmptyTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("screenid", typeof(int));
        dt.Columns.Add("screenname", typeof(string));
        dt.Columns.Add("alertid", typeof(int));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("email", typeof(bool));
        dt.Columns.Add("text", typeof(bool));
        dt.Columns.Add("alertcode", typeof(string));

        DataRow dr = dt.NewRow();
        dr["id"] = "0";
        dr["screenid"] = "0";
        dr["screenname"] = "";
        dr["alertid"] = "0";
        dr["name"] = "";
        dr["email"] = false;
        dr["text"] = false;
        dr["alertcode"] = "";

        dt.Rows.Add(dr);
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


    protected void lnkLocationHistory_Click(object sender, EventArgs e)
    {
        // Redirect when close the report
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        if (Request.QueryString["uid"] != null)
        {
            Response.Redirect("LocationHistory.aspx?lid=" + Request.QueryString["uid"]  + "&edirect=" + redirect);
        }
    }

    protected void lnkLocationTransLedger_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            Response.Redirect("LocationTransactionLedger.aspx?loc=" + Request.QueryString["uid"]);
        }
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

            //foreach (GridViewRow di in gvContacts.Rows)
            //{
            //    HiddenField hdnSelected = (HiddenField)di.Cells[1].FindControl("hdnSelected");
            //    Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

            //    if (hdnSelected.Value == "1")
            //    {
            //        dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));

            //    }
            //}


            dt.AcceptChanges();
            ViewState["contacttableloc"] = dt;


            //gvContacts.DataSource = dt;
            //gvContacts.DataBind();
            RadGrid_Contacts.VirtualItemCount = dt.Rows.Count;
            RadGrid_Contacts.DataSource = dt;
            RadGrid_Contacts.Rebind();

            if (ViewState["mode"].ToString() == "1")
            {
                SubmitContact();
                updateDataEmail();
                getContact();
                RadGrid_Contacts.Rebind();
            }
            getLocationLog();
            RadGrid_gvLogs.Rebind();
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



    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locationsId"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["loc"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            string url = "addlocation.aspx?uid=" + dt.Rows[index + 1]["loc"];
            if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locationsId"];
        }
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["loc"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            string url = "addlocation.aspx?uid=" + dt.Rows[index - 1]["loc"];
            if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url);

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locationsId"];
        }
        string url = "addlocation.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["loc"];
        if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
        }
        Response.Redirect(url);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["locationdataCust"];
            }
            else
            {
                dt = (DataTable)Session["locations"];
            }
        }
        else
        {
            dt = (DataTable)Session["locationsId"];
        }
        string url = "addlocation.aspx?uid=" + dt.Rows[0]["loc"];
        if (Request.QueryString["page"] != null && Request.QueryString["lid"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + "&lid=" + Request.QueryString["lid"].ToString();
        }
        Response.Redirect(url);

    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        chkCustomerAddress.Checked = false;
        selectCustomer();

    }

    private void selectCustomer()
    {
        objPropUser.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetCustomerByID.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
        _GetCustomerByID.DBName = Session["dbname"].ToString();
        _GetCustomerByID.ConnConfig = Session["config"].ToString();

        DataSet dsCust = new DataSet();
        DataSet dsCust1 = new DataSet();
        DataSet dsCust2 = new DataSet();
        DataSet dsCust3 = new DataSet();

        ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetCustomerByID";

            _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);
            dsCust1 = _listGetCustomerByID.lstTable1.ToDataSet();
            dsCust2 = _listGetCustomerByID.lstTable2.ToDataSet();
            dsCust3 = _listGetCustomerByID.lstTable3.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            dt1 = dsCust1.Tables[0];
            dt2 = dsCust2.Tables[0];
            dt3 = dsCust3.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            dsCust.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
        }
        else
        {
            dsCust = objBL_User.getCustomerByID(objPropUser);
        }

        if (dsCust.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToInt32(ViewState["mode"]) != 1) // Mode Condition Start .............
            {
                if (!Convert.ToBoolean(ViewState["IsLocAddressBlank"]))
                {

                    txtAddress.Text = dsCust.Tables[0].Rows[0]["Address"].ToString();
                    hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();
                    lat.Value = dsCust.Tables[0].Rows[0]["Lat"].ToString();
                    lng.Value = dsCust.Tables[0].Rows[0]["Lng"].ToString();
                    txtCity.Text = dsCust.Tables[0].Rows[0]["City"].ToString();
                    hdnCustomerCity.Value = txtCity.Text;

                    //ddlState.SelectedValue = dsCust.Tables[0].Rows[0]["State"].ToString();
                    txtState.Text = dsCust.Tables[0].Rows[0]["State"].ToString();
                    //hdnCustomerState.Value = ddlState.SelectedValue;
                    hdnCustomerState.Value = txtState.Text;

                    txtZip.Text = dsCust.Tables[0].Rows[0]["Zip"].ToString();
                    hdnCustomerZipCode.Value = txtZip.Text;

                    txtCountry.Text= dsCust.Tables[0].Rows[0]["Country"].ToString();
                    hdnCustomerCountry.Value = dsCust.Tables[0].Rows[0]["Country"].ToString();
                }
                else
                {
                    hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();
                    hdnCustomerCity.Value = dsCust.Tables[0].Rows[0]["City"].ToString();
                    hdnCustomerState.Value = dsCust.Tables[0].Rows[0]["State"].ToString();
                    hdnCustomerZipCode.Value = dsCust.Tables[0].Rows[0]["Zip"].ToString();
                    hdnCustomerCountry.Value = dsCust.Tables[0].Rows[0]["Country"].ToString();
                }


                //txtRemarks.Text = dsCust.Tables[0].Rows[0]["Remarks"].ToString();
                txtMaincontact.Text = dsCust.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = dsCust.Tables[0].Rows[0]["phone"].ToString();
                txtWebsite.Text = dsCust.Tables[0].Rows[0]["website"].ToString();
                txtEmail.Text = dsCust.Tables[0].Rows[0]["email"].ToString();
                txtCell.Text = dsCust.Tables[0].Rows[0]["cellular"].ToString();
                txtFax.Text = dsCust.Tables[0].Rows[0]["fax"].ToString();
                //txtLocName.Text = txtCustomer.Text;
                if (Session["COPer"].ToString() == "1")
                {
                    ddlCompany.SelectedValue = dsCust.Tables[0].Rows[0]["EN"].ToString();
                    txtCompany.Text = dsCust.Tables[0].Rows[0]["Company"].ToString();
                }
                if (dsCust.Tables.Count > 1)
                {
                    //gvContacts.DataSource = dsCust.Tables[1];
                    //gvContacts.DataBind();
                    RadGrid_Contacts.VirtualItemCount = dsCust.Tables[1].Rows.Count;
                    RadGrid_Contacts.DataSource = dsCust.Tables[1];
                    RadGrid_Contacts.Rebind();

                    //DataTable dsc = dsCust.Tables[1];
                    //dsc.Columns.Add("EmailTicket");                   
                    //dsc.Columns.Add("EmailRecInvoice");
                    //dsc.Columns.Add("ShutdownAlert");
                    //dsc.Columns.Add("EmailRecTestProp");
                    //foreach (DataRow dr in dsc.Rows)
                    //{
                    //    dr["EmailTicket"] = false;
                    //    dr["EmailRecInvoice"] = false;
                    //    dr["ShutdownAlert"] = false;
                    //    dr["EmailRecTestProp"] = false;
                    //}
                      
                    ViewState["contacttableloc"] = dsCust.Tables[1];
                }
                txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["BillRate"].ToString()));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateOT"].ToString()));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateNT"].ToString()));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateDT"].ToString()));
                txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateTravel"].ToString()));
                txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(dsCust.Tables[0].Rows[0]["RateMileage"].ToString()));
            }  //Mode Condition End ...............
            else
            {
                hdnCustomerAddress.Value = dsCust.Tables[0].Rows[0]["Address"].ToString();
                hdnCustomerCity.Value = dsCust.Tables[0].Rows[0]["City"].ToString();
                hdnCustomerState.Value = dsCust.Tables[0].Rows[0]["State"].ToString();
                hdnCustomerZipCode.Value = dsCust.Tables[0].Rows[0]["Zip"].ToString();
             
             
                // Revert code cs#14087
                //objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
                //DataSet ds = new DataSet();
                //ds = objBL_User.getLocationByID(objPropUser);
               
                //if (ds.Tables[0].Rows[0]["owner"].ToString() != ddlCustomer.SelectedValue)
                //{
                //    dsCust.Tables[1].Merge(ds.Tables[1]);
                //    dsCust.Tables[1].AcceptChanges();
                //    RadGrid_Contacts.DataSource = string.Empty;
                //    RadGrid_Contacts.VirtualItemCount = dsCust.Tables[1].Rows.Count;
                //    RadGrid_Contacts.DataSource = dsCust.Tables[1];
                //    RadGrid_Contacts.Rebind();
                //}
                //else
                //{
                //    RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;
                //    RadGrid_Contacts.DataSource = ds.Tables[1];
                //    RadGrid_Contacts.Rebind();
                //}
            }
        }

        FillGCCustomer();
    }

    protected void showModalPopupServerOperatorButton_Click(object sender, EventArgs e)
    {
        //this.programmaticModalPopup.Show();
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        //this.programmaticModalPopup.Hide();
        //iframe.Attributes["src"] = "";
        //GetCustomerAll();
        //if (Session["custidloc"] != null)
        //{
        //    ddlCustomer.SelectedValue = Session["custidloc"].ToString();
        //    Session["custidloc"] = null;
        //    ddlCustomer_SelectedIndexChanged(sender, e);
        //    txtCustomer.Text = ddlCustomer.SelectedItem.Text;
        //    hdnPatientId.Value = ddlCustomer.SelectedValue;
        //}
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        //this.ModalPopupExtender1.Hide();

        iframeCustomer.Attributes["src"] = "";

        BindOpenCallsRadGrid();
        RadGrid_OpenCalls.Rebind();
    }

 

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindOpenCallsRadGrid();
        RadGrid_OpenCalls.Rebind();
    }

    // Show All Tickets
    protected void lnkShowAllTicket_Click(object sender, EventArgs e)
    {
        txtfromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        ddlStatus.SelectedIndex = 0;
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        BindOpenCallsRadGrid();
        
        foreach (GridColumn column in RadGrid_OpenCalls.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_OpenCalls.MasterTableView.FilterExpression = string.Empty;
        RadGrid_OpenCalls.MasterTableView.Rebind();
        RadGrid_OpenCalls.Rebind();

      

    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        SaveFilter();
        var searchText = string.Empty;
        if (ddlSearch.SelectedValue == "t.cat")
        {
            searchText = ddlCategory.SelectedValue;
        }
        else
        {
            searchText = txtSearch.Text;
        }

      
        // Redirect when close the report
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        if (Request.RawUrl.IndexOf("f=r") == -1)
        {
            redirect = HttpUtility.UrlEncode(Request.RawUrl + "&f=r");

        }

        List<RetainFilter> filters = new List<RetainFilter>();
        var filterExpression = Convert.ToString(RadGrid_OpenCalls.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty("filterExpression"))
        {           

            foreach (GridColumn column in RadGrid_OpenCalls.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;
                if (column.UniqueName == "cat")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("{0}", x)));
                        columnName = "cat";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);                   
                }
            }
        }

        if (filters.Count > 0)
        {
            Session["TicketListRadGVFilters"] = filters;
        }        
        Response.Redirect("CompletedTicketReport.aspx?lid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText + "&department=-1" + "&redirect=" + redirect);
    }
    protected void lnkAddEQ_Click(object sender, EventArgs e)
    {
        string url = txtLocName.Text;
        Response.Redirect("addequipment.aspx?page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "&locname=" + Server.UrlEncode(url));
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

        _DeleteEquipment.EquipID = EquipID;
        _DeleteEquipment.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_DeleteEquipment";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteEquipment);
            }
            else
            {
                objBL_User.DeleteEquipment(objPropUser);
            }

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
            Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
        }
    }
    protected void lnkEditEq_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.SelectedItems)
        {
            Label lblUserID = (Label)item.FindControl("lblId");
            Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
        }
    }
    protected void lnkEditTicket_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvOpenCalls.Rows)
        //{
        //    CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
        //    Label lblTicketId = (Label)di.FindControl("lblTicketId");
        //    Label lblComp = (Label)di.FindControl("lblComp");

        //    if (chkSelected.Checked == true)
        //    {
        //        Panel2.Attributes.Add("style", "display:none");
        //        //iframeCustomer.Attributes["width"] = "1330px";
        //        iframeCustomer.Attributes["src"] = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text;
        //        this.ModalPopupExtender1.Show();
        //    }
        //}
    }

    protected void lnkEditInvoice_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Invoice.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Label lblType = (Label)item.FindControl("lblType");
            HiddenField hdnLinkTo = (HiddenField)item.FindControl("hdnLinkTo");           
           
                if (Convert.ToInt32(hdnLinkTo.Value) == 1)
                {
                    Response.Redirect("addinvoice.aspx?uid=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
                }
                else if (Convert.ToInt32(hdnLinkTo.Value) == 3)
                {
                    Response.Redirect("adddeposit.aspx?id=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect("addreceivepayment.aspx?id=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString());
                }  
        }

    }
    protected void lnkCopyInvoice_Click(object sender, EventArgs e)
    {

    }
    //protected void lnkDeleteInvoice_Click(object sender, EventArgs e)
    //{

    //}
    protected void lnkAddInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("addinvoice.aspx?page=addlocation&lid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkSearchInv_Click(object sender, EventArgs e)
    {
        getData();
        RadGrid_Invoice.Rebind();
    }


    private void getData()
    {
        string selectedSearchValue = Request.Form["radio-group"].ToString();
        hdnSearchValue.Value = Request.Form["radio-group"].ToString();
        string selectedSearchBy = Request.Form["radio-group1"].ToString();
        hdnSearchBy.Value = Request.Form["radio-group1"].ToString();
        string SearchValue = string.Empty;
        string SearchBy = string.Empty;
        if (selectedSearchValue == "rdAll")
        {
            SearchValue = "All";
        }
        else if (selectedSearchValue == "rdOpen")
        {
            SearchValue = "Open";
        }
        else
        {
            SearchValue = "Closed";
        }



        if (selectedSearchBy == "rdAll2")
        {
            SearchBy = "All";
        }
        else if (selectedSearchBy == "rdCharges")
        {
            SearchBy = "Charges";
        }
        else
        {
            SearchBy = "Credits";
        }

        BindInvoicesRadGrid(SearchValue, SearchBy);
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        ddlSearchInv_SelectedIndexChanged(sender, e);
        
        txtInvDtTo.Text = string.Empty;;
        txtInvDtFrom.Text = string.Empty;;
        isShowAllInvoices = true;
        BindInvoicesRadGrid("All", "All");
        cleanFilter();
        //RadGrid_Invoice.Rebind();      
        ishowAllInvoice.Value = "1";

    }
    public void processShowAllTrans()
    {   
      
        isShowAllInvoices = true;
        BindInvoicesRadGrid("All", "All");
        RadGrid_Invoice.Rebind();
        
    }




    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearch.SelectedValue == "t.cat")
        //{
        //    ddlCategory.Visible = true;
        //    txtSearch.Visible = false;
        //}
        //else
        //{
        //    ddlCategory.Visible = false;
        //    txtSearch.Visible = true;
        //}
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;        
        txtInvDt.Style.Add("display", "none");
        txtSearchInv.Style.Add("display", "block");

        if (ishowAllInvoice.Value == "1")
        {
            if (Session["InvToDate"] == null)
            {               
                txtInvDtTo.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtTo.Text = Session["InvToDate"].ToString();
            }
            if (Session["InvFromDate"] == null)
            {
                //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtInvDtFrom.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtFrom.Text = Session["InvFromDate"].ToString();
            }
            ishowAllInvoice.Value = "0";
        }        

        getData();
        cleanFilter();
    }
    public void cleanFilter()
    {
        foreach (GridColumn column in RadGrid_Invoice.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Invoice.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Invoice.MasterTableView.Rebind();
        RadGrid_Invoice.Rebind();
    }
    protected void ddlSearchInv_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearchInv.SelectedValue == "i.Type")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = true;
        //    txtInvDt.Visible = false;
        //}
        //else if (ddlSearchInv.SelectedValue == "i.Status")
        //{
        //    ddlStatusInv.Visible = true;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //}
        //else if (ddlSearchInv.SelectedValue == "i.fdate")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = true;
        //}
        //else if (ddlSearchInv.SelectedValue == "l.loc")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //}
        //else
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = true;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //}
    }
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetDepartment.ConnConfig = Session["config"].ToString();

        List<JobTypeViewModel> _lstJobType = new List<JobTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetDepartment";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepartment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstJobType = serializer.Deserialize<List<JobTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<JobTypeViewModel>(_lstJobType);
        }
        else
        {
            ds = objBL_User.getDepartment(objPropUser);
        }

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem("Select", ""));
    }


    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;

            // if (FileUpload1.HasFile)
            if (Request.QueryString["uid"] != null)
            {
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                    filename = postedFile.FileName;
                  //  filename = filename.Replace(",", "");
                    fullpath = savepath + filename;
                    MIME = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);

                    //if (File.Exists(fullpath))
                    //{
                    //    for (int i = 1; i < 100; i++)
                    //    {
                    //        string tmpFileName = string.Empty;
                    //        if (MIME != null)
                    //        {
                    //            tmpFileName = filename.Replace("." + MIME, "(" + i + ")." + MIME);
                    //        }
                    //        else
                    //        {
                    //            tmpFileName = filename + "(" + i + ")";
                    //        }
                    //        fullpath = savepath + tmpFileName;
                    //        if (!File.Exists(fullpath))
                    //        {
                    //            filename = tmpFileName;
                    //            fullpath = savepath + filename;
                    //            break;
                    //        }
                    //    }


                    //}

                    //if (!Directory.Exists(savepath))
                    //{
                    //    Directory.CreateDirectory(savepath);
                    //}

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

                    //FileUpload1.SaveAs(fullpath);
                    //}

                    objMapData.Screen = "Location";                                     
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = MIME;
                    objMapData.FilePath = fullpath;
                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);
                    //objMapData.Subject = txtNoteSub.Text.Trim();
                    //objMapData.Body = txtNoteBody.Text.Trim();
                    //objMapData.Mode = Convert.ToInt16(ViewState["notesmode"]);
                    //if (ViewState["notesmode"].ToString() == "1")
                    //     objMapData.DocID = Convert.ToInt32(hdnNoteID.Value);
                    //else




                    ////API
                    //_AddFile.Screen = "Location";
                    //_AddFile.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    //_AddFile.TempId = "0";
                    //_AddFile.FileName = filename;
                    //_AddFile.DocTypeMIME = MIME;
                    //_AddFile.FilePath = fullpath;
                    //_AddFile.DocID = 0;
                    //_AddFile.Mode = 0;
                    //_AddFile.ConnConfig = Session["config"].ToString();

                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    string APINAME = "LocationsAPI/AddLocation_AddFile";

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddFile);
                    //}
                    //else
                    //{
                    //    objBL_MapData.AddFile(objMapData);
                    //}
                }


                //UpdateDocInfo();
                //RadGrid_Documents.Rebind();

                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.dtDocs = SaveDocInfo();
                objBL_User.UpdateDocInfo(objPropUser);

                RadGrid_Documents.Rebind();
                //    //API
                //    _UpdateDocInfo.ConnConfig = Session["config"].ToString();
                //    DataTable viewstatedata = SaveDocInfo();

                //    if (viewstatedata.Rows.Count == 0)
                //    {
                //        DataTable returnVal = SaveDocInfoEmptyDatatable();
                //        _UpdateDocInfo.dtDocs = returnVal;
                //    }
                //    else
                //    {
                //        _UpdateDocInfo.dtDocs = SaveDocInfo();
                //    }

                //    if (IsAPIIntegrationEnable == "YES")
                //    {
                //        string APINAME = "LocationsAPI/AddLocation_UpdateDocInfo";

                //        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
                //    }
                //    else
                //    {
                //        objBL_User.UpdateDocInfo(objPropUser);
                //    }
                // UpdateDocInfo();
                //GetDocuments(chkShowAllDocs.Checked);


                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                
            }
          
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

    private void GetDocuments(bool isShowAll)
    {
        bool IsProspect = false;
        if (Request.QueryString["cpw"] != null)
            IsProspect = true;

        if (IsProspect)
        {
            objMapData.Screen = "SalesLead";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());

            _GetLocationDocuments.Screen = "SalesLead";
            _GetLocationDocuments.TicketID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
        }
        else
        {
            objMapData.Screen = "Location";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            _GetLocationDocuments.Screen = "Location";
            _GetLocationDocuments.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }

        objMapData.TempId = "0";

        objMapData.Mode = 1;
        objMapData.ConnConfig = Session["config"].ToString();

        _GetLocationDocuments.TempId = "0";

        _GetLocationDocuments.Mode = 1;
        _GetLocationDocuments.ConnConfig = Session["config"].ToString();
        //var isShowAll = true;
        DataSet ds = new DataSet();

        List<GetLocationDocumentsViewModel> _lstGetLocationDocuments = new List<GetLocationDocumentsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetLocationDocuments";

            _GetLocationDocuments.isShowAll = isShowAll;
            _GetLocationDocuments.isLocation = IsProspect;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationDocuments);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetLocationDocuments = serializer.Deserialize<List<GetLocationDocumentsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetLocationDocumentsViewModel>(_lstGetLocationDocuments);
        }
        else
        {
            ds = objBL_MapData.GetLocationDocuments(objMapData, isShowAll, !IsProspect);
        }

        RadGrid_Documents.DataSource = ds.Tables[0];
        RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
        //RadGrid_Documents.DataBind();

        //gvDocuments.DataSource = ds.Tables[0];
        //gvDocuments.DataBind();
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }
    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] CommandArgument = btn.CommandArgument.Replace(btn.Text, " ").Split(',');

        string FileName = btn.Text;
        string FilePath = CommandArgument[1].Trim() + btn.Text.Trim();
        //string[] CommandArgument = btn.CommandArgument.Replace(btn.Text, " ").Split(',');

        //string FileName = btn.Text;
        //string FilePath = CommandArgument[1].Trim() + btn.Text.Trim();

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
        if (hdnDeleteDocument.Value == "Y")
        {
            foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
            {
                //CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
                Label lblID = (Label)item.FindControl("lblId");
                Label lblScreen = (Label)item.FindControl("lblScreen");
                if (lblScreen.Text.Equals("Location", StringComparison.InvariantCultureIgnoreCase))
                {
                    DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "FileDeleteAccessWarning", "noty({text: 'Cant remove this document.  It was attacthed from another place',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;

            _DeleteFile.ConnConfig = Session["config"].ToString();
            _DeleteFile.DocumentID = DocumentID;

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_DeleteFile";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteFile);
            }
            else
            {
                objBL_MapData.DeleteFile(objMapData);
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();

            //API
            _UpdateDocInfo.ConnConfig = Session["config"].ToString();
            DataTable viewstatedata = SaveDocInfo();

            if (viewstatedata.Rows.Count == 0)
            {
                DataTable returnVal = SaveDocInfoEmptyDatatable();
                _UpdateDocInfo.dtDocs = returnVal;
            }
            else
            {
                _UpdateDocInfo.dtDocs = SaveDocInfo();
            }

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_UpdateDocInfo";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
            }
            else
            {
                objBL_User.UpdateDocInfo(objPropUser);
            }

            //GetDocuments(chkShowAllDocs.Checked);
            RadGrid_Documents.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private int CheckSageID(string job)
    {
        string DSN = System.Web.Configuration.WebConfigurationManager.AppSettings["SageDSN"].Trim();
        string query = "Select job from master_jcm_job_1 where Job = ?";
        OdbcConnection odbccon = new OdbcConnection(DSN);
        if (odbccon.State != ConnectionState.Open)
        {
            odbccon.Open();
        }
        System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(query, odbccon);
        da.SelectCommand.Parameters.AddWithValue("@Job", job);
        DataTable dt = new DataTable();
        da.Fill(dt);
        odbccon.Close();
        int count = dt.Rows.Count;
        return count;
    }
    protected void btnSageID_Click(object sender, EventArgs e)
    {
        int i = 1;
        string str = "Account # Already Exists in Sage";
        if (txtAcctno.Text.Trim() != string.Empty)
        {
            if (txtAcctno.Text.Trim().Length == 11 && txtAcctno.Text.Trim().Substring(2, 1) == "-" && txtAcctno.Text.Trim().Substring(8, 1) == "-")
            {
                if (ViewState["mode"].ToString() == "1")
                {
                    if (hdnAcctID.Value.Trim() == txtAcctno.Text.Trim())
                    {
                        return;
                    }
                }
                try
                {
                    i = CheckSageID(txtAcctno.Text.Trim());
                    if (i == 0)
                    {
                        str = "Account # Available!";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                    }
                }
                catch (Exception ex)
                {
                    string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,    closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: 'Invalid format',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
            }
        }
    }

    private int SageAlert()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        DataSet dsLastSync = new DataSet();
        List<GeneralViewModel> _lstGeneral = new List<GeneralViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetSagelatsync";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGeneral = serializer.Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
            dsLastSync = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneral);
        }
        else
        {
            dsLastSync = objBL_General.getSagelatsync(objGeneral);
        }

        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        int count = 0;
        if (intintegration == 1)
        {
            int i = 1;
            string str = "Account # Already Exists in Sage..";
            try
            {
                if (txtAcctno.Text.Trim() != string.Empty)
                {
                    if (txtAcctno.Text.Trim().Length == 11 && txtAcctno.Text.Trim().Substring(2, 1) == "-" && txtAcctno.Text.Trim().Substring(8, 1) == "-")
                        i = CheckSageID(txtAcctno.Text.Trim());
                    else
                        i = -1;
                }

                if (i != 0)
                {
                    if (i == -1)
                        str = "Invalid Account # format";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,  closable : true});", true);
                    count = 1;
                }
            }
            catch (Exception ex)
            {
                string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true, closable : true});", true);
                count = 1;
            }
        }
        return count;
    }
    private void FillContractBill()
    {
        List<ContractBill> _lstBill = new List<ContractBill>();
        _lstBill = ContractBilling.GetAll();

        //ddlContractBill.Items.Add(new ListItem(":: Select ::", "Select"));
        //ddlContractBill.AppendDataBoundItems = true;

        ddlContractBill.DataSource = _lstBill;
        ddlContractBill.DataValueField = "ID";
        ddlContractBill.DataTextField = "Name";
        ddlContractBill.DataBind();
    }

    protected void ddlContractBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"]);

            _IsExistContractByLoc.ConnConfig = Session["config"].ToString();
            _IsExistContractByLoc.Loc = Convert.ToInt32(Request.QueryString["uid"]);

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_IsExistContractByLoc";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistContractByLoc);

                objProp_Contracts.IsExistContract = Convert.ToBoolean(_APIResponse.ResponseData);
            }
            else
            {
                objProp_Contracts.IsExistContract = objBL_Contracts.IsExistContractByLoc(objProp_Contracts);
            }

            if (!objProp_Contracts.IsExistContract)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            }
        }
        else
        {
            if (ddlContractBill.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "dispWarningContract", "dispWarningContract();", true);
            }

        }

    }

    public string ShowHoverText(object desc, object reason)
    {
        string result = string.Empty;
        result = "<B>Reason</B>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><br/><B>Resolution</B>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }

    protected void gvOpenCalls_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AjaxControlToolkit.HoverMenuExtender ajxHover = (AjaxControlToolkit.HoverMenuExtender)e.Row.FindControl("hmeRes");
            e.Row.ID = e.Row.RowIndex.ToString();
            ajxHover.TargetControlID = e.Row.ID;
        }
    }




    private DataTable CreateAlertTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("AlertID", typeof(int));
        dt.Columns.Add("AlertCode", typeof(string));
        dt.Columns.Add("AlertName", typeof(string));
        dt.Columns.Add("AlertSubject", typeof(string));
        dt.Columns.Add("AlertMessage", typeof(string));
        return dt;
    }

    private void makeAlertList()
    {
        rtAlerts.DataSource = newrow(CreateAlertTable());
        rtAlerts.DataBind();
    }

    private DataTable newrow(DataTable dt)
    {
        DataTable dtType = FillAlertType();
        foreach (DataRow drt in dtType.Rows)
        {
            DataRow dr = dt.NewRow();
            dr["AlertID"] = 0;
            dr["AlertCode"] = drt["Code"];
            dr["AlertName"] = drt["alertname"];
            dr["AlertSubject"] = DBNull.Value;
            dr["AlertMessage"] = DBNull.Value;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    //private void AddNewRow()
    //{
    //    GridData();
    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["alerttable"];
    //    dt.Rows.Add(newrow(dt));
    //    Session["alerttable"] = dt;
    //    BindGrid();
    //}
    //private void GridData()
    //{
    //    DataTable dt = (DataTable)Session["alerttable"];

    //    DataTable dtDetails = dt.Clone();

    //    foreach (RepeaterItem gr in rtAlerts.Items)
    //    {
    //        DropDownList ddlCode = (DropDownList)gr.FindControl("ddlCode");
    //        TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
    //        TextBox lblMessage = (TextBox)gr.FindControl("lblMessage");
    //        Label lblID = (Label)gr.FindControl("lblID");

    //        DataRow dr = dtDetails.NewRow();
    //        dr["AlertCode"] = ddlCode.SelectedValue;
    //        dr["AlertSubject"] = lblDesc.Text;
    //        dr["AlertMessage"] = lblMessage.Text;
    //        dr["AlertID"] = lblID.Text;

    //        dtDetails.Rows.Add(dr);
    //    }

    //    Session["alerttable"] = dtDetails;
    //}
    //private void BindGrid()
    //{
    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["alerttable"];

    //    //gvAlerts.DataSource = dt;
    //    //gvAlerts.DataBind();
    //    rtAlerts.DataSource = dt;
    //    rtAlerts.DataBind();

    //    //((Label)rtAlerts.FooterTemplate.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
    //}
    //protected void ibtnDeleteItem_Click(object sender, EventArgs e)
    //{
    //    DeleteREPItem();
    //}

    //private void DeleteREPItem()
    //{
    //    GridData();

    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["alerttable"];

    //    int count = 0;
    //    foreach (RepeaterItem gr in rtAlerts.Items)
    //    {
    //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //        Label lblIndex = (Label)gr.FindControl("lblIndex");
    //        int index = Convert.ToInt32(lblIndex.Text) - 1;

    //        if (chkSelect.Checked == true)
    //        {
    //            dt.Rows.RemoveAt(index - count);
    //            count++;
    //        }
    //    }

    //    if (dt.Rows.Count == 0)
    //    {
    //        dt.Rows.Add(newrow(dt));
    //    }

    //    Session["alerttable"] = dt;
    //    BindGrid();
    //}

    private DataTable CreateAlertContactTable()
    {
        //Session["alertCtable"] = null;
        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("screenid", typeof(int));
        dt.Columns.Add("screenname", typeof(string));
        dt.Columns.Add("alertid", typeof(int));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("email", typeof(bool));
        dt.Columns.Add("text", typeof(bool));
        dt.Columns.Add("alertcode", typeof(string));
        dt.Rows.Add(newrowContact(dt));
        //Session["alertCtable"] = dt;
        return dt;
    }
    private DataRow newrowContact(DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["id"] = 0;
        dr["screenid"] = 0;
        dr["email"] = 0;
        dr["text"] = 0;
        return dr;
    }

    private void AddNewRowContact(Repeater rtAlertContacts)
    {

        DataTable dt = GridDataContact(rtAlertContacts);
        //dt = (DataTable)Session["alertCtable"];
        dt.Rows.Add(newrowContact(dt));
        //Session["alertCtable"] = dt;
        //BindGridContact(rtAlertContacts,dt);
        rtAlertContacts.DataSource = dt;
        rtAlertContacts.DataBind();
    }

    private DataTable GridDataContact(Repeater rtAlertContacts)
    {
        //DataTable dt = (DataTable)Session["alertCtable"];
        //DataTable dtDetails = dt.Clone();
        DataTable dtDetails = CreateAlertContactTable().Clone();
        foreach (RepeaterItem gr in rtAlertContacts.Items)
        {
            TextBox txtContact = (TextBox)gr.FindControl("txtContact");
            HiddenField hdnsid = (HiddenField)gr.FindControl("hdnsid");
            HiddenField hdnsname = (HiddenField)gr.FindControl("hdnsname");
            CheckBox chkMail = (CheckBox)gr.FindControl("chkMail");
            CheckBox chkText = (CheckBox)gr.FindControl("chkText");
            Label lblAlertCode = (Label)(rtAlertContacts.Parent.FindControl("lblAlertCode"));//(Label)gr.FindControl("lblAlertCode");
            //ImageButton lnkAddnewRow = (ImageButton)(rtAlertContacts.Parent.FindControl("lnkAddnewRow"));
            ////txtContact.Attributes["onBlur"] = "$('#'"+lnkAddnewRow.ClientID+"').click();" ;
            //txtContact.Attributes.Add("onblur", "$('#'" + lnkAddnewRow.ClientID + "').click();");
            DataRow dr = dtDetails.NewRow();
            dr["ID"] = 0;
            dr["alertcode"] = lblAlertCode.Text;
            dr["name"] = txtContact.Text;
            dr["screenname"] = hdnsname.Value;
            dr["screenid"] = hdnsid.Value;
            dr["email"] = chkMail.Checked;
            dr["text"] = chkText.Checked;

            dtDetails.Rows.Add(dr);
        }

        return dtDetails;
        //Session["alertCtable"] = dtDetails;
    }

    //private void BindGridContact(Repeater rtAlertContacts, DataTable dt)
    //{
    //    //DataTable dt = new DataTable();
    //    //dt = (DataTable)Session["alertCtable"];
    //    rtAlertContacts.DataSource = dt;
    //    rtAlertContacts.DataBind();
    //}

    private DataTable FillAlertType()
    {
        DataSet ds = new DataSet();
        objAlerts.ConnConfig = Session["config"].ToString();
        _GetAlertType.ConnConfig = Session["config"].ToString();

        List<GetAlertTypeViewModel> _lstGetAlertType = new List<GetAlertTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetAlertType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAlertType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetAlertType = serializer.Deserialize<List<GetAlertTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetAlertTypeViewModel>(_lstGetAlertType);
        }
        else
        {
            ds = objBL_Alerts.GetAlertType(objAlerts);
        }

        return ds.Tables[0];
        //DataRow dr = ds.Tables[0].NewRow();
        //dr["code"] = string.Empty;
        //dr["alertname"] = "-Select-";
        //ds.Tables[0].Rows.InsertAt(dr, 0);
        //dtAlertType = ds.Tables[0];
    }

    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ////string customerId = (e.Item.FindControl("hfCustomerId") as HiddenField).Value;
            //Repeater rtAlertContacts = e.Item.FindControl("rtAlertContacts") as Repeater;
            //rtAlertContacts.DataSource = CreateAlertContactTable();
            //rtAlertContacts.DataBind();
        }
    }

    protected void rtAlertContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            //Repeater rtAlertContacts = source as Repeater;
            //AddNewRowContact(rtAlertContacts);
        }
    }
    protected void rtAlerts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            Repeater rtAlertContacts = e.Item.FindControl("rtAlertContacts") as Repeater;
            AddNewRowContact(rtAlertContacts);
        }
    }

    private DataSet GetAlertData()
    {
        DataSet dsAlerts = new DataSet();
        DataTable dtAlert = CreateAlertTable();
        DataTable dtContact = CreateAlertContactTable().Clone();
        foreach (RepeaterItem rt in rtAlerts.Items)
        {
            //Label lblID = (Label)rt.FindControl("lblID");
            Label lblAlertCode = (Label)rt.FindControl("lblAlertCode");
            Label lblAlertType = (Label)rt.FindControl("lblAlertType");
            TextBox lblDesc = (TextBox)rt.FindControl("lblDesc");
            TextBox lblMessage = (TextBox)rt.FindControl("lblMessage");

            DataRow drAlert = dtAlert.NewRow();
            drAlert["AlertID"] = 0;
            drAlert["AlertCode"] = lblAlertCode.Text;
            drAlert["AlertName"] = lblAlertType.Text;
            drAlert["AlertSubject"] = lblDesc.Text;
            drAlert["AlertMessage"] = lblMessage.Text;


            Repeater rtAlertContacts = rt.FindControl("rtAlertContacts") as Repeater;
            DataTable dtContactData = GridDataContact(rtAlertContacts);
            foreach (DataRow dr in dtContactData.Rows)
            {
                dtContact.ImportRow(dr);
            }
            if (dtContactData.Rows.Count > 0)
            {
                dtAlert.Rows.Add(drAlert);
            }
            else
            {
                if (lblDesc.Text != string.Empty)
                    dtAlert.Rows.Add(drAlert);
            }
        }
        dsAlerts.Tables.Add(dtAlert);
        dsAlerts.Tables.Add(dtContact);
        return dsAlerts;
    }

    private void GetAlerts()
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            objAlerts.ConnConfig = Session["config"].ToString();
            objAlerts.loc = Convert.ToInt32(Request.QueryString["uid"].ToString());

            _GetAlerts.ConnConfig = Session["config"].ToString();
            _GetAlerts.loc = Convert.ToInt32(Request.QueryString["uid"].ToString());

            ListGetAlerts _lstGetAlerts = new ListGetAlerts();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_GetAlerts";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAlerts);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAlerts = serializer.Deserialize<ListGetAlerts>(_APIResponse.ResponseData);

                ds1 = _lstGetAlerts.lstTable1.ToDataSet();
                ds2 = _lstGetAlerts.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                ds = objBL_Alerts.getAlerts(objAlerts);
            }

            foreach (RepeaterItem rt in rtAlerts.Items)
            {
                Label lblAlertCode = (Label)rt.FindControl("lblAlertCode");
                TextBox lblDesc = (TextBox)rt.FindControl("lblDesc");
                TextBox lblMessage = (TextBox)rt.FindControl("lblMessage");
                Repeater rtAlertContacts = rt.FindControl("rtAlertContacts") as Repeater;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (lblAlertCode.Text == dr["AlertCode"].ToString())
                    {
                        lblDesc.Text = dr["AlertSubject"].ToString();
                        lblMessage.Text = dr["AlertMessage"].ToString();
                    }
                }

                DataTable dtContact = CreateAlertContactTable().Clone();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (lblAlertCode.Text == dr["AlertCode"].ToString())
                    {
                        DataRow drContact = dtContact.NewRow();
                        drContact["ID"] = 0;
                        drContact["alertcode"] = dr["alertcode"].ToString();
                        drContact["name"] = dr["name"];
                        drContact["screenname"] = dr["screenname"];
                        drContact["screenid"] = dr["screenid"];
                        drContact["email"] = dr["email"];
                        drContact["text"] = dr["text"];
                        dtContact.Rows.Add(drContact);
                    }
                }
                rtAlertContacts.DataSource = dtContact;
                rtAlertContacts.DataBind();

            }
        }
        catch { }
    }
    private void PopulateCustomer()
    {
        if (Request.QueryString["lid"] != null)
        {
            ddlCustomer.SelectedValue = Request.QueryString["lid"].ToString();
            txtCustomer.Text = ddlCustomer.SelectedItem.Text;
            hdnPatientId.Value = ddlCustomer.SelectedValue;
            //ddlCustomer_SelectedIndexChanged(sender, e);
            if (Request.QueryString["t"] == null)
            {
                selectCustomer();
            }
               
        }
    }

    private void GetDefaultWorker()
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();
        _GetDefaultRouteTerr.ConnConfig = Session["config"].ToString();

        ListGetDefaultRouteTerr _lstGetDefaultRouteTerr = new ListGetDefaultRouteTerr();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetDefaultRouteTerr";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDefaultRouteTerr);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetDefaultRouteTerr = serializer.Deserialize<ListGetDefaultRouteTerr>(_APIResponse.ResponseData);

            ds1 = _lstGetDefaultRouteTerr.lstTable1.ToDataSet();
            ds2 = _lstGetDefaultRouteTerr.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
        }
        else
        {
            ds = objBL_User.getDefaultRouteTerr(objPropUser);
        }

        if (ds.Tables[0].Rows.Count > 0)
            ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["id"].ToString();
        if (ds.Tables[1].Rows.Count > 0)
            ddlTerr.SelectedValue = ds.Tables[1].Rows[0]["id"].ToString();

    }

    private void FillGCCustomer()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);

        _GetGCCustomer.ConnConfig = Session["config"].ToString();
        _GetGCCustomer.CustomerID = Convert.ToInt32(hdnPatientId.Value);

        List<GetGCCustomerViewModel> _lstGetGCCustomer = new List<GetGCCustomerViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetGCCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetGCCustomer);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetGCCustomer = serializer.Deserialize<List<GetGCCustomerViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetGCCustomerViewModel>(_lstGetGCCustomer);
        }
        else
        {
            ds = objBL_User.getGCCustomer(objPropUser);
        }

        if (ds.Tables[0].Rows.Count == 1)
        {
            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("General Contractor", StringComparison.CurrentCultureIgnoreCase))
            {
                //hdnGCID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                //hdnGCIDtemp.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnGCName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                GCtxtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                GCtxtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                GCtxtAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                //GCddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                GCtxtState.Text = ds.Tables[0].Rows[0]["state"].ToString();
                GCtxtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                GCtxtPostalCode.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                GCtxtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                GCtxtCName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                GCtxtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                GCtxtFAX.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                GCtxtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                GCtxtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                hdnGContractorID.Value = ds.Tables[0].Rows[0]["Rol"].ToString();
                hdnGCNameupdate.Value = "0";
            }
            //else
            //{
            //    //hdnGCID.Value = string.Empty;
            //    //hdnGCIDtemp.Value = string.Empty;
            //    hdnGCName.Value = string.Empty;
            //    GCtxtName.Text = string.Empty;
            //    GCtxtcity.Text = string.Empty;
            //    GCtxtAddress.Text = string.Empty;
            //    GCddlState.SelectedValue = "Select State";
            //    GCtxtCountry.Text = string.Empty;
            //    GCtxtPostalCode.Text = string.Empty;
            //    GCtxtRemarks.Text = string.Empty;
            //    GCtxtCName.Text = string.Empty;
            //    GCtxtPhone.Text = string.Empty;
            //    GCtxtFAX.Text = string.Empty;
            //    GCtxtEmailWeb.Text = string.Empty;
            //    GCtxtMobile.Text = string.Empty;
            //    hdnGContractorID.Value = string.Empty;
            //    hdnGCNameupdate.Value = string.Empty;
            //}

            if (ds.Tables[0].Rows[0]["type"].ToString().Equals("Homeowner", StringComparison.CurrentCultureIgnoreCase))
            {
                hotxtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                hotxtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                HotxtAddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["state"].ToString()))
                {
                    //hotddlstate.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                    hottxtstate.Text = ds.Tables[0].Rows[0]["state"].ToString();
                }
                hotxtZIP.Text = ds.Tables[0].Rows[0]["zip"].ToString();
                hotxtCountry.Text = ds.Tables[0].Rows[0]["country"].ToString();
                hotxtPhone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                hotxtMobile.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                HotxtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                HotxtEmailWeb.Text = ds.Tables[0].Rows[0]["email"].ToString();
                hotxtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                hotxtCName.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                hdnHomeOwnerID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                hdnHOName.Value = ds.Tables[0].Rows[0]["name"].ToString();
                hdnHONameupdate.Value = "0";
            }
            //else
            //{
            //    hotxtname.Text = string.Empty;
            //    hotxtcity.Text = string.Empty;
            //    HotxtAddress.Text = string.Empty;
            //    hotddlstate.SelectedValue = "Select State";
            //    hotxtZIP.Text = string.Empty;
            //    hotxtCountry.Text = string.Empty;
            //    hotxtPhone.Text = string.Empty;
            //    hotxtMobile.Text = string.Empty;
            //    HotxtFax.Text = string.Empty;
            //    HotxtEmailWeb.Text = string.Empty;
            //    hotxtRemarks.Text = string.Empty;
            //    hotxtCName.Text = string.Empty;
            //    hdnHomeOwnerID.Value = string.Empty;
            //    hdnHOName.Value = string.Empty;
            //    hdnHONameupdate.Value = string.Empty;
            //}
        }
    }

    protected void chkEMail_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEmail.Checked)
        {
            //rfvEmailInvoice.Enabled = true;
            //vceEmailInvoice.Enabled = true;
        }
        else
        {
            //rfvEmailInvoice.Enabled = false;
            //vceEmailInvoice.Enabled = false;
        }
    }

    protected void incDate_Click(object sender, EventArgs e)
    {
        //if (rdDay.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(1).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}

        //if (rdWeek.Checked)
        //{

        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(7).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(7).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdMonth.Checked)
        //{

        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(1).ToShortDateString();

        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdQuarter.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(3).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(3).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdYear.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddYears(1).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}

    }
    protected void decDate_Click(object sender, EventArgs e)
    {
        //if (rdDay.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(-1).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(-1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}

        //if (rdWeek.Checked)
        //{

        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddDays(-7).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(7).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdMonth.Checked)
        //{

        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(-1).ToShortDateString();

        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(-1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdQuarter.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddMonths(-3).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddMonths(-3).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}
        //if (rdYear.Checked)
        //{
        //    txtfromDate.Text = Convert.ToDateTime(txtfromDate.Text).AddYears(-1).ToShortDateString();
        //    txtToDate.Text = Convert.ToDateTime(txtToDate.Text).AddYears(-1).ToShortDateString();
        //    Session["ToDate"] = txtToDate.Text;
        //    Session["fromDate"] = txtfromDate.Text;
        //}

    }
    protected void rdDay_CheckedChanged()
    {
        txtfromDate.Text = DateTime.Now.ToShortDateString();
        txtToDate.Text = DateTime.Now.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;
    }
    protected void rdWeek_CheckedChanged()
    {
        var now = System.DateTime.Now;

        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtfromDate.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtToDate.Text = LastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;
    }

    protected void rdMonth_CheckedChanged()
    {
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtfromDate.Text = firstDayOfMonth.ToShortDateString();
        txtToDate.Text = lastDayOfMonth.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;

    }
    protected void rdQuarter_CheckedChanged()
    {
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtfromDate.Text = firstDayOfQuarter.ToShortDateString();
        txtToDate.Text = lastDayOfQuarter.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;
    }
    protected void rdYear_CheckedChanged()
    {
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtfromDate.Text = firstDay.ToShortDateString();
        txtToDate.Text = lastDay.ToShortDateString();
        Session["ToDate"] = txtToDate.Text;
        Session["fromDate"] = txtfromDate.Text;
    }
    private void AddClass()
    {

        //if (rdDay.Checked)
        //{
        //    lblDay.Attributes.Add("class", "labelactive ");
        //    lblWeek.Attributes.Add("class", "");
        //    lblMonth.Attributes.Add("class", "");
        //    lblQuarter.Attributes.Add("class", "");
        //    lblYear.Attributes.Add("class", "");

        //}

        //if (rdWeek.Checked)
        //{
        //    lblDay.Attributes.Add("class", "");
        //    lblWeek.Attributes.Add("class", "labelactive ");
        //    lblMonth.Attributes.Add("class", "");
        //    lblQuarter.Attributes.Add("class", "");
        //    lblYear.Attributes.Add("class", "");

        //}
        //if (rdMonth.Checked)
        //{
        //    lblDay.Attributes.Add("class", "");
        //    lblWeek.Attributes.Add("class", "");
        //    lblMonth.Attributes.Add("class", "labelactive ");
        //    lblQuarter.Attributes.Add("class", "");
        //    lblYear.Attributes.Add("class", "");
        //}
        //if (rdQuarter.Checked)
        //{
        //    lblDay.Attributes.Add("class", "");
        //    lblWeek.Attributes.Add("class", "");
        //    lblMonth.Attributes.Add("class", "");
        //    lblQuarter.Attributes.Add("class", "labelactive ");
        //    lblYear.Attributes.Add("class", "");

        //}
        //if (rdYear.Checked)
        //{
        //    lblDay.Attributes.Add("class", "");
        //    lblWeek.Attributes.Add("class", "");
        //    lblMonth.Attributes.Add("class", "");
        //    lblQuarter.Attributes.Add("class", "");
        //    lblYear.Attributes.Add("class", "labelactive ");
        //}

    }

    private void InvAddClass(String eventTarget)
    {
        //switch (eventTarget)
        //{
        //    case "Day":
        //        lblInvDay.Attributes.Add("class", "labelactive ");
        //        lblInvWeek.Attributes.Add("class", "");
        //        lblInvMonth.Attributes.Add("class", "");
        //        lblInvQuarter.Attributes.Add("class", "");
        //        lblInvYear.Attributes.Add("class", "");
        //        break;
        //    case "Week":
        //        lblInvDay.Attributes.Add("class", "");
        //        lblInvWeek.Attributes.Add("class", "labelactive ");
        //        lblInvMonth.Attributes.Add("class", "");
        //        lblInvQuarter.Attributes.Add("class", "");
        //        lblInvYear.Attributes.Add("class", "");
        //        break;
        //    case "Month":
        //        lblInvDay.Attributes.Add("class", "");
        //        lblInvWeek.Attributes.Add("class", "");
        //        lblInvMonth.Attributes.Add("class", "labelactive ");
        //        lblInvQuarter.Attributes.Add("class", "");
        //        lblInvYear.Attributes.Add("class", "");
        //        break;
        //    case "Quarter":
        //        lblInvDay.Attributes.Add("class", "");
        //        lblInvWeek.Attributes.Add("class", "");
        //        lblInvMonth.Attributes.Add("class", "");
        //        lblInvQuarter.Attributes.Add("class", "labelactive ");
        //        lblInvYear.Attributes.Add("class", "");

        //        break;
        //    case "Year":
        //        lblInvDay.Attributes.Add("class", "");
        //        lblInvWeek.Attributes.Add("class", "");
        //        lblInvMonth.Attributes.Add("class", "");
        //        lblInvQuarter.Attributes.Add("class", "");
        //        lblInvYear.Attributes.Add("class", "labelactive ");
        //        break;
        //}


        string script = "function f(){$('#accrdtransactions').click(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "anchor", "location.hash = '#accrdtransactions';", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }



    protected void InvDecDate_Click(object sender, EventArgs e)
    {
        //if (InvrdDay.Checked)
        //{
        //    txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddDays(-1).ToShortDateString();
        //    txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddDays(-1).ToShortDateString();
        //    Session["InvToDate"] = txtInvDtTo.Text;
        //    Session["InvfromDate"] = txtfromDate.Text;
        //}

        //if (InvrdWeek.Checked)
        //{

        //    txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddDays(-7).ToShortDateString();
        //    txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddDays(-7).ToShortDateString();
        //    Session["InvToDate"] = txtInvDtTo.Text;
        //    Session["InvfromDate"] = txtInvDtFrom.Text;
        //}
        //if (InvrdMonth.Checked)
        //{

        //    txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddMonths(-1).ToShortDateString();

        //    txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddMonths(-1).ToShortDateString();
        //    Session["InvToDate"] = txtInvDtTo.Text;
        //    Session["InvfromDate"] = txtInvDtFrom.Text;
        //}
        //if (InvrdQuarter.Checked)
        //{
        //    txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddMonths(-3).ToShortDateString();
        //    txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddMonths(-3).ToShortDateString();
        //    Session["InvToDate"] = txtInvDtTo.Text;
        //    Session["InvfromDate"] = txtInvDtFrom.Text;
        //}
        //if (InvrdYear.Checked)
        //{
        //    txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddYears(-1).ToShortDateString();
        //    txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddYears(-1).ToShortDateString();
        //    Session["InvToDate"] = txtInvDtTo.Text;
        //    Session["InvfromDate"] = txtInvDtFrom.Text;
        //}
        string script = "function f(){$('#accrdtransactions').click(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "anchor", "location.hash = '#accrdtransactions';", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void InvIncDate_Click(object sender, EventArgs e)
    {
    //    if (InvrdDay.Checked)
    //    {
    //        txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddDays(1).ToShortDateString();
    //        txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddDays(1).ToShortDateString();
    //        Session["InvToDate"] = txtInvDtTo.Text;
    //        Session["InvfromDate"] = txtInvDtFrom.Text;
    //    }

    //    if (InvrdWeek.Checked)
    //    {

    //        txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddDays(7).ToShortDateString();
    //        txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddDays(7).ToShortDateString();
    //        Session["InvToDate"] = txtInvDtTo.Text;
    //        Session["InvfromDate"] = txtInvDtFrom.Text;
    //    }
    //    if (InvrdMonth.Checked)
    //    {

    //        txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddMonths(1).ToShortDateString();

    //        txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddMonths(1).ToShortDateString();
    //        Session["InvToDate"] = txtInvDtTo.Text;
    //        Session["InvfromDate"] = txtInvDtFrom.Text;
    //    }
    //    if (InvrdQuarter.Checked)
    //    {
    //        txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddMonths(3).ToShortDateString();
    //        txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddMonths(3).ToShortDateString();
    //        Session["InvToDate"] = txtInvDtTo.Text;
    //        Session["InvfromDate"] = txtInvDtFrom.Text;
    //    }
    //    if (InvrdYear.Checked)
    //    {
    //        txtInvDtFrom.Text = Convert.ToDateTime(txtInvDtFrom.Text).AddYears(1).ToShortDateString();
    //        txtInvDtTo.Text = Convert.ToDateTime(txtInvDtTo.Text).AddYears(1).ToShortDateString();
    //        Session["InvToDate"] = txtInvDtTo.Text;
    //        Session["InvfromDate"] = txtInvDtFrom.Text;
    //    }
        string script = "function f(){$('#accrdtransactions').click(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "anchor", "location.hash = '#accrdtransactions';", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    protected void InvrdDay_CheckedChanged()
    {
        txtInvDtFrom.Text = DateTime.Now.ToShortDateString();
        txtInvDtTo.Text = DateTime.Now.ToShortDateString();
        Session["InvToDate"] = txtInvDtTo.Text;
        Session["InvfromDate"] = txtInvDtFrom.Text;
        
    }
    protected void InvrdWeek_CheckedChanged()
    {
        var now = System.DateTime.Now;

        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtInvDtFrom.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtInvDtTo.Text = LastDay.ToShortDateString();
        Session["InvToDate"] = txtInvDtTo.Text;
        Session["InvfromDate"] = txtInvDtFrom.Text;
    }

    protected void InvrdMonth_CheckedChanged()
    {
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtInvDtFrom.Text = firstDayOfMonth.ToShortDateString();
        txtInvDtTo.Text = lastDayOfMonth.ToShortDateString();
        Session["InvToDate"] = txtInvDtTo.Text;
        Session["InvfromDate"] = txtInvDtFrom.Text;

    }
    protected void InvrdQuarter_CheckedChanged()
    {
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtInvDtFrom.Text = firstDayOfQuarter.ToShortDateString();
        txtInvDtTo.Text = lastDayOfQuarter.ToShortDateString();
        Session["InvToDate"] = txtInvDtTo.Text;
        Session["InvfromDate"] = txtInvDtFrom.Text;
    }
    protected void InvrdYear_CheckedChanged()
    {
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtInvDtFrom.Text = firstDay.ToShortDateString();
        txtInvDtTo.Text = lastDay.ToShortDateString();
        Session["InvToDate"] = txtInvDtTo.Text;
        Session["InvfromDate"] = txtInvDtFrom.Text;

        BindInvoicesRadGrid("All", "All");
        RadGrid_Invoice.Rebind();
    }

    protected void txtInvDtTo_TextChanged(object sender, EventArgs e)
    {
        Session["InvToDate"] = txtInvDtTo.Text;

    }
    protected void txtInvDtFrom_TextChanged(object sender, EventArgs e)
    {
        Session["InvfromDate"] = txtInvDtFrom.Text;
    }

    protected void RadGrid_Contacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetLocationByID.DBName = Session["dbname"].ToString();
                _GetLocationByID.ConnConfig = Session["config"].ToString();
                _GetLocationByID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataSet ds5 = new DataSet();

                ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetLocationByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

                    ds1 = _lstGetLocationByID.lstTable1.ToDataSet();
                    ds2 = _lstGetLocationByID.lstTable2.ToDataSet();
                    ds3 = _lstGetLocationByID.lstTable3.ToDataSet();
                    ds4 = _lstGetLocationByID.lstTable4.ToDataSet();
                    ds5 = _lstGetLocationByID.lstTable5.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    DataTable dt5 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];
                    dt4 = ds4.Tables[0];
                    dt5 = ds5.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    dt4.TableName = "Table4";
                    dt5.TableName = "Table5";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                }
                else
                {
                    ds = objBL_User.getLocationByID(objPropUser);
                }

                ViewState["contacttableloc"] = ds.Tables[1];
                RadGrid_Contacts.VirtualItemCount = ds.Tables[1].Rows.Count;
                RadGrid_Contacts.DataSource = ds.Tables[1];
            }
        }
        catch { }
    }

    protected void RadGrid_Contacts_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Contacts.Items)
        {
            Label lblMail = (Label)item.FindControl("lblEmail");
            if (lblMail != null)
            {
                String email = lblMail.Text;
                item.Attributes["onclick"] = "SelectRowmailPage('" + lblMail.ClientID + "','" + lnkMail.ClientID + "');";
            }
            //item.Attributes["ondblclick"] = "window.open('addcustomer.aspx?uid=" + lblCustID.Text + "','_self');";
        }
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Contacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_Contacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Contacts.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Equip_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Equip.AllowCustomPaging = !ShouldApplySortFilterOrGroupEquip();

            if (Request.QueryString["uid"] != null)
            {
                DataSet ds = new DataSet();
                var _objUser = new User();
                _objUser.ConnConfig = Session["config"].ToString();
                _objUser.SearchBy = string.Empty;

                _objUser.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _objUser.InstallDate = string.Empty;
                _objUser.ServiceDate = string.Empty;
                _objUser.Price = string.Empty;
                _objUser.Manufacturer = string.Empty;
                _objUser.Status = -1;
                _objUser.building = "All";

                //API
                _GetElev.ConnConfig = Session["config"].ToString();
                _GetElev.SearchBy = string.Empty;
                _GetElev.LocID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _GetElev.InstallDate = string.Empty;
                _GetElev.ServiceDate = string.Empty;
                _GetElev.Price = string.Empty;
                _GetElev.Manufacturer = string.Empty;
                _GetElev.Status = -1;
                _GetElev.building = "All";

                List<GetElevViewModel> _lstGetElev = new List<GetElevViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetElev";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElev);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetElev = serializer.Deserialize<List<GetElevViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetElevViewModel>(_lstGetElev);
                }
                else
                {
                    ds = objBL_User.getElev(_objUser);
                }

                RadGrid_Equip.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Equip.DataSource = ds.Tables[0];

                if (ds.Tables[0].Rows.Count == 0)
                {
                    GetAllEquip();
                }
                else
                {
                    Session["ElevSrchLoc"] = ds.Tables[0];
                }

            }
        }
        catch { }
    }

    protected void RadGrid_Equip_ItemEvent(object sender, GridItemEventArgs e)
    {
        if (RadGrid_Equip.Items.Count > 0)
        {
            GridFooterItem footerItem = (GridFooterItem)RadGrid_Equip.MasterTableView.GetItems(GridItemType.Footer).FirstOrDefault();
            if (footerItem != null)
            {
                var dt = (DataTable)Session["ElevSrchLoc"];
                var totalActive = dt.Select("Status = 0").Count();

                Label lblTotalActive = footerItem.FindControl("lblTotalActive") as Label;
                lblTotalActive.Text = string.Format("Active Count: {0:N0}", totalActive);
            }
        }
    }

    public void GetAllEquip()
    {
        DataTable dtFinal = new DataTable();
        DataSet ds1 = new DataSet();

        objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.InstallDate = string.Empty;
        objPropUser.InstallDateString = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.ServiceDateString = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.PriceString = string.Empty;

        //API
        _GetElev.UserID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        _GetElev.ConnConfig = Session["config"].ToString();
        _GetElev.SearchBy = string.Empty;
        _GetElev.InstallDate = string.Empty;
        _GetElev.InstallDateString = string.Empty;
        _GetElev.ServiceDate = string.Empty;
        _GetElev.ServiceDateString = string.Empty;
        _GetElev.Price = string.Empty;
        _GetElev.PriceString = string.Empty;

        int RoleID = 0;
        objPropUser.RoleID = RoleID;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        objPropUser.Type = string.Empty;
        objPropUser.Category = string.Empty;
        objPropUser.Status = -1;
        objPropUser.building = "All";
        objPropUser.Classification = string.Empty;
        objPropUser.EN = 0;
        //For filter by user login
        objPropUser.EmpId = 0;
        objPropUser.IsAssignedProject = false;
        objPropUser.LocID = 0;

        //API
        _GetElev.RoleID = RoleID;
        _GetElev.Manufacturer = string.Empty;
        _GetElev.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        _GetElev.Type = string.Empty;
        _GetElev.Category = string.Empty;
        _GetElev.Status = -1;
        _GetElev.building = "All";
        _GetElev.Classification = string.Empty;
        _GetElev.EN = 0;
        //For filter by user login
        _GetElev.EmpId = 0;
        _GetElev.IsAssignedProject = false;
        _GetElev.LocID = 0;

        List<GetElevViewModel> _lstGetElev = new List<GetElevViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetElev";

            _GetElev.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElev);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetElev = serializer.Deserialize<List<GetElevViewModel>>(_APIResponse.ResponseData);
            ds1 = CommonMethods.ToDataSet<GetElevViewModel>(_lstGetElev);
        }
        else
        {
            ds1 = objBL_User.getElev(objPropUser, new GeneralFunctions().GetSalesAsigned());
        }

        Session["ElevSrchLoc"] = ds1.Tables[0];
    }

    bool isGroupingEquip = false;
    public bool ShouldApplySortFilterOrGroupEquip()
    {
        return RadGrid_Equip.MasterTableView.FilterExpression != "" ||
            (RadGrid_Equip.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquip) ||
            RadGrid_Equip.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Equip_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.Items)
        {
            Label lblID = (Label)item.FindControl("lblId");

            if (hdnEditeEquipment.Value == "Y" || hdnViewEquipment.Value == "Y")
            {
                item.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "&page=addlocation&lid=" + Request.QueryString["uid"].ToString() + "'";
            }
            else
            {
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }

    protected void RadGrid_OpenCalls_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
              try
        {
            RadGrid_OpenCalls.AllowCustomPaging = !ShouldApplySortFilterOrGroupOpenCalls();

            if (!IsPostBack)
            {
                if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
                {
                    List<RetainFilter> filters = new List<RetainFilter>();

                    RadGrid_OpenCalls.MasterTableView.FilterExpression = Convert.ToString(Session["AddLocation_TicketListFilter"]);
                    if (Session["TicketListRadGVFilters"] != null)
                    {


                        var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {
                                GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                                column.CurrentFilterValue = _filter.FilterValue;
                            }
                        }

                    }

                }

            }




            if ((Request.QueryString["uid"]) != null)
            {
                BindOpenCallsRadGrid();
                showFilterSearch();
            }
        }
        catch { }
    }

    private void BindOpenCallsRadGrid()
    {
        DataSet ds = new DataSet();
        string stdate = txtfromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);

        _GetCallHistory.ConnConfig = Session["config"].ToString();
        _GetCallHistory.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        _GetCallHistory.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);

        if (txtfromDate.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(stdate);
            _GetCallHistory.StartDate = Convert.ToDateTime(stdate);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
            _GetCallHistory.StartDate = System.DateTime.MinValue;
        }

        if (txtToDate.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(enddate);
            _GetCallHistory.EndDate = Convert.ToDateTime(enddate);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
            _GetCallHistory.EndDate = System.DateTime.MinValue;
        }

        objMapData.SearchBy = ddlSearch.SelectedValue.Trim();
        _GetCallHistory.SearchBy = ddlSearch.SelectedValue.Trim();

        if (ddlSearch.SelectedValue == "t.cat")
        {
            objMapData.SearchValue = ddlCategory.SelectedValue;
            _GetCallHistory.SearchValue = ddlCategory.SelectedValue;
        }
        else
        {
            objMapData.SearchValue = txtSearch.Text.Replace("'", "''");
            _GetCallHistory.SearchValue = txtSearch.Text.Replace("'", "''");
        }
        objMapData.Department = -1;
        _GetCallHistory.Department = -1;
        try
        {
            List<GetCallHistoryViewModel> _lstGetCallHistory = new List<GetCallHistoryViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_GetCallHistory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCallHistory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCallHistory = serializer.Deserialize<List<GetCallHistoryViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCallHistoryViewModel>(_lstGetCallHistory);
            }
            else
            {
                ds = new BL_Tickets().getCallHistory(objMapData);
            }

            DataTable result = processDataFilter(ds.Tables[0]);
            RadGrid_OpenCalls.VirtualItemCount = result.Rows.Count;
            RadGrid_OpenCalls.DataSource = result;
        }
        catch (Exception ex) {

        }
    }

    bool isGroupingOpenCalls = false;
    public bool ShouldApplySortFilterOrGroupOpenCalls()
    {
        return RadGrid_OpenCalls.MasterTableView.FilterExpression != "" ||
            (RadGrid_OpenCalls.MasterTableView.GroupByExpressions.Count > 0 || isGroupingOpenCalls) ||
            RadGrid_OpenCalls.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_OpenCalls_PreRender(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                RadGrid_OpenCalls.MasterTableView.FilterExpression = Convert.ToString(Session["AddLocation_TicketListFilter"]);
                if (Session["TicketListRadGVFilters"] != null)
                {


                    var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }

                }
                Session.Remove("TicketListRadGVFilters");
                Session.Remove("AddLocation_TicketListFilter");
            }

        }

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_OpenCalls);
        foreach (GridDataItem item in RadGrid_OpenCalls.Items)
        {
            Label lblComp = (Label)item.FindControl("lblComp");
            Label lblTicketId = (Label)item.FindControl("lblTicketId");

            if (hdnEditeTicket.Value == "Y" || hdnViewTicket.Value == "Y")
            {
                item.Attributes["ondblclick"] = "window.open('addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1','_blank');";
            }
            else
            {
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }
    protected void RadGrid_OpenCalls_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();
            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }
    }
    protected void lnkServiceExcel_Click(object sender, EventArgs e)
    {
        RadGrid_OpenCalls.MasterTableView.GetColumn("locname").Visible = true;
        RadGrid_OpenCalls.ExportSettings.FileName = "LocationHistory";
        RadGrid_OpenCalls.ExportSettings.IgnorePaging = true;
        RadGrid_OpenCalls.ExportSettings.ExportOnlyData = true;
        RadGrid_OpenCalls.ExportSettings.OpenInNewWindow = true;
        RadGrid_OpenCalls.ExportSettings.HideStructureColumns = true;
        RadGrid_OpenCalls.MasterTableView.UseAllDataFields = true;
        RadGrid_OpenCalls.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_OpenCalls.MasterTableView.ExportToExcel();

    }
    protected void RadGrid_Invoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Invoice.AllowCustomPaging = !ShouldApplySortFilterOrGroupInvoice();
            if (Request.QueryString["uid"] != null)
            {
                string selectedSearchValue = Request.Form["radio-group"].ToString();
                hdnSearchValue.Value = Request.Form["radio-group"].ToString();
                string selectedSearchBy = Request.Form["radio-group1"].ToString();
                hdnSearchBy.Value = Request.Form["radio-group1"].ToString();
                string SearchValue = string.Empty;
                string SearchBy = string.Empty;
                if (selectedSearchValue == "rdAll")
                {
                    SearchValue = "All";
                }
                else if (selectedSearchValue == "rdOpen")
                {
                    SearchValue = "Open";
                }
                else
                {
                    SearchValue = "Closed";
                }



                if (selectedSearchBy == "rdAll2")
                {
                    SearchBy = "All";
                }
                else if (selectedSearchBy == "rdCharges")
                {
                    SearchBy = "Charges";
                }
                else
                {
                    SearchBy = "Credits";
                }
                BindInvoicesRadGrid(SearchValue, SearchBy);

            }
        }
        catch { }
    }

    private void BindInvoicesRadGrid(string SearchValue, string SearchBy)
    {


        if (ddlSearchInv.SelectedValue == "i.ref")
        {
            objProp_Contracts.filterBy = "Invoice";
            objProp_Contracts.filterValue = "'" + txtSearchInv.Text + "'";

            _GetARRevenue.filterBy = "Invoice";
            _GetARRevenue.filterValue = "'" + txtSearchInv.Text + "'";
        }
        else if (ddlSearchInv.SelectedValue == "i.fdate")
        {
            objProp_Contracts.filterBy = "InvoiceDate";
            _GetARRevenue.filterBy = "InvoiceDate";
            DateTime temp;
            if (DateTime.TryParse(txtInvDt.Text, out temp))
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtInvDt.Text);
                _GetARRevenue.StartDate = Convert.ToDateTime(txtInvDt.Text);
            }
            else
            {
                objProp_Contracts.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                _GetARRevenue.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
            }
            
        }

        else
        {
            objProp_Contracts.filterBy = "";
            objProp_Contracts.filterValue = txtSearchInv.Text;

            _GetARRevenue.filterBy = "";
            _GetARRevenue.filterValue = txtSearchInv.Text;
        }

        //if (SearchValue == "Open")
        //{
        //    txtInvDtFrom.Text = "";
        //    txtInvDtTo.Text = "";
        //}
        DataSet ds = new DataSet();
        string stdate = "";
        string enddate = "";
        if (txtInvDtFrom.Text == "")
        {
            stdate = "01/01/1900";
        }
        else
        {
            stdate = txtInvDtFrom.Text + " 00:00:00";
        }
        if (txtInvDtTo.Text == "")
        {
            enddate = "01/01/2100"; 
        }
        else
        {
            
            enddate = txtInvDtTo.Text + " 23:59:59";
        }
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.CustID = 0;
        objProp_Contracts.Loc = Convert.ToInt32(Request.QueryString["uid"].ToString());

        _GetARRevenue.ConnConfig = Session["config"].ToString();
        _GetARRevenue.CustID = 0;
        _GetARRevenue.Loc = Convert.ToInt32(Request.QueryString["uid"].ToString());

        if (ddlSearchInv.SelectedValue != "i.fdate")
        {
            objProp_Contracts.StartDate = Convert.ToDateTime(stdate);
            _GetARRevenue.StartDate = Convert.ToDateTime(stdate);
        }

        objProp_Contracts.EndDate = Convert.ToDateTime(enddate);
        objProp_Contracts.SearchBy = SearchBy;
        objProp_Contracts.SearchValue = SearchValue;

        _GetARRevenue.EndDate = Convert.ToDateTime(enddate);
        _GetARRevenue.SearchBy = SearchBy;
        _GetARRevenue.SearchValue = SearchValue;

        ListGetARRevenue _lstGetARRevenue = new ListGetARRevenue();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetARRevenue";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetARRevenue);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetARRevenue = serializer.Deserialize<ListGetARRevenue>(_APIResponse.ResponseData);

            ds1 = _lstGetARRevenue.lstTable1.ToDataSet();
            ds2 = _lstGetARRevenue.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

        }
        else
        {
            ds = objBL_Invoice.GetARRevenue(objProp_Contracts);
        }

        PrevRuntotal = Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());

        DataTable dt = new DataTable();
        dt = BindInvoiceGridDatatable(ds.Tables[0]);

        RadGrid_Invoice.VirtualItemCount = dt.Rows.Count;
        RadGrid_Invoice.DataSource = dt;
        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found";

        
       // lblLocationBalance.Text = String.Format("{0:C}", dt.Compute("Sum(Amount)", string.Empty));
        //ViewState["LocBalance"] = PrevRuntotal;


    }
    private DataTable BindInvoiceGridDatatable(DataTable dt)
    {
        string selectedSearchValue = "";
            if (Request.Form["radio-group"] != null) {
            selectedSearchValue = Request.Form["radio-group"].ToString();
        }
        Session["InvoiceSrchLoc"] = dt;
        if (!dt.Columns.Contains("RunTotal"))
        {
            dt.Columns.Add("RunTotal", typeof(Double));
        }
        //Double Runtotal = 0.0;
        //Runtotal = PrevRuntotal;
        //foreach (DataRow row in dt.Rows)
        //{
        //    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;
        //    Runtotal = Convert.ToDouble(row["RunTotal"].ToString());
        //}
        Double Runtotal = 0.00;
        Runtotal = PrevRuntotal;
        if (selectedSearchValue == "rdOpen")
        {
            int i = 0;
            Runtotal = 0;
            //to convert deposit, recievepayment  to -ve values 
            foreach (DataRow row in dt.Rows)
            {
                if (i == 0)
                {
                    row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString());

                }
                else
                {
                    //if (Convert.ToDouble(row["Credits"]) != 0 && Convert.ToDouble(row["Balance"]) != 0)
                    //{
                    //    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) + Convert.ToDouble(row["Balance"].ToString()) + Runtotal;

                    //}
                    //else
                    //{
                    //    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;


                    //}
                    row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString()) + Runtotal;
                }
                Runtotal = Convert.ToDouble(row["RunTotal"].ToString());
              
                i++;

            }
        }
        else
        {
            //if (txtInvDtFrom.Text == "")
            //{
                //to convert deposit, recievepayment  to -ve values 
                foreach (DataRow row in dt.Rows)
                {

                    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;
                    Runtotal = Convert.ToDouble(row["RunTotal"].ToString());

                   
                }
            //}
            //else
            //{
            //    int i = 0;
            //    Runtotal = 0;
            //    //to convert deposit, recievepayment  to -ve values 
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (i == 0)
            //        {
            //            row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString());

            //        }
            //        else
            //        {
            //            if (Convert.ToDouble(row["Credits"]) != 0 && Convert.ToDouble(row["Balance"]) != 0)
            //            {
            //                row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) + Convert.ToDouble(row["Balance"].ToString()) + Runtotal;

            //            }
            //            else
            //            {
            //                row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;


            //            }
            //        }
            //        Runtotal = Convert.ToDouble(row["RunTotal"].ToString());
                 
            //        i++;

            //    }
            //}

        }

        return dt;

    }

    bool isGroupingInvoice = false;
    public bool ShouldApplySortFilterOrGroupInvoice()
    {
        return RadGrid_Invoice.MasterTableView.FilterExpression != "" ||
            (RadGrid_Invoice.MasterTableView.GroupByExpressions.Count > 0 || isGroupingInvoice) ||
            RadGrid_Invoice.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Invoice_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_Invoice);
        //foreach (GridDataItem item in RadGrid_Invoice.Items)
        //{
        //    Label lblID = (Label)item.FindControl("lblId");
        //    Label lblType = (Label)item.FindControl("lblType");
        //    HiddenField hdnLinkTo = (HiddenField)item.FindControl("hdnLinkTo");


        //    item.Attributes["ondblclick"] = "ShowHistoryTransactionPopup(" + lblID.Text + "," + Convert.ToInt32(hdnLinkTo.Value) + ",0," + Request.QueryString["uid"].ToString() +")";

        //}

       

    }
    protected void RadGrid_Invoice_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 5;

        if (e.Worksheet.Table.Rows.Count == RadGrid_Invoice.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Invoice.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

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
    protected void RadGrid_Invoice_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            if (Convert.ToString(RadGrid_Invoice.MasterTableView.FilterExpression) != "")
            {
                lblRecordCount.Text = totalCount + " Record(s) found";
            }
            else
            {
                lblRecordCount.Text = RadGrid_Invoice.VirtualItemCount + " Record(s) found";
            }
            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();
            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }

   
    }
    protected void lnkInvoiceExcel_Click(object sender, EventArgs e)
    {
        //  RadGrid_Invoice.MasterTableView.GetColumn("CustomerName").Visible = true;
        RadGrid_Invoice.ExportSettings.FileName = "Transactions";
        RadGrid_Invoice.ExportSettings.IgnorePaging = true;
        RadGrid_Invoice.ExportSettings.ExportOnlyData = true;
        RadGrid_Invoice.ExportSettings.OpenInNewWindow = true;
        RadGrid_Invoice.ExportSettings.HideStructureColumns = true;
        RadGrid_Invoice.MasterTableView.UseAllDataFields = true;
        RadGrid_Invoice.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Invoice.MasterTableView.ExportToExcel();

    }
    protected void RadGrid_Project_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Project.AllowCustomPaging = !ShouldApplySortFilterOrGroupProject();
            if (Request.QueryString["uid"] != null)
            {
                objProp_Customer.SearchBy = "j.loc";
                objProp_Customer.SearchValue = Request.QueryString["uid"].ToString();
                objProp_Customer.Range = 0;
                objProp_Customer.JobType = -1;

                _GetJobProject.SearchBy = "j.loc";
                _GetJobProject.SearchValue = Request.QueryString["uid"].ToString();
                _GetJobProject.Range = 0;
                _GetJobProject.JobType = -1;

                DataSet ds = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                _GetJobProject.ConnConfig = Session["config"].ToString();

                DataTable dtFilters = CreateFiltersToDataTable();

                List<GetJobProjectViewModel> _lstGetJobProject = new List<GetJobProjectViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetJobProject";

                    _GetJobProject.filtersData = dtFilters;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetJobProject);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetJobProject = serializer.Deserialize<List<GetJobProjectViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetJobProjectViewModel>(_lstGetJobProject);
                }
                else
                {
                    ds = objBL_Customer.getJobProject(objProp_Customer, dtFilters);
                }

                RadGrid_Project.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_Project.DataSource = ds.Tables[0];
            }
        }
        catch { }

    }


    private DataTable CreateFiltersToDataTable()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate"); 
        dtFilters.Columns.Add("Estimate");
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
    }

    bool isGroupingProject = false;
    public bool ShouldApplySortFilterOrGroupProject()
    {
        return RadGrid_Project.MasterTableView.FilterExpression != "" ||
            (RadGrid_Project.MasterTableView.GroupByExpressions.Count > 0 || isGroupingProject) ||
            RadGrid_Project.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Project_PreRender(object sender, EventArgs e)
    {

    }
    protected void lnkAddCustomer_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowAddCustomer.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        getLocationLog();
        //try
        //{
        //    RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

        //    if (Request.QueryString["uid"] != null)
        //    {
        //        objPropUser.DBName = Session["dbname"].ToString();
        //        objPropUser.ConnConfig = Session["config"].ToString();
        //        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        //        DataSet ds = new DataSet();
        //        ds = objBL_User.getLocationByID(objPropUser);
        //        if (ds.Tables[3].Rows.Count > 0)
        //        {
        //            RadGrid_gvLogs.VirtualItemCount = ds.Tables[3].Rows.Count;
        //            RadGrid_gvLogs.DataSource = ds.Tables[3];
        //        }
        //        else
        //        {
        //            RadGrid_gvLogs.DataSource = string.Empty;
        //        }
        //    }
        //}
        //catch { }
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
   
    public void updateDataEmail()
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

        _GetLocationByID.DBName = Session["dbname"].ToString();
        _GetLocationByID.ConnConfig = Session["config"].ToString();
        _GetLocationByID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataSet ds5 = new DataSet();

        ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetLocationByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

            ds1 = _lstGetLocationByID.lstTable1.ToDataSet();
            ds2 = _lstGetLocationByID.lstTable2.ToDataSet();
            ds3 = _lstGetLocationByID.lstTable3.ToDataSet();
            ds4 = _lstGetLocationByID.lstTable4.ToDataSet();
            ds5 = _lstGetLocationByID.lstTable5.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];
            dt3 = ds3.Tables[0];
            dt4 = ds4.Tables[0];
            dt5 = ds5.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            dt4.TableName = "Table4";
            dt5.TableName = "Table5";

            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
        }
        else
        {
            ds = objBL_User.getLocationByID(objPropUser);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtEmailTo.Text = ds.Tables[0].Rows[0]["custom14"].ToString();
            
            txtEmailToInv.Text = ds.Tables[0].Rows[0]["custom12"].ToString();
        }
            
    }
    #endregion

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        _GetDefaultWorkerHeader.ConnConfig = Session["config"].ToString();

        string getValue;
        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetDefaultWorkerHeader";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDefaultWorkerHeader);

            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
            getValue = JsonData.ToString();
        }
        else
        {
            getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        }

        if (!string.IsNullOrEmpty(getValue))
        {
            lblDefaultWorker.InnerText = getValue;
        }
        else
        {
            lblDefaultWorker.InnerText = "Default Worker";
        }
    }

    protected void RadGrid_Equip_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        DataTable dtEquip = CreateDataTableForEquipMultiFilter();
        if (dtEquip != null)
        {
            e.ListBox.DataSource = dtEquip;
            e.ListBox.DataKeyField = "Equip";
            e.ListBox.DataTextField = "Equip";
            e.ListBox.DataValueField = "Status";
            e.ListBox.DataBind();
        }
    }

    private DataTable CreateDataTableForEquipMultiFilter()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Equip");
        dtFilters.Columns.Add("Status"); 
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFiltersRow["Equip"] = "Active";
        dtFiltersRow["Status"] = "0";
        dtFilters.Rows.Add(dtFiltersRow);
        DataRow dtFiltersRow2 = dtFilters.NewRow();
        dtFiltersRow2["Equip"] = "Inactive";
        dtFiltersRow2["Status"] = "1";
        dtFilters.Rows.Add(dtFiltersRow2);

        return dtFilters;

    }


    private void getLocationLog()
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetLocationLog.DBName = Session["dbname"].ToString();
                _GetLocationLog.ConnConfig = Session["config"].ToString();
                _GetLocationLog.LocID = Convert.ToInt32(Request.QueryString["uid"]);
                DataSet ds = new DataSet();

                List<GetLocationLogViewModel> _lstGetLocationLog = new List<GetLocationLogViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetLocationLog";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationLog);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetLocationLog = serializer.Deserialize<List<GetLocationLogViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetLocationLogViewModel>(_lstGetLocationLog);
                }
                else
                {
                    ds = objBL_User.getLocationLog(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvLogs.VirtualItemCount = ds.Tables[0].Rows.Count;
                    RadGrid_gvLogs.DataSource = ds.Tables[0];
                }
                else
                {
                    RadGrid_gvLogs.DataSource = string.Empty;
                }
            }
        }
        catch { }
    }

    private void getContactLog()
    {
        try
        {
            RadGrid_gvContactLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetContactLogByLocID.DBName = Session["dbname"].ToString();
                _GetContactLogByLocID.ConnConfig = Session["config"].ToString();
                _GetContactLogByLocID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();

                List<GetContactLogByLocIDViewModel> _lstGetContactLogByLocID = new List<GetContactLogByLocIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetContactLogByLocID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetContactLogByLocID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetContactLogByLocID = serializer.Deserialize<List<GetContactLogByLocIDViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetContactLogByLocIDViewModel>(_lstGetContactLogByLocID);
                }
                else
                {
                    ds = objBL_User.getContactLogByLocID(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvContactLogs.VirtualItemCount = ds.Tables[0].Rows.Count;
                    RadGrid_gvContactLogs.DataSource = ds.Tables[0];

                }
                else
                {
                    RadGrid_gvContactLogs.DataSource = string.Empty;
                }
            }
           
        }
        catch { }
    }
    private void getContact()
    {

        try
        {
            RadGrid_Contacts.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.RolId = Convert.ToInt32(ViewState["rol"].ToString());

                _GetLocContactByRolID.DBName = Session["dbname"].ToString();
                _GetLocContactByRolID.ConnConfig = Session["config"].ToString();
                _GetLocContactByRolID.RolId = Convert.ToInt32(ViewState["rol"].ToString());
                DataSet ds = new DataSet();

                List<GetLocContactByRolIDViewModel> _lstGetLocContactByRolID = new List<GetLocContactByRolIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_GetLocContactByRolID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocContactByRolID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetLocContactByRolID = serializer.Deserialize<List<GetLocContactByRolIDViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetLocContactByRolIDViewModel>(_lstGetLocContactByRolID);
                }
                else
                {
                    ds = objBL_User.getLocContactByRolID(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadGrid_Contacts.VirtualItemCount = ds.Tables[0].Rows.Count;
                    RadGrid_Contacts.DataSource = ds.Tables[0];
                    ViewState["contacttableloc"] = ds.Tables[0];
                }
                else
                {
                    RadGrid_Contacts.DataSource = string.Empty;
                    ViewState["contacttableloc"] = null;
                }
            }

        }
        catch { }
       
    }


    protected void lnkShowLog_Click(object sender, EventArgs e)
    {
        getContactLog();
        RadGrid_gvContactLogs.Rebind();
        string script = "function f(){$find(\"" + RadWindowContactLog.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void chkShowAllDocs_CheckedChanged(object sender, EventArgs e)
    {
        //GetDocuments(chkShowAllDocs.Checked);
        RadGrid_Documents.Rebind();
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments(chkShowAllDocs.Checked);
    }

    //protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    //{
    //    
    //}

    private String getBusinessTypeLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        _GetBT.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetBT";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBT);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetBT = serializer.Deserialize<List<GetBTViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetBTViewModel>(_lstGetBT);
        }
        else
        {
            ds = objBL_Customer.getBT(objCustomer);
        }

        try
        {
            return ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            return "Business Type";
        }


    }
    private void FillBusinessType()
    {

        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        _GetBT.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetBT";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBT);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetBT = serializer.Deserialize<List<GetBTViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetBTViewModel>(_lstGetBT);
        }
        else
        {
            ds = objBL_Customer.getBT(objCustomer);
        }

        ddlBusinessType.DataSource = ds.Tables[0];
        ddlBusinessType.DataTextField = "Description";
        ddlBusinessType.DataValueField = "ID";
        ddlBusinessType.DataBind();
        ddlBusinessType.Items.Insert(0, new ListItem(":: Select ::", "0"));
        try
        {
            lbBusinessType.Text=ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            lbBusinessType.Text= "Business Type";
        }
     
    }


    private void SaveFilter()
    {

        Session["filterstateAddLocationHistory"] = ddlSearch.SelectedValue + ";"
            + txtSearch.Text + ";"
            + ddlCategory.SelectedValue + ";"
            + ddlStatus.SelectedValue + ";"
            + txtfromDate.Text + ";"
            + txtToDate.Text + ";"
            + hdnSelectedDtRangeHistory.Value;

    }

    public void UpdateControl()
    {

        if (Session["filterstateAddLocationHistory"] != null)
        {
            if (Session["filterstateAddLocationHistory"].ToString() != string.Empty)
            {
                string[] strFilter = Session["filterstateAddLocationHistory"].ToString().Split(';');
                ddlSearch.SelectedValue = strFilter[0];
                txtSearch.Text = strFilter[1];
                ddlCategory.SelectedValue = strFilter[2];
                ddlStatus.SelectedValue = strFilter[3];
                txtfromDate.Text = strFilter[4];
                txtToDate.Text = strFilter[5];
                hdnSelectedDtRangeHistory.Value = strFilter[6];
                lblDay.Attributes.Remove("class");
                lblWeek.Attributes.Remove("class");
                lblMonth.Attributes.Remove("class");
                lblQuarter.Attributes.Remove("class");
                lblYear.Attributes.Remove("class");
                switch (hdnSelectedDtRangeHistory.Value)
                {
                    case "Day":
                        lblDay.Attributes.Add("class", "labelactive");
                        break;
                    case "Week":
                        lblWeek.Attributes.Add("class", "labelactive");
                        break;
                    case "Month":
                       lblMonth.Attributes.Add("class", "labelactive");
                        break;
                    case "Quarter":
                        lblQuarter.Attributes.Add("class", "labelactive");
                        break;
                    case "Year":
                        lblYear.Attributes.Add("class", "labelactive");
                        break;
                }
                showFilterSearch();
            }
        }
        Session.Remove("filterstateAddLocationHistory");
    }


    public void showFilterSearch()
    {
        ddlCategory.Style.Add("display", "none");
        txtSearch.Style.Add("display", "none");

        if (ddlSearch.SelectedValue == "t.cat")
        {

            ddlCategory.Style.Add("display", "block");


        }
        else
        {
            txtSearch.Style.Add("display", "block");
        }
    }
    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                String expression = Convert.ToString(Session["AddLocation_TicketListFilter"]);
                if (Session["TicketListRadGVFilters"] != null)
                {


                    var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName == "ID")
                            {
                                sql = sql + " And " + column.UniqueName + " = " + _filter.FilterValue;

                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                            }

                        }
                    }

                }
                return result.Select(sql).CopyToDataTable();

            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }


    }

    protected void lnkEditOpp_Click(object sender, EventArgs e)
    {
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
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

                _DeleteOpportunity.ConnConfig = Session["config"].ToString();
                _DeleteOpportunity.OpportunityID = Convert.ToInt32(lblID.Text);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "LocationsAPI/AddLocation_DeleteOpportunity";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteOpportunity);
                }
                else
                {
                    objBL_Customer.DeleteOpportunity(objProp_Customer);
                }

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

    private void FillOpportunity(string name)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "l.rol";
        objProp_Customer.SearchValue = name;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;

        _GetOpportunityNew.ConnConfig = Session["config"].ToString();
        _GetOpportunityNew.SearchBy = "l.rol";
        _GetOpportunityNew.SearchValue = name;
        _GetOpportunityNew.StartDate = string.Empty;
        _GetOpportunityNew.EndDate = string.Empty;

        List<GetOpportunityNewViewModel> _lstGetOpportunityNew = new List<GetOpportunityNewViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetOpportunityNew";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetOpportunityNew);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetOpportunityNew = serializer.Deserialize<List<GetOpportunityNewViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetOpportunityNewViewModel>(_lstGetOpportunityNew);
        }
        else
        {
            ds = objBL_Customer.getOpportunityNew(objProp_Customer);
        }

        //gvOpportunity.DataSource = ds.Tables[0];
        //gvOpportunity.DataBind();

        RadGrid_Opportunity.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Opportunity.DataSource = ds.Tables[0];
        //RadGrid_Opportunity.Rebind();

        //menuLeads.Items[3].Text = "Opportunities(" + ds.Tables[0].Rows.Count + ")";

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

    protected void RadGrid_Invoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            foreach (GridColumn col in RadGrid_Invoice.MasterTableView.RenderColumns)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (col.UniqueName == "Balance")
                {                   

                    Label lblID = (Label)dataItem.FindControl("lblId");

                    Label lblType = (Label)dataItem.FindControl("lblType");

                    HiddenField hdnLinkTo = (HiddenField)dataItem.FindControl("hdnLinkTo");

                    HiddenField hdnTransID = (HiddenField)dataItem.FindControl("hdnTransID");

                    Label lblStatus = (Label)dataItem.FindControl("lblStatus"); 
                    
                    dataItem[col.UniqueName].Attributes.Add("onclick", "ShowHistoryTransactionPopup(" + lblID.Text + "," + Convert.ToInt32(hdnLinkTo.Value) + ",0," + Convert.ToInt32(Request.QueryString["uid"].ToString()) + ",'" + lblStatus.Text + "'," + hdnTransID.Value + ")");
                    
                }

            }
        }
    }

    protected void lnkShowAllOpen_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;

        txtSearchInv.Text = string.Empty;

        ddlStatusInv.SelectedIndex = 0;

        ddlDepartment.SelectedIndex = 0;

        txtInvDt.Text = string.Empty;

        ddlSearchInv_SelectedIndexChanged(sender, e);

        txtInvDtTo.Text = string.Empty; 

        txtInvDtFrom.Text = string.Empty; 

        isShowAllInvoices = true;

        BindInvoicesRadGrid("Open", "All");

        cleanFilter();  
        
        ishowAllInvoice.Value = "1";
    }


    private void showHomeOwner()
    {
        DataSet ds = new DataSet();

        BusinessEntity.User objProp_User = new BusinessEntity.User();

        objProp_User.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/AddLocation_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            ds = objBL_User.getControl(objProp_User);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            liHomeowner.Visible= Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]);

            adHomeowner.Visible= Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]);
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ApplyServiceTypeRule();
    }

    protected void ApplyServiceTypeRule()
    {

        hdnApplyServiceTypeRule.Value = "0";

        hdnServiceTypeName.Value = "";

        hdnProjectPerDepartmentCount.Value = "-1";

        string ConnectionString = Session["config"].ToString(); string LocType = ""; int RoutID = 0; int LocationID = 0;

        if (ddlRoute.SelectedIndex != 0) { int.TryParse(ddlRoute.SelectedValue, out RoutID); }

        if (ddlType.SelectedIndex != 0) { LocType = ddlType.SelectedValue; }

        if (Request.QueryString["uid"] != null && Request.QueryString["t"] == null && Request.QueryString["t"] != "c") { int.TryParse(Request.QueryString["uid"], out LocationID); }

        if (RoutID != 0 && LocationID != 0 && LocType != "")

        {
            DataSet _dsContract = new DataSet();
            List<GetLocationServiceTypeinfoViewModel> _lstGetLocationServiceTypeinfo = new List<GetLocationServiceTypeinfoViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/AddLocation_spGetLocationServiceTypeinfo";

                _GetLocationServiceTypeinfo.ConnectionString = ConnectionString;
                _GetLocationServiceTypeinfo.LocType = LocType;
                _GetLocationServiceTypeinfo.RoutID = RoutID;
                _GetLocationServiceTypeinfo.LocationID = LocationID;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationServiceTypeinfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _lstGetLocationServiceTypeinfo = serializer.Deserialize<List<GetLocationServiceTypeinfoViewModel>>(_APIResponse.ResponseData);
                _dsContract = CommonMethods.ToDataSet<GetLocationServiceTypeinfoViewModel>(_lstGetLocationServiceTypeinfo);
            }
            else
            {
                _dsContract = new BusinessLayer.Programs.BL_ServiceType().spGetLocationServiceTypeinfo(ConnectionString, LocType, RoutID, LocationID);
            }

            if (_dsContract.Tables[0].Rows[0]["ServiceTypeName"].ToString() != "0" && _dsContract.Tables[0].Rows[0]["ServiceTypeCount"].ToString() == "1" && _dsContract.Tables[0].Rows[0]["ProjectPerDepartmentCount"].ToString() == "1")

            {
                hdnApplyServiceTypeRule.Value = _dsContract.Tables[0].Rows[0]["ServiceTypeCount"].ToString();

                hdnServiceTypeName.Value = _dsContract.Tables[0].Rows[0]["ServiceTypeName"].ToString();

                hdnProjectPerDepartmentCount.Value = _dsContract.Tables[0].Rows[0]["ProjectPerDepartmentCount"].ToString();

                hdnProjectaregoingtoUpdate.Value = _dsContract.Tables[0].Rows[0]["ProjectaregoingtoUpdate"].ToString();

                ClientScript.RegisterStartupScript(Page.GetType(), "keyServiceType1", "ApplyServiceTypeRule();", true);


            }
            else if (_dsContract.Tables[0].Rows[0]["ServiceTypeName"].ToString() != "0" && _dsContract.Tables[0].Rows[0]["ServiceTypeCount"].ToString() == "1" && _dsContract.Tables[0].Rows[0]["ProjectPerDepartmentCount"].ToString() != "0")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyServiceType2", "NotApplyServiceTypeRule();", true);
            }

            else if (_dsContract.Tables[0].Rows[0]["ServiceTypeName"].ToString() != "0" && _dsContract.Tables[0].Rows[0]["ServiceTypeCount"].ToString() != "0" && _dsContract.Tables[0].Rows[0]["ProjectPerDepartmentCount"].ToString() != "0")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyServiceType2", "NotApplyServiceTypeRule();", true);
            }
        }

    }

    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        ApplyServiceTypeRule(); 
    }

    protected void RadGrid_OpenCalls_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getFilterCategory(objPropUser);
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        if (ds.Tables[0] != null)
        {
            e.ListBox.DataSource = ds.Tables[0];
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }
    }

    private void DisableButtonWhenLocationInactive()
    {
        if (hdnAddeContact.Value == "Y")
        {
            lnkAddnew.Enabled = true;
            lnkAddnew.CssClass = "";
        }

        //Equipment      
        if (hdnAddeEquipment.Value == "Y")
        {
            lnkAddEQ.Enabled = true;
            lnkAddEQ.CssClass = "";
            btnCopyEQ.Enabled = true;
            btnCopyEQ.CssClass = "";
           
        }
        //Ticket
        if (hdnAddeTicket.Value == "Y")
        {
            lnkAddTicket.Enabled = true;
            lnkAddTicket.CssClass = "";
        }

        //Transaction              
                 
        if (hdnAddInvoice.Value == "Y")
        {
            lnkAddInvoice.Enabled = true;
            lnkAddInvoice.CssClass = "";
        }

        //Opportunities               
        if (lnkAddopp.Visible == true)
        {
            lnkAddopp.Enabled = true;
            lnkCopyOpp.Enabled = true;
            lnkAddopp.CssClass = "";
            lnkCopyOpp.CssClass = "";
        }

        ////Project      
        if (hdnAddeJob.Value == "Y")
        {
            lnkAddProject.Enabled = true;
            lnkAddProject.CssClass = "";
        }

        if (Request.QueryString["uid"] != null)
        {
            if (ddlLocStatus.SelectedValue == "1")
            {
                //Contract
                if (hdnAddeContact.Value == "Y")
                {
                    lnkAddnew.Enabled = false;
                    lnkAddnew.CssClass = "disableButton";
                }                                 
                
                //Equipment               
               
                if (hdnAddeEquipment.Value == "Y")
                {
                    lnkAddEQ.Enabled = false;
                    lnkAddEQ.CssClass = "disableButton";
                    btnCopyEQ.Enabled = false;
                    btnCopyEQ.CssClass = "disableButton";
                }
                  
                //Ticket
                if (hdnAddeTicket.Value == "Y")
                {
                    lnkAddTicket.Enabled = false;
                    lnkAddTicket.CssClass = "disableButton";
                }                   

                //Transaction               
                if (hdnAddInvoice.Value == "Y")
                {
                    lnkAddInvoice.Enabled = false;
                    lnkAddInvoice.CssClass = "disableButton";
                }
                //Opportunities          
                if (lnkAddopp.Visible == true)
                {
                    lnkAddopp.Enabled = false;
                    lnkAddopp.CssClass = "disableButton";
                    lnkCopyOpp.Enabled = false;
                    lnkCopyOpp.CssClass = "disableButton";
                }
                //Project      
                if (hdnAddeJob.Value == "Y")
                {
                    lnkAddProject.Enabled = false;
                    lnkAddProject.CssClass = "disableButton";
                }
                    
            }
        }
    }

    protected void lnkDeleteInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Invoice.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                HiddenField hdnType = (HiddenField)item.FindControl("hdnType");
                if (hdnType.Value== "AR Invoice")
                {
                    //Delete Invoice
                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                    objProp_Contracts.Batch = 0;
                    objProp_Contracts.Loc = 0;
                    objProp_Contracts.Ref  = Convert.ToInt32(lblID.Text);
                    objBL_Contracts.DeleteInvoice(objProp_Contracts);

                }
                if (hdnType.Value == "Received Payment")
                {
                    //Received Payment
                    ReceivedPayment _objReceiPmt = new ReceivedPayment();
                    BL_Deposit _objBL_Deposit = new BL_Deposit();
                    _objReceiPmt.ConnConfig = Session["config"].ToString();
                    _objReceiPmt.ID = Convert.ToInt32(Convert.ToInt32(lblID.Text));
                    _objBL_Deposit.DeletePayment(_objReceiPmt);
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyS", "noty({text: 'Transaction deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            }
            RadGrid_Invoice.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }
    private void DeleteTicket(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        objMapData.Worker = Session["username"].ToString();
        objBL_MapData.DeleteTicket(objMapData);
    }
    protected void lnkDeleteTicket_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_OpenCalls.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblTicketId");           
                Int32 TicketId = 0;
                if (Int32.TryParse(lblID.Text, out TicketId))
                {
                    DeleteTicket(TicketId);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDeleteTicket", "noty({text: 'Ticket # " + lblID.Text + " Deleted Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyS", "noty({text: 'Transaction deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            } 
            RadGrid_OpenCalls.Rebind();
        }catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "DeleteTicketErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }
    public void getAllContractInfo()
    {
        if (Request.QueryString["uid"] != null)
        {
            StringBuilder str = new StringBuilder();
            BL_Customer objBL_Customer = new BL_Customer();
            Customer objCustomer = new Customer();

            //Get Customer Note
            DataSet dsContract = new DataSet();
            dsContract = objBL_Customer.GetContractByLoc(Convert.ToString(Session["config"]), Convert.ToInt32(Request.QueryString["uid"]));

            if (dsContract != null)
            {
                if (dsContract.Tables[0].Rows.Count > 0)
                {

                    str.Append("<table><thead><tr><th>Contract #</th><th>Created date</th><th>Status</th><th>Service type</th><th>Closed/Hold date</th></tr></thead>");
                    str.Append("<tbody>");
                    foreach (DataRow row in dsContract.Tables[0].Rows)
                    {
                        str.AppendFormat("<tr><td>{0}</td>", row["Job"]);
                        str.AppendFormat("<td>{0}</td>", row["fDate"].ToString() == "" ? "" : string.Format("{0:M/d/yyyy}", Convert.ToDateTime(row["fDate"])));
                        str.AppendFormat("<td>{0}</td>", row["StatusName"]);
                        str.AppendFormat("<td>{0}</td>", row["CType"]);
                        str.AppendFormat("<td>{0}</td></tr>", row["CloseDate"].ToString() == "" ? "" : string.Format("{0:M/d/yyyy}", Convert.ToDateTime(row["CloseDate"])));
                    }

                    str.Append(" </tbody></table>");
                }
            }

            ContractStatusTooltip.Text = str.ToString();
        }
           
       
    }
}

public class EditContactModel1
{
    public String Name { get; set; }
    public String Phone { get; set; }
    public String Fax { get; set; }
    public String Cell { get; set; }
    public String Email { get; set; }
    public String lblIndex { get; set; }
    public Boolean EmailTicket { get; set; }

}


