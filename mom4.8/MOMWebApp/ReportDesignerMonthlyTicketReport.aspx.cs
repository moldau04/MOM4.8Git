using System;
using System.Web;
using System.Web.UI;
using Stimulsoft.Report;
using BusinessLayer;
using BusinessEntity;
using System.Data;

public partial class ReportDesignerMonthlyTicketReport : System.Web.UI.Page
{
    private static string AppDirectory = HttpContext.Current.Server.MapPath(string.Empty);
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    String CompanyAddress = "";
    String DefaultCategory = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get the report name fror URL query
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "MonthlyMaintenanceofTicketReport";

        var report = new StiReport();
        report.Load(string.Format("{0}\\StimulsoftReports\\{1}.mrt", AppDirectory, keyValue));
        StiWebDesignerMonthlyTicket.Report = report;
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


    protected void StiWebDesignerMonthlyTicket_PreviewReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string reportPath = Server.MapPath("StimulsoftReports/MonthlyMaintenanceofTicketReport.mrt");
        GetAddress();
        GetDefaultCategory();
        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();
        report["StartDate"] = StartDate;
        report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        report["CompanyAddress"] = CompanyAddress;
        report["DefaultCategory"] = DefaultCategory;

        e.Report = report;
        if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
        {

          
            DataSet MonthlyTicketsDataSet = new DataSet("MonthlyTicketsDataSet");
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.StartDate = StartDate;
            objMapData.EndDate = EndDate;
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
            e.Report.RegData("MonthlyTickets", MonthlyTickets);



        }
    }

    protected void StiWebDesignerMonthlyTicket_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport report = e.Report;
        report.Save(Server.MapPath("StimulsoftReports/MonthlyMaintenanceofTicketReport.mrt"));
    }

    protected void StiWebDesignerMonthlyTicket_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string keyValue = Page.Request.QueryString.Get("reportname");
        // if (keyValue == null) keyValue = "InvReportsParts";


        this.Response.Redirect("MonthlyMaintenanceTicketReport.aspx?reportname=" + keyValue + "&StartDate=" + StartDate + "&EndDate=" + EndDate, true);
    }
}