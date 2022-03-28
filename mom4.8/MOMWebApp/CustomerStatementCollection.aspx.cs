using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

public partial class CustomerStatementCollection : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_User objBL_User = new BL_User();
    User objPropUser = new User();
    int count = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            GetCustomerStatementReport(rvCustomer);
        }
    }
    protected void rvCustomer_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetCustomerStatementReport(rvCustomer);
    }
    private void GetCustomerStatementReport(ReportViewer rvCustomer)
    {
        try
        {
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            ViewState["Company"] = dsC.Tables[0];
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            objProp_Contracts.LocationIDs = Convert.ToString(Session["ColLocationIds"]);
            objProp_Contracts.CustomerIDs = Convert.ToString(Session["CollCustomers"]);

            #region Company Check
            objProp_Contracts.UserID = Session["UserID"].ToString();
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objProp_Contracts.EN = 1;
            }
            else
            {
                objProp_Contracts.EN = 0;
            }
            #endregion
            DataTable dt = new DataTable();
            if (ViewState["FilterInvoice"] == null)
            {
                DataSet ds = objBL_Contracts.GetCustomerStatementCollection(objProp_Contracts, chkIncludeCredit.Checked, chkIncludeCustomerCredit.Checked);
                ViewState["InvoiceResult"] = ds.Tables[0];
                dt = ds.Tables[0];
            }
            else
            {
                dt = (DataTable)ViewState["FilterInvoice"];
                ViewState["FilterInvoice"] = null;
            }

            ViewState["Invoices"] = dt;
            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + " " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            address = "Please review the attached customer statement from: " + Environment.NewLine + Environment.NewLine + address;
            ViewState["CompanyAdd"] = address;
            rvCustomer.LocalReport.DataSources.Clear();

            rvCustomer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvCustomer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dt));
            rvCustomer.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));

            string reportPath;
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
                reportPath = "Reports/CustomerStatementSouthern.rdlc";
            else
                reportPath = "Reports/CustomerStatement.rdlc";

            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"].Trim();
            // string Report = string.Empty;
            if (!string.IsNullOrEmpty(Report.Trim()) && Report.Contains(".rdlc"))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            rvCustomer.LocalReport.ReportPath = reportPath;
            rvCustomer.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

            rvCustomer.LocalReport.SetParameters(param1);

            rvCustomer.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            DataTable dt = (DataTable)ViewState["Invoices"];
            int loc = Convert.ToInt32(dt.Rows[count]["Loc"]);

            objProp_Contracts.Loc = loc;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
            {
                ds = objBL_Contracts.GetCustomerStatementInvoicesSouthern(objProp_Contracts, chkIncludeCredit.Checked);
            }
            else
            {
                ds = objBL_Contracts.GetCustomerStatementInvoices(objProp_Contracts, chkIncludeCredit.Checked);
            }


            if (dt.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dsInvoiceItem", ds.Tables[0]);

                e.DataSources.Add(rdsItems);
            }
            if (count == dt.Rows.Count - 1)
            {
                ViewState["Invoices"] = null;
            }
            count++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString());
        }
        else
        {
            Response.Redirect("iCollections.aspx");
        }
    }
    //private string GetFromEmailAddress()
    //{
    //    string fromEmail = "";
    //    objPropUser.ConnConfig = Session["config"].ToString();
    //    objPropUser.Username = Session["username"].ToString();
    //    try
    //    {
    //        fromEmail = objBL_User.getUserEmail(objPropUser);

    //        if (fromEmail == string.Empty)
    //        {
    //            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
    //            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //            string username = mailSettings.Smtp.Network.UserName;
    //            fromEmail = username;
    //            ////txtFrom.ReadOnly = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //    return fromEmail;
    //}
    //protected void lnkMailReport_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
    //        DataTable dtInv = (DataTable)ViewState["InvoiceResult"];
    //        DataTable dtC = (DataTable)ViewState["Company"];

    //        string fromEmail;
    //        if (ViewState["EmailFrom"] == null)
    //        {
    //            fromEmail = WebBaseUtility.GetFromEmailAddress();
    //        }
    //        else
    //        {
    //            fromEmail = ViewState["EmailFrom"].ToString();
    //        }
    //        List<string> lstLoc = new List<string>();
    //        //string strLoc;

    //        DataTable dtFilter = new DataTable();
    //        var rows = dtInv.AsEnumerable()
    //          .Where(x => x.Field<int>("IsExistsEmail").Equals(1));
    //        if (rows.Any())
    //            dtFilter = rows.CopyToDataTable();

    //        var groupLoc = (
    //                        from DataRow row in dtFilter.AsEnumerable()
    //                        select new
    //                        {
    //                            loc = row.Field<Int32>("Loc")
    //                        }
    //                     ).Distinct().AsEnumerable();

    //        foreach (var g in groupLoc)
    //        {
    //            count = 0;
    //            string toEmail = "";
    //            string ccEmail = "";

    //            #region Generate Report

    //            int loc = Convert.ToInt32(g.loc);
    //            DataTable dtLoc = dtFilter
    //                                .Select("Loc = " + loc)
    //                                .CopyToDataTable();

    //            ViewState["Invoices"] = dtLoc;
    //            ReportViewer rvCs = new ReportViewer();

    //            rvCs.LocalReport.DataSources.Clear();

    //            rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
    //            rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
    //            rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dtC));

    //            string reportPath = "Reports/CustomerStatement.rdlc";

    //            string Report = string.Empty;
    //            if (!string.IsNullOrEmpty(Report.Trim()))
    //            {
    //                reportPath = "Reports/" + Report.Trim();
    //            }
    //            rvCs.LocalReport.ReportPath = reportPath;
    //            rvCs.LocalReport.EnableExternalImages = true;
    //            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
    //            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
    //            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

    //            rvCs.LocalReport.SetParameters(param1);

    //            rvCs.LocalReport.Refresh();

    //            #endregion

    //            #region Email

    //            toEmail = dtLoc.Rows[0]["custom12"].ToString();

    //            if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
    //            {
    //                ccEmail = dtLoc.Rows[0]["custom13"].ToString();
    //            }

    //            List<string> toEmaillst = new List<string>();
    //            toEmaillst = toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).OfType<string>().ToList();
    //            List<string> ccEmaillst = new List<string>();
    //            ccEmaillst = ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).OfType<string>().ToList();

    //            Mail mail = new Mail();
    //            mail.From = fromEmail;
    //            mail.To = toEmaillst;
    //            mail.Cc = ccEmaillst;

    //            mail.Title = "Customer Statement - " + dtLoc.Rows[0]["LocID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();

    //            //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
    //            if (txtBody.Text.Trim() != string.Empty)
    //            {
    //                mail.Text = txtBody.Text.Replace("\n", "<BR/>");
    //            }
    //            else
    //            {
    //                mail.Text = ViewState["CompanyAdd"].ToString().Replace(Environment.NewLine, "<BR/>");
    //            }
    //            mail.attachmentBytes = ExportReportToPDF1("", rvCs);
    //            mail.FileName = "CustomerStatement_" + _todayDate + ".pdf";

    //            mail.DeleteFilesAfterSend = true;
    //            mail.RequireAutentication = false;
    //            // ES-33
    //            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
    //            mail.Send();

    //            #endregion

    //        }

    //        //var rows1 = dtInv.AsEnumerable()
    //        //                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
    //        //string strLoc;
    //        //strLoc = string.Join(", ", rows1.AsEnumerable()
    //        //                     .Select(x => x["locname"].ToString())
    //        //                     .ToArray());

    //        //if (!string.IsNullOrEmpty(strLoc))
    //        //{
    //        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
    //        //} 
    //        GetCustomerStatementReport(rvCustomer);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});dispWarningMesg();", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    protected void lnkMailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = (DataTable)ViewState["InvoiceResult"];
            DataTable dtC = (DataTable)ViewState["Company"];

            string fromEmail;
            if (ViewState["EmailFrom"] == null)
            {
                fromEmail = WebBaseUtility.GetFromEmailAddress();
            }
            else
            {
                fromEmail = ViewState["EmailFrom"].ToString();
            }
            List<string> lstLoc = new List<string>();
            //string strLoc;

            DataTable dtFilter = new DataTable();
            var rows = dtInv.AsEnumerable()
              .Where(x => x.Field<int>("IsExistsEmail").Equals(1));
            if (rows.Any())
                dtFilter = rows.CopyToDataTable();

            var groupLoc = (
                            from DataRow row in dtFilter.AsEnumerable()
                            select new
                            {
                                loc = row.Field<Int32>("Loc")
                            }
                         ).Distinct().AsEnumerable();

            var groupAllLoc = (
                            from DataRow row in dtInv.AsEnumerable()
                            select new
                            {
                                loc = row.Field<Int32>("Loc")
                            }
                         ).Distinct().AsEnumerable();
            int totalLocInList = groupAllLoc != null ? groupAllLoc.Count() : 0;
            
            int totalLocForEmail = groupAllLoc != null ? groupLoc.Count() : 0;//dt != null ? dt.Rows.Count : 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            int totalNotSend = totalLocInList - totalLocForEmail;
            //int mailCount = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            //List<string> invoiceIdsSentEmail = new List<string>();
            //List<string> invoiceIdsError = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Customer Statement Report: Email All";
            emailLog.Screen = "Collections";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();

            foreach (var g in groupLoc)
            {
                count = 0;
                string toEmail = "";
                string ccEmail = "";

                #region Generate Report

                int loc = Convert.ToInt32(g.loc);
                DataTable dtLoc = dtFilter
                                    .Select("Loc = " + loc)
                                    .CopyToDataTable();
                emailLog.Ref = loc;
                ViewState["Invoices"] = dtLoc;
                ReportViewer rvCs = new ReportViewer();

                rvCs.LocalReport.DataSources.Clear();

                rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
                rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dtC));

                string reportPath = "Reports/CustomerStatement.rdlc";

                string Report = string.Empty;
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
                rvCs.LocalReport.ReportPath = reportPath;
                rvCs.LocalReport.EnableExternalImages = true;
                List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                rvCs.LocalReport.SetParameters(param1);

                rvCs.LocalReport.Refresh();

                #endregion

                #region Email

                toEmail = dtLoc.Rows[0]["custom12"].ToString();

                if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
                {
                    ccEmail = dtLoc.Rows[0]["custom13"].ToString();
                }

                List<string> toEmaillst = new List<string>();
                toEmaillst = toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).OfType<string>().ToList();
                List<string> ccEmaillst = new List<string>();
                ccEmaillst = ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries).OfType<string>().ToList();

                Mail mail = new Mail();
                mail.From = fromEmail;
                mail.To = toEmaillst;
                mail.Cc = ccEmaillst;

                mail.Title = "Customer Statement - " + dtLoc.Rows[0]["LocID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();

                //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["CompanyAdd"].ToString().Replace(Environment.NewLine, "<BR/>");
                }
                mail.attachmentBytes = ExportReportToPDF1("", rvCs);
                mail.FileName = "CustomerStatement_" + _todayDate + ".pdf";

                mail.DeleteFilesAfterSend = true;
                //mail.RequireAutentication = false;
                // ES-33
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                //mail.Send();
                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                if (emailSendError != null && emailSendError.Item1 == 1)
                {
                    sbdSentError.Append(emailSendError.Item2);
                    break;
                }
                else
                {
                    emailSendError = mail.Send(mimeMessage, true, emailLog);
                    if (emailSendError != null)
                    {
                        if (emailSendError.Item1 == 1)
                        {
                            sbdSentError.Append(emailSendError.Item2);
                            break;
                        }
                        else
                        {
                            sbdSentError.Append(emailSendError.Item2);
                            mimeErrorMessages.Add(mimeMessage);
                            //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                            totalSendErr++;
                        }
                    }
                    else
                    {
                        mimeSentMessages.Add(mimeMessage);
                        //invoiceIdsSentEmail.Add("Invoice #" + _ref.ToString());
                    }
                }

                #endregion

            }

            totalSentEmails = mimeSentMessages.Count;
            if (totalSentEmails > 0)
            {
                Mail mail = new Mail();
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                if (mail.TakeASentEmailCopy)
                {
                    emailLog.Ref = 0;
                    if (totalSentEmails >= 10)
                    {
                        List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                        while (mimeSentMessages.Any())
                        {
                            lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                            mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                        }

                        foreach (var lst in lstTenMessages)
                        {
                            emailGetSentError = mail.GetSentItems(lst, true, emailLog);
                        }
                    }
                    else
                    {
                        emailGetSentError = mail.GetSentItems(mimeSentMessages, true, emailLog);
                    }
                }
            }

            GetCustomerStatementReport(rvCustomer);

            if (sbdSentError.Length > 0)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            else
            {
                if (totalSentEmails > 0)
                {
                    var successfullMess = "There were " + totalSentEmails + " of "
                        + totalLocInList.ToString() + " customer statement sent out successfully";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    if (totalSendErr > 0)
                    {
                        var errMess = "There were " + totalSendErr + " failed";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                    if (totalNotSend > 0)
                    {
                        var notSentMess = "There were " + totalNotSend + " locations have no email address";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + notSentMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }


                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out.";
                    if (totalSendErr > 0)
                    {
                        str += "<br>" + totalSendErr + " failed emails";
                    }
                    if (totalNotSend > 0)
                    {
                        //successfullMess += "<br>Total " + ticketIdsError.Count + " failed of "
                        //    + totalCompletedTicket.ToString() + " tickets could not be sent.";
                        str += "<br>" + totalNotSend + " locations have no email address";
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }

            
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});dispWarningMesg();", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPrintOnly_Click(object sender, EventArgs e)
    {
        //Print invoices which are not emailed to locations.
        try
        {
            PrintOnly();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void PrintOnly()
    {
        try
        {
            string filename = "PrintOnly_CustomerStatement";
            DataTable dtFilter = new DataTable();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                objProp_Contracts.Owner = Convert.ToInt32(Request.QueryString["uid"]);
            }
            #region Company Check
            objProp_Contracts.UserID = Session["UserID"].ToString();
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objProp_Contracts.EN = 1;
            }
            else
            {
                objProp_Contracts.EN = 0;
            }
            #endregion
            DataSet ds = objBL_Contracts.GetCustomerStatement(objProp_Contracts, chkIncludeCredit.Checked, chkIncludeCustomerCredit.Checked);
            DataTable dt = ds.Tables[0];
            var rows = dt.AsEnumerable()
                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));
            if (rows.Any())
                dtFilter = rows.CopyToDataTable();

            count = 0;
            if (dtFilter != null)
            {
                if (dtFilter.Rows.Count > 0)
                {
                    ViewState["FilterInvoice"] = dtFilter;
                    ReportViewer rvCs = new ReportViewer();
                    GetCustomerStatementReport(rvCs);
                    byte[] getPDF = ExportReportToPDF1("", rvCs);

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (getPDF.Length).ToString());
                    Response.BinaryWrite(getPDF);
                    Response.Flush();
                    Response.Close();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No customer statement found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No customer statement found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private byte[] ExportReportToPDF1(string reportName, ReportViewer reportviewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        reportviewer1.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportviewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
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
                mail.Title = txtSubject.Text.Trim(); //"Invoice " + Request.QueryString["uid"].ToString() + " - " + ViewState["subject"].ToString();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.FileName = "Invoice-" + Request.QueryString["uid"].ToString() + ".pdf";
                mail.attachmentBytes = ExportReportToPDF("");

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                // ES-33
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
                //this.programmaticModalPopup.Hide();
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
    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvCustomer.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;

        //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", reportName);
        //using (var fs = new FileStream(filename, FileMode.Create))
        //{
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();
        //}

        //return filename;
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        //Print invoices which are not emailed to locations.
        try
        {
            PrintOnly();
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "hideModel();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetCustomerStatementReport(rvCustomer);
    }
}