using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using System.IO;
using BusinessLayer.Schedule;

public partial class PrintTicketDate : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            FillWorker();
            rbSelect_SelectedIndexChanged(sender, e);
            //FillReport();
        }
        Permission();
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("schMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkSchd");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkScheduleMenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSchd");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("schdMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (rbSelect.SelectedValue == "0")
        {
            FillReport();
        }
        else
        {
            FillReportGrouped();
        }
    }

    private void FillReport()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.Worker = ddlworker.SelectedValue;
        if (txtStart.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(txtStart.Text);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
        }

        if (txtEnd.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(txtEnd.Text);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
        }
        
        //ds = objBL_MapData.GetTicketsByWorkerDate(objMapData);
        objMapData.Assigned = -1;
        objMapData.Department = -1;
        ds = new BL_Tickets().getCallHistory(objMapData);

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);
        if (dsC.Tables[0].Rows.Count > 0)
        {
            txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
        }

        ReportViewer1.LocalReport.DataSources.Clear();        
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        string reportPath = "Reports/TicketDate.rdlc";
        ReportViewer1.LocalReport.ReportPath = reportPath;

        string stdate = "";
        if (txtStart.Text != string.Empty)
        {
            stdate = " Date range from : " + txtStart.Text;
        }
        if (txtStart.Text != string.Empty && txtEnd.Text != string.Empty)
        {
            stdate += " To : " + txtEnd.Text;
        }
        if (txtStart.Text == string.Empty && txtEnd.Text != string.Empty)
        {
            stdate += "Date range To : " + txtEnd.Text;
        }
        if (txtStart.Text == string.Empty && txtEnd.Text == string.Empty)
        {
            stdate += "Date range : All Tickets";
        }
        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Worker", ddlworker.SelectedItem.Text));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("From", stdate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("To", txtEnd.Text));
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();
    }

    private void FillReportGrouped()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.Worker = ddlworker.SelectedValue;
        if (txtStart.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(txtStart.Text);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
        }

        if (txtEnd.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(txtEnd.Text);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
        }

        ds = objBL_MapData.GetReportTicket(objMapData);

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);
        ReportViewer1.LocalReport.DataSources.Clear();        
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

        string reportPath = "Reports/TicketGrouped.rdlc";
        ReportViewer1.LocalReport.ReportPath = reportPath;

        string stdate = "";
        if (txtStart.Text != string.Empty)
        {
            stdate = " Date range from : " + txtStart.Text;
        }
        if (txtStart.Text != string.Empty && txtEnd.Text != string.Empty)
        {
            stdate += " To : " + txtEnd.Text;
        }
        if (txtStart.Text == string.Empty && txtEnd.Text != string.Empty)
        {
            stdate += "Date range To : " + txtEnd.Text;
        }
        if (txtStart.Text == string.Empty && txtEnd.Text == string.Empty)
        {
            stdate += "Date range : All Tickets";
        }

        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Worker", ddlworker.SelectedItem.Text));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("From", stdate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("To", txtEnd.Text));
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();
    } 

    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 1;
        ds = objBL_User.getEMP(objPropUser);
        ddlworker.DataSource = ds.Tables[0];
        ddlworker.DataTextField = "fDesc";
        ddlworker.DataValueField = "fDesc";
        ddlworker.DataBind();

        ddlworker.Items.Insert(0, new ListItem("All", ""));
    }

    private void RenderReport()
    {

        //LocalReport localReport = new LocalReport();

        //localReport.ReportPath = Server.MapPath("~/Report.rdlc");


        ////A method that returns a collection for our report

        ////Note: A report can have multiple data sources

        //List<Employee> employeeCollection = GetData();



        //Give the collection a name (EmployeeCollection) so that we can reference it in our report designer

        //ReportDataSource reportDataSource = new ReportDataSource("EmployeeCollection", employeeCollection);

        //localReport.DataSources.Add(reportDataSource);



        string reportType = "PDF";

        string mimeType;

        string encoding;

        string fileNameExtension;



        //The DeviceInfo settings should be changed based on the reportType

        //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

        string deviceInfo =

        "<DeviceInfo>" +

        "  <OutputFormat>PDF</OutputFormat>" +

        "  <PageWidth>8.5in</PageWidth>" +

        "  <PageHeight>11in</PageHeight>" +

        "  <MarginTop>0.5in</MarginTop>" +

        "  <MarginLeft>1in</MarginLeft>" +

        "  <MarginRight>1in</MarginRight>" +

        "  <MarginBottom>0.5in</MarginBottom>" +

        "</DeviceInfo>";



        Warning[] warnings;

        string[] streams;

        byte[] renderedBytes;



        //Render the report

        renderedBytes = ReportViewer1.LocalReport.Render(

            reportType,

            deviceInfo,

            out mimeType,

            out encoding,

            out fileNameExtension,

            out streams,

            out warnings);



        //Clear the response stream and write the bytes to the outputstream

        //Set content-disposition to "attachment" so that user is prompted to take an action

        //on the file (open or save)

        Response.Clear();

        Response.ContentType = mimeType;

        Response.AddHeader("content-disposition", "attachment; filename=foo." + fileNameExtension);

        Response.BinaryWrite(renderedBytes);

        Response.End();



    }

    protected void rbSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbSelect.SelectedValue == "0")
        {
            //tblfilter.Visible = true;            
            ReportViewer1.Reset();
            FillReport();
        }
        else
        {
            //tblfilter.Visible = false;
            ReportViewer1.Reset();
            FillReportGrouped();
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        txtTo.Text = string.Empty;
        txtCC.Text = string.Empty;
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
