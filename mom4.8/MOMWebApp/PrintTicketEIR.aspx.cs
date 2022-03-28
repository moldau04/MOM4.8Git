using System;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;

public partial class PrintTicketEIR : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = new DataSet();
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["c"].ToString());
            ds = objBL_MapData.GetTicketByID(objMapData);


            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            string reportPath = "reports/TicketEIR.rdlc";
            ReportViewer1.LocalReport.ReportPath = reportPath;

            ReportViewer1.LocalReport.Refresh();
        }
    }
}
