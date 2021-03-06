using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Configuration;
using BusinessEntity.Utility;
using MOMWebApp;
using Newtonsoft.Json;
using BusinessEntity.Payroll;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;

public partial class EquipmentReport : System.Web.UI.Page
{
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    CustomerReport objCustReport = new CustomerReport();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    public static int pubReportId = 0;
    public static string sortBy = string.Empty;
    public static string getPrintData = string.Empty;
    string reportType;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetUserEmailParam _GetUserEmail = new GetUserEmailParam();
    GetReportDetailByIdParam _GetReportDetailById = new GetReportDetailByIdParam();
    GetCustomerTypeParam _getCustomerType = new GetCustomerTypeParam();
    GetEquipReportFiltersValueParam _GetEquipReportFiltersValue = new GetEquipReportFiltersValueParam();
    GetCustomerNameParam _GetCustomerName = new GetCustomerNameParam();
    GetCustomerAddressParam _GetCustomerAddress = new GetCustomerAddressParam();
    GetCustomerCityParam _GetCustomerCity = new GetCustomerCityParam();
    GetDynamicReportsParam _GetDynamicReports = new GetDynamicReportsParam();
    GetReportColByRepIdParam _GetReportColByRepId = new GetReportColByRepIdParam();
    GetReportFiltersByRepIdParam _GetReportFiltersByRepId = new GetReportFiltersByRepIdParam();
    GetEquipmentInspectionParam _GetEquipmentInspection = new GetEquipmentInspectionParam();
    GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetail = new GetAccountSummaryListingDetailParam();
    CheckExistingReportParam _CheckExistingReport = new CheckExistingReportParam();
    InsertCustomerReportParam _InsertCustomerReport = new InsertCustomerReportParam();
    IsStockReportExistParam _IsStockReportExist = new IsStockReportExistParam();
    UpdateCustomerReportParam _UpdateCustomerReport = new UpdateCustomerReportParam();
    DeleteCustomerReportParam _DeleteCustomerReport = new DeleteCustomerReportParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetHeaderFooterDetailParam _GetHeaderFooterDetail = new GetHeaderFooterDetailParam();
    GetOwnersParam _GetOwners = new GetOwnersParam();
    GetColumnWidthByReportIdParam _GetColumnWidthByReportId = new GetColumnWidthByReportIdParam();
    UpdateCustomerReportResizedWidthParam _UpdateCustomerReportResizedWidth = new UpdateCustomerReportResizedWidthParam();
    
    protected void Page_Init(object sender, EventArgs e)
    {      
        if (!IsPostBack)
        {
            try
            {
                reportType = Request.QueryString["type"];
                DeleteExcelFiles();
                DeletePDFFiles();
            }
            catch
            {
                //
            }
            dvSaveReport.Attributes.Add("style", "display:none");
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Request.QueryString["reportId"] != null || Convert.ToInt32(Request.QueryString["reportId"]) != 0)
            {
                objCustReport.ReportId = Convert.ToInt32(Request.QueryString["reportId"]);
            }


            //else
            //{
            //    Response.Redirect("customers.aspx", false);
            //    return;
            //}
            if (Request.QueryString["reportName"] != null)
            {
                objCustReport.ReportName = Request.QueryString["reportName"];
                hdnCustomizeReportName.Value = objCustReport.ReportName;
            }
            pubReportId = objCustReport.ReportId;

            sortBy = string.Empty;
            GetCustomerDetails();
            GetReportsName();
            if (pubReportId != 0)
            {
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }


            GetCustReportFiltersValue();
            ConvertToJSON();
            GetUserEmail();
            //GetPreviewFields();
        }
        reportType = Request.QueryString["type"];
    }
    private void GetUserEmail()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Username = Session["username"].ToString();

        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _GetUserEmail.ConnConfig = Session["config"].ToString();
        _GetUserEmail.Username = Session["username"].ToString();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetUserEmail";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetUserEmail, true);

            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
            txtFrom.Text = JsonData.ToString();
        }
        else
        {
            txtFrom.Text = objBL_User.getUserEmail(objProp_User);
        }

        DataSet dsC = new DataSet();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig, true);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            dsC = objBL_User.getControl(objProp_User);
        }

        if (dsC.Tables[0].Rows.Count > 0)
        {
            if (txtFrom.Text.Trim() == string.Empty)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                }
            }
        }

        if (txtFrom.Text.Trim() == string.Empty)
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            string username = mailSettings.Smtp.Network.UserName;
            txtFrom.Text = username;
        }
    }

    private void GetReportDetailByRptId()
    {
        try
        {
            DataSet dsGetRptDetails = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;

            _GetReportDetailById.DBName = Session["dbname"].ToString();
            _GetReportDetailById.ConnConfig = Session["config"].ToString();
            _GetReportDetailById.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_GetReportDetailById";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReportDetailById, true);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstCustomerReport = serializer.Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                    dsGetRptDetails = CommonMethods.ToDataSet<CustomerReportViewModel>(_lstCustomerReport);
                }
                else
                {
                    dsGetRptDetails = objBL_ReportsData.GetReportDetailById(objCustReport);
                }

                if (dsGetRptDetails.Tables.Count > 0)
                {
                    bool isGlobal = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsGlobal"]);
                    bool isAscending = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsAscendingOrder"]);

                    if (isGlobal)
                    {
                        chkIsGlobal.Checked = true;
                    }
                    else
                    {
                        chkIsGlobal.Checked = false;
                    }

                    if (isAscending)
                    {
                        rdbOrders.SelectedValue = "1";
                    }
                    else
                    {
                        rdbOrders.SelectedValue = "2";
                    }

                    hdnDrpSortBy.Value =  dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString() ;
                    hdnIsStock.Value = dsGetRptDetails.Tables[0].Rows[0]["IsStock"].ToString();
                    sortBy = "["+ dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString()+"]" + " " + (isAscending == true ? "Asc" : "Desc");
                }
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerType()
    {
        try
        {
            DataSet dsGetCustType = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _getCustomerType.DBName = Session["dbname"].ToString();
            _getCustomerType.ConnConfig = Session["config"].ToString();

            List<CustomerReportParam> _lstCustomerReport = new List<CustomerReportParam>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCustomerType";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomerType, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerReport = serializer.Deserialize<List<CustomerReportParam>>(_APIResponse.ResponseData);
                dsGetCustType = CommonMethods.ToDataSet<CustomerReportParam>(_lstCustomerReport);
            }
            else
            {
                dsGetCustType = objBL_ReportsData.GetCustomerType(objCustReport);
            }

            if (dsGetCustType.Tables[0].Rows.Count > 0)
            {
                drpType.DataSource = dsGetCustType.Tables[0];
                drpType.DataTextField = "Type";
                drpType.DataValueField = "Type";
                drpType.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustReportFiltersValue()
    {
        DataSet dsGetCustReportFiltersValue = new DataSet();
        DataSet dsGetCustReportFiltersValue1 = new DataSet();
        DataSet dsGetCustReportFiltersValue2 = new DataSet();
        DataSet dsGetCustReportFiltersValue3 = new DataSet();
        DataSet dsGetCustReportFiltersValue4 = new DataSet();
        DataSet dsGetCustReportFiltersValue5 = new DataSet();
        DataSet dsGetCustReportFiltersValue6 = new DataSet();
        DataSet dsGetCustReportFiltersValue7 = new DataSet();
        DataSet dsGetCustReportFiltersValue8 = new DataSet();
        DataSet dsGetCustReportFiltersValue9 = new DataSet();
        DataSet dsGetCustReportFiltersValue10 = new DataSet();
        DataSet dsGetCustReportFiltersValue11 = new DataSet();
        DataSet dsGetCustReportFiltersValue12 = new DataSet();
        DataSet dsGetCustReportFiltersValue13 = new DataSet();
        DataSet dsGetCustReportFiltersValue14 = new DataSet();
        DataSet dsGetCustReportFiltersValue15 = new DataSet();

        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();

        ListGetEquipReportFiltersValue _lstGetEquipReportFiltersValue = new ListGetEquipReportFiltersValue();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetEquipReportFiltersValue";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipReportFiltersValue, true);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquipReportFiltersValue = serializer.Deserialize<ListGetEquipReportFiltersValue>(_APIResponse.ResponseData);

            dsGetCustReportFiltersValue1 = _lstGetEquipReportFiltersValue.lstTable.ToDataSet();
            dsGetCustReportFiltersValue2 = _lstGetEquipReportFiltersValue.lstTable1.ToDataSet();
            dsGetCustReportFiltersValue3 = _lstGetEquipReportFiltersValue.lstTable2.ToDataSet();
            dsGetCustReportFiltersValue4 = _lstGetEquipReportFiltersValue.lstTable3.ToDataSet();
            dsGetCustReportFiltersValue5 = _lstGetEquipReportFiltersValue.lstTable4.ToDataSet();
            dsGetCustReportFiltersValue6 = _lstGetEquipReportFiltersValue.lstTable5.ToDataSet();
            dsGetCustReportFiltersValue7 = _lstGetEquipReportFiltersValue.lstTable6.ToDataSet();
            dsGetCustReportFiltersValue8 = _lstGetEquipReportFiltersValue.lstTable7.ToDataSet();
            dsGetCustReportFiltersValue9 = _lstGetEquipReportFiltersValue.lstTable8.ToDataSet();
            dsGetCustReportFiltersValue10 = _lstGetEquipReportFiltersValue.lstTable9.ToDataSet();
            dsGetCustReportFiltersValue11 = _lstGetEquipReportFiltersValue.lstTable10.ToDataSet();
            dsGetCustReportFiltersValue12 = _lstGetEquipReportFiltersValue.lstTable11.ToDataSet();
            dsGetCustReportFiltersValue13 = _lstGetEquipReportFiltersValue.lstTable12.ToDataSet();
            dsGetCustReportFiltersValue14 = _lstGetEquipReportFiltersValue.lstTable13.ToDataSet();
            dsGetCustReportFiltersValue15 = _lstGetEquipReportFiltersValue.lstTable14.ToDataSet();


            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();
            DataTable dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dt8 = new DataTable();
            DataTable dt9 = new DataTable();
            DataTable dt10 = new DataTable();
            DataTable dt11 = new DataTable();
            DataTable dt12 = new DataTable();
            DataTable dt13 = new DataTable();
            DataTable dt14 = new DataTable();
            DataTable dt15 = new DataTable();

            dt1 = dsGetCustReportFiltersValue1.Tables[0];
            dt2 = dsGetCustReportFiltersValue2.Tables[1];
            dt3 = dsGetCustReportFiltersValue3.Tables[2];
            dt4 = dsGetCustReportFiltersValue4.Tables[3];
            dt5 = dsGetCustReportFiltersValue5.Tables[4];
            dt6 = dsGetCustReportFiltersValue6.Tables[5];
            dt7 = dsGetCustReportFiltersValue7.Tables[6];
            dt8 = dsGetCustReportFiltersValue8.Tables[7];
            dt9 = dsGetCustReportFiltersValue9.Tables[8];
            dt10 = dsGetCustReportFiltersValue10.Tables[9];
            dt11 = dsGetCustReportFiltersValue11.Tables[10];
            dt12 = dsGetCustReportFiltersValue12.Tables[11];
            dt13 = dsGetCustReportFiltersValue13.Tables[12];
            dt14 = dsGetCustReportFiltersValue14.Tables[13];
            dt15 = dsGetCustReportFiltersValue15.Tables[14];


            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            dt4.TableName = "Table4";
            dt5.TableName = "Table5";
            dt6.TableName = "Table6";
            dt7.TableName = "Table7";
            dt8.TableName = "Table8";
            dt9.TableName = "Table9";
            dt10.TableName = "Table10";
            dt11.TableName = "Table11";
            dt12.TableName = "Table12";
            dt13.TableName = "Table13";
            dt14.TableName = "Table14";
            dt15.TableName = "Table15";

            dsGetCustReportFiltersValue.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy(), dt6.Copy(), dt7.Copy(), dt8.Copy(), dt9.Copy(), dt10.Copy(), dt11.Copy(), dt12.Copy(), dt13.Copy(), dt14.Copy(), dt15.Copy() });

        }
        else
        {
            dsGetCustReportFiltersValue = objBL_ReportsData.GetEquipReportFiltersValue(objProp_User);
        }

        if (dsGetCustReportFiltersValue.Tables[0].Rows.Count > 0)
        {
            drpLocation.DataSource = dsGetCustReportFiltersValue.Tables[0];
            drpLocation.DataTextField = "Location";
            drpLocation.DataValueField = "Location";
            drpLocation.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[1].Rows.Count > 0)
        {
            drpOwnerID.DataSource = dsGetCustReportFiltersValue.Tables[1];
            drpOwnerID.DataTextField = "OwnerID";
            drpOwnerID.DataValueField = "OwnerID";
            drpOwnerID.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[2].Rows.Count > 0)
        {
            drpOwnerName.DataSource = dsGetCustReportFiltersValue.Tables[2];
            drpOwnerName.DataTextField = "OwnerName";
            drpOwnerName.DataValueField = "OwnerName";
            drpOwnerName.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[3].Rows.Count > 0)
        {
            drpequipment.DataSource = dsGetCustReportFiltersValue.Tables[3];
            drpequipment.DataTextField = "equipment";
            drpequipment.DataValueField = "equipment";
            drpequipment.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[4].Rows.Count > 0)
        {
            drpUnique.DataSource = dsGetCustReportFiltersValue.Tables[4];
            drpUnique.DataTextField = "Unique#";
            drpUnique.DataValueField = "Unique#";
            drpUnique.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[5].Rows.Count > 0)
        {
            drpfive_year_Insp_Date.DataSource = dsGetCustReportFiltersValue.Tables[5];
            drpfive_year_Insp_Date.DataTextField = "five_year_Insp_Date";
            drpfive_year_Insp_Date.DataValueField = "five_year_Insp_Date";
            drpfive_year_Insp_Date.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[6].Rows.Count > 0)
        {
            drpannual_Insp_Date.DataSource = dsGetCustReportFiltersValue.Tables[6];
            drpannual_Insp_Date.DataTextField = "annual_Insp_Date";
            drpannual_Insp_Date.DataValueField = "annual_Insp_Date";
            drpannual_Insp_Date.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[7].Rows.Count > 0)
        {
            drpcustomer.DataSource = dsGetCustReportFiltersValue.Tables[7];
            drpcustomer.DataTextField = "customer";
            drpcustomer.DataValueField = "customer";
            drpcustomer.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[8].Rows.Count > 0)
        {
            drpInspector_Name.DataSource = dsGetCustReportFiltersValue.Tables[8];
            drpInspector_Name.DataTextField = "Inspector_Name";
            drpInspector_Name.DataValueField = "Inspector_Name";
            drpInspector_Name.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[9].Rows.Count > 0)
        {
            drpManuf.DataSource = dsGetCustReportFiltersValue.Tables[9];
            drpManuf.DataTextField = "Manuf";
            drpManuf.DataValueField = "Manuf";
            drpManuf.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[10].Rows.Count > 0)
        {
            drpEquipmentType.DataSource = dsGetCustReportFiltersValue.Tables[10];
            drpEquipmentType.DataTextField = "EquipmentType";
            drpEquipmentType.DataValueField = "EquipmentType";
            drpEquipmentType.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[11].Rows.Count > 0)
        {
            drpServiceType.DataSource = dsGetCustReportFiltersValue.Tables[11];
            drpServiceType.DataTextField = "ServiceType";
            drpServiceType.DataValueField = "ServiceType";
            drpServiceType.DataBind();
        }

        //if (dsGetCustReportFiltersValue.Tables[12].Rows.Count > 0)
        //{
        //    drpInstalledOn.DataSource = dsGetCustReportFiltersValue.Tables[12];
        //    drpInstalledOn.DataTextField = "InstalledOn";
        //    drpInstalledOn.DataValueField = "InstalledOn";
        //    drpInstalledOn.DataBind();
        //}

        if (dsGetCustReportFiltersValue.Tables[13].Rows.Count > 0)
        {
            drpBuldingType.DataSource = dsGetCustReportFiltersValue.Tables[13];
            drpBuldingType.DataTextField = "BuildingType";
            drpBuldingType.DataValueField = "BuildingType";
            drpBuldingType.DataBind();
        }
        if (dsGetCustReportFiltersValue.Tables[14].Rows.Count > 0)
        {
            drpEquipmentState.DataSource = dsGetCustReportFiltersValue.Tables[14];
            drpEquipmentState.DataTextField = "EquipmentState";
            drpEquipmentState.DataValueField = "EquipmentState";
            drpEquipmentState.DataBind();
        }
        //if (dsGetCustReportFiltersValue.Tables[15].Rows.Count > 0)
        //{
        //    drpBuldingType.DataSource = dsGetCustReportFiltersValue.Tables[15];
        //    drpBuldingType.DataTextField = "BuildingType";
        //    drpBuldingType.DataValueField = "BuildingType";
        //    drpBuldingType.DataBind();
        //}
        //if (dsGetCustReportFiltersValue.Tables[16].Rows.Count > 0)
        //{
        //    drpEquipmentState.DataSource = dsGetCustReportFiltersValue.Tables[16];
        //    drpEquipmentState.DataTextField = "EquipmentState";
        //    drpEquipmentState.DataValueField = "EquipmentState";
        //    drpEquipmentState.DataBind();
        //}
        //if (dsGetCustReportFiltersValue.Tables[17].Rows.Count > 0)
        //{
        //    drpDefaultSalesPerson.DataSource = dsGetCustReportFiltersValue.Tables[17];
        //    drpDefaultSalesPerson.DataTextField = "DefaultSalesPerson";
        //    drpDefaultSalesPerson.DataValueField = "DefaultSalesPerson";
        //    drpDefaultSalesPerson.DataBind();
        //}
        //if (dsGetCustReportFiltersValue.Tables[18].Rows.Count > 0)
        //{
        //    drpLocationSTax.DataSource = dsGetCustReportFiltersValue.Tables[18];
        //    drpLocationSTax.DataTextField = "LocationSTax";
        //    drpLocationSTax.DataValueField = "LocationSTax";
        //    drpLocationSTax.DataBind();
        //}
    }

    private void GetCustomerName()
    {
        try
        {
            DataSet dsGetCustName = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _GetCustomerName.DBName = Session["dbname"].ToString();
            _GetCustomerName.ConnConfig = Session["config"].ToString();
            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCustomerName";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerName, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetCustName = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            }
            else
            {
                dsGetCustName = objBL_ReportsData.GetCustomerName(objCustReport);
            }

            if (dsGetCustName.Tables[0].Rows.Count > 0)
            {
                drpName.DataSource = dsGetCustName.Tables[0];
                drpName.DataTextField = "Name";
                drpName.DataValueField = "Name";
                drpName.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerAddress()
    {
        try
        {
            DataSet dsGetCustAddress = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _GetCustomerAddress.DBName = Session["dbname"].ToString();
            _GetCustomerAddress.ConnConfig = Session["config"].ToString();
            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCustomerAddress";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerAddress, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetCustAddress = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            }
            else
            {
                dsGetCustAddress = objBL_ReportsData.GetCustomerAddress(objCustReport);
            }

            if (dsGetCustAddress.Tables[0].Rows.Count > 0)
            {
                drpAddress.DataSource = dsGetCustAddress.Tables[0];
                drpAddress.DataTextField = "Address";
                drpAddress.DataValueField = "Address";
                drpAddress.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerCity()
    {
        try
        {
            DataSet dsGetCustCity = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _GetCustomerCity.DBName = Session["dbname"].ToString();
            _GetCustomerCity.ConnConfig = Session["config"].ToString();

            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();
           
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCustomerCity";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerCity, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetCustCity = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            }
            else
            {
                dsGetCustCity = objBL_ReportsData.GetCustomerCity(objCustReport);
            }

            if (dsGetCustCity.Tables[0].Rows.Count > 0)
            {
                drpCity.DataSource = dsGetCustCity.Tables[0];
                drpCity.DataTextField = "City";
                drpCity.DataValueField = "City";
                drpCity.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    private void GetReportsName()
    {
        try
        {
            string globalImageURL = "images/Globel_Report.png";
            string privateImageURL = "images/Private_Report.png";

            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());

            _GetDynamicReports.DBName = Session["dbname"].ToString();
            _GetDynamicReports.ConnConfig = Session["config"].ToString();
            _GetDynamicReports.UserID = Convert.ToInt32(Session["UserID"].ToString());
            //dsGetReports = objBL_ReportsData.GetReports(objProp_User);

            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetDynamicReports";
                _GetDynamicReports.Type = reportType;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDynamicReports, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetReports = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            }
            else
            {
                dsGetReports = objBL_ReportsData.GetDynamicReports(objProp_User, reportType);
            }

            if (dsGetReports.Tables.Count > 0)
            {
                drpReports.DataSource = dsGetReports.Tables[0];
                drpReports.DataTextField = "ReportName";
                drpReports.DataValueField = "Id";
                drpReports.DataBind();
                drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "-Select-", Value = "0" });
                // drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "Customer Detail", Value = "0" });
                System.Web.UI.WebControls.ListItem itemCD = drpReports.Items[0];
                itemCD.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                drpReports.SelectedValue = pubReportId.ToString();

                if (dsGetReports.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGetReports.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"].ToString()) == true)
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                        }
                        else
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + privateImageURL + ");background-repeat:no-repeat;";
                        }
                    }
                }

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetReportColumnsByRepId()
    {
        try
        {
            DataSet dsGetColumns = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;

            _GetReportColByRepId.DBName = Session["dbname"].ToString();
            _GetReportColByRepId.ConnConfig = Session["config"].ToString();
            _GetReportColByRepId.ReportId = pubReportId;

            _GetReportFiltersByRepId.DBName = Session["dbname"].ToString();
            _GetReportFiltersByRepId.ConnConfig = Session["config"].ToString();
            _GetReportFiltersByRepId.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_GetReportColByRepId";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReportColByRepId, true);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                    dsGetColumns = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
                }
                else
                {
                    dsGetColumns = objBL_ReportsData.GetReportColByRepId(objCustReport);
                }

                string[] checkedColumns = null;
                string[] selectedFiltersColumns = null;
                string[] selectedFiltersValues = null;
                if (dsGetColumns.Tables[0].Rows.Count > 0)
                {
                    checkedColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();
                    hdnColumnList.Value = string.Join(",", checkedColumns);
                }

                DataSet dsSelectedFilters = new DataSet();

                List<CustomerFilterViewModel> _lstCustFilter = new List<CustomerFilterViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_GetReportFiltersByRepId";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReportFiltersByRepId, true);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstCustFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                    dsSelectedFilters = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustFilter);
                }
                else
                {
                    dsSelectedFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
                }

                if (dsSelectedFilters.Tables[0].Rows.Count > 0)
                {
                    selectedFiltersColumns = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterColumn")).ToArray<string>();
                    selectedFiltersValues = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterSet")).ToArray<string>();
                }

                //if (drpReports.SelectedItem.ToString().ToLower() != "resize and reorder")
                //{
                //   // BindReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                //   // dvGridReport.Attributes.Add("style", "display:none");
                //}
                //else
                //{
                BindGridReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                dvGridReport.Attributes.Add("style", "display:block;height:350px;overflow:auto;");
                //}
            }
            else
            {
                // GetGroupedCustomersLocation();
                dvGridReport.Attributes.Add("style", "display:none");
            }

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private List<CustomerReport> GetReportFilters()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetFilters = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;

            _GetReportFiltersByRepId.DBName = Session["dbname"].ToString();
            _GetReportFiltersByRepId.ConnConfig = Session["config"].ToString();
            _GetReportFiltersByRepId.ReportId = pubReportId;
            List<CustomerFilterViewModel> _lstCustFilter = new List<CustomerFilterViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetReportFiltersByRepId";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReportFiltersByRepId, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetFilters = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustFilter);
            }
            else
            {
                dsGetFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
            }
            for (int i = 0; i <= dsGetFilters.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustmerReport = new CustomerReport();
                objCustmerReport.FilterColumns = dsGetFilters.Tables[0].Rows[i]["FilterColumn"].ToString();
                objCustmerReport.FilterValues = dsGetFilters.Tables[0].Rows[i]["FilterSet"].ToString();

                lstCustomerReport.Add(objCustmerReport);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportFilters());
        string filters = "var filters=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", filters, true);
    }


    private void GetCustomerDetails()
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            DataSet dsGetCustDetails1 = new DataSet();
            DataSet dsGetCustDetails2 = new DataSet();

            DataSet dsGetAccountSummaryListing = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();

            _GetEquipmentInspection.ConnConfig = Session["config"].ToString();

            _GetAccountSummaryListingDetail.DBName = Session["dbname"].ToString();
            _GetAccountSummaryListingDetail.ConnConfig = Session["config"].ToString();

            //dsGetCustDetails = objBL_ReportsData.getCustomerDetails(objProp_User);

            //ListGetEquipmentInspection _lstGetEquipmentInspection = new ListGetEquipmentInspection();
            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    string APINAME = "EquipmentAPI/EquipmentReport_GetEquipmentInspection";

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentInspection, true);
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();

            //    serializer.MaxJsonLength = Int32.MaxValue;

            //    _lstGetEquipmentInspection = serializer.Deserialize<ListGetEquipmentInspection>(_APIResponse.ResponseData);

            //    dsGetCustDetails1 = _lstGetEquipmentInspection.lstTable1.ToDataSet();
            //    dsGetCustDetails2 = _lstGetEquipmentInspection.lstTable2.ToDataSet();

            //    DataTable dt1 = new DataTable();
            //    DataTable dt2 = new DataTable();

            //    dt1 = dsGetCustDetails1.Tables[0];
            //    dt2 = dsGetCustDetails2.Tables[0];

            //    dt1.TableName = "Table1";
            //    dt2.TableName = "Table2";

            //    dsGetCustDetails.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

            //}
            //else
            //{
                dsGetCustDetails = objBL_ReportsData.getEquipmentInspection(objProp_User);
            //}


            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetAccountSummaryListingDetail";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAccountSummaryListingDetail, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstUserViewModel = serializer.Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsGetAccountSummaryListing = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsGetAccountSummaryListing = objBL_ReportsData.GetAccountSummaryListingDetail(objProp_User);
            }

            if (dsGetCustDetails.Tables.Count > 0)
            {
                //List<string> lstHeaders = new List<string>();
                //string[] columnNames = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                //                        select dc.ColumnName).ToArray();

                //lstHeaders = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                //              //orderby dc.ColumnName
                //              select dc.ColumnName).ToList();

                //if (dsGetAccountSummaryListing.Tables.Count > 0)
                //{
                //    var _accSummary = (from dc in dsGetAccountSummaryListing.Tables[0].Columns.Cast<DataColumn>()
                //                       select dc.ColumnName).ToList();
                //    lstHeaders.AddRange(_accSummary);
                //}

                dsGetCustDetails.Tables[0].TableName = "Equipment";
                dsGetCustDetails.Tables[1].TableName = "Custom";

                foreach (DataTable _table in dsGetCustDetails.Tables)
                {
                    List<System.Web.UI.WebControls.ListItem> lstHeaders = new List<System.Web.UI.WebControls.ListItem>();
                    List<string> lstHeaders1 = (from dc in _table.Columns.Cast<DataColumn>()
                                                select dc.ColumnName).OrderBy(d => d).ToList();

                    System.Web.UI.WebControls.ListItem _newLstBox = new System.Web.UI.WebControls.ListItem();
                    _newLstBox.Text += "<label id='lblText' style='font-weight:bolder'>" + _table.TableName + "</label>";
                    chkColumnList.Items.Add(_newLstBox);

                    System.Web.UI.WebControls.ListItem _newLstBox1 = new System.Web.UI.WebControls.ListItem();
                    _newLstBox1.Text = _table.TableName;
                    _newLstBox1.Attributes.CssStyle.Add("font-size", "15px");
                    _newLstBox1.Attributes.CssStyle.Add("font-weight", "bolder !important");
                    _newLstBox1.Attributes.CssStyle.Add("padding", "7px 0");
                    lstFilter.Items.Add(_newLstBox1);

                    foreach (var _header in lstHeaders1)
                    {
                        chkColumnList.Items.Add(_header);
                        lstFilter.Items.Add(_header);
                    }
                    //chkColumnList.Items.AddRange(lstHeaders);
                }

                //foreach (var _header in lstHeaders)
                //{
                //    switch (_header)
                //    {
                //        case "Name":
                //            System.Web.UI.WebControls.ListItem _newLstBox = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox.Text += "<label id='lblText' style='font-weight:bolder'>Customer</label>";

                //            chkColumnList.Items.Add(_newLstBox);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        case "LocationId":
                //            System.Web.UI.WebControls.ListItem _newLstBox1 = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox1.Text += "<label id='lblText' style='font-weight:bolder'>Location</label>";

                //            chkColumnList.Items.Add(_newLstBox1);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        case "EquipmentName":
                //            System.Web.UI.WebControls.ListItem _newLstBox2 = new System.Web.UI.WebControls.ListItem();
                //            _newLstBox2.Text += "<label id='lblText' style='font-weight:bolder'>Equipment</label>";

                //            chkColumnList.Items.Add(_newLstBox2);
                //            chkColumnList.Items.Add(_header);
                //            break;

                //        default:
                //            chkColumnList.Items.Add(_header);
                //            break;
                //    }
                //}


                //var _lstHeaders = lstHeaders.OrderBy(d => d);
                //chkColumnList.DataSource = _lstHeaders;

                //chkColumnList.Items. = "sasdsad";
                //from dc in lstHeaders orderby 
                //chkColumnList.Items.Add(new System.Web.UI.WebControls.ListBox());
                chkColumnList.DataBind();

                //lstFilter.DataSource = _lstHeaders;
                lstFilter.DataBind();
                ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        try
        {
            //string[] checkedColumns = chkColumnList.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToArray();

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _CheckExistingReport.ConnConfig = Session["config"].ToString();
            _InsertCustomerReport.DBName = Session["dbname"].ToString();
            _InsertCustomerReport.ConnConfig = Session["config"].ToString();
            _IsStockReportExist.ConnConfig = Session["config"].ToString();
            _UpdateCustomerReport.DBName = Session["dbname"].ToString();
            _UpdateCustomerReport.ConnConfig = Session["config"].ToString();

            //  objCustReport.ReportId = Convert.ToInt32(ViewState["ReportId"]);
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
                _CheckExistingReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
                _InsertCustomerReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
                _UpdateCustomerReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            objCustReport.ReportName = txtReportName.Text;
            _CheckExistingReport.ReportName = txtReportName.Text;
            _InsertCustomerReport.ReportName = txtReportName.Text;
            _IsStockReportExist.ReportName = txtReportName.Text;
            _UpdateCustomerReport.ReportName = txtReportName.Text;

            //Changed by Yashasvi Jadav
            objCustReport.ReportType = !string.IsNullOrEmpty(Request.QueryString["type"]) ? objCustReport.ReportType = Request.QueryString["type"].ToString() : objCustReport.ReportType = "Customer";
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            // objCustReport.ColumnName = string.Join(",", checkedColumns);
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            objCustReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            objCustReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));

            //API
            //Changed by Yashasvi Jadav
            _InsertCustomerReport.ReportType = !string.IsNullOrEmpty(Request.QueryString["type"]) ? _InsertCustomerReport.ReportType = Request.QueryString["type"].ToString() : _InsertCustomerReport.ReportType = "Customer";
            _InsertCustomerReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            _InsertCustomerReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            _InsertCustomerReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            _InsertCustomerReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            _InsertCustomerReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            _InsertCustomerReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));

            //Changed by Yashasvi Jadav
            _UpdateCustomerReport.ReportType = !string.IsNullOrEmpty(Request.QueryString["type"]) ? _UpdateCustomerReport.ReportType = Request.QueryString["type"].ToString() : _UpdateCustomerReport.ReportType = "Customer";
            _UpdateCustomerReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            _UpdateCustomerReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            _UpdateCustomerReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            _UpdateCustomerReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            _UpdateCustomerReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            _UpdateCustomerReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));

            hdnCustomizeReportName.Value = objCustReport.ReportName;
            hdnCustomizeReportName.Value = _CheckExistingReport.ReportName;
            hdnCustomizeReportName.Value = _InsertCustomerReport.ReportName;
            hdnCustomizeReportName.Value = _IsStockReportExist.ReportName;
            hdnCustomizeReportName.Value = _UpdateCustomerReport.ReportName;

            objCustReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            //objCustReport.SortBy = drpSortBy.Text;
            objCustReport.SortBy = hdnDrpSortBy.Value;
            objCustReport.MainHeader = chkMainHeader.Checked ? true : false;
            objCustReport.CompanyName = txtCompanyName.Text;
            objCustReport.ReportTitle = txtReportTitle.Text;
            objCustReport.SubTitle = txtSubtitle.Text;
            objCustReport.IsStock = false;
            objCustReport.Module = "Equipment";

            //API
            _InsertCustomerReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            _InsertCustomerReport.SortBy = hdnDrpSortBy.Value;
            _InsertCustomerReport.MainHeader = chkMainHeader.Checked ? true : false;
            _InsertCustomerReport.CompanyName = txtCompanyName.Text;
            _InsertCustomerReport.ReportTitle = txtReportTitle.Text;
            _InsertCustomerReport.SubTitle = txtSubtitle.Text;
            _InsertCustomerReport.IsStock = false;
            _InsertCustomerReport.Module = "Equipment";

            _UpdateCustomerReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            _UpdateCustomerReport.SortBy = hdnDrpSortBy.Value;
            _UpdateCustomerReport.MainHeader = chkMainHeader.Checked ? true : false;
            _UpdateCustomerReport.CompanyName = txtCompanyName.Text;
            _UpdateCustomerReport.ReportTitle = txtReportTitle.Text;
            _UpdateCustomerReport.SubTitle = txtSubtitle.Text;
            _UpdateCustomerReport.IsStock = false;
            _UpdateCustomerReport.Module = "Equipment";

            if (chkDatePrepared.Checked)
            {
                objCustReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
                _InsertCustomerReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
                _UpdateCustomerReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
            }
            else
            {
                objCustReport.DatePrepared = "";
                _InsertCustomerReport.DatePrepared = "";
                _UpdateCustomerReport.DatePrepared = "";
            }
            objCustReport.TimePrepared = chkTimePrepared.Checked ? true : false;
            _InsertCustomerReport.TimePrepared = chkTimePrepared.Checked ? true : false;
            _UpdateCustomerReport.TimePrepared = chkTimePrepared.Checked ? true : false;

            if (chkPageNumber.Checked)
            {
                objCustReport.PageNumber = drpPageNumber.SelectedValue.ToString();
                _InsertCustomerReport.PageNumber = drpPageNumber.SelectedValue.ToString();
                _UpdateCustomerReport.PageNumber = drpPageNumber.SelectedValue.ToString();
            }
            else
            {
                objCustReport.PageNumber = "";
                _InsertCustomerReport.PageNumber = "";
                _UpdateCustomerReport.PageNumber = "";
            }
            objCustReport.ExtraFooterLine = txtExtraFooterLine.Text;
            objCustReport.Alignment = drpAlignment.SelectedValue.ToString();
            objCustReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            //API
            _InsertCustomerReport.ExtraFooterLine = txtExtraFooterLine.Text;
            _InsertCustomerReport.Alignment = drpAlignment.SelectedValue.ToString();
            _InsertCustomerReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            _UpdateCustomerReport.ExtraFooterLine = txtExtraFooterLine.Text;
            _UpdateCustomerReport.Alignment = drpAlignment.SelectedValue.ToString();
            _UpdateCustomerReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_CheckExistingReport";

                _CheckExistingReport.reportAction = hdnReportAction.Value;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _CheckExistingReport, true);

                bool returnVal = Convert.ToBoolean(_APIResponse.ResponseData);

                if (returnVal == true)
                {
                    dvSaveReport.Attributes.Add("style", "display:block");
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report with this name already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    return;
                }
            }
            else
            {
                if (objBL_ReportsData.CheckExistingReport(objCustReport, hdnReportAction.Value) == true)
                {
                    dvSaveReport.Attributes.Add("style", "display:block");
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report with this name already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    return;
                }
            }
            DataSet ds = new DataSet();
            if (hdnReportAction.Value == "Save")
            {
                List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_InsertCustomerReport";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _InsertCustomerReport, true);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstCustomerReport = serializer.Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<CustomerReportViewModel>(_lstCustomerReport);
                }
                else
                {
                    ds = objBL_ReportsData.InsertCustomerReport(objCustReport);
                }

                pubReportId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReportId"]);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_IsStockReportExist";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsStockReportExist, true);

                    bool returnVal = Convert.ToBoolean(_APIResponse.ResponseData);

                    if (returnVal == true)
                    {
                        string APINAME1 = "EquipmentAPI/EquipmentReport_UpdateCustomerReport";

                        APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _UpdateCustomerReport);

                        pubReportId = _UpdateCustomerReport.ReportId;
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report customized successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        pubReportId = _UpdateCustomerReport.ReportId;
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report. Please choose another title for this report',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
                else
                {
                    if (objBL_ReportsData.IsStockReportExist(objCustReport, hdnReportAction.Value) == true)
                    {
                        // if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
                        //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
                        //{
                        objBL_ReportsData.UpdateCustomerReport(objCustReport);
                        pubReportId = objCustReport.ReportId;
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report customized successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}
                        //  else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
                        //{
                        //  ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be updated!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}
                        //else
                        //{
                        //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        //}
                    }
                    else
                    {
                        pubReportId = objCustReport.ReportId;
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report. Please choose another title for this report',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
            }
            dvSaveReport.Attributes.Add("style", "display:none");
            GetReportDetailByRptId();
            GetReportsName();
            GetReportColumnsByRepId();
            ConvertToJSON();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            _DeleteCustomerReport.DBName = Session["dbname"].ToString();
            _DeleteCustomerReport.ConnConfig = Session["config"].ToString();
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
                _DeleteCustomerReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Please select report to delete.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            _DeleteCustomerReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            //{

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_DeleteCustomerReport";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteCustomerReport, true);
            }
            else
            {
                objBL_ReportsData.DeleteCustomerReport(objCustReport);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            GetReportsName();
            if (drpReports.Items.Count > 0)
            {
                drpReports.SelectedIndex = 0;
                pubReportId = Convert.ToInt32(drpReports.SelectedValue);
                hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();

                GetReportDetailByRptId();
                GetReportsName();
                GetReportColumnsByRepId();
                ConvertToJSON();
            }
            else
            {
                // //CrystalReportViewer1.ReportSource = null;
            }
            // return;
            //}
            //else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be deleted!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to delete this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}
            // GetReportsName();
            //drpReports.SelectedValue = pubReportId.ToString();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if (drpReports.SelectedIndex != 0)
            // {
            pubReportId = Convert.ToInt32(drpReports.SelectedValue);
            hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();
            GetReportsName();
            drpReports.SelectedValue = pubReportId.ToString();

            if (pubReportId != 0)
            {
                //grdCustomerReportData.PageIndex = 0;
                // dvGridReport.Attributes.Add("style", "display:block");
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }
            else
            {
                // GetGroupedCustomersLocation();
                // grdCustomerReportData.DataSource = null;
                dvGridReport.Attributes.Add("style", "display:none");
            }


            //  GetReportDetailByRptId();
            // GetReportColumnsByRepId();
            ConvertToJSON();
            ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
            // }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Equipments.aspx", false);
        }
        catch
        {
            //
        }
    }


    private void BindGridReport(string[] checkedColumns, string[] selectedFiltersColumns, string[] selectedFiltersValues, string sortBy)
    {
        string query = "SELECT distinct ";

        foreach (var item in checkedColumns)
        {
             
             query += "["+item+"]" + ",";
        }

        query = query.Substring(0, query.Length - 1);
        if (selectedFiltersColumns == null)
        {

           
            //query += " FROM vw_EquipmentReport order by OwnerID,location," + sortBy;
            query += " FROM vw_EquipReportDetails order by " + sortBy;
        }

        else
        {
            string filters = string.Empty;
            if (selectedFiltersColumns != null)
            {
                for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                {
                    if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice" && selectedFiltersColumns[i].ToLower() != "equipmentcounts")
                    {
                        if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        {
                            filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        }
                        else
                        {
                            int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                            if (indexOfSingleQuote == 0)
                            {
                                filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                        }
                    }
                    else
                    {
                        if (selectedFiltersValues[i].Contains("and"))
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        }
                        else
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        }
                    }
                }
            }
            filters = filters.Substring(0, filters.Length - 4);
            query += " FROM vw_EquipReportDetails where " + filters + " order by " + sortBy;
        }


        BindHeaderDetails();
        GetGridData(query);

    }

    private void BindHeaderDetails()
    {
        try
        {
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();

            _GetHeaderFooterDetail.ConnConfig = Session["config"].ToString();

            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                byte[] compLogo = null;
                if (!Convert.IsDBNull(dsCompDetail.Tables[0].Rows[0]["Logo"]))
                {
                    compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                    imgLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(compLogo);
                }
                //MemoryStream ms = new MemoryStream(compLogo, 0, compLogo.Length);     //As asked by Anand, commented on 9th dec
                //// Convert byte[] to Image
                //ms.Write(compLogo, 0, compLogo.Length);
                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                //string imagePathToSave = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + Server.MapPath("ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png");
                //string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";

                //System.Drawing.Image resizedImage = image.GetThumbnailImage(130, 130, null, IntPtr.Zero);
                //resizedImage.Save(imagePathToSave, System.Drawing.Imaging.ImageFormat.Png);
                //// imgLogo.ImageUrl = imagePathToShow;

                lblCompanyName.Text = dsCompDetail.Tables[0].Rows[0]["Name"].ToString();
                lblCompAddress.Text = dsCompDetail.Tables[0].Rows[0]["Address"].ToString();
                lblCompEmail.Text = dsCompDetail.Tables[0].Rows[0]["Email"].ToString();
            }

            objCustReport.ReportId = pubReportId;
            _GetHeaderFooterDetail.ReportId = pubReportId;

            DataSet dsGetHeaderFooterDetail = new DataSet();

            List<HeaderFooterDetailViewModel> _lstHeaderFooterDetail = new List<HeaderFooterDetailViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetHeaderFooterDetail";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetHeaderFooterDetail, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstHeaderFooterDetail = serializer.Deserialize<List<HeaderFooterDetailViewModel>>(_APIResponse.ResponseData);
                dsGetHeaderFooterDetail = CommonMethods.ToDataSet<HeaderFooterDetailViewModel>(_lstHeaderFooterDetail);
            }
            else
            {
                dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);
            }

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                hdnMainHeader.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString();
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {
                    chkMainHeader.Checked = true;
                }
                else
                {

                    chkMainHeader.Checked = false;
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    txtCompanyName.Enabled = true;
                    txtCompanyName.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    lblCompanyName2.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    chkCompanyName.Checked = true;
                }
                else
                {
                    txtCompanyName.Enabled = false;
                    txtCompanyName.Text = "";
                    lblCompanyName2.Text = "";
                    chkCompanyName.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    txtReportTitle.Enabled = true;
                    txtReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    lblReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    chkReportTitle.Checked = true;
                }
                else
                {
                    txtReportTitle.Enabled = false;
                    txtReportTitle.Text = "";
                    lblReportTitle.Text = "";
                    chkReportTitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    txtSubtitle.Enabled = true;
                    txtSubtitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    lblSubTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    chkSubtitle.Checked = true;
                }
                else
                {
                    txtSubtitle.Enabled = false;
                    txtSubtitle.Text = "";
                    lblSubTitle.Text = "";
                    chkSubtitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "12/31/01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yyyy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    }

                    drpDatePrepared.Enabled = true;
                    drpDatePrepared.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString();
                    chkDatePrepared.Checked = true;
                }
                else
                {
                    lblDate.Text = "";
                    drpDatePrepared.Enabled = false;
                    drpDatePrepared.SelectedIndex = 0;
                    chkDatePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    lblTime.Text = DateTime.Now.ToString("hh:mm tt");
                    chkTimePrepared.Checked = true;
                }
                else
                {
                    lblTime.Text = "";
                    chkTimePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString() != "")
                {
                    drpPageNumber.Enabled = true;
                    drpPageNumber.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString();
                    chkPageNumber.Checked = true;
                }
                else
                {
                    drpPageNumber.Enabled = false;
                    drpPageNumber.SelectedIndex = 0;
                    chkPageNumber.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    txtExtraFooterLine.Enabled = true;
                    txtExtraFooterLine.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString();
                    chkExtraFootLine.Checked = true;
                }
                else
                {
                    txtExtraFooterLine.Enabled = false;
                    txtExtraFooterLine.Text = "";
                    chkExtraFootLine.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() != "")
                {
                    drpAlignment.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-left");
                        dvSubHeader.Attributes.Add("align", "left");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-right");
                        dvSubHeader.Attributes.Add("align", "right");
                    }
                    else
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-center");
                        dvSubHeader.Attributes.Add("align", "center");
                    }
                }
                else
                {
                    drpDatePrepared.SelectedIndex = 0;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString() != "")
                {
                    drpPDFPageSize.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString();
                }
                Session["ReportDate"] = lblDate.Text;
                Session["ReportTime"] = lblTime.Text;
            }
        }
        catch (Exception e)
        {
        }
    }

    private void GetGridData(string query)
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();

            _GetOwners.DBName = Session["dbname"].ToString();
            _GetOwners.ConnConfig = Session["config"].ToString();
            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();

            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    string APINAME = "EquipmentAPI/EquipmentReport_GetOwners";

            //    _GetOwners.query = query;

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetOwners, true);
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();

            //    serializer.MaxJsonLength = Int32.MaxValue;

            //    _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
            //    dsGetCustDetails = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            //}
            //else
            //{
                dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);
            //}

            Session["DsGetCustomerDetails"] = dsGetCustDetails;
            Session["Query"] = query;
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                BindReportTable(dsGetCustDetails);

            }
            else
            {
            }


            ConvertToJSON();

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void BindReportTable(DataSet dsGetCustDetails)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            DataSet dsGetColumnWidth = new DataSet();
            objCustReport.ReportId = pubReportId;

            _GetColumnWidthByReportId.DBName = Session["dbname"].ToString();
            _GetColumnWidthByReportId.ConnConfig = Session["config"].ToString();
            _GetColumnWidthByReportId.ReportId = pubReportId;

            List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetColumnWidthByReportId";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetColumnWidthByReportId, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerReport = serializer.Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
                dsGetColumnWidth = CommonMethods.ToDataSet<CustomerReportViewModel>(_lstCustomerReport);
            }
            else
            {
                dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
            }

            hdnColumnWidth.Value = "";
            //var _list = dsGetCustDetails.Tables[0].Rows;
            if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
                {
                    hdnColumnWidth.Value += dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString() + ",";
                }

                hdnColumnWidth.Value = hdnColumnWidth.Value.TrimEnd(',');
            }

            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            string footer = string.Empty;
            //Table start.
            html.Append("<table id='tblResize' border = '0'>");

            //Building the Header row.
            html.Append("<thead><tr>");

            foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
            {
                html.Append("<th class='resize-header'>");
                //html.Append("<th style='border:13px solid transparent;color:black;font-size:11px;width:150px; border-image: url(images/icons_big/list-bullet2.PNG) " + b + " '>");
                //html.Append("<th style='border:1;color:black;font-size:11px;'>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr></thead>");


            //Building the Data rows.
            foreach (DataRow row in dsGetCustDetails.Tables[0].Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                {
                    html.Append("<td style='border:0;padding:10px 20px 3px 10px;color:black;'>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");

            }

            // html.Append("<tr><td>&nbsp;</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td></tr>");

            for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
            {
                if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString() + "</td>";
                }
                else
                {
                    footer += "<td style='border:0;padding:10px 20px 3px 10px;color:black;'>&nbsp;</td>";
                }
            }

            if (footer != "")
            {
                html.Append("<tr>" + footer + "</tr>");
            }

            //Table end.
            html.Append("</table>");
            html.Append("<div><b>Total Counts: </b>" + dsGetCustDetails.Tables[0].Rows.Count + "</div>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GeneratePdfTable(DataSet dsGetCustDetails)
    {
        try
        {
            DeletePDFFiles();
        }
        catch
        {
            //
        }
        //server folder path which is stored your PDF documents         
        string serverPath = Server.MapPath("ReportFiles/PDF");
        if (!Directory.Exists(serverPath))
        {
            Directory.CreateDirectory(serverPath);
        }

        //string filename = path + "/Doc1.pdf";
        int userId = Convert.ToInt32(Session["UserID"].ToString());
        string fileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".pdf";
        //string filePath = serverPath + "/" + fileName;
        string filePath = Path.Combine(serverPath, fileName); ;

        //File.Delete(filePath);
        Session["FilePath"] = filePath;
        // File.Create(filePath);
        // lblAttachedFile.Text = fileName;


        objCustReport.DBName = Session["dbname"].ToString();
        objCustReport.ConnConfig = Session["config"].ToString();

        _GetHeaderFooterDetail.DBName = Session["dbname"].ToString();
        _GetHeaderFooterDetail.ConnConfig = Session["config"].ToString();

        _GetColumnWidthByReportId.DBName = Session["dbname"].ToString();
        _GetColumnWidthByReportId.ConnConfig = Session["config"].ToString();

        int[] getColumnsWidth = new int[dsGetCustDetails.Tables[0].Columns.Count];
        int countTotalWidth = 0;
        DataSet dsGetColumnWidth = new DataSet();
        objCustReport.ReportId = pubReportId;
        _GetHeaderFooterDetail.ReportId = pubReportId;
        _GetColumnWidthByReportId.ReportId = pubReportId;

        List<CustomerReportViewModel> _lstCustomerReport = new List<CustomerReportViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetColumnWidthByReportId";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetColumnWidthByReportId, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCustomerReport = serializer.Deserialize<List<CustomerReportViewModel>>(_APIResponse.ResponseData);
            dsGetColumnWidth = CommonMethods.ToDataSet<CustomerReportViewModel>(_lstCustomerReport);
        }
        else
        {
            dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
        }

        if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
            {
                getColumnsWidth[i] = Convert.ToInt32(dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString().Replace("px", ""));
                countTotalWidth = countTotalWidth + getColumnsWidth[i];
            }
        }

        DataSet dsGetHeaderFooterDetail = new DataSet();

        List<HeaderFooterDetailViewModel> _lstHeaderFooterDetail = new List<HeaderFooterDetailViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetHeaderFooterDetail";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetHeaderFooterDetail, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstHeaderFooterDetail = serializer.Deserialize<List<HeaderFooterDetailViewModel>>(_APIResponse.ResponseData);
            dsGetHeaderFooterDetail = CommonMethods.ToDataSet<HeaderFooterDetailViewModel>(_lstHeaderFooterDetail);
        }
        else
        {
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);
        }

        Rectangle rcPageSize = PageSize.A4;
        //Create new PDF document  
        if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
        {
            rcPageSize = PageSize.GetRectangle(dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString());
        }
        // Rectangle rcPageSize = PageSize.GetRectangle("A1");
        //Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        Document document = new Document(rcPageSize, 20f, 20f, 20f, 20f);

        try
        {
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create, FileAccess.Write));

            PdfPTable headerTable = new PdfPTable(2);
            PdfPTable tblDateTime = new PdfPTable(1);
            PdfPTable tblExtraFooter = new PdfPTable(1);
            tblExtraFooter.TotalWidth = countTotalWidth;
            tblExtraFooter.LockedWidth = true;
            tblExtraFooter.SpacingBefore = 30f;

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {

                    headerTable.TotalWidth = 300;
                    headerTable.LockedWidth = true;

                    //  headerTable.SpacingBefore = 20f;
                    //  headerTable.SpacingAfter = 30f;
                    headerTable.HorizontalAlignment = 0;

                    tblDateTime.TotalWidth = 200;
                    tblDateTime.LockedWidth = true;
                    tblDateTime.HorizontalAlignment = 2;

                    DataSet dsCompDetail = new DataSet();
                    objProp_User.DBName = Session["dbname"].ToString();
                    objProp_User.ConnConfig = Session["config"].ToString();
                    dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

                    byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(compLogo);
                    logo.ScaleAbsolute(110, 110);
                    PdfPCell imageCell = new PdfPCell(logo);
                    imageCell.Border = 0;
                    headerTable.AddCell(imageCell);

                    Font compNameStyle = FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD);
                    Font compStyle = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);

                    PdfPTable tblCompDetails = new PdfPTable(1);

                    PdfPCell companyName = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Name"].ToString(), compNameStyle));
                    companyName.Border = 0;
                    companyName.PaddingTop = 20;
                    companyName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(companyName);

                    PdfPCell compAddress = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Address"].ToString(), compStyle));
                    compAddress.Border = 0;
                    compAddress.PaddingTop = 10;
                    compAddress.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compAddress);

                    PdfPCell compEmail = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Email"].ToString(), compStyle));
                    compEmail.Border = 0;
                    compEmail.PaddingTop = 10;
                    compEmail.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compEmail);

                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    headerTable.AddCell(tblCompDetails);
                }
            }
            Font header2ompNameStyle = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
            Font header2ReportTitleStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            Font header2SubTitleStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
            PdfPTable tblCompDetails2 = new PdfPTable(1);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                tblCompDetails2.DefaultCell.Border = Rectangle.NO_BORDER;
                tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                tblCompDetails2.SpacingBefore = 15;
                tblCompDetails2.SpacingAfter = 10;

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    PdfPCell compName = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString(), header2ompNameStyle));
                    compName.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(compName);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    PdfPCell reportTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString(), header2ReportTitleStyle));
                    reportTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(reportTitle);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    PdfPCell subTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString(), header2SubTitleStyle));
                    subTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(subTitle);
                }
                Font dateTimeStyle = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    if (Session["ReportTime"] != null)
                    {
                        PdfPCell time = new PdfPCell(new Phrase(Session["ReportTime"].ToString(), dateTimeStyle));
                        time.Border = 0;
                        time.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(time);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (Session["ReportDate"] != null)
                    {
                        PdfPCell date = new PdfPCell(new Phrase(Session["ReportDate"].ToString(), dateTimeStyle));
                        date.Border = 0;
                        date.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(date);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    PdfPCell extraFooter = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString(), header2ReportTitleStyle));
                    extraFooter.Border = 0;
                    extraFooter.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tblExtraFooter.AddCell(extraFooter);
                }
            }

            PdfPTable table = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            table.TotalWidth = countTotalWidth;

            //fix the absolute width of the table
            table.LockedWidth = true;

            table.SetWidths(getColumnsWidth);
            table.HorizontalAlignment = 0;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            Font headerStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
            Font rowsStyle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
            PdfPTable footer = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j <= dsGetCustDetails.Tables[0].Columns.Count - 1; j++)
                {
                    PdfPCell columns = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Columns[j].ToString(), headerStyle));
                    columns.Border = 0;
                    table.AddCell(columns);
                }

                for (int j = 0; j <= dsGetCustDetails.Tables[0].Rows.Count - 1; j++)
                {
                    for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Rows[j][i].ToString().Trim(), rowsStyle));
                        rows.Border = 0;
                        table.AddCell(rows);
                    }
                }

                Font footerStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                footer.TotalWidth = countTotalWidth;
                footer.LockedWidth = true;
                footer.SetWidths(getColumnsWidth);
                footer.HorizontalAlignment = 0;
                footer.DefaultCell.Border = Rectangle.NO_BORDER;


                for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(""));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                }
            }


            document.Open();
            document.Add(tblDateTime);
            document.Add(headerTable);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                document.Add(tblCompDetails2);
            }
            document.Add(table);
            document.Add(footer);
            document.Add(tblExtraFooter);
        }
        catch (Exception ex)
        {
            document.Close();
        }
        finally
        {
            document.Close();
        }
    }

    public void ShowPdf(string filename)
    {
        //Clears all content output from Buffer Stream
        Response.ClearContent();
        //Clears all headers from Buffer Stream
        Response.ClearHeaders();
        //Adds an HTTP header to the output stream
        Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
        //Gets or Sets the HTTP MIME type of the output stream
        Response.ContentType = "application/pdf";
        //Writes the content of the specified file directory to an HTTP response output stream as a file block
        Response.WriteFile(filename);
        //sends all currently buffered output to the client
        Response.Flush();
        //Clears all content output from Buffer Stream
        Response.Clear();
    }

    private void SaveResizedAndReorderReport()
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.ReportId = pubReportId;
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');

            _UpdateCustomerReportResizedWidth.DBName = Session["dbname"].ToString();
            _UpdateCustomerReportResizedWidth.ConnConfig = Session["config"].ToString();
            _UpdateCustomerReportResizedWidth.UserId = Convert.ToInt32(Session["UserID"].ToString());
            _UpdateCustomerReportResizedWidth.ReportId = pubReportId;
            _UpdateCustomerReportResizedWidth.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            _UpdateCustomerReportResizedWidth.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            //{

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_UpdateCustomerReportResizedWidth";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateCustomerReportResizedWidth, true);
            }
            else
            {
                objBL_ReportsData.UpdateCustomerReportResizedWidth(objCustReport);
            }
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //}
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];

        if (dsGetCustomerDetails != null)
        {
            SaveResizedAndReorderReport();
            GetReportColumnsByRepId();

            GetReportsName();
            if (pubReportId != 0)
            {
                drpReports.SelectedValue = pubReportId.ToString();
            }

            if (dsGetCustomerDetails.Tables.Count > 0)
            {
                if (dsGetCustomerDetails.Tables[0].Rows.Count > 0)
                {
                    GeneratePdfTable(dsGetCustomerDetails);
                    Session["ReportId"] = pubReportId;

                    string script = String.Format("window.open('CustomerReportPreview.aspx');");
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
                }
            }
        }
    }


    public void GetPDFData()
    {
        GetReportColumnsByRepId();
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
        Session["ReportId"] = pubReportId;
        GeneratePdfTable(dsGetCustomerDetails);
        //BindReportTable(dsGetCustomerDetails);
    }

    protected void btnSendReport_Click(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        if (hdnSendReportType.Value == "btnSendPDFReport")
        {
            DataSet dsGetCustomerDetails = new DataSet();
            dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
            GeneratePdfTable(dsGetCustomerDetails);
        }
        else
        {
            GenerateExcelFile();
        }
        if (txtTo.Text.Trim() != string.Empty)
        {
            Mail mail = new Mail();
            try
            {
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text;
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Report attached.";
                }

                string filePath = Session["FilePath"].ToString();
                mail.AttachmentFiles.Add(filePath);
                // mail.attachmentBytes = ExportReportToPDF("");                    

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                GetReportsName();
                if (pubReportId != 0)
                {
                    drpReports.SelectedValue = pubReportId.ToString();
                }
                //  this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }


    private void GenerateExcelFile()
    {
        try
        {
            try
            {
                DeleteExcelFiles();
            }
            catch
            {
                //
            }
            string htmlToExport = string.Empty;
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbExtraFooter = new StringBuilder();
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;

            _GetHeaderFooterDetail.DBName = Session["dbname"].ToString();
            _GetHeaderFooterDetail.ConnConfig = Session["config"].ToString();
            _GetHeaderFooterDetail.ReportId = pubReportId;
            DataSet dsGetHeaderFooterDetail = new DataSet();

            List<HeaderFooterDetailViewModel> _lstHeaderFooterDetail = new List<HeaderFooterDetailViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetHeaderFooterDetail";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetHeaderFooterDetail, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstHeaderFooterDetail = serializer.Deserialize<List<HeaderFooterDetailViewModel>>(_APIResponse.ResponseData);
                dsGetHeaderFooterDetail = CommonMethods.ToDataSet<HeaderFooterDetailViewModel>(_lstHeaderFooterDetail);
            }
            else
            {
                dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);
            }


            string excelServerPath = Server.MapPath("ReportFiles/Excel");
            if (!Directory.Exists(excelServerPath))
            {
                Directory.CreateDirectory(excelServerPath);
            }
            int userId = Convert.ToInt32(Session["UserID"].ToString());

            string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + DateTime.Now.ToString("MM-dd-yyyy") + ".xls";
            string filePath = excelServerPath + "\\" + excelFileName + ".xls";


            DataSet ds = (DataSet)Session["DsGetCustomerDetails"];
            //Write the HTML back to the browser.
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + excelFileName + "");
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            DataGrid dgrExport = new DataGrid();
            dgrExport.DataSource = ds.Tables[0];
            dgrExport.DataBind();

            byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
            sbHeader.Append("<html><body><div><table border = '0'>");
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                string imagePathToShow = baseUrl + "Logo.ashx?db=" + dsCompDetail.Tables[0].Rows[0]["DBName"].ToString();
                if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
                {

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                    {
                        sbHeader.Append("<tr><td><table><tr><td rowspan='4' style='height:150px;vertical-align:center;text-align:center;'><img src=" + imagePathToShow + "></img></td><td>&nbsp;</td></tr><tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:60px;'>&nbsp;</td></tr>");
                        sbHeader.Append("</table></td><td style='vertical-align:top;font-weight:bold;font-size:10px;color:black;'>" + lblTime.Text + " <br/> " + lblDate.Text + "</td></tr>");
                    }
                    string alignment = string.Empty;
                    alignment = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (alignment.ToLower() == "standard" || alignment.ToLower() == "centered")
                    {
                        alignment = "center";
                    }

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:16px;font-weight:bold;'>" + lblCompanyName2.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:14px;font-weight:bold;'>" + lblReportTitle.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:12px;font-weight:bold;'>" + lblSubTitle.Text + "</td></tr>");
                    }
                    sbHeader.Append("<tr><td colspan='2' style='height:15px;'>&nbsp;</td></tr>");
                    sbHeader.Append("<tr><td colspan='2'>");

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                    {
                        sbExtraFooter.Append("<tr><td colspan='2' style='height:80px;text-align:center;color:Black;font-size:14px;font-weight:bold;'>" + dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() + "</td></tr>");
                    }
                }
            }
            dgrExport.RenderControl(htmlWrite);

            Response.Write(sbHeader);
            Response.Write(stringWrite.ToString());
            Response.Write(sbExtraFooter.ToString());
            Response.End();

            //commented by Mayuri 20th july, 16
            //string headerTable = @"<Table><tr><td>Report Header</td></tr><tr><td><img src=""D:\\Folder\\Report Header.jpg"" \></td></tr></Table>";
            //string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".xls";
            //if (dsCompDetail.Tables[0].Rows.Count > 0)
            //{
            //   // string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";
            //    string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() +"/logo.ashx?db=" + dsCompDetail.Tables[0].Rows[0]["DBName"].ToString();
            //    sbHeader.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            //    sbHeader.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div>");
            //    sbHeader.Append("<table border = '0'>");

            //    if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            //    {
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
            //        {
            //            sbHeader.Append("<tr><td><table><tr><td rowspan='4' style='height:150px;vertical-align:center;text-align:center;'><img src=" + imagePathToShow + "></img></td><td>&nbsp;</td></tr><tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
            //            sbHeader.Append("<tr><td style='height:60px;'>&nbsp;</td></tr>");
            //            sbHeader.Append("</table></td><td style='vertical-align:top;font-weight:bold;font-size:10px;color:black;'>" + lblTime.Text + " <br/> " + lblDate.Text + "</td></tr>");
            //        }
            //        string alignment = string.Empty;
            //        alignment = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
            //        if (alignment.ToLower() == "standard" || alignment.ToLower() == "centered")
            //        {
            //            alignment = "center";
            //        }

            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:16px;font-weight:bold;'>" + lblCompanyName2.Text + "</td></tr>");
            //        }
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:14px;font-weight:bold;'>" + lblReportTitle.Text + "</td></tr>");
            //        }
            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
            //        {
            //            sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:12px;font-weight:bold;'>" + lblSubTitle.Text + "</td></tr>");
            //        }
            //        sbHeader.Append("<tr><td colspan='2' style='height:15px;'>&nbsp;</td></tr>");
            //        sbHeader.Append("<tr><td colspan='2'>");

            //        if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
            //        {
            //            sbExtraFooter.Append("<tr><td colspan='2' style='height:80px;text-align:center;color:Black;font-size:14px;font-weight:bold;'>" + dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() + "</td></tr>");
            //        }
            //    }
            //}

            //string html = hdnDivToExport.Value;

            //html = html.Replace("&gt;", ">");
            //html = html.Replace("&lt;", "<");

            //htmlToExport = sbHeader + "<br /> " + html + "</td></tr>" + sbExtraFooter + "</table></div></body></html>";

            //string excelServerPath = Server.MapPath("ReportFiles/Excel");
            //if (!Directory.Exists(excelServerPath))
            //{
            //    Directory.CreateDirectory(excelServerPath);
            //}
            //int userId = Convert.ToInt32(Session["UserID"].ToString());
            ////commented by Mayuri 20th july, 16
            ////string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".xls";
            //string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + DateTime.Now.ToString("MM-dd-yyyy") + ".xlsx";
            //string filePath = excelServerPath + "\\" + excelFileName;
            //FileStream fStream = new FileStream(filePath, FileMode.Create);
            //BinaryWriter BWriter = new BinaryWriter(fStream);
            //BWriter.Write(htmlToExport);
            //BWriter.Close();
            //fStream.Close();

            Session["FilePath"] = filePath;
        }
        catch
        {

        }
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        GetReportsName();
        if (pubReportId != 0)
        {
            drpReports.SelectedValue = pubReportId.ToString();
        }

        GenerateExcelFile();

        string script = String.Format("window.open('CustomerReportPreview.aspx');");
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
    }

    private void DeletePDFFiles()
    {
        if (Directory.Exists(Server.MapPath("ReportFiles/PDF")))
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/PDF"));
            foreach (string filePath in filePaths)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    //
                }
            }
        }
    }

    private void DeleteExcelFiles()
    {
        string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/Excel"));
        string[] imagesPaths = Directory.GetFiles(Server.MapPath("ReportFiles/Images"));
        foreach (string filePath in filePaths)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                //
            }
        }
    }
    protected void btnSaveReport2_Click(object sender, EventArgs e)
    {
        try
        {
            SaveResizedAndReorderReport();
            GetReportColumnsByRepId();
            if (pubReportId != 0)
            {
                drpReports.SelectedValue = pubReportId.ToString();
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Report updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    //Changed by Yashasvi Jadav
    //private void GetPreviewFields()
    //{
    //    Session["imgPreview"] = imgPreview;
    //    Session["lblPreviewCompName"] = lblPreviewCompanyName;
    //    Session["lblPreviewCompAddress"] = lblPreviewCompAddress;
    //    Session["lblPreviewCompEmail"] = lblPreviewCompEmail;
    //}

    [System.Web.Services.WebMethod]
    public static string GetEquipmentPreviewDetails(int reportId, string FilterColumn, string FilterValues, string ColumnWidth, string SortColumn, string DataSortBy)
    {
        try
        {
            EquipmentReport _a = new EquipmentReport();
            BL_ReportsData objBL_ReportsData = new BL_ReportsData();
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            CustomerReport objCustReport = new CustomerReport();
            DataSet dsCompDetail = new DataSet();
            GetHeaderFooterDetailParam _GetHeaderFooterDetail = new GetHeaderFooterDetailParam();
            GetOwnersParam _GetOwners = new GetOwnersParam();
            objProp_User.DBName = HttpContext.Current.Session["dbname"].ToString();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objCustReport.DBName = HttpContext.Current.Session["dbname"].ToString();
            objCustReport.ConnConfig = HttpContext.Current.Session["config"].ToString();

            _GetHeaderFooterDetail.DBName = HttpContext.Current.Session["dbname"].ToString();
            _GetHeaderFooterDetail.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _GetOwners.DBName = HttpContext.Current.Session["dbname"].ToString();
            _GetOwners.ConnConfig = HttpContext.Current.Session["config"].ToString();

            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);
            string myJsonString = string.Empty;
            int pubReportId = 0;
            if (reportId != 0)
            {
                pubReportId = reportId;
            }
            objCustReport.ReportId = pubReportId;
            _GetHeaderFooterDetail.ReportId = pubReportId;

            string lblDate = string.Empty;
            string lblTime = string.Empty;
            DataSet dsGetHeaderFooterDetail = new DataSet();

            #region Get Preview Header Details
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
            List<HeaderFooterDetailViewModel> _lstHeaderFooterDetail = new List<HeaderFooterDetailViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetHeaderFooterDetail";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetHeaderFooterDetail, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstHeaderFooterDetail = serializer.Deserialize<List<HeaderFooterDetailViewModel>>(_APIResponse.ResponseData);
                dsGetHeaderFooterDetail = CommonMethods.ToDataSet<HeaderFooterDetailViewModel>(_lstHeaderFooterDetail);
            }
            else
            {
                dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);
            }

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "12/31/01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MM/dd/yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 01")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 2001")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMM dd, yyyy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 2001")
                    {
                        lblDate = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        lblDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    }
                }
                else
                {
                    lblDate = "";
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    lblTime = DateTime.Now.ToString("hh:mm tt");
                }
                else
                {
                    lblTime = "";
                }

            }
            else
            {
                lblDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
                lblTime = lblTime = DateTime.Now.ToString("hh:mm tt");
            }
            #endregion

            Dictionary<string, string> _compList = new Dictionary<string, string>();
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                #region Get Columns
                if (!Convert.IsDBNull(dsCompDetail.Tables[0].Rows[0]["Logo"]))
                {
                    byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                    _compList.Add("Image", "data:image/png;base64," + Convert.ToBase64String(compLogo));
                }
                _compList.Add("Name", dsCompDetail.Tables[0].Rows[0]["Name"].ToString());
                _compList.Add("Email", dsCompDetail.Tables[0].Rows[0]["Email"].ToString());
                _compList.Add("Address", dsCompDetail.Tables[0].Rows[0]["Address"].ToString());
                _compList.Add("Date", lblDate);
                _compList.Add("Time", lblTime);
                myJsonString = (new JavaScriptSerializer()).Serialize(_compList);
                #endregion
            }

            string decodedFilterValues = string.IsNullOrEmpty(FilterValues) ? null : HttpUtility.HtmlDecode(FilterValues);
            string[] checkedColumns = string.IsNullOrEmpty(SortColumn) ? null : SortColumn.TrimEnd('^').Split('^');
            string[] selectedFiltersColumns = string.IsNullOrEmpty(FilterColumn) ? null : FilterColumn.TrimEnd('^').Split('^');
            string[] selectedFiltersValues = string.IsNullOrEmpty(decodedFilterValues) ? null : decodedFilterValues.TrimEnd('^').Split('^');

            #region Bind Grid Report
            string query = "SELECT distinct ";
            foreach (var item in checkedColumns)
            {
                query += "[" + item + "]" + ",";
            }

            query = query.Substring(0, query.Length - 1);
            if (selectedFiltersColumns == null)
            {
                query += " FROM vw_EquipReportDetails order by [" + DataSortBy + "]";
            }
            else
            {
                string filters = string.Empty;
                if (selectedFiltersColumns != null)
                {
                    for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                    {
                        // Single selected value
                        if (!selectedFiltersValues[i].Contains("|")) //!selectedFiltersValues[i].Contains("'") && 
                        {
                            filters += "[" + selectedFiltersColumns[i] + "]" + "=" + "'" + selectedFiltersValues[i].Replace("'", "''") + "'" + " AND ";
                        }
                        else
                        {
                            var filterValArr = selectedFiltersValues[i].Split('|');
                            if(filterValArr.Length > 0)
                            {
                                filters += "[" + selectedFiltersColumns[i] + "] in (";
                                foreach (var item in filterValArr)
                                {
                                    filters += "'" + item.Replace("'", "''") + "',";
                                }
                                filters = filters.Trim().TrimEnd(',');
                                filters += ")" + " AND ";
                            }
                            
                            //filters += "[" + selectedFiltersColumns[i] + "] in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";//filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                        }

                        //if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice" && selectedFiltersColumns[i].ToLower() != "equipmentcounts")
                        //{
                        //    if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        //    {
                        //        filters += "[" + selectedFiltersColumns[i] + "]" + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";//filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        //    }
                        //    else
                        //    {
                        //        int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                        //        if (indexOfSingleQuote == 0)
                        //        {
                        //            filters += "[" + selectedFiltersColumns[i] + "] in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";//filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                        //        }
                        //        else
                        //        {
                        //            filters += "[" + selectedFiltersColumns[i] + "] in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";//filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (selectedFiltersValues[i].Contains("and"))
                        //    {
                        //        filters += "[" + selectedFiltersColumns[i] + "]" + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";//filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        //    }
                        //    else
                        //    {
                        //        filters += "[" + selectedFiltersColumns[i] + "]" + selectedFiltersValues[i] + " AND ";//filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        //    }
                        //}
                    }
                }
                // Removing AND_ in the subfix
                filters = filters.Substring(0, filters.Length - 4);
                query += " FROM vw_EquipReportDetails where " + filters + " order by [" + DataSortBy + "]";
            }
            #endregion

            #region Get Grid Data
            StringBuilder html = new StringBuilder();
            DataSet dsGetCustDetails = new DataSet();

            List<CustomerFilterViewModel> _lstCustomerFilter = new List<CustomerFilterViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetOwners";

                _GetOwners.query = query;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetOwners, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstCustomerFilter = serializer.Deserialize<List<CustomerFilterViewModel>>(_APIResponse.ResponseData);
                dsGetCustDetails = CommonMethods.ToDataSet<CustomerFilterViewModel>(_lstCustomerFilter);
            }
            else
            {
                dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);
            }

            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                #region Bind Report Table

                DataSet dsGetColumnWidth = new DataSet();

                string footer = string.Empty;
                //Table start.
                html.Append("<table id='tblResize' border = '0'>");

                //Building the Header row.
                html.Append("<thead><tr>");

                foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                {
                    html.Append("<th class='resize-header'>");
                    //html.Append("<th style='border:13px solid transparent;color:black;font-size:11px;width:150px; border-image: url(images/icons_big/list-bullet2.PNG) " + b + " '>");
                    //html.Append("<th style='border:1;color:black;font-size:11px;'>");
                    html.Append(column.ColumnName);
                    html.Append("</th>");
                }
                html.Append("</tr></thead>");


                //Building the Data rows.
                foreach (DataRow row in dsGetCustDetails.Tables[0].Rows)
                {
                    html.Append("<tr>");
                    foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                    {
                        html.Append("<td style='border:0;padding:10px 20px 3px 10px;color:black;'>");
                        html.Append(row[column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");

                }

                // html.Append("<tr><td>&nbsp;</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td></tr>");

                for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString() + "</td>";
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                    {
                        footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString() + "</td>";
                    }
                    else
                    {
                        footer += "<td style='border:0;padding:10px 20px 3px 10px;color:black;'>&nbsp;</td>";
                    }
                }

                if (footer != "")
                {
                    html.Append("<tr>" + footer + "</tr>");
                }

                //Table end.
                html.Append("</table>");
                html.Append("<div><b>Total Counts: </b>" + dsGetCustDetails.Tables[0].Rows.Count + "</div>");
                #endregion
            }
            #endregion

            _compList.Add("PreviewData", html.ToString());
            myJsonString = (new JavaScriptSerializer()).Serialize(_compList);
            return myJsonString;
        }
        catch (Exception exp)
        {
            //throw exp;
            return null;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        reportType = Request.QueryString["type"];
    }
}