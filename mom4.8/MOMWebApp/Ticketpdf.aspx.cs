using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
 

public partial class Ticketpdf : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        PrintReport();
    }

    private void PrintReport( )
    {
        string Signedby = string.Empty;
         
        objPropGeneral.ConnConfig = Session["config"].ToString();
        objPropGeneral.CustomName = "Ticket1";


        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["c"].ToString());
        ds = objBL_MapData.GetTicketByID(objMapData);
        if (ds.Tables[0].Rows.Count > 0)
        {
            
            Signedby = ds.Tables[0].Rows[0]["custom1"].ToString(); 
            ds.Tables[0].Columns.Add("rtaddress");
            ds.Tables[0].Columns.Add("osaddress");
            ds.Tables[0].Columns.Add("ctaddress");
        }

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();
        DataSet dsC = new DataSet();
        if (Session["MSM"].ToString() != "TS")
        {
            dsC = objBL_User.getControl(objPropUser);
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
            dsC = objBL_User.getControlBranch(objPropUser);
        }

        string reportPath = "Reports/Ticket.rdlc";
        string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["TicketReport"].Trim();
        if (!string.IsNullOrEmpty(Report.Trim()))
        {
            reportPath = "Reports/" + Report.Trim();
        }
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
        DataSet dsOtherWorker = new DataSet();

        ReportViewer ReportViewer1 = new ReportViewer();
        ReportViewer1.LocalReport.DataSources.Clear();

        string RDLC = Request.QueryString["RDLC"].Trim() == null ? "" : Request.QueryString["RDLC"].Trim();
        if (RDLC == "Ticket-Adams.rdlc" || RDLC == "TimeSheet_Template.rdlc")
        {
            reportPath = "Reports/" + RDLC;
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            dsOtherWorker = objBL_MapData.getticketdtlbywday(objMapData);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dsOtherWorker.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dsOtherWorker.Tables[1]));
        }

        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", fillREPHistory()));
        if (ds.Tables.Count > 1)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", ds.Tables[1]));
        if (ds.Tables.Count > 2)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", ds.Tables[2]));
        ReportViewer1.LocalReport.ReportPath = reportPath;
        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom", Signedby));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("TicketID", Request.QueryString["id"].ToString()));
        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();

        reportdownload( ReportViewer1);
    }

    private DataTable fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
    }

    private void reportdownload(ReportViewer ReportViewer1)
    {
        byte[] buffer = null;
        buffer = ExportReportToPDF("", ReportViewer1);
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "Inline;filename=Tickets.pdf");
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(buffer);
        Response.Flush();
        Response.Close();
    }

    private byte[] ExportReportToPDF(string reportName, ReportViewer ReportViewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = ReportViewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);
        return bytes;
    }
}
 