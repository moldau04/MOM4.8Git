using System;
using System.Web;
using System.Web.UI;
using Stimulsoft.Report;
using BusinessLayer;
using BusinessEntity;
using System.Data;

public partial class ReportDesignerListOfLeads : System.Web.UI.Page
{
    private static string AppDirectory = HttpContext.Current.Server.MapPath(string.Empty);
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get the report name fror URL query
        string keyValue = Page.Request.QueryString.Get("reportname");
        if (keyValue == null) keyValue = "ListLeadsReport";

        var report = new StiReport();
        report.Load(string.Format("{0}\\StimulsoftReports\\{1}.mrt", AppDirectory, keyValue));
        StiWebDesigner1.Report = report;
    }







    protected void StiWebDesigner1_PreviewReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var data = new DataSet();
        String SearchBy = (Request.QueryString["SearchBy"]);
        String SearchValue = (Request.QueryString["SearchValue"]);
        string reportPath = Server.MapPath("StimulsoftReports/ListLeadsReport.mrt");
        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();
        //report["StartDate"] = StartDate;
        //report["EndDate"] = EndDate;
        report["UserName"] = Session["Username"];
        report["dbname"] = Session["dbname"];
        e.Report = report;
        if (!string.IsNullOrEmpty(Request.QueryString["SearchBy"]) || !string.IsNullOrEmpty(Request.QueryString["SearchValue"]))
        {



          
            DataSet ListOfLeadsDataset = new DataSet("ListOfLeadsDataset");
            DataSet ListOfContacts = new DataSet("ListOfContacts");
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.SearchBy = SearchBy;
            objProp_Customer.SearchValue = SearchValue;

            ListOfLeadsDataset = objBL_Customer.getProspect(objProp_Customer);
            ListOfContacts = objBL_Customer.getContactList(objProp_Customer);



            DataSet ListLeads = new DataSet();
            DataTable dtLeads = new DataTable();
            DataTable dtContact = new DataTable();
            dtLeads = ListOfLeadsDataset.Tables[0].Copy();
            dtContact = ListOfContacts.Tables[0].Copy();
            dtLeads.TableName = "Leads";
            dtContact.TableName = "Contact";
            ListLeads.Tables.Add(dtLeads);
            ListLeads.Tables.Add(dtContact);
            e.Report.RegData("ListLeads", ListLeads);



        }





    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        StiReport report = e.Report;
        report.Save(Server.MapPath("StimulsoftReports/ListLeadsReport.mrt"));
    }

    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        String SearchBy = (Request.QueryString["SearchBy"]);
        String SearchValue = (Request.QueryString["SearchValue"]);
        string keyValue = Page.Request.QueryString.Get("reportname");
        // if (keyValue == null) keyValue = "InvReportsParts";

        this.Response.Redirect("ListOfLeadsReport.aspx?reportname=" + keyValue + "&SearchBy=" + SearchBy + "&SearchValue=" + SearchValue, true);
       
    }


}