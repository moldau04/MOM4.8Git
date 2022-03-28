using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using iTextSharp.text.pdf;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Telerik.Web.UI;

public partial class SalesTaxReport : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BusinessEntity.Invoices objInvoices = new BusinessEntity.Invoices();
    BL_Invoice objBLInvoices = new BL_Invoice();

    BL_Report bL_Report = new BL_Report();
    Chart objChart = new Chart();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            txtfromDate.Text = firstDay.Date.ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");

            //Session["ToDate"] = txtToDate.Text;
            //Session["FromDate"] = txtfromDate.Text;

            if (Request.QueryString["toDate"] != null && !string.IsNullOrEmpty(Request.QueryString["toDate"]))
            {
                txtToDate.Text = DateTime.ParseExact(Request.QueryString["toDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (Request.QueryString["fromDate"] != null && !string.IsNullOrEmpty(Request.QueryString["fromDate"]))
            {
                txtfromDate.Text = DateTime.ParseExact(Request.QueryString["fromDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }


            if (Request.QueryString["type"] != null && !string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                if (Request.QueryString["type"] == "1")
                {
                    rdExpandAll.Checked = true;
                    rdCollapseAll.Checked = false;
                }
                else
                {
                    rdExpandAll.Checked = false;
                    rdCollapseAll.Checked = true;
                }
            }

            HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
        }


        if (string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            StiWebViewerSalesTax.Visible = false;
        }
        else
        {
            StiWebViewerSalesTax.Visible = true;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("ToDate");
        Session.Remove("FromDate");

        if (!string.IsNullOrEmpty(Request["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        processSaleTaxReport();
    }

    protected void StiWebViewerSalesTax_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReport.mrt");        
        if (string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReport.mrt");
        }
        else
        {
            if (Request.QueryString["type"] == "0")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReportSummary.mrt");
            }           
        }       
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        e.Report = report;
    }

    protected void StiWebViewerSalesTax_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();

            cTable.Rows.Add(cRow);

            report.RegData("CompanyDetails", cTable);

            objInvoices.ConnConfig = Session["config"].ToString();

            #region start-end date
           
            if (Request.QueryString["toDate"] != null && !string.IsNullOrEmpty(Request.QueryString["toDate"]))
            {

                objInvoices.EndDate = DateTime.ParseExact(Request.QueryString["toDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (Request.QueryString["fromDate"] != null && !string.IsNullOrEmpty(Request.QueryString["fromDate"]))
            {
                objInvoices.StartDate = DateTime.ParseExact(Request.QueryString["fromDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            #endregion

            var isSumarry = false;
            if (string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                isSumarry = false;
            }
            else
            {
                if (Request.QueryString["type"] == "0")
                {
                    isSumarry = true;
                }
            }

            DataSet ds = objBLInvoices.GetSalesTax(objInvoices, isSumarry);
            if (ds != null)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            report.Dictionary.Variables["StartDate"].Value = objInvoices.StartDate.ToShortDateString();
            report.Dictionary.Variables["EndDate"].Value = objInvoices.EndDate.ToShortDateString();
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
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
                mail.Title = "Sales Tax Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This report is generated from Mobile Office Manager. Please find attached the Sales Tax Report.";
                }

                // File attachment
                StiWebViewer rvTemplate = new StiWebViewer();
                List<byte[]> poToPrint = PrintTemplate(rvTemplate);

                if (poToPrint != null && poToPrint.Count > 0)
                {
                    mail.attachmentBytes = ConcatAndAddContent(poToPrint);
                    mail.FileName = "SalesTaxReport.pdf";
                }

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        return companyDetailsTable;
    }

    private byte[] ExportReportToPDF()
    {
        byte[] bytes = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(StiWebViewerSalesTax.Report, stream, settings);
        bytes = stream.ToArray();

        return bytes;
    }

    public static byte[] ConcatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        iTextSharp.text.Document doc = new iTextSharp.text.Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }

    private List<byte[]> PrintTemplate(StiWebViewer rvTemplate)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReport.mrt");
            var isSumarry = false;

            if (string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReport.mrt");
                isSumarry = false;
            }
            else
            {
                if (Request.QueryString["type"] == "0")
                {
                    reportPathStimul = Server.MapPath("StimulsoftReports/SalesTaxReportSummary.mrt");
                    isSumarry = true;
                }
            }
          
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            report.Compile();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet dsC = new DataSet();
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();

            cTable.Rows.Add(cRow);

            report.RegData("CompanyDetails", cTable);

            objInvoices.ConnConfig = Session["config"].ToString();

            #region start-end date


            if (Request.QueryString["toDate"] != null && !string.IsNullOrEmpty(Request.QueryString["toDate"]))
            {

                objInvoices.EndDate = DateTime.ParseExact(Request.QueryString["toDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            if (Request.QueryString["fromDate"] != null && !string.IsNullOrEmpty(Request.QueryString["fromDate"]))
            {
                objInvoices.StartDate = DateTime.ParseExact(Request.QueryString["fromDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            #endregion

            DataSet ds = objBLInvoices.GetSalesTax(objInvoices, isSumarry);
            if (ds != null)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            report.Dictionary.Variables["StartDate"].Value = objInvoices.StartDate.ToShortDateString();
            report.Dictionary.Variables["EndDate"].Value = objInvoices.EndDate.ToShortDateString();
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

            report.Dictionary.Synchronize();
            report.Render();
            rvTemplate.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvTemplate.Report, stream, settings);
            buffer1 = stream.ToArray();
            templateAsBytes.Add(buffer1);

            return templateAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return templateAsBytes;
        }
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        processSaleTaxReport();
    }

    public void processSaleTaxReport()
    {
        try
        {
            var endDate = Convert.ToDateTime(txtToDate.Text);
            var fromDate = Convert.ToDateTime(txtfromDate.Text);


            String type = "0";
            if (rdExpandAll.Checked)
            {
                type = "1";
            }
            String url = "SalesTaxReport.aspx?redirect=Invoices.aspx&type=" + type + "&fromDate=" + fromDate.ToString("MM/dd/yyyy") + "&toDate=" + endDate.ToString("MM/dd/yyyy");

            this.Response.Redirect(url, true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
}
