using System;
using System.Collections.Generic;
using System.Data;
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

public partial class CallsPerRoute : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report bL_Report = new BL_Report();
    BL_Budgets bL_Budgets = new BL_Budgets();

    Chart objChart = new Chart();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");

            objChart.ConnConfig = Session["config"].ToString();

            rcRoute.DataSource = bL_Report.GetContractGroupRoute(objChart);
            rcRoute.DataTextField = "RouteName";
            rcRoute.DataValueField = "RouteName";
            rcRoute.DataBind();

            DateTime firstDay = new DateTime(DateTime.Now.Year, 1, 1);
            txtFromDate.Text = firstDay.Date.ToShortDateString();
            txtToDate.Text = DateTime.Now.Date.ToShortDateString();

            Session["ReportStartDate"] = Convert.ToDateTime(txtFromDate.Text);
            Session["ReportEndDate"] = Convert.ToDateTime(txtToDate.Text);
            Session["ReportRoute"] = GetRadComboBoxSelectedItems(rcRoute);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("ReportStartDate");
        Session.Remove("ReportEndDate");
        Session.Remove("ReportRoute");

        if (!string.IsNullOrEmpty(Request["redirect"]))
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }
        else
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtFromDate.Text))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyFromDateWarning", "noty({text: 'Start date is required!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else if (string.IsNullOrEmpty(txtToDate.Text))
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyToDateWarning", "noty({text: 'End date is required!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else
        {
            StiWebViewerContractListingByRoute.Visible = true;
            Session["ReportStartDate"] = Convert.ToDateTime(txtFromDate.Text);
            Session["ReportEndDate"] = Convert.ToDateTime(txtToDate.Text);
            Session["ReportRoute"] = GetRadComboBoxSelectedItems(rcRoute);
        }
    }

    protected void StiWebViewerContractListingByRoute_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/CallsPerRouteReport.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        e.Report = report;
    }

    protected void StiWebViewerContractListingByRoute_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            objChart.ConnConfig = connString;

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

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";

            report.RegData("CompanyDetails", CompanyDetails);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["StartDate"].Value = Session["ReportStartDate"].ToString();
            report.Dictionary.Variables["EndDate"].Value = Session["ReportEndDate"].ToString();

            objChart.StartDate = Convert.ToDateTime(Session["ReportStartDate"].ToString());
            objChart.EndDate = Convert.ToDateTime(Session["ReportEndDate"].ToString());
            var routes = Session["ReportRoute"].ToString();

            var ds = bL_Report.GetCompletedTicketByRoute(objChart, routes);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtDetail = ds.Tables[0];
                report.RegData("ReportData", dtDetail);
            }
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
                mail.Title = "Completed Ticket Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Contract Listing by Route Report attached.";
                }

                // File attachment
                StiWebViewer rvTemplate = new StiWebViewer();
                List<byte[]> poToPrint = PrintTemplate(rvTemplate);

                if (poToPrint != null && poToPrint.Count > 0)
                {
                    mail.attachmentBytes = ConcatAndAddContent(poToPrint);
                    mail.FileName = "ContractListingByRoute.pdf";
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
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ContactListingByRouteReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            objChart.ConnConfig = connString;

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

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";

            report.RegData("CompanyDetails", CompanyDetails);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["StartDate"].Value = Session["ReportStartDate"].ToString();
            report.Dictionary.Variables["EndDate"].Value = Session["ReportEndDate"].ToString();

            objChart.StartDate = Convert.ToDateTime(Session["ReportStartDate"].ToString());
            objChart.EndDate = Convert.ToDateTime(Session["ReportEndDate"].ToString());
            var routes = Session["ReportRoute"].ToString();

            var ds = bL_Report.GetCompletedTicketByRoute(objChart, routes);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtDetail = ds.Tables[0];
                report.RegData("ReportData", dtDetail);
            }

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

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        int itemschecked = radComboBox.CheckedItems.Count;
        String[] ServiceTypesArray = new String[itemschecked];

        var collection = radComboBox.CheckedItems;
        int i = 0;
        foreach (var item in collection)
        {
            String value = item.Value;
            ServiceTypesArray[i] = $"'{value}'";
            i++;
        }
        var ServiceTypes = String.Join(",", ServiceTypesArray);

        return ServiceTypes;
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
