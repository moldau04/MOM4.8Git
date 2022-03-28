using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Telerik.Web.UI;
using Stimulsoft.Report.Components;
using Stimulsoft.Base.Drawing;
using System.Drawing;
using Stimulsoft.Report.Dictionary;
using System.Configuration;
using System.Reflection;
using ReportLayer.IncomeStatements;
using Microsoft.ApplicationBlocks.Data;
using Stimulsoft.Report.Components.TextFormats;

public partial class DashboardReport : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();
    BL_Report bL_Report = new BL_Report();

    protected DataSet _dsColumns = new DataSet();
    protected DataTable _dtFinal = new DataTable();

    private string _lbMonth1;
    private string _lbMonth2;
    private string _lbMonth3;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["fDate"]))
            {
                txtSearchDate.Text = Request.QueryString["fDate"].ToString();
                StiWebViewerReport.Visible = true;
            }

            HighlightSideMenu("financeMgr", "lnkCOA", "financeMgrSub");
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void StiWebViewerReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    protected void StiWebViewerReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void lnkLoadReport_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchDate.Text))
        {
            string urlString = "DashboardReport.aspx?fDate=" + txtSearchDate.Text;
            Response.Redirect(urlString, true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please select As of Date!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            txtSearchDate.Focus();
        }
    }

    private List<DashboardColumnRequest> GetReportColumns()
    {
        List<DashboardColumnRequest> objColumns = new List<DashboardColumnRequest>();

        if (!string.IsNullOrEmpty(Request.QueryString["fDate"]))
        {
            var fDate = Convert.ToDateTime(Request.QueryString["fDate"].ToString());

            // Last year
            var lastYearN = fDate.Year - 1;
            DashboardColumnRequest lastYear = new DashboardColumnRequest();
            lastYear.Index = 1;
            lastYear.Field = "LastYear";
            lastYear.Type = "Actual";
            lastYear.Label = $"YE {lastYearN}";
            lastYear.StartDate = new DateTime(lastYearN, 1, 1);
            lastYear.EndDate = new DateTime(lastYearN, 12, 31);
            objColumns.Add(lastYear);

            for (int i = 2; i >= 0; i--)
            {
                //DateTime dt = DateTime.Now; //if you want check current date, use DateTime.Now
                // Is this the last day of the month
                bool isLastDayOfMonth = (fDate.Day == DateTime.DaysInMonth(fDate.Year, fDate.Month));
                var lastDayMonth = fDate.AddMonths(-i);
                if (isLastDayOfMonth == true)
                {
                    lastDayMonth = new DateTime(lastDayMonth.Year, lastDayMonth.Month, DateTime.DaysInMonth(lastDayMonth.Year, lastDayMonth.Month));                   
                }

                //lastDayMonth = new DateTime(lastDayMonth.Year, lastDayMonth.Month, DateTime.DaysInMonth(lastDayMonth.Year, lastDayMonth.Month)) ;
                //DateTime lastDate = new DateTime(fDate.Year, fDate.Month, 1).AddMonths(-i);
                //DateTime P= DateTime.DaysInMonth(lastDayMonth.Year, lastDayMonth.Month);
                DashboardColumnRequest month = new DashboardColumnRequest();
                month.Index = 4 - i;
                month.Field = $"Month{3 - i}";
                month.Type = "Actual";
                month.Label = lastDayMonth.ToString("MMM yyyy");
                month.StartDate = new DateTime(lastDayMonth.Year, lastDayMonth.Month, 1);
                month.EndDate = lastDayMonth;
                objColumns.Add(month);

                _lbMonth1 = month.Field == "Month1" ? month.Label : _lbMonth1;
                _lbMonth2 = month.Field == "Month2" ? month.Label : _lbMonth2;
                _lbMonth3 = month.Field == "Month3" ? month.Label : _lbMonth3;
            }

            // Last month year
            var lastMonthYearDate = fDate.AddYears(-1);
            DashboardColumnRequest lastMonthYear = new DashboardColumnRequest();
            lastMonthYear.Index = 5;
            lastMonthYear.Field = "LastMonthYear";
            lastMonthYear.Type = "Actual";
            lastMonthYear.Label = lastMonthYearDate.ToString("MMM yyyy");
            lastMonthYear.StartDate = new DateTime(lastYearN, lastMonthYearDate.Month, 1);
            lastMonthYear.EndDate = lastMonthYearDate;
            objColumns.Add(lastMonthYear);

            // MTD
            DashboardColumnRequest mtd = new DashboardColumnRequest();
            mtd.Index = 6;
            mtd.Field = "MTD";
            mtd.Type = "Difference";
            mtd.Label = $"MTD {fDate.Year % 100} vs {fDate.Year % 100 - 1}";
            mtd.Column1 = 4;
            mtd.Column2 = 5;
            objColumns.Add(mtd);

            // MTD %
            DashboardColumnRequest mtdp = new DashboardColumnRequest();
            mtdp.Index = 7;
            mtdp.Field = "MTDPercent";
            mtdp.Type = "Variance";
            mtdp.Label = $"MTD %";
            mtdp.Column1 = 4;
            mtdp.Column2 = 5;
            objColumns.Add(mtdp);

            // QTR
            var qrtStartDate = fDate.AddMonths(-2);
            DashboardColumnRequest qtr = new DashboardColumnRequest();
            qtr.Index = 8;
            qtr.Field = "QTR";
            qtr.Type = "Actual";
            qtr.Label = "QTR Total";
            qtr.StartDate = new DateTime(qrtStartDate.Year, qrtStartDate.Month, 1);
            qtr.EndDate = fDate;
            objColumns.Add(qtr);

            // YTD
            DashboardColumnRequest ytd = new DashboardColumnRequest();
            ytd.Index = 9;
            ytd.Field = "YTD";
            ytd.Type = "Actual";
            ytd.Label = $"YTD {fDate.Year}";
            ytd.StartDate = new DateTime(fDate.Year, 1, 1);
            ytd.EndDate = fDate;
            objColumns.Add(ytd);

            // Previous YTD
            DashboardColumnRequest preYTD = new DashboardColumnRequest();
            preYTD.Index = 10;
            preYTD.Field = "PreviousYTD";
            preYTD.Type = "Actual";
            preYTD.Label = $"YTD {fDate.Year - 1}";
            preYTD.StartDate = new DateTime(fDate.Year - 1, 1, 1);
            preYTD.EndDate = fDate.AddYears(-1);
            objColumns.Add(preYTD);

            // Difference YTD
            DashboardColumnRequest diffYTD = new DashboardColumnRequest();
            diffYTD.Index = 11;
            diffYTD.Field = "DiffYTD";
            diffYTD.Type = "Difference";
            diffYTD.Label = $"YTD {fDate.Year % 100} vs {fDate.Year % 100 - 1}";
            diffYTD.Column1 = 9;
            diffYTD.Column2 = 10;
            objColumns.Add(diffYTD);

            // YTD %
            DashboardColumnRequest ytdp = new DashboardColumnRequest();
            ytdp.Index = 12;
            ytdp.Field = "YTDPercent";
            ytdp.Type = "Variance";
            ytdp.Label = $"YTD %";
            ytdp.Column1 = 9;
            ytdp.Column2 = 10;
            objColumns.Add(ytdp);
        }

        return objColumns;
    }

    private StiReport LoadReport()
    {
        try
        {
            var fDate = Convert.ToDateTime(Request.QueryString["fDate"]);
            var connString = Session["config"].ToString();
            objChart.ConnConfig = connString;
            objChart.StartDate = fDate.AddMonths(-3).AddDays(1);
            objChart.EndDate = fDate;

            List<DashboardColumnRequest> objColumns = GetReportColumns();

            _dtFinal = BuildTotalTable(objColumns);

            var ds = bL_Report.GetDashboardReportData(objChart, objColumns);

            BuildCenterTotal(objColumns, ds.Tables[0], 0, string.Empty);

            string reportPathStimul = Server.MapPath("StimulsoftReports/DashboardReport-Gable.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncomeCum"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OperatingIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                {
                    report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["OtherIncomeCum"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["GrossProfit"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["NetProfit"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                    report.Dictionary.DataSources["OperatingIncome"].Columns.Add(string.Format("Column{0}Percent", column.Index), typeof(double));
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count * 2 - 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            // Format pattern
            var percentFormat = new StiPercentageFormatService(1, 1, 0, ".", 2, ",", 3, "%", true, false, " ", StiTextFormatState.NegativeInRed);
            var numberFormat = new StiNumberFormatService(0, 0, ".", 2, ",", 3, true, false, " ", StiTextFormatState.NegativeInRed);

            if (objColumns.Count > 0)
            {

                StiHeaderBand centerTitle = new StiHeaderBand();
                centerTitle.Height = 0.35;
                centerTitle.PrintIfEmpty = true;
                page.Components.Add(centerTitle);

                StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
                centerTitleText.Text.Value = $"Income Statement - Total- All Divisions";
                centerTitleText.HorAlignment = StiTextHorAlignment.Left;
                centerTitleText.VertAlignment = StiVertAlignment.Center;
                centerTitleText.Font = new Font("Arial", 10F, System.Drawing.FontStyle.Bold);
                centerTitleText.WordWrap = true;
                centerTitle.Components.Add(centerTitleText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.35;
                headerBand.Name = "HeaderBand1";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                //Create DataBand item
                StiText acctText = new StiText(new RectangleD(0, 0.1, columnWidth * 2, 0.25));
                acctText.Text.Value = "";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 8F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0.1, columnWidth, 0.25));
                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                    hText.Border.Style = StiPenStyle.None;
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText hTextPercent = new StiText(new RectangleD(pos + columnWidth, 0.1, columnWidth, 0.25));

                        if (column.Field == "QTR")
                        {
                            hTextPercent.Text.Value = $"{column.Label} %";
                        }
                        else
                        {
                            hTextPercent.Text.Value = "% of Sales";
                        }

                        hTextPercent.HorAlignment = StiTextHorAlignment.Right;
                        hTextPercent.VertAlignment = StiVertAlignment.Center;
                        hTextPercent.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                        hTextPercent.Border.Style = StiPenStyle.None;
                        hTextPercent.WordWrap = true;
                        hTextPercent.CanGrow = true;
                        headerBand.Components.Add(hTextPercent);

                        pos = pos + columnWidth * 2;
                    }
                    else
                    {
                        pos = pos + columnWidth;
                    }
                }

                // Revenues
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "Revenues";
                    dataBand.Name = "Revenues";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == 0");
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{Revenues.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{Revenues.Column" + column.Index + "}";
                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{Revenues.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Cost Of Sales
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "CostOfSales";
                    dataBand.Name = "CostOfSales";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == 0");
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{CostOfSales.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{CostOfSales.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Gross Profit
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "GrossProfit";
                    dataBand.Name = "GrossProfit";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == 0");
                    dataBand.Height = 0.4;
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{GrossProfit.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{GrossProfit.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{GrossProfit.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Overhead Expense
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "Expenses";
                    dataBand.Name = "Expenses";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == 0");
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{Expenses.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{Expenses.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Operating Income
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "OperatingIncome";
                    dataBand.Name = "OperatingIncome";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"OperatingIncome.Department == 0");
                    dataBand.Height = 0.4;
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{OperatingIncome.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{OperatingIncome.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{OperatingIncome.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Other Income/Expense
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "OtherIncome";
                    dataBand.Name = "OtherIncome";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"OtherIncome.Department == 0");
                    dataBand.Border.Style = StiPenStyle.None;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{OtherIncome.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{OtherIncome.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }

                // Net Income
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "NetProfit";
                    dataBand.Name = "NetProfit";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == 0");
                    dataBand.Height = 0.3;
                    dataBand.Border.Style = StiPenStyle.None;
                    dataBand.NewPageAfter = true;
                    page.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{NetProfit.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{NetProfit.Column" + column.Index + "}";

                        if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                        {
                            dataText.TextFormat = percentFormat;
                        }
                        else
                        {
                            dataText.TextFormat = numberFormat;
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{NetProfit.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }
                }
            }

            objPropUser.ConnConfig = connString;

            // Get and build fot center
            var centers = bL_Report.GetCenters(objPropUser);
            var department = BuildDepartmentTable(centers.Tables[0], ds.Tables[0]);

            foreach (DataRow center in department.Rows)
            {
                var centerID = Convert.ToInt32(center["ID"]);
                BuildCenterTotal(objColumns, ds.Tables[0], centerID, center["SubDepartment"].ToString());

                if (objColumns.Count > 0)
                {
                    StiHeaderBand centerTitle = new StiHeaderBand();
                    centerTitle.Height = 0.6;
                    centerTitle.PrintIfEmpty = true;
                    page.Components.Add(centerTitle);

                    StiText centerTitleText = new StiText(new RectangleD(0, 0.3, page.Width, 0.25));
                    centerTitleText.Text.Value = $"Income Statement - {center["CentralName"]}";
                    centerTitleText.HorAlignment = StiTextHorAlignment.Left;
                    centerTitleText.VertAlignment = StiVertAlignment.Center;
                    centerTitleText.Font = new Font("Arial", 10F, System.Drawing.FontStyle.Bold);
                    centerTitleText.WordWrap = true;
                    centerTitle.Components.Add(centerTitleText);

                    //Create HeaderBand
                    StiHeaderBand headerBand = new StiHeaderBand();
                    headerBand.Height = 0.35;
                    headerBand.Name = $"HeaderBandCenter{centerID}";
                    headerBand.Border.Style = StiPenStyle.None;
                    headerBand.PrintOnAllPages = false;
                    headerBand.PrintIfEmpty = true;
                    page.Components.Add(headerBand);

                    //Create DataBand item
                    StiText acctText = new StiText(new RectangleD(0, 0.1, columnWidth * 2, 0.25));
                    acctText.Text.Value = "";
                    acctText.HorAlignment = StiTextHorAlignment.Left;
                    acctText.VertAlignment = StiVertAlignment.Center;
                    acctText.Border.Side = StiBorderSides.All;
                    acctText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctText.Border.Style = StiPenStyle.None;
                    acctText.WordWrap = true;
                    headerBand.Components.Add(acctText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        //Create text on header
                        StiText hText = new StiText(new RectangleD(pos, 0.1, columnWidth, 0.25));
                        hText.Text.Value = column.Label;
                        hText.HorAlignment = StiTextHorAlignment.Right;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                        hText.Border.Style = StiPenStyle.None;
                        hText.WordWrap = true;
                        hText.CanGrow = true;
                        headerBand.Components.Add(hText);

                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText hTextPercent = new StiText(new RectangleD(pos + columnWidth, 0.1, columnWidth, 0.25));

                            if (column.Field == "QTR")
                            {
                                hTextPercent.Text.Value = $"{column.Label} %";
                            }
                            else
                            {
                                hTextPercent.Text.Value = "% of Sales";
                            }

                            hTextPercent.HorAlignment = StiTextHorAlignment.Right;
                            hTextPercent.VertAlignment = StiVertAlignment.Center;
                            hTextPercent.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                            hTextPercent.Border.Style = StiPenStyle.None;
                            hTextPercent.WordWrap = true;
                            hTextPercent.CanGrow = true;
                            headerBand.Components.Add(hTextPercent);

                            pos = pos + columnWidth * 2;
                        }
                        else
                        {
                            pos = pos + columnWidth;
                        }
                    }

                    // Revenues
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "Revenues";
                        dataBand.Name = "Revenues";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == {centerID}");
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{Revenues.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{Revenues.Column" + column.Index + "}";
                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{Revenues.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Cost Of Sales
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "CostOfSales";
                        dataBand.Name = "CostOfSales";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == {centerID}");
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{CostOfSales.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{CostOfSales.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Gross Profit
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "GrossProfit";
                        dataBand.Name = "GrossProfit";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == {centerID}");
                        dataBand.Height = 0.4;
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{GrossProfit.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{GrossProfit.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{GrossProfit.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Overhead Expense
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "Expenses";
                        dataBand.Name = "Expenses";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == {centerID}");
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{Expenses.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{Expenses.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Operating Income
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "OperatingIncome";
                        dataBand.Name = "OperatingIncome";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"OperatingIncome.Department == {centerID}");
                        dataBand.Height = 0.4;
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{OperatingIncome.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{OperatingIncome.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{OperatingIncome.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Other Income/Expense
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "OtherIncome";
                        dataBand.Name = "OtherIncome";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"OtherIncome.Department == {centerID}");
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{OtherIncome.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border.Style = StiPenStyle.None;
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{OtherIncome.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border.Style = StiPenStyle.None;
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }

                    // Net Income
                    if (objColumns.Count > 0)
                    {
                        //Create Databand
                        StiDataBand dataBand = new StiDataBand();
                        dataBand.DataSourceName = "NetProfit";
                        dataBand.Name = "NetProfit";
                        dataBand.Filters.Add(new StiFilter());
                        dataBand.Filters[0].Item = StiFilterItem.Expression;
                        dataBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == {centerID}");
                        dataBand.Height = 0.3;
                        dataBand.Border.Style = StiPenStyle.None;
                        page.Components.Add(dataBand);

                        StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                        acctDataText.Text.Value = "{NetProfit.fDesc}";
                        acctDataText.HorAlignment = StiTextHorAlignment.Left;
                        acctDataText.VertAlignment = StiVertAlignment.Center;
                        acctDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                        acctDataText.OnlyText = false;
                        acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        acctDataText.WordWrap = true;
                        acctDataText.CanGrow = true;
                        acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(acctDataText);

                        pos = (columnWidth * 2);

                        foreach (var column in objColumns)
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{NetProfit.Column" + column.Index + "}";

                            if (column.Field == "MTDPercent" || column.Field == "YTDPercent")
                            {
                                dataText.TextFormat = percentFormat;
                            }
                            else
                            {
                                dataText.TextFormat = numberFormat;
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                            {
                                StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                                dataPercentText.Text.Value = "{NetProfit.Column" + column.Index + "Percent}";
                                dataPercentText.TextFormat = percentFormat;
                                dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                                dataPercentText.VertAlignment = StiVertAlignment.Top;
                                dataPercentText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                                dataPercentText.OnlyText = false;
                                dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataPercentText.WordWrap = true;
                                dataPercentText.CanGrow = true;
                                dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataPercentText);

                                pos = pos + columnWidth * 2;
                            }
                            else
                            {
                                pos = pos + columnWidth;
                            }
                        }
                    }
                }
            }

            var netRevenue = BuildNetRevenueTable(objColumns, ds.Tables[0], department);

            // Build Dashboard
            BuildDashboard(report.Pages[1], objColumns, department);

            DataSet dsC = objBL_User.getControl(objPropUser);

            DataView dView = _dtFinal.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();

            dView.RowFilter = "Type = 82";
            DataTable OtherIncomeCum = dView.ToTable();

            dView.RowFilter = "Type = 41";
            DataTable GrossProfit = dView.ToTable();

            dView.RowFilter = "Type = 51";
            DataTable NetProfit = dView.ToTable();

            dView.RowFilter = "Type = 81";
            DataTable OperatingIncome = dView.ToTable();

            report.RegData("CompanyDetails", dsC.Tables[0]);
            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("OtherIncomeCum", OtherIncomeCum);
            report.RegData("GrossProfit", GrossProfit);
            report.RegData("OperatingIncome", OperatingIncome);
            report.RegData("NetProfit", NetProfit);
            report.RegData("NetRevenue", netRevenue);
            report.RegData("InvoicesWeek", ds.Tables[1]);

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = Request.QueryString["fDate"].ToString();
            report.Dictionary.Variables["LabelMonth1"].Value = _lbMonth1;
            report.Dictionary.Variables["LabelMonth2"].Value = _lbMonth2;
            report.Dictionary.Variables["LabelMonth3"].Value = _lbMonth3;
            report.Dictionary.Variables["YearOfDate"].Value = fDate.Year.ToString();
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

    private void BuildDashboard(StiPage page, List<DashboardColumnRequest> objColumns, DataTable centers)
    {
        // Format pattern
        var percentFormat = new StiPercentageFormatService(1, 1, 0, ".", 2, ",", 3, "%", true, false, " ", StiTextFormatState.NegativeInRed);
        var numberFormat = new StiNumberFormatService(0, 0, ".", 2, ",", 3, true, false, " ", StiTextFormatState.NegativeInRed);

        var panel1 = (StiPanel)page.Components["Panel1"];
        double columnWidth = panel1.Width / 12;
        double pos = 0;

        if (objColumns.Count > 0)
        {

            StiColumnHeaderBand centerTitle = new StiColumnHeaderBand();
            centerTitle.Height = 0.35;
            centerTitle.PrintIfEmpty = true;
            panel1.Components.Add(centerTitle);

            StiText centerTitleText = new StiText(new RectangleD(0, 0.1, panel1.Width, 0.25));
            centerTitleText.Text.Value = $"Income Statement";
            centerTitleText.HorAlignment = StiTextHorAlignment.Left;
            centerTitleText.VertAlignment = StiVertAlignment.Center;
            centerTitleText.Font = new Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            centerTitleText.WordWrap = true;
            centerTitle.Components.Add(centerTitleText);

            //Create HeaderBand
            StiColumnHeaderBand headerBand = new StiColumnHeaderBand();
            headerBand.Height = 0.35;
            headerBand.Name = "HeaderBand1";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            panel1.Components.Add(headerBand);

            //Create DataBand item
            StiText acctText = new StiText(new RectangleD(0, 0.1, columnWidth * 2, 0.25));
            acctText.Text.Value = "";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 8F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                    && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0.1, columnWidth, 0.25));
                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                    hText.Border.Style = StiPenStyle.None;
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);


                    StiText hTextPercent = new StiText(new RectangleD(pos + columnWidth, 0.1, columnWidth, 0.25));
                    hTextPercent.Text.Value = "% of Sales";
                    hTextPercent.HorAlignment = StiTextHorAlignment.Right;
                    hTextPercent.VertAlignment = StiVertAlignment.Center;
                    hTextPercent.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                    hTextPercent.Border.Style = StiPenStyle.None;
                    hTextPercent.WordWrap = true;
                    hTextPercent.CanGrow = true;
                    headerBand.Components.Add(hTextPercent);

                    pos = pos + columnWidth * 2;
                }
            }

            // Revenues
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Revenues";
                dataBand.Name = "Revenues";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == 0");
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{Revenues.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{Revenues.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{Revenues.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Cost Of Sales
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "CostOfSales";
                dataBand.Name = "CostOfSales";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == 0");
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{CostOfSales.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{CostOfSales.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Gross Profit
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "GrossProfit";
                dataBand.Name = "GrossProfit";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == 0");
                dataBand.Height = 0.4;
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{GrossProfit.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{GrossProfit.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{GrossProfit.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Overhead Expense
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Expenses";
                dataBand.Name = "Expenses";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == 0");
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{Expenses.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{Expenses.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{Expenses.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Operating Income
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OperatingIncome";
                dataBand.Name = "OperatingIncome";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"OperatingIncome.Department == 0");
                dataBand.Height = 0.4;
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{OperatingIncome.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{OperatingIncome.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{OperatingIncome.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Other Income/Expense
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OtherIncomeCum";
                dataBand.Name = "OtherIncomeCum";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"OtherIncomeCum.Department == 0");
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{OtherIncomeCum.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{OtherIncomeCum.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{OtherIncomeCum.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border.Style = StiPenStyle.None;
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }

            // Net Income
            if (objColumns.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "NetProfit";
                dataBand.Name = "NetProfit";
                dataBand.Filters.Add(new StiFilter());
                dataBand.Filters[0].Item = StiFilterItem.Expression;
                dataBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == 0");
                dataBand.Height = 0.3;
                dataBand.Border.Style = StiPenStyle.None;
                panel1.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctDataText.Text.Value = "{NetProfit.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                acctDataText.OnlyText = false;
                acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        dataText.Text.Value = "{NetProfit.Column" + column.Index + "}";
                        dataText.TextFormat = numberFormat;
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.VertAlignment = StiVertAlignment.Top;
                        dataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                        dataText.OnlyText = false;
                        dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataText.WordWrap = true;
                        dataText.CanGrow = true;
                        dataText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataText);

                        StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                        dataPercentText.Text.Value = "{NetProfit.Column" + column.Index + "Percent}";
                        dataPercentText.TextFormat = percentFormat;
                        dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                        dataPercentText.VertAlignment = StiVertAlignment.Top;
                        dataPercentText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                        dataPercentText.OnlyText = false;
                        dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        dataPercentText.WordWrap = true;
                        dataPercentText.CanGrow = true;
                        dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                        dataBand.Components.Add(dataPercentText);

                        pos = pos + columnWidth * 2;
                    }
                }
            }
        }

        foreach (DataRow center in centers.Rows)
        {
            var centerID = Convert.ToInt32(center["ID"]);

            if (objColumns.Count > 0)
            {
                StiHeaderBand centerTitle = new StiHeaderBand();
                centerTitle.Height = 0.6;
                centerTitle.PrintIfEmpty = true;
                panel1.Components.Add(centerTitle);

                StiText centerTitleText = new StiText(new RectangleD(0, 0.3, panel1.Width, 0.25));
                centerTitleText.Text.Value = $"Income Statement - {center["CentralName"]} Department";
                centerTitleText.HorAlignment = StiTextHorAlignment.Left;
                centerTitleText.VertAlignment = StiVertAlignment.Center;
                centerTitleText.Font = new Font("Arial", 10F, System.Drawing.FontStyle.Bold);
                centerTitleText.WordWrap = true;
                centerTitle.Components.Add(centerTitleText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.35;
                headerBand.Name = $"HeaderBandCenterDash{centerID}";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                panel1.Components.Add(headerBand);

                //Create DataBand item
                StiText acctText = new StiText(new RectangleD(0, 0.1, columnWidth * 2, 0.25));
                acctText.Text.Value = "";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 8F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                        && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                    {
                        //Create text on header
                        StiText hText = new StiText(new RectangleD(pos, 0.1, columnWidth, 0.25));
                        hText.Text.Value = column.Label;
                        hText.HorAlignment = StiTextHorAlignment.Right;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                        hText.Border.Style = StiPenStyle.None;
                        hText.WordWrap = true;
                        hText.CanGrow = true;
                        headerBand.Components.Add(hText);

                        StiText hTextPercent = new StiText(new RectangleD(pos + columnWidth, 0.1, columnWidth, 0.25));
                        hTextPercent.Text.Value = "% of Sales";
                        hTextPercent.HorAlignment = StiTextHorAlignment.Right;
                        hTextPercent.VertAlignment = StiVertAlignment.Center;
                        hTextPercent.Font = new Font("Arial", 8F, FontStyle.Bold | FontStyle.Underline);
                        hTextPercent.Border.Style = StiPenStyle.None;
                        hTextPercent.WordWrap = true;
                        hTextPercent.CanGrow = true;
                        headerBand.Components.Add(hTextPercent);

                        pos = pos + columnWidth * 2;
                    }
                }

                // Revenues
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "Revenues";
                    dataBand.Name = "Revenues";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == {centerID}");
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{Revenues.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{Revenues.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{Revenues.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Cost Of Sales
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "CostOfSales";
                    dataBand.Name = "CostOfSales";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == {centerID}");
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{CostOfSales.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{CostOfSales.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Gross Profit
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "GrossProfit";
                    dataBand.Name = "GrossProfit";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == {centerID}");
                    dataBand.Height = 0.4;
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{GrossProfit.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{GrossProfit.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{GrossProfit.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Overhead Expense
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "Expenses";
                    dataBand.Name = "Expenses";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == {centerID}");
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{Expenses.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{Expenses.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{Expenses.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Operating Income
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "OperatingIncome";
                    dataBand.Name = "OperatingIncome";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"OperatingIncome.Department == {centerID}");
                    dataBand.Height = 0.4;
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{OperatingIncome.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{OperatingIncome.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{OperatingIncome.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Other Income/Expense
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "OtherIncomeCum";
                    dataBand.Name = "OtherIncomeCum";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"OtherIncomeCum.Department == {centerID}");
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{OtherIncomeCum.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border.Style = StiPenStyle.None;
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{OtherIncomeCum.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border.Style = StiPenStyle.None;
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{OtherIncomeCum.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border.Style = StiPenStyle.None;
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }

                // Net Income
                if (objColumns.Count > 0)
                {
                    //Create Databand
                    StiDataBand dataBand = new StiDataBand();
                    dataBand.DataSourceName = "NetProfit";
                    dataBand.Name = "NetProfit";
                    dataBand.Filters.Add(new StiFilter());
                    dataBand.Filters[0].Item = StiFilterItem.Expression;
                    dataBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == {centerID}");
                    dataBand.Height = 0.3;
                    dataBand.Border.Style = StiPenStyle.None;
                    panel1.Components.Add(dataBand);

                    StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctDataText.Text.Value = "{NetProfit.fDesc}";
                    acctDataText.HorAlignment = StiTextHorAlignment.Left;
                    acctDataText.VertAlignment = StiVertAlignment.Center;
                    acctDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                    acctDataText.OnlyText = false;
                    acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    acctDataText.WordWrap = true;
                    acctDataText.CanGrow = true;
                    acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(acctDataText);

                    pos = (columnWidth * 2);

                    foreach (var column in objColumns)
                    {
                        if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "LastMonthYear" && column.Field != "QTR"
                            && column.Field != "PreviousYTD" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
                        {
                            StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            dataText.Text.Value = "{NetProfit.Column" + column.Index + "}";
                            dataText.TextFormat = numberFormat;
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.VertAlignment = StiVertAlignment.Top;
                            dataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                            dataText.OnlyText = false;
                            dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataText.WordWrap = true;
                            dataText.CanGrow = true;
                            dataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataText);

                            StiText dataPercentText = new StiText(new RectangleD(pos + columnWidth, 0, columnWidth, 0.25));
                            dataPercentText.Text.Value = "{NetProfit.Column" + column.Index + "Percent}";
                            dataPercentText.TextFormat = percentFormat;
                            dataPercentText.HorAlignment = StiTextHorAlignment.Right;
                            dataPercentText.VertAlignment = StiVertAlignment.Top;
                            dataPercentText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Double);
                            dataPercentText.OnlyText = false;
                            dataPercentText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            dataPercentText.WordWrap = true;
                            dataPercentText.CanGrow = true;
                            dataPercentText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(dataPercentText);

                            pos = pos + columnWidth * 2;
                        }
                    }
                }
            }
        }
    }

    private DataTable BuildDepartmentTable(DataTable dtCentral, DataTable dt)
    {
        int maxID = Convert.ToInt32(dtCentral.Compute("MAX(ID)", string.Empty));
        DataView dtView = new DataView(dt);
        var subDepartment = dtView.ToTable(true, "SubDepartment");

        dtCentral.Columns.Add("SubDepartment");

        foreach (DataRow dr in subDepartment.Rows)
        {
            if (!string.IsNullOrEmpty(dr["SubDepartment"].ToString()))
            {
                var nr = dtCentral.NewRow();
                nr["ID"] = ++maxID;
                nr["CentralName"] = dr["SubDepartment"];
                nr["SubDepartment"] = dr["SubDepartment"];

                dtCentral.Rows.Add(nr);
            }
        }

        return dtCentral;
    }

    private DataTable BuildColumnDetailsTable()
    {
        DataTable columnDetailsTable = new DataTable();
        columnDetailsTable.Columns.Add("Line");
        columnDetailsTable.Columns.Add("Index");
        columnDetailsTable.Columns.Add("Label");
        columnDetailsTable.Columns.Add("FromDate");
        columnDetailsTable.Columns.Add("ToDate");

        return columnDetailsTable;
    }

    private DataTable BuildNetRevenueTable(List<DashboardColumnRequest> objColumns, DataTable dt, DataTable departments)
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("CentralName");
        dtResult.Columns.Add("LastYear");
        dtResult.Columns.Add("Month1");
        dtResult.Columns.Add("Month2");
        dtResult.Columns.Add("Month3");
        dtResult.Columns.Add("YTD");

        DataTable filteredTable = dt.Copy();
        DataView dView = filteredTable.DefaultView;

        // Revenue
        dView.RowFilter = "Type = 3 AND Department = 0";
        DataTable dtRevenues = dView.ToTable();
        var revenRows = dtRevenues.Copy().Rows.OfType<DataRow>();

        foreach (DataRow dep in departments.Rows)
        {
            // Revenue of Departments 
            if (!string.IsNullOrEmpty(dep["SubDepartment"].ToString()))
            {
                dView.RowFilter = $"Type = 3 AND SubDepartment = '{dep["SubDepartment"]}'";
            }
            else
            {
                dView.RowFilter = $"Type = 3 AND Department = {dep["ID"]}";
            }

            DataTable dtDepRevenues = dView.ToTable();
            var depRevenueRows = dtDepRevenues.Copy().Rows.OfType<DataRow>();

            var dr = dtResult.NewRow();
            dr["CentralName"] = dep["CentralName"];

            foreach (var column in objColumns)
            {
                if (dtResult.Columns.Contains(column.Field))
                {
                    var columnName = string.Format("Column{0}", column.Index);
                    var revenuTotal = revenRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
                    var depRevenuTotal = depRevenueRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));

                    dr[column.Field] = revenuTotal == 0 ? 0 : depRevenuTotal / revenuTotal;
                }
            }

            dtResult.Rows.Add(dr);
        }

        return dtResult;
    }

    private void BuildCenterTotal(List<DashboardColumnRequest> objColumns, DataTable dtAll, int centerID, string subDepartment)
    {
        var filter = dtAll.DefaultView;

        if (!string.IsNullOrEmpty(subDepartment))
        {
            filter.RowFilter = $"SubDepartment = '{subDepartment}'";
        }
        else
        {
            filter.RowFilter = $"Department = {centerID}";
        }

        var dt = filter.ToTable();

        DataTable filteredTable = dt.Copy();
        DataView dView = filteredTable.DefaultView;

        // Revenue
        dView.RowFilter = "Type = 3";
        DataTable dtRevenues = dView.ToTable();
        var revenRows = dtRevenues.Copy().Rows.OfType<DataRow>();

        DataRow drRevenues = _dtFinal.NewRow();
        drRevenues["fDesc"] = "Revenue";
        drRevenues["Type"] = 3;
        drRevenues["Department"] = centerID;

        // Cost Of Sales
        dView.RowFilter = "Type = 4";
        DataTable dtCostOfSales = dView.ToTable();
        var costRows = dtCostOfSales.Copy().Rows.OfType<DataRow>();

        dView.RowFilter = "Type = 4 AND fDesc = 'COGS-Wages'";
        DataTable dtWages = dView.ToTable();
        var wagesRows = dtWages.Copy().Rows.OfType<DataRow>();

        DataRow drWages = _dtFinal.NewRow();
        drWages["fDesc"] = "COGS-Wages";
        drWages["Type"] = 4;
        drWages["Department"] = centerID;

        dView.RowFilter = "Type = 4 AND fDesc = 'COGS-Benefits'";
        DataTable dtBenefits = dView.ToTable();
        var benefitsRows = dtBenefits.Copy().Rows.OfType<DataRow>();

        DataRow drBenefits = _dtFinal.NewRow();
        drBenefits["fDesc"] = "COGS-Benefits";
        drBenefits["Type"] = 4;
        drBenefits["Department"] = centerID;

        dView.RowFilter = "Type = 4 AND fDesc = 'COGS-MAT'";
        DataTable dtMaterials = dView.ToTable();
        var materialsRows = dtMaterials.Copy().Rows.OfType<DataRow>();

        DataRow drMaterials = _dtFinal.NewRow();
        drMaterials["fDesc"] = "COGS-MAT";
        drMaterials["Type"] = 4;
        drMaterials["Department"] = centerID;

        dView.RowFilter = "Type = 4 AND fDesc = 'Other COGS'";
        DataTable dtOther = dView.ToTable();
        var otherRows = dtOther.Copy().Rows.OfType<DataRow>();

        DataRow drOther = _dtFinal.NewRow();
        drOther["fDesc"] = "Other COGS";
        drOther["Type"] = 4;
        drOther["Department"] = centerID;

        // Expenses
        dView.RowFilter = "Type = 5";
        DataTable dtExpenses = dView.ToTable();
        var expenRows = dtExpenses.Copy().Rows.OfType<DataRow>();

        dView.RowFilter = "Type = 5 AND fDesc = 'Overhead Expense'";
        DataTable dtOverheadExpense = dView.ToTable();
        var overheadExpenseRows = dtOverheadExpense.Copy().Rows.OfType<DataRow>();

        DataRow drOverheadExpense = _dtFinal.NewRow();
        drOverheadExpense["fDesc"] = "Overhead Expense";
        drOverheadExpense["Type"] = 5;
        drOverheadExpense["Department"] = centerID;

        dView.RowFilter = "Type = 5 AND fDesc = 'Other Income'";
        DataTable dtOtherIncome = dView.ToTable();
        var otherIncomeRows = dtOtherIncome.Copy().Rows.OfType<DataRow>();

        DataRow drOtherIncome = _dtFinal.NewRow();
        drOtherIncome["fDesc"] = "Other Income";
        drOtherIncome["Type"] = 8;
        drOtherIncome["Department"] = centerID;

        dView.RowFilter = "Type = 5 AND fDesc = 'Other Expense'";
        DataTable dtOtherExpense = dView.ToTable();
        var otherExpenseRows = dtOtherExpense.Copy().Rows.OfType<DataRow>();

        DataRow drOtherExpense = _dtFinal.NewRow();
        drOtherExpense["fDesc"] = "Other Expense";
        drOtherExpense["Type"] = 8;
        drOtherExpense["Department"] = centerID;

        dView.RowFilter = "Type = 5 AND fDesc <> 'Overhead Expense'";
        DataTable dtOtherIncomeCum = dView.ToTable();
        var otherIncomeCumRows = dtOtherIncomeCum.Copy().Rows.OfType<DataRow>();

        DataRow drOtherIncomeCum = _dtFinal.NewRow();
        drOtherIncomeCum["fDesc"] = "Other Income/Expenses";
        drOtherIncomeCum["Type"] = 82;
        drOtherIncomeCum["Department"] = centerID;

        // Gross Profit
        DataRow drGrossProfit = _dtFinal.NewRow();
        drGrossProfit["fDesc"] = "Gross Profit";
        drGrossProfit["Type"] = 41;
        drGrossProfit["Department"] = centerID;

        // Net Income
        DataRow drNetProfit = _dtFinal.NewRow();
        drNetProfit["fDesc"] = "Net Income";
        drNetProfit["Type"] = 51;
        drNetProfit["Department"] = centerID;

        // Operating Income
        DataRow drOperatingIncome = _dtFinal.NewRow();
        drOperatingIncome["fDesc"] = "Operating Income";
        drOperatingIncome["Type"] = 81;
        drOperatingIncome["Department"] = centerID;

        foreach (var column in objColumns)
        {
            var columnName = string.Format("Column{0}", column.Index);

            var revenuTotal = revenRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var costTotal = costRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var wagesTotal = wagesRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var benefitsTotal = benefitsRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var materialsTotal = materialsRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var otherTotal = otherRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var expenTotal = expenRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var overheadExpenseTotal = overheadExpenseRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var otherIncomeCumTotal = otherIncomeCumRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var otherIncomeTotal = otherIncomeRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
            var otherExpenseTotal = otherExpenseRows.Sum(r => Convert.ToDouble(r[columnName].ToString()));

            if (column.Type != "Variance")
            {
                drRevenues[columnName] = revenuTotal;
                drWages[columnName] = wagesTotal;
                drBenefits[columnName] = benefitsTotal;
                drMaterials[columnName] = materialsTotal;
                drOther[columnName] = otherTotal;
                drOverheadExpense[columnName] = overheadExpenseTotal;
                drOtherIncomeCum[columnName] = otherIncomeCumTotal;
                drOtherIncome[columnName] = otherIncomeTotal;
                drOtherExpense[columnName] = otherExpenseTotal;
                drGrossProfit[columnName] = revenuTotal + costTotal;
                drNetProfit[columnName] = revenuTotal + costTotal + expenTotal;
                drOperatingIncome[columnName] = revenuTotal + costTotal + overheadExpenseTotal;
            }
            else
            {
                var col1Name = string.Format("Column{0}", column.Column1.Value);
                var col2Name = string.Format("Column{0}", column.Column2.Value);

                drRevenues[columnName] = Convert.ToDouble(drRevenues[col2Name]) == 0 ? 0 : (Convert.ToDouble(drRevenues[col1Name]) - Convert.ToDouble(drRevenues[col2Name])) / Convert.ToDouble(drRevenues[col2Name]);
                drWages[columnName] = Convert.ToDouble(drWages[col2Name]) == 0 ? 0 : (Convert.ToDouble(drWages[col1Name]) - Convert.ToDouble(drWages[col2Name])) / Convert.ToDouble(drWages[col2Name]);
                drBenefits[columnName] = Convert.ToDouble(drBenefits[col2Name]) == 0 ? 0 : (Convert.ToDouble(drBenefits[col1Name]) - Convert.ToDouble(drBenefits[col2Name])) / Convert.ToDouble(drBenefits[col2Name]);
                drMaterials[columnName] = Convert.ToDouble(drMaterials[col2Name]) == 0 ? 0 : (Convert.ToDouble(drMaterials[col1Name]) - Convert.ToDouble(drMaterials[col2Name])) / Convert.ToDouble(drMaterials[col2Name]);
                drOther[columnName] = Convert.ToDouble(drOther[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOther[col1Name]) - Convert.ToDouble(drOther[col2Name])) / Convert.ToDouble(drOther[col2Name]);
                drOverheadExpense[columnName] = Convert.ToDouble(drOverheadExpense[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOverheadExpense[col1Name]) - Convert.ToDouble(drOverheadExpense[col2Name])) / Convert.ToDouble(drOverheadExpense[col2Name]);
                drOtherIncomeCum[columnName] = Convert.ToDouble(drOtherIncomeCum[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOtherIncomeCum[col1Name]) - Convert.ToDouble(drOtherIncomeCum[col2Name])) / Convert.ToDouble(drOtherIncomeCum[col2Name]);
                drOtherIncome[columnName] = Convert.ToDouble(drOtherIncome[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOtherIncome[col1Name]) - Convert.ToDouble(drOtherIncome[col2Name])) / Convert.ToDouble(drOtherIncome[col2Name]);
                drOtherExpense[columnName] = Convert.ToDouble(drOtherExpense[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOtherExpense[col1Name]) - Convert.ToDouble(drOtherExpense[col2Name])) / Convert.ToDouble(drOtherExpense[col2Name]);
                drGrossProfit[columnName] = Convert.ToDouble(drGrossProfit[col2Name]) == 0 ? 0 : (Convert.ToDouble(drGrossProfit[col1Name]) - Convert.ToDouble(drGrossProfit[col2Name])) / Convert.ToDouble(drGrossProfit[col2Name]);
                drNetProfit[columnName] = Convert.ToDouble(drNetProfit[col2Name]) == 0 ? 0 : (Convert.ToDouble(drNetProfit[col1Name]) - Convert.ToDouble(drNetProfit[col2Name])) / Convert.ToDouble(drNetProfit[col2Name]);
                drOperatingIncome[columnName] = Convert.ToDouble(drOperatingIncome[col2Name]) == 0 ? 0 : (Convert.ToDouble(drOperatingIncome[col1Name]) - Convert.ToDouble(drOperatingIncome[col2Name])) / Convert.ToDouble(drOperatingIncome[col2Name]);
            }

            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
            {
                var columnPercentName = string.Format("Column{0}Percent", column.Index);

                drRevenues[columnPercentName] = 1;
                drWages[columnPercentName] = revenuTotal == 0 ? 0 : wagesTotal / revenuTotal;
                drBenefits[columnPercentName] = revenuTotal == 0 ? 0 : benefitsTotal / revenuTotal;
                drMaterials[columnPercentName] = revenuTotal == 0 ? 0 : materialsTotal / revenuTotal;
                drOther[columnPercentName] = revenuTotal == 0 ? 0 : otherTotal / revenuTotal;
                drOverheadExpense[columnPercentName] = revenuTotal == 0 ? 0 : overheadExpenseTotal / revenuTotal;
                drOtherIncomeCum[columnPercentName] = revenuTotal == 0 ? 0 : otherIncomeCumTotal / revenuTotal;
                drOtherIncome[columnPercentName] = revenuTotal == 0 ? 0 : otherIncomeTotal / revenuTotal;
                drOtherExpense[columnPercentName] = revenuTotal == 0 ? 0 : otherExpenseTotal / revenuTotal;
                drGrossProfit[columnPercentName] = revenuTotal == 0 ? 0 : (revenuTotal + costTotal) / revenuTotal;
                drNetProfit[columnPercentName] = revenuTotal == 0 ? 0 : (revenuTotal + costTotal + expenTotal) / revenuTotal;
                drOperatingIncome[columnPercentName] = revenuTotal == 0 ? 0 : (revenuTotal + costTotal + overheadExpenseTotal) / revenuTotal;
            }
        }

        _dtFinal.Rows.Add(drRevenues);
        _dtFinal.Rows.Add(drWages);
        _dtFinal.Rows.Add(drBenefits);
        _dtFinal.Rows.Add(drMaterials);
        _dtFinal.Rows.Add(drOther);
        _dtFinal.Rows.Add(drOverheadExpense);
        _dtFinal.Rows.Add(drOtherIncomeCum);
        _dtFinal.Rows.Add(drOtherIncome);
        _dtFinal.Rows.Add(drOtherExpense);

        _dtFinal.Rows.Add(drGrossProfit);
        _dtFinal.Rows.Add(drOperatingIncome);
        _dtFinal.Rows.Add(drNetProfit);
    }

    private DataTable BuildTotalTable(List<DashboardColumnRequest> objColumns)
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("fDesc");
        dtResult.Columns.Add("Type");
        dtResult.Columns.Add("Department");

        foreach (var column in objColumns)
        {
            dtResult.Columns.Add(string.Format("Column{0}", column.Index));

            if (column.Field != "MTD" && column.Field != "MTDPercent" && column.Field != "DiffYTD" && column.Field != "YTDPercent")
            {
                dtResult.Columns.Add(string.Format("Column{0}Percent", column.Index));
            }
        }

        return dtResult;
    }

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        int itemsChecked = radComboBox.CheckedItems.Count;
        String[] itemsArray = new String[itemsChecked];

        int i = 0;
        var collection = radComboBox.CheckedItems;
        foreach (var item in collection)
        {

            String value = item.Value;
            itemsArray[i] = value;
            i++;
        }

        var items = String.Join(",", itemsArray);

        return items;
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
