using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class EquipmentShutdownReport : System.Web.UI.Page
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

    EstimateForm objEF = new EstimateForm();
    Lead leadData = new Lead();
    BL_Lead objBL_Lead = new BL_Lead();

    STax staxData = new STax();
    BL_STax objBL_STax = new BL_STax();

    BL_EstimateForm objBL_EF = new BL_EstimateForm();

    EstimateTemplate objET = new EstimateTemplate();
    BL_EstimateTemplate objBL_ET = new BL_EstimateTemplate();

    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();

    User objPropUser = new User();

    Customer objPropCustomer = new Customer();
    BL_Report bL_Report = new BL_Report();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetCompanyDetailsParam _getCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _getSMTPByUserID = new GetSMTPByUserIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport = new GetEquipmentShutdownForReportParam();
    GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport = new GetEquipShutdownActivityForReportParam();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            HighlightSideMenu("cstmMgr", "lnkEquipmentsSMenu", "cstmMgrSub");

            var reportType = Request.QueryString["type"];
            DateTime _now = DateTime.Now;
            DateTime _startDate, _endDate;
            _endDate = _now;
            if (string.IsNullOrEmpty(reportType) || reportType != "1")
            {
                txtStartDate.Visible = false;
                lblStartDate.Visible = false;
                txtStartDate.Text = string.Empty;
                lblEndDate.Text = "Report Date";
                divStartDate.Visible = false;
                lblHeader.Text = "Equipment Shut Down Report";
            }
            else
            {
                txtStartDate.Visible = true;
                lblStartDate.Visible = true;
                divStartDate.Visible = true;
                _startDate = _now.AddDays(-2);
                txtStartDate.Text = _startDate.ToShortDateString();
                lblHeader.Text = "Equipment Shut Down Activity Report";
            }
            txtEndDate.Text = _endDate.ToShortDateString();
            //var _startDate = new DateTime(_now.Year, _now.Month, 1);
            //var _endDate = _startDate.AddMonths(1).AddDays(-1);

            GetReportDataEquipmentShutdown();
        }
    }

    protected void StiWebViewer_EquipmentShutdownReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        
    }

    protected void StiWebViewer_EquipmentShutdownReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Equipments.aspx");
    }

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        return companyDetailsTable;
    }

    private StiReport GetReportDataEquipmentShutdown()
    {
        StiReport report = new StiReport();

        string reportPathStimul = "";
        try
        {
            DataSet companyLogo = new DataSet();

            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentReport_GetCompanyDetails";

                _getCompanyDetails.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyDetails, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetCompanyDetailsViewModel =serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyLogo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            }
            else
            {
                companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            }

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();
            cTable.Rows.Add(cRow);

            var reportType = Request.QueryString["type"];
            
            if (string.IsNullOrEmpty(reportType) || reportType != "1")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/EquipmentShutdown.mrt");
                report.Load(reportPathStimul);
                report.CacheAllData = true;
                //report.Compile();

                var startDate = DateTime.MinValue;
                var endDate = Convert.ToDateTime(txtEndDate.Text);
                report.Dictionary.Variables["startDate"].Value = startDate.ToLongDateString();
                report.Dictionary.Variables["endDate"].Value = endDate.ToLongDateString();
                DataSet ds = new DataSet();
                objProp_Customer.ConnConfig = Session["config"].ToString();
                _GetEquipmentShutdownForReport.ConnConfig = Session["config"].ToString();

                List<GetEquipmentShutdownForReportViewModel> _lstGetEquipmentShutdownForReport = new List<GetEquipmentShutdownForReportViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentReport_GetEquipmentShutdownForReport";

                    _GetEquipmentShutdownForReport.endDate = endDate;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentShutdownForReport, true);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;
                    _lstGetEquipmentShutdownForReport = serializer.Deserialize<List<GetEquipmentShutdownForReportViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquipmentShutdownForReportViewModel>(_lstGetEquipmentShutdownForReport);
                    ds.Tables[0].Columns["Row"].ColumnName = "Row#";
                }
                else
                {
                    ds = objBL_Customer.GetEquipmentShutdownForReport(objProp_Customer, endDate);
                }

                report.RegData("dtEquipShutdown", ds.Tables[0]);
            }
            else
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/EquipmentShutdownActivity.mrt");
                report.Load(reportPathStimul);
                report.CacheAllData = true;
                //report.Compile();

                var startDate = Convert.ToDateTime(txtStartDate.Text);
                var endDate = Convert.ToDateTime(txtEndDate.Text);
                report.Dictionary.Variables["startDate"].Value = startDate.ToLongDateString();
                report.Dictionary.Variables["endDate"].Value = endDate.ToLongDateString();
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                var filtered = Request.QueryString["filtered"];
                var eqId = Request.QueryString["eqId"];
                var isSingleEq = string.IsNullOrEmpty(eqId) ? false : true;
                report.Dictionary.Variables["IsSingleEq"].Value = isSingleEq ? "true" : "false";
                objProp_Customer.ConnConfig = Session["config"].ToString();
                _GetEquipShutdownActivityForReport.ConnConfig = Session["config"].ToString();

                if (isSingleEq)
                {

                    ListGetEquipShutdownActivityForReport _lstGetEquipShutdownActivityForReport = new ListGetEquipShutdownActivityForReport();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "EquipmentAPI/EquipmentReport_GetEquipmentShutdownActivityForReport";

                        _GetEquipShutdownActivityForReport.startDate = startDate;
                        _GetEquipShutdownActivityForReport.endDate = endDate;
                        _GetEquipShutdownActivityForReport.eqId = eqId;
                        _GetEquipShutdownActivityForReport.filtered = false;

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipShutdownActivityForReport, true);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;
                        _lstGetEquipShutdownActivityForReport = serializer.Deserialize<ListGetEquipShutdownActivityForReport>(_APIResponse.ResponseData);

                        ds1 = _lstGetEquipShutdownActivityForReport.lstTable1.ToDataSet();
                        ds2 = _lstGetEquipShutdownActivityForReport.lstTable2.ToDataSet();

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
                        ds = objBL_Customer.GetEquipmentShutdownActivityForReport(objProp_Customer, startDate, endDate, eqId, false);
                    }
                }
                else
                {
                    if (filtered == "1")
                    {
                        var eqIds = (string)Session["ElevFilteredIds"];

                        ListGetEquipShutdownActivityForReport _lstGetEquipShutdownActivityForReport = new ListGetEquipShutdownActivityForReport();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "EquipmentAPI/EquipmentReport_GetEquipmentShutdownActivityForReport";

                            _GetEquipShutdownActivityForReport.startDate = startDate;
                            _GetEquipShutdownActivityForReport.endDate = endDate;
                            _GetEquipShutdownActivityForReport.eqId = eqIds;
                            _GetEquipShutdownActivityForReport.filtered = true;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipShutdownActivityForReport, true);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;
                            _lstGetEquipShutdownActivityForReport = serializer.Deserialize<ListGetEquipShutdownActivityForReport>(_APIResponse.ResponseData);

                            ds1 = _lstGetEquipShutdownActivityForReport.lstTable1.ToDataSet();
                            ds2 = _lstGetEquipShutdownActivityForReport.lstTable2.ToDataSet();

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
                            ds = objBL_Customer.GetEquipmentShutdownActivityForReport(objProp_Customer, startDate, endDate, eqIds, true);
                        }
                    }
                    else
                    {
                        ListGetEquipShutdownActivityForReport _lstGetEquipShutdownActivityForReport = new ListGetEquipShutdownActivityForReport();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "EquipmentAPI/EquipmentReport_GetEquipmentShutdownActivityForReport";

                            _GetEquipShutdownActivityForReport.startDate = startDate;
                            _GetEquipShutdownActivityForReport.endDate = endDate;
                            _GetEquipShutdownActivityForReport.eqId = eqId;
                            _GetEquipShutdownActivityForReport.filtered = false;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipShutdownActivityForReport, true);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;
                            _lstGetEquipShutdownActivityForReport = serializer.Deserialize<ListGetEquipShutdownActivityForReport>(_APIResponse.ResponseData);

                            ds1 = _lstGetEquipShutdownActivityForReport.lstTable1.ToDataSet();
                            ds2 = _lstGetEquipShutdownActivityForReport.lstTable2.ToDataSet();

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
                            ds = objBL_Customer.GetEquipmentShutdownActivityForReport(objProp_Customer, startDate, endDate, eqId, false);
                        }
                    }
                }

                report.RegData("dtEquipShutdown", ds.Tables[0]);
                report.RegData("dtEquipShutdownHistory", ds.Tables[1]);
                 
            }
            report.RegData("CompanyTable", cTable);

            //Set parameter
            report.Dictionary.Variables["username"].Value = Session["Username"].ToString();

            StiWebViewer_EquipmentShutdownReport.Report = report;
            report.Render();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return report;
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetReportDataEquipmentShutdown();
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void rvEqShutdown_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetReportDataEquipmentShutdown();
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
}