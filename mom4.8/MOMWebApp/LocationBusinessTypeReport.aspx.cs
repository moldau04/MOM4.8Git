using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Telerik.Web.UI;

public partial class LocationBusinessTypeReport : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    Loc objLoc = new Loc();
    BL_Report objBL_Report = new BL_Report();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();

    Chart objChart = new Chart();
    MapData objPropMapData = new MapData();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _GetSMTPByUserID = new GetSMTPByUserIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetTerritoryParam _GetTerritory = new GetTerritoryParam();
    GetLocationByBusinessTypeParam _GetLocationByBusinessType = new GetLocationByBusinessTypeParam();
    GetBTParam _GetBT = new GetBTParam();
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
                HighlightSideMenu("cstmMgr", "lnkLocationsSMenu", "cstmMgrSub");

                Session["BusinessTypeLabel"] = GetBusinessTypeLabel();
                pageTitle.InnerText = $"Location by {Session["BusinessTypeLabel"].ToString()} Report";
                pageEmailTitle.InnerText = $"Location by {Session["BusinessTypeLabel"].ToString()} Report";

                GetSMTPUser();
                SetAddress();
                string FileName = $"LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.pdf";
                ArrayList lstPath = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lstPath = (ArrayList)ViewState["pathmailatt"];
                    lstPath.Add(FileName);
                }
                else
                {
                    lstPath.Add(FileName);
                }

                ViewState["pathmailatt"] = lstPath;
                dlAttachmentsDelete.DataSource = lstPath;
                dlAttachmentsDelete.DataBind();

                hdnFirstAttachement.Value = FileName;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerLocationBusiness_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadLocationBusinessTypeReport();
    }

    protected void StiWebViewerLocationBusiness_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("BusinessTypeLabel");

        if (!string.IsNullOrEmpty(Request["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        byte[] buffer1 = null;
        StiReport report = new StiReport();
        report = LoadLocationBusinessTypeReport();
        var settings = new Stimulsoft.Report.Export.StiExcelExportSettings();
        var service = new Stimulsoft.Report.Export.StiExcelExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(report, stream, settings);
        buffer1 = stream.ToArray();

        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", $"attachment;filename=LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.xls");
        Response.ContentType = "application/xls";
        Response.AddHeader("Content-Length", (buffer1.Length).ToString());
        Response.BinaryWrite(buffer1);
        Response.Flush();
        Response.Close();
    }

    private StiReport LoadLocationBusinessTypeReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/LocationBusinessTypeReport.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            _GetTerritory.ConnConfig = connString;

            DataSet dsTerr = new DataSet();
            List<GetTerritoryViewModel> _lstGetTerritory = new List<GetTerritoryViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationsList_GetTerritory";

                _GetTerritory.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTerritory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetTerritory = serializer.Deserialize<List<GetTerritoryViewModel>>(_APIResponse.ResponseData);
                dsTerr = CommonMethods.ToDataSet<GetTerritoryViewModel>(_lstGetTerritory);
            }
            else
            {
                dsTerr = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());
            }

            foreach (DataRow dr in dsTerr.Tables[0].Rows){
                report.Dictionary.DataSources["LocationCount"].Columns.Add(string.Format("sp{0}", dr["ID"]), typeof(int));
                report.Dictionary.DataSources["LocationCount"].Columns.Add(string.Format("pc{0}", dr["ID"]), typeof(double));

                report.Dictionary.DataSources["ElevCount"].Columns.Add(string.Format("sp{0}", dr["ID"]), typeof(int));
                report.Dictionary.DataSources["ElevCount"].Columns.Add(string.Format("pc{0}", dr["ID"]), typeof(double));
            }

            //Get data
            DataSet dsC = new DataSet();

            DataSet companyInfo = new DataSet();

            List<GetCompanyDetailsViewModel> _lstGetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationReport_GetCompanyDetails";

                _GetCompanyDetails.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_lstGetCompanyDetailsViewModel);
            }
            else
            {
                companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());
            }

            objPropMapData.ConnConfig = connString;
            _GetLocationByBusinessType.ConnConfig = connString;
            // Search text
            if (!string.IsNullOrEmpty(Request["stype"]) && !string.IsNullOrEmpty(Request["stext"]))
            {
                objPropMapData.SearchBy = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                objPropMapData.SearchValue = HttpUtility.UrlDecode(Request.QueryString["stext"]);

                _GetLocationByBusinessType.SearchBy = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                _GetLocationByBusinessType.SearchValue = HttpUtility.UrlDecode(Request.QueryString["stext"]);
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            if (Session["Location_Filters"] != null)
            {
                ///Get radgrid filter value from Location list View
                filters = (List<RetainFilter>)Session["Location_Filters"];
            }

            bool inclInactive = false;
            if (!string.IsNullOrEmpty(Request["inclInactive"]))
            {
                inclInactive = Convert.ToBoolean(Request.QueryString["inclInactive"]);
            }

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();
            ListGetLocationByBusinessType _lstGetLocationByBusinessType = new ListGetLocationByBusinessType();
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "LocationsAPI/LocationReport_GetLocationByBusinessType";

                _GetLocationByBusinessType.filters = filters;
                _GetLocationByBusinessType.includeInactive = inclInactive;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByBusinessType, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationByBusinessType = serializer.Deserialize<ListGetLocationByBusinessType>(_APIResponse.ResponseData);

                ds1= _lstGetLocationByBusinessType.lstTable.ToDataSet();
                ds2 = _lstGetLocationByBusinessType.lstTable1.ToDataSet();
                ds3 = _lstGetLocationByBusinessType.lstTable2.ToDataSet();
                ds4 = _lstGetLocationByBusinessType.lstTable3.ToDataSet();
                ds5 = _lstGetLocationByBusinessType.lstTable4.ToDataSet();

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
                ds = objBL_Report.GetLocationByBusinessType(objPropMapData, filters, inclInactive);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    var dtLocCount = LocationCountProcessing(ds.Tables[1], ds.Tables[2], report);
                    report.RegData("LocationCount", dtLocCount);
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    var dtElevCount = ElevCountProcessing(ds.Tables[3], ds.Tables[4], report);
                    report.RegData("ElevCount", dtElevCount);
                }
            }

            var businessTypeLabel = GetBusinessTypeLabel();

            var chart1 = report.GetComponentByName("ChartLocation") as StiChart;
            chart1.Title.Text = $"Location by {businessTypeLabel}";

            var chart2 = report.GetComponentByName("ChartElev") as StiChart;
            chart2.Title.Text = $"Equipment by {businessTypeLabel}";

            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["BusinessTypeLabel"].Value = businessTypeLabel;
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataTable LocationCountProcessing(DataTable dt, DataTable dtDetail, StiReport report)
    {
        var dtTerr = dtDetail.DefaultView.ToTable(true, "Terr", "Salesperson");

        var sumLocCount = Convert.ToInt32(dt.Compute("Sum(LocCount)", string.Empty));

        dt.Columns.Add("LocPercen");
        foreach (DataRow dr in dt.Rows)
        {
            dr["LocPercen"] = sumLocCount > 0 ? (double)Convert.ToInt32(dr["LocCount"]) / sumLocCount : 0;
        }

        foreach (DataRow terr in dtTerr.Rows)
        {
            dt.Columns.Add(string.Format("sp{0}", terr["Terr"].ToString()));
            dt.Columns.Add(string.Format("pc{0}", terr["Terr"].ToString()));
        }

        foreach (DataRow dr in dtDetail.Rows)
        {
            var drType = dt.Rows.OfType<DataRow>().FirstOrDefault(x => x["BusinessType"].ToString() == dr["BusinessType"].ToString());

            if (drType != null)
            {
                drType[string.Format("sp{0}", dr["Terr"].ToString())] = dr["LocCount"];
                drType[string.Format("pc{0}", dr["Terr"].ToString())] = sumLocCount > 0 ? (double)Convert.ToInt32(dr["LocCount"]) / sumLocCount : 0;
            }
        }

        // Component Initialization
        LocationComponentInitialization(report, dtTerr);

        return dt;
    }

    private void LocationComponentInitialization(StiReport report, DataTable dtTerr)
    {
        var headerBand = report.GetComponentByName("LoactionHeaderBand") as StiHeaderBand;
        var dataBand = report.GetComponentByName("LoactionDataBand") as StiDataBand;
        var footerBand = report.GetComponentByName("LocationFooterBand") as StiFooterBand;

        StiPage page = report.Pages[0];
        var widthUnit = Math.Round(page.Width / (dtTerr.Rows.Count * 2 + 5), 2);

        // Add Business Type header text
        StiText typeHeaderText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.5));
        typeHeaderText.Text.Value = GetBusinessTypeLabel();
        typeHeaderText.HorAlignment = StiTextHorAlignment.Center;
        typeHeaderText.VertAlignment = StiVertAlignment.Center;
        typeHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        typeHeaderText.Border.Side = StiBorderSides.All;
        typeHeaderText.Border.Style = StiPenStyle.Solid;
        typeHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        typeHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
        typeHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        typeHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        typeHeaderText.WordWrap = true;
        typeHeaderText.CanGrow = true;
        typeHeaderText.GrowToHeight = true;
        headerBand.Components.Add(typeHeaderText);

        // Add Business Type data
        StiText typeDataText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.25));
        typeDataText.Text.Value = "{LocationCount.BusinessType}";
        typeDataText.HorAlignment = StiTextHorAlignment.Left;
        typeDataText.VertAlignment = StiVertAlignment.Center;
        typeDataText.OnlyText = false;
        typeDataText.Border.Side = StiBorderSides.All;
        typeDataText.Border.Style = StiPenStyle.Solid;
        typeDataText.Border.Color = Color.FromArgb(165, 165, 165);
        typeDataText.Font = new Font("Arial", 8F);
        typeDataText.WordWrap = true;
        typeDataText.CanGrow = true;
        typeDataText.Margins = new StiMargins(2, 2, 2, 2);
        dataBand.Components.Add(typeDataText);

        // Add Grand total text
        StiText grandTotalText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.25));
        grandTotalText.Text.Value = "Grand Total";
        grandTotalText.HorAlignment = StiTextHorAlignment.Left;
        grandTotalText.VertAlignment = StiVertAlignment.Center;
        grandTotalText.Border.Side = StiBorderSides.All;
        grandTotalText.Border.Style = StiPenStyle.Solid;
        grandTotalText.Border.Color = Color.FromArgb(165, 165, 165);
        grandTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
        grandTotalText.TextBrush = new StiSolidBrush(Color.Black);
        grandTotalText.Margins = new StiMargins(2, 2, 2, 2);
        grandTotalText.WordWrap = true;
        footerBand.Components.Add(grandTotalText);

        // Add Sum of location header text
        StiText sumHeaderText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit * 2, 0.25));
        sumHeaderText.Text.Value = "Sum of location";
        sumHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumHeaderText.VertAlignment = StiVertAlignment.Top;
        sumHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumHeaderText.Border.Side = StiBorderSides.All;
        sumHeaderText.Border.Style = StiPenStyle.Solid;
        sumHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
        sumHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumHeaderText.WordWrap = true;
        sumHeaderText.CanGrow = true;
        sumHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumHeaderText);

        StiText sumCountHeaderText = new StiText(new RectangleD(widthUnit * 3, 0.25, widthUnit, 0.25));
        sumCountHeaderText.Text.Value = "Count";
        sumCountHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumCountHeaderText.VertAlignment = StiVertAlignment.Center;
        sumCountHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumCountHeaderText.Border.Side = StiBorderSides.All;
        sumCountHeaderText.Border.Style = StiPenStyle.Solid;
        sumCountHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
        sumCountHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountHeaderText.WordWrap = true;
        sumCountHeaderText.CanGrow = true;
        sumCountHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumCountHeaderText);

        StiText sumPercenHeaderText = new StiText(new RectangleD(widthUnit * 4, 0.25, widthUnit, 0.25));
        sumPercenHeaderText.Text.Value = "%";
        sumPercenHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumPercenHeaderText.VertAlignment = StiVertAlignment.Center;
        sumPercenHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumPercenHeaderText.Border.Side = StiBorderSides.All;
        sumPercenHeaderText.Border.Style = StiPenStyle.Solid;
        sumPercenHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
        sumPercenHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenHeaderText.WordWrap = true;
        sumPercenHeaderText.CanGrow = true;
        sumPercenHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumPercenHeaderText);

        // Add Sum of location data text
        StiText sumCountDataText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit, 0.25));
        sumCountDataText.Text.Value = "{LocationCount.LocCount}";
        sumCountDataText.HorAlignment = StiTextHorAlignment.Right;
        sumCountDataText.VertAlignment = StiVertAlignment.Center;
        sumCountDataText.Border.Side = StiBorderSides.All;
        sumCountDataText.Border.Style = StiPenStyle.Solid;
        sumCountDataText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountDataText.Font = new Font("Arial", 8F);
        sumCountDataText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountDataText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountDataText.WordWrap = true;
        sumCountDataText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
        dataBand.Components.Add(sumCountDataText);

        StiText sumPercenDataText = new StiText(new RectangleD(widthUnit * 4, 0, widthUnit, 0.25));
        sumPercenDataText.Text.Value = "{LocationCount.LocPercen}";
        sumPercenDataText.HorAlignment = StiTextHorAlignment.Right;
        sumPercenDataText.VertAlignment = StiVertAlignment.Center;
        sumPercenDataText.Border.Side = StiBorderSides.All;
        sumPercenDataText.Border.Style = StiPenStyle.Solid;
        sumPercenDataText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenDataText.Font = new Font("Arial", 8F);
        sumPercenDataText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenDataText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenDataText.WordWrap = true;
        sumPercenDataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
        dataBand.Components.Add(sumPercenDataText);

        // Add Sum of location footer text
        StiText sumCountFooterText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit, 0.25));
        sumCountFooterText.Text.Value = "{Sum(LoactionDataBand,LocationCount.LocCount)}";
        sumCountFooterText.HorAlignment = StiTextHorAlignment.Right;
        sumCountFooterText.VertAlignment = StiVertAlignment.Center;
        sumCountFooterText.Border.Side = StiBorderSides.All;
        sumCountFooterText.Border.Style = StiPenStyle.Solid;
        sumCountFooterText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountFooterText.Font = new Font("Arial", 8F);
        sumCountFooterText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountFooterText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountFooterText.WordWrap = true;
        sumCountFooterText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
        footerBand.Components.Add(sumCountFooterText);

        StiText sumPercenFooterText = new StiText(new RectangleD(widthUnit * 4, 0, widthUnit, 0.25));
        sumPercenFooterText.Text.Value = "100.00%";
        sumPercenFooterText.HorAlignment = StiTextHorAlignment.Right;
        sumPercenFooterText.VertAlignment = StiVertAlignment.Center;
        sumPercenFooterText.Border.Side = StiBorderSides.All;
        sumPercenFooterText.Border.Style = StiPenStyle.Solid;
        sumPercenFooterText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenFooterText.Font = new Font("Arial", 8F);
        sumPercenFooterText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenFooterText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenFooterText.WordWrap = true;
        footerBand.Components.Add(sumPercenFooterText);

        var pos = widthUnit * 5;

        foreach (DataRow terr in dtTerr.Rows)
        {
            // Add Sum of location header text by Salesperson
            StiText terrHeaderText = new StiText(new RectangleD(pos, 0, widthUnit * 2, 0.25));
            terrHeaderText.Text.Value = terr["Salesperson"].ToString();
            terrHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrHeaderText.VertAlignment = StiVertAlignment.Top;
            terrHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrHeaderText.Border.Side = StiBorderSides.All;
            terrHeaderText.Border.Style = StiPenStyle.Solid;
            terrHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            terrHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrHeaderText.WordWrap = true;
            terrHeaderText.CanGrow = true;
            terrHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrHeaderText);

            StiText terrCountHeaderText = new StiText(new RectangleD(pos, 0.25, widthUnit, 0.25));
            terrCountHeaderText.Text.Value = "Count";
            terrCountHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrCountHeaderText.VertAlignment = StiVertAlignment.Center;
            terrCountHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrCountHeaderText.Border.Side = StiBorderSides.All;
            terrCountHeaderText.Border.Style = StiPenStyle.Solid;
            terrCountHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
            terrCountHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountHeaderText.WordWrap = true;
            terrCountHeaderText.CanGrow = true;
            terrCountHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrCountHeaderText);

            StiText terrPercenHeaderText = new StiText(new RectangleD(pos + widthUnit, 0.25, widthUnit, 0.25));
            terrPercenHeaderText.Text.Value = "%";
            terrPercenHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrPercenHeaderText.VertAlignment = StiVertAlignment.Center;
            terrPercenHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrPercenHeaderText.Border.Side = StiBorderSides.All;
            terrPercenHeaderText.Border.Style = StiPenStyle.Solid;
            terrPercenHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
            terrPercenHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenHeaderText.WordWrap = true;
            terrPercenHeaderText.CanGrow = true;
            terrPercenHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrPercenHeaderText);

            // Add Sum of location data text by Salesperson
            StiText terrCountDataText = new StiText(new RectangleD(pos, 0, widthUnit, 0.25));
            terrCountDataText.Text.Value = "{LocationCount." + $"sp{terr["Terr"].ToString()}" + "}";
            terrCountDataText.HorAlignment = StiTextHorAlignment.Right;
            terrCountDataText.VertAlignment = StiVertAlignment.Center;
            terrCountDataText.Border.Side = StiBorderSides.All;
            terrCountDataText.Border.Style = StiPenStyle.Solid;
            terrCountDataText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountDataText.Font = new Font("Arial", 8F);
            terrCountDataText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountDataText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountDataText.WordWrap = true;
            terrCountDataText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
            dataBand.Components.Add(terrCountDataText);

            StiText terrPercenDataText = new StiText(new RectangleD(pos + widthUnit, 0, widthUnit, 0.25));
            terrPercenDataText.Text.Value = "{LocationCount." + $"pc{terr["Terr"].ToString()}" + "}";
            terrPercenDataText.HorAlignment = StiTextHorAlignment.Right;
            terrPercenDataText.VertAlignment = StiVertAlignment.Center;
            terrPercenDataText.Border.Side = StiBorderSides.All;
            terrPercenDataText.Border.Style = StiPenStyle.Solid;
            terrPercenDataText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenDataText.Font = new Font("Arial", 8F);
            terrPercenDataText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenDataText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenDataText.WordWrap = true;
            terrPercenDataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
            dataBand.Components.Add(terrPercenDataText);

            // Add footer
            StiText terrCountFooterText = new StiText(new RectangleD(pos, 0, widthUnit, 0.25));
            terrCountFooterText.Text.Value = "{Sum(LoactionDataBand,LocationCount." + $"sp{terr["Terr"].ToString()}" + ")}";
            terrCountFooterText.HorAlignment = StiTextHorAlignment.Right;
            terrCountFooterText.VertAlignment = StiVertAlignment.Center;
            terrCountFooterText.Border.Side = StiBorderSides.All;
            terrCountFooterText.Border.Style = StiPenStyle.Solid;
            terrCountFooterText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountFooterText.Font = new Font("Arial", 8F);
            terrCountFooterText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountFooterText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountFooterText.WordWrap = true;
            terrCountFooterText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
            footerBand.Components.Add(terrCountFooterText);

            StiText terrPercenFooterText = new StiText(new RectangleD(pos + widthUnit, 0, widthUnit, 0.25));
            terrPercenFooterText.Text.Value = "{Sum(LoactionDataBand,LocationCount." + $"pc{terr["Terr"].ToString()}" + ")}";
            terrPercenFooterText.HorAlignment = StiTextHorAlignment.Right;
            terrPercenFooterText.VertAlignment = StiVertAlignment.Center;
            terrPercenFooterText.Border.Side = StiBorderSides.All;
            terrPercenFooterText.Border.Style = StiPenStyle.Solid;
            terrPercenFooterText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenFooterText.Font = new Font("Arial", 8F);
            terrPercenFooterText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenFooterText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenFooterText.WordWrap = true;
            terrPercenFooterText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
            footerBand.Components.Add(terrPercenFooterText);

            pos += widthUnit * 2;
        }
    }

    private DataTable ElevCountProcessing(DataTable dt, DataTable dtDetail, StiReport report)
    {
        var dtTerr = dtDetail.DefaultView.ToTable(true, "Terr", "Salesperson");

        var sumElevCount = Convert.ToInt32(dt.Compute("Sum(ElevCount)", string.Empty));

        dt.Columns.Add("ElevPercen");
        foreach (DataRow dr in dt.Rows)
        {
            dr["ElevPercen"] = sumElevCount > 0 ? (double)Convert.ToInt32(dr["ElevCount"]) / sumElevCount : 0;
        }

        foreach (DataRow terr in dtTerr.Rows)
        {
            dt.Columns.Add(string.Format("sp{0}", terr["Terr"].ToString()));
            dt.Columns.Add(string.Format("pc{0}", terr["Terr"].ToString()));
        }

        foreach (DataRow dr in dtDetail.Rows)
        {
            var drType = dt.Rows.OfType<DataRow>().FirstOrDefault(x => x["BusinessType"].ToString() == dr["BusinessType"].ToString());

            if (drType != null)
            {
                drType[string.Format("sp{0}", dr["Terr"].ToString())] = dr["ElevCount"];
                drType[string.Format("pc{0}", dr["Terr"].ToString())] = sumElevCount > 0 ? (double)Convert.ToInt32(dr["ElevCount"]) / sumElevCount : 0;
            }
        }

        // Component Initialization
        ElevComponentInitialization(report, dtTerr);

        return dt;
    }

    private void ElevComponentInitialization(StiReport report, DataTable dtTerr)
    {
        var headerBand = report.GetComponentByName("ElevHeaderBand") as StiHeaderBand;
        var dataBand = report.GetComponentByName("ElevDataBand") as StiDataBand;
        var footerBand = report.GetComponentByName("ElevFooterBand") as StiFooterBand;

        StiPage page = report.Pages[0];
        var widthUnit = Math.Round(page.Width / (dtTerr.Rows.Count * 2 + 5), 2);

        // Add Business Type header text
        StiText typeHeaderText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.5));
        typeHeaderText.Text.Value = GetBusinessTypeLabel();
        typeHeaderText.HorAlignment = StiTextHorAlignment.Center;
        typeHeaderText.VertAlignment = StiVertAlignment.Center;
        typeHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        typeHeaderText.Border.Side = StiBorderSides.All;
        typeHeaderText.Border.Style = StiPenStyle.Solid;
        typeHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        typeHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
        typeHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        typeHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        typeHeaderText.WordWrap = true;
        typeHeaderText.CanGrow = true;
        typeHeaderText.GrowToHeight = true;
        headerBand.Components.Add(typeHeaderText);

        // Add Business Type data
        StiText typeDataText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.25));
        typeDataText.Text.Value = "{ElevCount.BusinessType}";
        typeDataText.HorAlignment = StiTextHorAlignment.Left;
        typeDataText.VertAlignment = StiVertAlignment.Center;
        typeDataText.OnlyText = false;
        typeDataText.Border.Side = StiBorderSides.All;
        typeDataText.Border.Style = StiPenStyle.Solid;
        typeDataText.Border.Color = Color.FromArgb(165, 165, 165);
        typeDataText.Font = new Font("Arial", 8F);
        typeDataText.WordWrap = true;
        typeDataText.CanGrow = true;
        typeDataText.Margins = new StiMargins(2, 2, 2, 2);
        dataBand.Components.Add(typeDataText);

        // Add Grand total text
        StiText grandTotalText = new StiText(new RectangleD(0, 0, widthUnit * 3, 0.25));
        grandTotalText.Text.Value = "Grand Total";
        grandTotalText.HorAlignment = StiTextHorAlignment.Left;
        grandTotalText.VertAlignment = StiVertAlignment.Center;
        grandTotalText.Border.Side = StiBorderSides.All;
        grandTotalText.Border.Style = StiPenStyle.Solid;
        grandTotalText.Border.Color = Color.FromArgb(165, 165, 165);
        grandTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
        grandTotalText.TextBrush = new StiSolidBrush(Color.Black);
        grandTotalText.Margins = new StiMargins(2, 2, 2, 2);
        grandTotalText.WordWrap = true;
        footerBand.Components.Add(grandTotalText);

        // Add Sum of Elev header text
        StiText sumHeaderText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit * 2, 0.25));
        sumHeaderText.Text.Value = "Sum of Equipment";
        sumHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumHeaderText.VertAlignment = StiVertAlignment.Top;
        sumHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumHeaderText.Border.Side = StiBorderSides.All;
        sumHeaderText.Border.Style = StiPenStyle.Solid;
        sumHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
        sumHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumHeaderText.WordWrap = true;
        sumHeaderText.CanGrow = true;
        sumHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumHeaderText);

        StiText sumCountHeaderText = new StiText(new RectangleD(widthUnit * 3, 0.25, widthUnit, 0.25));
        sumCountHeaderText.Text.Value = "Count";
        sumCountHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumCountHeaderText.VertAlignment = StiVertAlignment.Center;
        sumCountHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumCountHeaderText.Border.Side = StiBorderSides.All;
        sumCountHeaderText.Border.Style = StiPenStyle.Solid;
        sumCountHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
        sumCountHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountHeaderText.WordWrap = true;
        sumCountHeaderText.CanGrow = true;
        sumCountHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumCountHeaderText);

        StiText sumPercenHeaderText = new StiText(new RectangleD(widthUnit * 4, 0.25, widthUnit, 0.25));
        sumPercenHeaderText.Text.Value = "%";
        sumPercenHeaderText.HorAlignment = StiTextHorAlignment.Center;
        sumPercenHeaderText.VertAlignment = StiVertAlignment.Center;
        sumPercenHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
        sumPercenHeaderText.Border.Side = StiBorderSides.All;
        sumPercenHeaderText.Border.Style = StiPenStyle.Solid;
        sumPercenHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
        sumPercenHeaderText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenHeaderText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenHeaderText.WordWrap = true;
        sumPercenHeaderText.CanGrow = true;
        sumPercenHeaderText.GrowToHeight = true;
        headerBand.Components.Add(sumPercenHeaderText);

        // Add Sum of Elev data text
        StiText sumCountDataText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit, 0.25));
        sumCountDataText.Text.Value = "{ElevCount.ElevCount}";
        sumCountDataText.HorAlignment = StiTextHorAlignment.Right;
        sumCountDataText.VertAlignment = StiVertAlignment.Center;
        sumCountDataText.Border.Side = StiBorderSides.All;
        sumCountDataText.Border.Style = StiPenStyle.Solid;
        sumCountDataText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountDataText.Font = new Font("Arial", 8F);
        sumCountDataText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountDataText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountDataText.WordWrap = true;
        sumCountDataText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
        dataBand.Components.Add(sumCountDataText);

        StiText sumPercenDataText = new StiText(new RectangleD(widthUnit * 4, 0, widthUnit, 0.25));
        sumPercenDataText.Text.Value = "{ElevCount.ElevPercen}";
        sumPercenDataText.HorAlignment = StiTextHorAlignment.Right;
        sumPercenDataText.VertAlignment = StiVertAlignment.Center;
        sumPercenDataText.Border.Side = StiBorderSides.All;
        sumPercenDataText.Border.Style = StiPenStyle.Solid;
        sumPercenDataText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenDataText.Font = new Font("Arial", 8F);
        sumPercenDataText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenDataText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenDataText.WordWrap = true;
        sumPercenDataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
        dataBand.Components.Add(sumPercenDataText);

        // Add Sum of Elev footer text
        StiText sumCountFooterText = new StiText(new RectangleD(widthUnit * 3, 0, widthUnit, 0.25));
        sumCountFooterText.Text.Value = "{Sum(ElevDataBand,ElevCount.ElevCount)}";
        sumCountFooterText.HorAlignment = StiTextHorAlignment.Right;
        sumCountFooterText.VertAlignment = StiVertAlignment.Center;
        sumCountFooterText.Border.Side = StiBorderSides.All;
        sumCountFooterText.Border.Style = StiPenStyle.Solid;
        sumCountFooterText.Border.Color = Color.FromArgb(165, 165, 165);
        sumCountFooterText.Font = new Font("Arial", 8F);
        sumCountFooterText.TextBrush = new StiSolidBrush(Color.Black);
        sumCountFooterText.Margins = new StiMargins(2, 2, 2, 2);
        sumCountFooterText.WordWrap = true;
        sumCountFooterText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
        footerBand.Components.Add(sumCountFooterText);

        StiText sumPercenFooterText = new StiText(new RectangleD(widthUnit * 4, 0, widthUnit, 0.25));
        sumPercenFooterText.Text.Value = "100.00%";
        sumPercenFooterText.HorAlignment = StiTextHorAlignment.Right;
        sumPercenFooterText.VertAlignment = StiVertAlignment.Center;
        sumPercenFooterText.Border.Side = StiBorderSides.All;
        sumPercenFooterText.Border.Style = StiPenStyle.Solid;
        sumPercenFooterText.Border.Color = Color.FromArgb(165, 165, 165);
        sumPercenFooterText.Font = new Font("Arial", 8F);
        sumPercenFooterText.TextBrush = new StiSolidBrush(Color.Black);
        sumPercenFooterText.Margins = new StiMargins(2, 2, 2, 2);
        sumPercenFooterText.WordWrap = true;
        footerBand.Components.Add(sumPercenFooterText);

        var pos = widthUnit * 5;

        foreach (DataRow terr in dtTerr.Rows)
        {
            // Add Sum of Elev header text by Salesperson
            StiText terrHeaderText = new StiText(new RectangleD(pos, 0, widthUnit * 2, 0.25));
            terrHeaderText.Text.Value = terr["Salesperson"].ToString();
            terrHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrHeaderText.VertAlignment = StiVertAlignment.Top;
            terrHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrHeaderText.Border.Side = StiBorderSides.All;
            terrHeaderText.Border.Style = StiPenStyle.Solid;
            terrHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            terrHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrHeaderText.WordWrap = true;
            terrHeaderText.CanGrow = true;
            terrHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrHeaderText);

            StiText terrCountHeaderText = new StiText(new RectangleD(pos, 0.25, widthUnit, 0.25));
            terrCountHeaderText.Text.Value = "Count";
            terrCountHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrCountHeaderText.VertAlignment = StiVertAlignment.Center;
            terrCountHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrCountHeaderText.Border.Side = StiBorderSides.All;
            terrCountHeaderText.Border.Style = StiPenStyle.Solid;
            terrCountHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
            terrCountHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountHeaderText.WordWrap = true;
            terrCountHeaderText.CanGrow = true;
            terrCountHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrCountHeaderText);

            StiText terrPercenHeaderText = new StiText(new RectangleD(pos + widthUnit, 0.25, widthUnit, 0.25));
            terrPercenHeaderText.Text.Value = "%";
            terrPercenHeaderText.HorAlignment = StiTextHorAlignment.Center;
            terrPercenHeaderText.VertAlignment = StiVertAlignment.Center;
            terrPercenHeaderText.Brush = new StiSolidBrush(Color.FromArgb(156, 195, 229));
            terrPercenHeaderText.Border.Side = StiBorderSides.All;
            terrPercenHeaderText.Border.Style = StiPenStyle.Solid;
            terrPercenHeaderText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
            terrPercenHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenHeaderText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenHeaderText.WordWrap = true;
            terrPercenHeaderText.CanGrow = true;
            terrPercenHeaderText.GrowToHeight = true;
            headerBand.Components.Add(terrPercenHeaderText);

            // Add Sum of Elev data text by Salesperson
            StiText terrCountDataText = new StiText(new RectangleD(pos, 0, widthUnit, 0.25));
            terrCountDataText.Text.Value = "{ElevCount." + $"sp{terr["Terr"].ToString()}" + "}";
            terrCountDataText.HorAlignment = StiTextHorAlignment.Right;
            terrCountDataText.VertAlignment = StiVertAlignment.Center;
            terrCountDataText.Border.Side = StiBorderSides.All;
            terrCountDataText.Border.Style = StiPenStyle.Solid;
            terrCountDataText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountDataText.Font = new Font("Arial", 8F);
            terrCountDataText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountDataText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountDataText.WordWrap = true;
            terrCountDataText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
            dataBand.Components.Add(terrCountDataText);

            StiText terrPercenDataText = new StiText(new RectangleD(pos + widthUnit, 0, widthUnit, 0.25));
            terrPercenDataText.Text.Value = "{ElevCount." + $"pc{terr["Terr"].ToString()}" + "}";
            terrPercenDataText.HorAlignment = StiTextHorAlignment.Right;
            terrPercenDataText.VertAlignment = StiVertAlignment.Center;
            terrPercenDataText.Border.Side = StiBorderSides.All;
            terrPercenDataText.Border.Style = StiPenStyle.Solid;
            terrPercenDataText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenDataText.Font = new Font("Arial", 8F);
            terrPercenDataText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenDataText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenDataText.WordWrap = true;
            terrPercenDataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
            dataBand.Components.Add(terrPercenDataText);

            // Add footer
            StiText terrCountFooterText = new StiText(new RectangleD(pos, 0, widthUnit, 0.25));
            terrCountFooterText.Text.Value = "{Sum(ElevDataBand,ElevCount." + $"sp{terr["Terr"].ToString()}" + ")}";
            terrCountFooterText.HorAlignment = StiTextHorAlignment.Right;
            terrCountFooterText.VertAlignment = StiVertAlignment.Center;
            terrCountFooterText.Border.Side = StiBorderSides.All;
            terrCountFooterText.Border.Style = StiPenStyle.Solid;
            terrCountFooterText.Border.Color = Color.FromArgb(165, 165, 165);
            terrCountFooterText.Font = new Font("Arial", 8F);
            terrCountFooterText.TextBrush = new StiSolidBrush(Color.Black);
            terrCountFooterText.Margins = new StiMargins(2, 2, 2, 2);
            terrCountFooterText.WordWrap = true;
            terrCountFooterText.TextFormat = new StiNumberFormatService(1, ".", 0, ",", 3, true, false, " ");
            footerBand.Components.Add(terrCountFooterText);

            StiText terrPercenFooterText = new StiText(new RectangleD(pos + widthUnit, 0, widthUnit, 0.25));
            terrPercenFooterText.Text.Value = "{Sum(ElevDataBand,ElevCount." + $"pc{terr["Terr"].ToString()}" + ")}";
            terrPercenFooterText.HorAlignment = StiTextHorAlignment.Right;
            terrPercenFooterText.VertAlignment = StiVertAlignment.Center;
            terrPercenFooterText.Border.Side = StiBorderSides.All;
            terrPercenFooterText.Border.Style = StiPenStyle.Solid;
            terrPercenFooterText.Border.Color = Color.FromArgb(165, 165, 165);
            terrPercenFooterText.Font = new Font("Arial", 8F);
            terrPercenFooterText.TextBrush = new StiSolidBrush(Color.Black);
            terrPercenFooterText.Margins = new StiMargins(2, 2, 2, 2);
            terrPercenFooterText.WordWrap = true;
            terrPercenFooterText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
            footerBand.Components.Add(terrPercenFooterText);

            pos += widthUnit * 2;
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
    //    //DataTable dt = (DataTable)HttpContext.Current.Session["DistributionList"];
    //    DataTable dt = WebBaseUtility.GetContactListOnExchangeServer();

    //    List<string> txtItems = new List<string>();
    //    String dbValues;

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["MemberName"].ToString() + "(" + row["MemberEmail"].ToString() + ")", row["MemberEmail"].ToString());
    //        txtItems.Add(dbValues);
    //    }

    //    return txtItems.ToArray();
    //}

    private void FillDistributionList(string searchType, string searchValue)
    {
        DataTable distributionList = new DataTable();
        DataTable distributionList1 = new DataTable();
        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            distributionList1.Columns.Add("MemberEmail");
            distributionList1.Columns.Add("MemberName");
            distributionList1.Columns.Add("GroupName");
            distributionList1.Columns.Add("Type");
            DataRow dr = distributionList1.NewRow();
            dr[0] = txtTo.Text;
            dr[1] = txtTo.Text;
            dr[2] = "";
            dr[3] = "";
            distributionList1.Rows.InsertAt(dr, 0);
        }
        distributionList = WebBaseUtility.GetContactListOnExchangeServer();
        distributionList.Merge(distributionList1);
        IEnumerable<DataRow> rowSources;

        var emailList = distributionList.Clone();
        switch (searchType)
        {
            case "1":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            case "2":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "3":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "4":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            default:
                rowSources = (from myRow in distributionList.AsEnumerable()
                              where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                              select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
        }

        if (rowSources.Any())
        {
            emailList = rowSources.CopyToDataTable();
        }
        else
        {
            emailList = distributionList.Clone();
        }

        lblRecordCount.Text = emailList.Rows.Count + " Record(s) found";
        RadGrid_Emails.DataSource = emailList;
        RadGrid_Emails.VirtualItemCount = emailList.Rows.Count;

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

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
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

        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
    }

    private void GetSMTPUser()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"]);

        _GetSMTPByUserID.ConnConfig = Session["config"].ToString();
        _GetSMTPByUserID.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        List<SMTPEmailViewModel> _lstSMTPEmailViewModel = new List<SMTPEmailViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationReport_GetSMTPByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSMTPByUserID, true);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstSMTPEmailViewModel = serializer.Deserialize<List<SMTPEmailViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<SMTPEmailViewModel>(_lstSMTPEmailViewModel);
        }
        else
        {
            ds = objBL_User.getSMTPByUserID(objPropUser);
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            String emailFrom = "";
            emailFrom = Convert.ToString(ds.Tables[0].Rows[0]["From"]);
            if (emailFrom == "")
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                string user = section.Network.UserName;
                txtFrom.Text = user;
            }
            else
            {
                txtFrom.Text = emailFrom;
            }
            txtEmailBCC.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCCEmail"]);
            //txtFrom.ReadOnly = true;
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                mail.Title = $"Location by {Session["BusinessTypeLabel"].ToString()} Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = $"This is report email sent from Mobile Office Manager. Please find the Location by {Session["BusinessTypeLabel"].ToString()} Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadLocationBusinessTypeReport(), stream, settings);
                buffer1 = stream.ToArray();

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = buffer1;
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (strpath != $"LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = $"LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = FileUpload1.FileName;
        string fullpath = savepath + filename;

        if (File.Exists(fullpath))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
            fullpath = savepath + filename;
        }

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        FileUpload1.SaveAs(fullpath);


        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fullpath);
        }
        else
        {
            lstPath.Add(fullpath);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        txtBody.Focus();
    }

    protected void imgDelAttach_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        if (hdnFirstAttachement.Value == path)
        {
            hdnFirstAttachement.Value = "-1";
        }
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                //return;
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
            if (DownloadFileName == $"LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadLocationBusinessTypeReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment;filename=LocationBy{Session["BusinessTypeLabel"].ToString().Replace(" ", "")}Report.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = $"Please review the attached Location by {Session["BusinessTypeLabel"].ToString()} Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;
        
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

    private string GetBusinessTypeLabel()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        _GetBT.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<GetBTViewModel> _lstGetBT = new List<GetBTViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetBT";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBT, true);

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
}
