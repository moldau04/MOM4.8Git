using Stimulsoft.Report;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLayer;
using BusinessEntity;
public partial class ReportDesigner : System.Web.UI.Page
{
    private static string AppDirectory = HttpContext.Current.Server.MapPath(string.Empty);
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get the report name fror URL query
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "WeeklySaleReport";

        var report = new StiReport();
        report.Load(string.Format("{0}\\StimulsoftReports\\{1}.mrt", AppDirectory, keyValue));
        StiWebDesigner1.Report = report;
    }







    protected void StiWebDesigner1_PreviewReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var data = new DataSet();
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string reportPath = Server.MapPath("StimulsoftReports/WeeklySaleReport.mrt");
        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();
        report["StartDate"] = StartDate;
        report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        e.Report = report;
        if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
        {

           // DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
           
            DataSet WeeklyQuotedDataSet = new DataSet("WeeklyQuotedDataSet");
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.StartDate = StartDate.ToShortDateString().ToString();
            objProp_Customer.EndDate = EndDate.ToShortDateString().ToString();
            WeeklyQuotedDataSet = objBL_Customer.getWeeklySaleReportQuoted(objProp_Customer);
           
            e.Report.RegData("WeeklyQuoted", WeeklyQuotedDataSet);
            e.Report.RegData("WeekEndDates", WeeklyQuotedDataSet.Tables[1]);

        }


        

       
    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        StiReport report = e.Report;
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "WeeklySaleReport";

        report.Save(string.Format("{0}\\StimulsoftReports\\{1}.mrt", AppDirectory, keyValue));
    }

    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string keyValue = Page.Request.QueryString.Get("reportname");
       // if (keyValue == null) keyValue = "InvReportsParts";

 
        this.Response.Redirect("EstimateWeeklySaleReport.aspx?reportname=" + keyValue + "&StartDate=" + StartDate + "&EndDate=" + EndDate, true);
    }

   


   
}