using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using System;
using System.Data;
using System.Web.UI;

public partial class MonthlyMaintenanceTicketReport : System.Web.UI.Page
{

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    String CompanyAddress = "";
    String DefaultCategory = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Report Design Button Hide
            if (Convert.ToString(Session["type"]) == "am")
            {
                StiWebViewerMonthlyTickets.ShowDesignButton = true;
            }
            else
            {
                StiWebViewerMonthlyTickets.ShowDesignButton = false;
            }


            #endregion

            if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
            {

                DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
                DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));

            }
        }
    }
    private void GetAddress()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString(); 
        CompanyAddress = objBL_User.getCompanyAddress(objProp_User);
    }

    private void GetDefaultCategory()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        DefaultCategory = objBL_User.getDefaultCategory(objProp_User);
    }


    
    protected void StiWebViewerMonthlyTickets_DesignReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "MonthlyMaintenanceofTicketReport";

        this.Response.Redirect("ReportDesignerMonthlyTicketReport.aspx?reportname=" + keyValue + "&StartDate=" + StartDate + "&EndDate=" + EndDate, true);
    }

    protected void StiWebViewerMonthlyTickets_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string reportPath = Server.MapPath("StimulsoftReports/MonthlyMaintenanceofTicketReport.mrt");
        GetAddress();
        GetDefaultCategory();
        StiReport report = new StiReport();
        report.Load(reportPath);
        //report.Compile();

        report["StartDate"] = StartDate;
        report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        report["CompanyAddress"] = CompanyAddress;
        report["DefaultCategory"] = DefaultCategory;

        e.Report = report;
    }

    protected void StiWebViewerMonthlyTickets_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
        {

            DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
            DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
            DataSet MonthlyTicketsDataSet = new DataSet("MonthlyTicketsDataSet");
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.FromDate = StartDate.ToShortDateString().ToString();
            objMapData.Todate = EndDate.ToShortDateString().ToString();
            MonthlyTicketsDataSet = objBL_MapData.GetMonthlyCompleteOpenTickets(objMapData);



            DataSet MonthlyTickets = new DataSet();
            DataTable CompletedTickets = new DataTable();
            DataTable OpenTickets = new DataTable();
            CompletedTickets = MonthlyTicketsDataSet.Tables[0].Copy();
            OpenTickets = MonthlyTicketsDataSet.Tables[1].Copy();
            CompletedTickets.TableName = "CompletedTickets";
            OpenTickets.TableName = "OpenTickets";
            MonthlyTickets.Tables.Add(CompletedTickets);
            MonthlyTickets.Tables.Add(OpenTickets);
            report.RegData("MonthlyTickets", MonthlyTickets);

           

        }
    }
}