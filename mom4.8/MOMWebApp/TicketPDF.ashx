<%@ WebHandler Language="C#" Class="TicketPDF" %>

using System;
using System.Web;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.SessionState;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;

public class TicketPDF : IHttpHandler, IReadOnlySessionState
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            PrintReport(context);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private void PrintReport(HttpContext context)
    {
        string Signedby = string.Empty;
        //DataSet dscustom = new DataSet();
        objPropGeneral.ConnConfig = context.Session["config"].ToString();
        objPropGeneral.CustomName = "Ticket1";
        //dscustom = objBL_General.getCustomFields(objPropGeneral);
        //if (dscustom.Tables[0].Rows.Count > 0)
        //{
        //    if (dscustom.Tables[0].Rows[0]["label"].ToString().ToLower() == "signed by")
        //    {
        //        Signedby = "Signed by: ";
        //    }
        //}

        DataSet ds = new DataSet();
        objMapData.ConnConfig = context.Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(context.Request.QueryString["id"].ToString());
        objMapData.ISTicketD = Convert.ToInt32(context.Request.QueryString["c"].ToString());
        ds = objBL_MapData.GetTicketByID(objMapData);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //if (Signedby != string.Empty)
            //{
                Signedby = ds.Tables[0].Rows[0]["custom1"].ToString();
            //}
            ds.Tables[0].Columns.Add("rtaddress");
            ds.Tables[0].Columns.Add("osaddress");
            ds.Tables[0].Columns.Add("ctaddress");
        }
        
        objPropUser.ConnConfig = context.Session["config"].ToString();
        objPropUser.Username = context.Session["username"].ToString();
        DataSet dsC = new DataSet();
        if (context.Session["MSM"].ToString() != "TS")
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

        string RDLC = context.Request.QueryString["RDLC"].Trim() == null ? "" : context.Request.QueryString["RDLC"].Trim();
        if (RDLC == "Ticket-Adams.rdlc" || RDLC == "TimeSheet_Template.rdlc")
        {
            reportPath = "Reports/" + RDLC;
            objMapData.ConnConfig = context.Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(context.Request.QueryString["id"].ToString());
            dsOtherWorker = objBL_MapData.getticketdtlbywday(objMapData);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dsOtherWorker.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dsOtherWorker.Tables[1]));
        }  
        
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", fillREPHistory(context)));
        if (ds.Tables.Count > 1)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", ds.Tables[1]));
        if (ds.Tables.Count > 2)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", ds.Tables[2]));     
        ReportViewer1.LocalReport.ReportPath = reportPath;
        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", "~/companylogo.ashx"));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom", Signedby));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("TicketID", context.Request.QueryString["id"].ToString()));
        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();

        reportdownload(context, ReportViewer1);
    }

    private DataTable fillREPHistory(HttpContext context)
    {
        objPropUser.ConnConfig = context.Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = context.Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
    }

    private void reportdownload(HttpContext context, ReportViewer ReportViewer1)
    {
        byte[] buffer = null;
        buffer = ExportReportToPDF("", ReportViewer1);
        context.Response.ClearContent();
        context.Response.ClearHeaders();
        context.Response.AddHeader("Content-Disposition", "Inline;filename=Tickets.pdf");
        context.Response.ContentType = "application/pdf";
        context.Response.BinaryWrite(buffer);
        context.Response.Flush();
        context.Response.Close();
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