using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InvoicesReportwithPayment : System.Web.UI.Page
{
    #region Variables
    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    #endregion

    #region Events
    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            DateTime _now = DateTime.Now;
            var _startDate = new DateTime(_now.Year, _now.Month, 1);
            var _endDate = _startDate.AddMonths(1).AddDays(-1);
            if (Session["filterstate"] != null)
            {
                var invoiceFilterState = Session["filterstate"].ToString();
                var filterDetails = invoiceFilterState.Split(';');
                if (filterDetails.Length > 7)
                {
                    _startDate = DateTime.Parse(filterDetails[5]);
                    _endDate = DateTime.Parse(filterDetails[6]);
                }
            }

            txtStartDate.Text = _startDate.ToShortDateString();
            txtEndDate.Text = _endDate.ToShortDateString();

            GetInvoicesReport();
        }
    }
    #endregion

    protected void rvInvoices_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetInvoicesReport();
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetInvoicesReport();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("invoices?fil=1");
    }
         
    #endregion

    #region Custom Functions
    private StiReport GetInvoicesReport()
    {
        StiReport report = new StiReport();
        try
        {
            var _filteredbyloc = false;
            string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/ARInvoiceList.mrt");

           
            report.Load(reportPathStimul);
            report.Compile();

            var _startDate = DateTime.Now;
            var _endDate = DateTime.Now;

            if (Session["filterstate"] != null)
            {
                var invoiceFilterState = Session["filterstate"].ToString();
                var filterDetails = invoiceFilterState.Split(';');
                if (filterDetails.Length > 7)
                {
                    _startDate = DateTime.Parse(filterDetails[5]);
                    _endDate = DateTime.Parse(filterDetails[6]);
                }

                objContract.SearchBy = filterDetails[0];
                if (filterDetails[0] == "i.Type")
                {
                    objContract.SearchValue = filterDetails[2];
                }
                else if (filterDetails[0] == "i.fdate")
                {
                    objContract.SearchValue = filterDetails[3];
                }
                else if (filterDetails[0] == "l.loc")
                {
                    objContract.SearchValue = filterDetails[1];
                    _filteredbyloc = true;
                }
                else if (filterDetails[0] == "i.ref")

                {

                    if (filterDetails[4][0] == '=' || filterDetails[4][0] == '>' || filterDetails[4][0] == '<' || filterDetails[4][0] == 'b' || filterDetails[4][0] == 'B')
                    {
                        if (filterDetails[4].Trim().Length > 2)
                        {
                            filterDetails[4] = " " + filterDetails[4];
                            objContract.SearchValue = filterDetails[4].Replace("'", "''");
                        }
                    }
                    else
                    {
                        if (filterDetails[4].IndexOf('=') > -1 || filterDetails[4].IndexOf('>') > -1 || filterDetails[4].IndexOf('<') > -1)
                            filterDetails[4] = filterDetails[4];
                        else
                            filterDetails[4] = "=" + filterDetails[4];
                        objContract.SearchValue = filterDetails[4].Replace("'", "''");
                    }

                }
                else
                {
                    objContract.SearchValue = filterDetails[4].Replace("'", "''");
                }
                if (filterDetails[5] != string.Empty)
                {
                    objContract.StartDate = Convert.ToDateTime(filterDetails[5]);
                }
                else
                {
                    objContract.StartDate = System.DateTime.MinValue;
                }

                if (filterDetails[6] != string.Empty)
                {
                    objContract.EndDate = Convert.ToDateTime(filterDetails[6]);
                }
                else
                {
                    objContract.EndDate = System.DateTime.MinValue;
                }

                if (filterDetails[8] == "All")
                    objContract.SearchAmtPaidUnpaid = string.Empty;
                else if (filterDetails[8] == "Paid")
                    objContract.SearchAmtPaidUnpaid = "P";
                else if (filterDetails[8] == "Open")
                    objContract.SearchAmtPaidUnpaid = "O";

                if (filterDetails[9] == "All")
                    objContract.SearchPrintMail = string.Empty;
                else if (filterDetails[9] == "PrintOnly")
                    objContract.SearchPrintMail = "P";
                else if (filterDetails[9] == "Mail")
                    objContract.SearchPrintMail = "M";
            }
            else
            {
                objContract.SearchValue = string.Empty;
                objContract.SearchBy = string.Empty;
                objContract.StartDate = System.DateTime.MinValue;
                objContract.EndDate = System.DateTime.MinValue;
            }


            objContract.CustID = Convert.ToInt32(Session["custid"].ToString());
            objContract.Paid = 0;

            if (Session["type"].ToString() == "c")
            {
                DataTable dtcust = new DataTable();
                dtcust = (DataTable)Session["userinfo"];
                int RoleID = 0;
                if (dtcust.Rows.Count > 0)
                {
                    RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                    objContract.RoleId = RoleID;
                }
            }
            /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objContract.isTS = 1;
            }
            /***/
            #region Company Check
            objContract.UserID = Session["UserID"].ToString();
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objContract.EN = 1;
            }
            else
            {
                objContract.EN = 0;
            }
            #endregion

            objContract.ConnConfig = Session["config"].ToString();
            Session["StartDate"] = Convert.ToDateTime(txtStartDate.Text);
            Session["EndDate"] = Convert.ToDateTime(txtEndDate.Text);
            if (!string.IsNullOrEmpty(txtStartDate.Text))
                objContract.StartDate = Convert.ToDateTime(txtStartDate.Text);
            else
                objContract.StartDate = Convert.ToDateTime(Session["StartDate"]);
            if (!string.IsNullOrEmpty(txtEndDate.Text))
                objContract.EndDate = Convert.ToDateTime(txtEndDate.Text);
            else
                objContract.EndDate = Convert.ToDateTime(Session["EndDate"]);
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            //   DataSet _ds = objBLContracts.GetInvoices(objContract);
            DataSet _ds = objBLContracts.GetInvoicesReceivePayment(objContract);
            //ReportParameter rpStartDate = new ReportParameter("paramStartDate", objContract.StartDate.ToShortDateString());
            //ReportParameter rpEndDate = new ReportParameter("paramEndDate", objContract.EndDate.ToShortDateString());
            report["paramStartDate"] = objContract.StartDate.ToShortDateString();
            report["paramEndDate"] = objContract.EndDate.ToShortDateString();
            report["TotalRecords"] = _ds.Tables[0].Rows.Count.ToString();
            report.RegData("dsCompany", dsC.Tables[0]);
            report.RegData("dsInvoice", _ds.Tables[0]);

            StiWebViewerInvoicesReport.Report = report;
            report.Render();
            //rvInvoices.LocalReport.DataSources.Clear();
            //rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", _ds.Tables[0]));
            //rvInvoices.LocalReport.ReportPath = "Reports/ARInvoiceList.rdlc";
            //rvInvoices.LocalReport.EnableExternalImages = true;
            //rvInvoices.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
            //rvInvoices.LocalReport.DisplayName = "Invoice Summary Report " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            //rvInvoices.LocalReport.Refresh();
           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return report;
    }
    private byte[] ExportReportToPDF(string reportName)
    {       

        byte[] buffer1 = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(GetInvoicesReport(), stream, settings);
        buffer1 = stream.ToArray();
        return buffer1;
    }

    protected void btn_SendEmail_Click(object sender, EventArgs e)
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
                mail.Title = "AR Invoices Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Invoice Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "InvoicesList.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                // ES-33:Task#2: Added
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseMailReport();", true);
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
    #endregion


    protected void StiWebViewerInvoicesReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        
    }

    protected void StiWebViewerInvoicesReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var _filteredbyloc = false;
        string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/ARInvoiceList.mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();

        var _startDate = DateTime.Now;
        var _endDate = DateTime.Now;

        if (Session["filterstate"] != null)
        {
            var invoiceFilterState = Session["filterstate"].ToString();
            var filterDetails = invoiceFilterState.Split(';');
            if (filterDetails.Length > 7)
            {
                _startDate = DateTime.Parse(filterDetails[5]);
                _endDate = DateTime.Parse(filterDetails[6]);
            }

            objContract.SearchBy = filterDetails[0];
            if (filterDetails[0] == "i.Type")
            {
                objContract.SearchValue = filterDetails[2];
            }
            else if (filterDetails[0] == "i.fdate")
            {
                objContract.SearchValue = filterDetails[3];
            }
            else if (filterDetails[0] == "l.loc")
            {
                objContract.SearchValue = filterDetails[1];
                _filteredbyloc = true;
            }
            else if (filterDetails[0] == "i.ref")
            {
                if (filterDetails[4][0] == '=' || filterDetails[4][0] == '>' || filterDetails[4][0] == '<' || filterDetails[4][0] == 'b' || filterDetails[4][0] == 'B')
                {
                    if (filterDetails[4].Trim().Length > 2)
                    {
                        filterDetails[4] = " " + filterDetails[4];
                        objContract.SearchValue = filterDetails[4].Replace("'", "''");
                    }
                }
                else
                {
                    if (filterDetails[4].IndexOf('=') > -1 || filterDetails[5].IndexOf('>') > -1 || filterDetails[4].IndexOf('<') > -1)
                        filterDetails[4] = filterDetails[4];
                    else
                        filterDetails[4] = "=" + filterDetails[4];
                    objContract.SearchValue = filterDetails[4].Replace("'", "''");
                }
            }
            else
            {
                objContract.SearchValue = filterDetails[4].Replace("'", "''");
            }
            if (filterDetails[5] != string.Empty)
            {
                objContract.StartDate = Convert.ToDateTime(filterDetails[5]);
            }
            else
            {
                objContract.StartDate = System.DateTime.MinValue;
            }

            if (filterDetails[6] != string.Empty)
            {
                objContract.EndDate = Convert.ToDateTime(filterDetails[6]);
            }
            else
            {
                objContract.EndDate = System.DateTime.MinValue;
            }

            if (filterDetails[8] == "All")
                objContract.SearchAmtPaidUnpaid = string.Empty;
            else if (filterDetails[8] == "Paid")
                objContract.SearchAmtPaidUnpaid = "P";
            else if (filterDetails[8] == "Open")
                objContract.SearchAmtPaidUnpaid = "O";

            if (filterDetails[9] == "All")
                objContract.SearchPrintMail = string.Empty;
            else if (filterDetails[9] == "PrintOnly")
                objContract.SearchPrintMail = "P";
            else if (filterDetails[9] == "Mail")
                objContract.SearchPrintMail = "M";
        }
        else
        {
            objContract.SearchValue = string.Empty;
            objContract.SearchBy = string.Empty;
            objContract.StartDate = System.DateTime.MinValue;
            objContract.EndDate = System.DateTime.MinValue;
        }

        objContract.CustID = Convert.ToInt32(Session["custid"].ToString());
        objContract.Paid = 0;

        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objContract.RoleId = RoleID;
            }
        }
        /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objContract.isTS = 1;
        }
        /***/
        #region Company Check
        objContract.UserID = Session["UserID"].ToString();
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objContract.EN = 1;
        }
        else
        {
            objContract.EN = 0;
        }
        #endregion

        e.Report = report;
        objContract.ConnConfig = Session["config"].ToString();
        if (!string.IsNullOrEmpty(txtStartDate.Text))
            objContract.StartDate = Convert.ToDateTime(txtStartDate.Text);
        else
            objContract.StartDate = Convert.ToDateTime(Session["StartDate"]);
        if (!string.IsNullOrEmpty(txtEndDate.Text))
            objContract.EndDate = Convert.ToDateTime(txtEndDate.Text);
        else
            objContract.EndDate = Convert.ToDateTime(Session["EndDate"]);

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        DataSet _ds = objBLContracts.GetInvoicesReceivePayment(objContract);
        report["paramStartDate"] = objContract.StartDate.ToShortDateString();
        report["paramEndDate"] = objContract.EndDate.ToShortDateString();
        report["TotalRecords"] = _ds.Tables[0].Rows.Count.ToString();
        report.RegData("dsCompany", dsC.Tables[0]);
        report.RegData("dsInvoice", _ds.Tables[0]);

        report.Render();
    }
}