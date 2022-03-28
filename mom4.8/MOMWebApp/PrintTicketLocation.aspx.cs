using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.IO;
using BusinessLayer.Schedule;

public partial class PrintTicketLocation : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();

    int status = 0;
    DateTime Fromdate = new DateTime();
    DateTime Todate = new DateTime();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            status = Convert.ToInt32(Request.QueryString["s"]);

            if (Convert.ToString(Request.QueryString["sd"]) != string.Empty)
            {
                Fromdate = Convert.ToDateTime(Request.QueryString["sd"]);
            }
            else
            {
                Fromdate = System.DateTime.MinValue;
            }

            if (Convert.ToString( Request.QueryString["ed"]) != string.Empty)
            {
                Todate = Convert.ToDateTime(Request.QueryString["ed"]);
            }
            else
            {
                Todate = System.DateTime.MinValue;
            }

            FillCallHistory();
        }
        Permission();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnklocationsSmenu");
        lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");       
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillCallHistory();
    }

    private void FillCallHistory()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        objMapData.Assigned = Convert.ToInt32(Request.QueryString["s"]);
        objMapData.StartDate = Fromdate;
        objMapData.EndDate = Todate;
        objMapData.Department = -1;
        //if (txtfromDate.Text != string.Empty)
        //{
        //    objMapData.StartDate = Convert.ToDateTime(Request.QueryString["sd"]);
        //}
        //else
        //{
        //    objMapData.StartDate = System.DateTime.MinValue;
        //}

        //if (txtToDate.Text != string.Empty)
        //{
        //    objMapData.EndDate = Convert.ToDateTime(Request.QueryString["ed"]);
        //}
        //else
        //{
        //    objMapData.EndDate = System.DateTime.MinValue;
        //}

        //ds = objBL_MapData.getCallHistoryLocation(objMapData);
        ds = new BL_Tickets().getCallHistory(objMapData);

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);
        if (dsC.Tables[0].Rows.Count > 0)
        {
            txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
        }

        DataSet dccust = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        dccust = objBL_User.getLocationByIDReport(objPropUser);

        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);
        DataSet dsloc = new DataSet();
        dsloc = objBL_User.getLocationByID(objPropUser);
        if (dsloc.Tables[0].Rows.Count > 0)
        {
            txtTo.Text = dsloc.Tables[0].Rows[0]["custom14"].ToString();
            txtCC.Text = dsloc.Tables[0].Rows[0]["custom15"].ToString();
        }

        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Customer", dccust.Tables[0]));
        string reportPath = "Reports/Location.rdlc";
        ReportViewer1.LocalReport.ReportPath = reportPath;

        string stdate = "";
        if (Request.QueryString["sd"].ToString() != string.Empty)
        {
            stdate = " Date range from : " + Request.QueryString["sd"].ToString();
        }
        if (Request.QueryString["sd"].ToString() != string.Empty && Request.QueryString["ed"].ToString() != string.Empty)
        {
            stdate += " To : " + Request.QueryString["ed"].ToString();
        }
        if (Request.QueryString["sd"].ToString() == string.Empty && Request.QueryString["ed"].ToString() != string.Empty)
        {
            stdate += "Date range To : " + Request.QueryString["ed"].ToString();
        }
        if (Request.QueryString["sd"].ToString() == string.Empty && Request.QueryString["ed"].ToString() == string.Empty)
        {
            stdate += "Date range : All Tickets";
        }

        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Status", Request.QueryString["sn"].ToString()));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("From", stdate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("To", Request.QueryString["ed"].ToString()));
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();
    }
    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("addlocation.aspx?uid=" + Request.QueryString["uid"].ToString());
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        //txtTo.Text = string.Empty;
        //txtCC.Text = string.Empty;
        //ExportReportToPDF("Report_" + generateRandomString(10) + ".pdf");
    }

    private string ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = ReportViewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", reportName);
        using (var fs = new FileStream(filename, FileMode.Create))
        {
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        return filename;
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = "Ticket Report";
                mail.Text = "";
                mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                mail.Send();
                this.programmaticModalPopup.Hide();
                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }

}
