using System;
using System.Web.UI;
using Stimulsoft.Report.Web;
using Stimulsoft.Report;
using BusinessLayer;
using BusinessEntity;
using System.Data;

public partial class EstimateWeeklySaleReport : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    String CompanyAddress = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Report Design Button Hide
            if (Convert.ToString(Session["type"]) == "am")
            {
                StiWebViewer1.ShowDesignButton = true;
            }
            else
            {
                StiWebViewer1.ShowDesignButton = false;
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
        objProp_User.DBName = Session["dbname"].ToString(); ;
        CompanyAddress = objBL_User.getCompanyAddress(objProp_User);
    }

    protected void StiWebViewer1_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string reportPath = Server.MapPath("StimulsoftReports/WeeklySaleReport.mrt");
        GetAddress();
        StiReport report = new StiReport();
        report.Load(reportPath);
        //report.Compile();

        report["StartDate"] = StartDate;
        report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        report["CompanyAddress"] = CompanyAddress;
        
        e.Report = report;
    }

    protected void StiWebViewer1_DesignReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "WeeklySaleReport";

        this.Response.Redirect("ReportDesigner.aspx?reportname=" + keyValue + "&StartDate=" + StartDate + "&EndDate=" + EndDate, true);
    }


    protected void StiWebViewer1_GetReportData(object sender, StiReportDataEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
        {

            DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
            DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
            DataSet WeeklyQuotedDataSet = new DataSet("WeeklyQuotedDataSet");
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.StartDate = StartDate.ToShortDateString().ToString();
            objProp_Customer.EndDate = EndDate.ToShortDateString().ToString();
            WeeklyQuotedDataSet = objBL_Customer.getWeeklySaleReportQuoted(objProp_Customer);

            e.Report.RegData("WeeklyQuoted", WeeklyQuotedDataSet.Tables[0]);
            e.Report.RegData("WeekEndDates", WeeklyQuotedDataSet.Tables[1]);
            e.Report.RegData("WeeklyAwardedJobs", WeeklyQuotedDataSet.Tables[2]);

        }

    }
}