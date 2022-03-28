using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class ReportDesignerMonthlyServiceCallBack : System.Web.UI.Page
{
    private static string AppDirectory = HttpContext.Current.Server.MapPath(string.Empty);
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    String CompanyAddress = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get the report name fror URL query
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "MonthlyServiceCallBackReport";

        var report = new StiReport();
        report.Load(string.Format("{0}\\StimulsoftReports\\{1}.mrt", AppDirectory, keyValue));
        StiWebDesignerMonthlyServiceCallBack.Report = report;
    }

    private void GetAddress()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString();
        CompanyAddress = objBL_User.getCompanyAddress(objProp_User);
    }

  


    protected void StiWebDesignerMonthlyServiceCallBack_PreviewReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string reportPath = Server.MapPath("StimulsoftReports/MonthlyServiceCallBackReport.mrt");
        GetAddress();
       
        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();
        report["StartDate"] = StartDate;
        report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        report["CompanyAddress"] = CompanyAddress;
        

        e.Report = report;
        if (!string.IsNullOrEmpty(Request.QueryString["StartDate"]) || !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
        {

            DataSet MonthlyServiceCallBackDataSet = new DataSet("MonthlyServiceCallBackDataSet");
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.FromDate = StartDate.ToShortDateString().ToString();
            objMapData.Todate = EndDate.AddDays(1).ToShortDateString().ToString();
            MonthlyServiceCallBackDataSet = objBL_MapData.GetAllServiceCallBackReport(objMapData);



            DataSet MonthlyCallBack = new DataSet();
            DataTable TotalCallBack = new DataTable();
            //DataTable BillableCallBack = new DataTable();
            //DataTable NonBillableCallBack = new DataTable();
            TotalCallBack = MonthlyServiceCallBackDataSet.Tables[0].Copy();
            //BillableCallBack = MonthlyServiceCallBackDataSet.Tables[1].Copy();
            //NonBillableCallBack = MonthlyServiceCallBackDataSet.Tables[2].Copy();
            TotalCallBack.TableName = "TotalCallBack";
            //BillableCallBack.TableName = "BillableCallBack";
            //NonBillableCallBack.TableName = "NonBillableCallBack";
            MonthlyCallBack.Tables.Add(TotalCallBack);
            //MonthlyCallBack.Tables.Add(BillableCallBack);
            //MonthlyCallBack.Tables.Add(NonBillableCallBack);
            report.RegData("MonthlyCallBack", MonthlyCallBack);
        }
    }

    protected void StiWebDesignerMonthlyServiceCallBack_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport report = e.Report;
        report.Save(Server.MapPath("StimulsoftReports/MonthlyServiceCallBackReport.mrt"));
    }

    protected void StiWebDesignerMonthlyServiceCallBack_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DateTime StartDate = Convert.ToDateTime((Request.QueryString["StartDate"]));
        DateTime EndDate = Convert.ToDateTime((Request.QueryString["EndDate"]));
        string keyValue = Page.Request.QueryString.Get("reportname");
        // if (keyValue == null) keyValue = "InvReportsParts";


        this.Response.Redirect("MonthlyServiceCallBackReport.aspx?reportname=" + keyValue + "&StartDate=" + StartDate + "&EndDate=" + EndDate, true);
    }
}