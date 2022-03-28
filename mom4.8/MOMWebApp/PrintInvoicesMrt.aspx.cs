using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Configuration;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;
using Stimulsoft.Report.Web;
using Stimulsoft.Report;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Text;

public partial class PrintInvoicesMrt : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();
    BL_Report bL_Report = new BL_Report();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    bool IsGst = false;
    #region Events
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();
        if (SSL == "1")
        {
            bool isLocal = HttpContext.Current.Request.IsLocal;
            if (!isLocal)
            {
                bool isSecure = HttpContext.Current.Request.IsSecureConnection;
                string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                if (!isSecure)
                {
                    if (Session["type"].ToString() == "c")
                    {
                        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                        string Auth = HttpContext.Current.Request.Url.Authority;
                        if (!port)
                        {
                            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                        }
                        string URL = Auth + webPath;
                        string redirect = "HTTPS://" + URL + "/PrintInvoice.aspx";
                        int ii = 0;
                        foreach (String key in Request.QueryString.AllKeys)
                        {
                            if (ii == 0)
                                redirect += "?" + key + "=" + Request.QueryString[key];
                            else
                                redirect += "&" + key + "=" + Request.QueryString[key];
                            ii++;
                        }

                        Response.Redirect(redirect);
                    }
                }
            }
        }

        if (!IsPostBack)
        {
            string InvoicesCheckpath = Server.MapPath("StimulsoftReports/Invoices/");
            DirectoryInfo dirPath = new DirectoryInfo(InvoicesCheckpath);
            FileInfo[] Files = dirPath.GetFiles("*.mrt");
            foreach (FileInfo file in Files)
            {
                string FileName = string.Empty;
                if (file.Name.Contains(".mrt"))
                    FileName = file.Name.Replace(".mrt", " ");
                ddlInvoicesForLoad.Items.Add((FileName));
            }

            ddlInvoicesForLoad.DataBind();
            ddlInvoicesForLoad.SelectedIndex = 0;
            ddlInvoicesForLoad.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Invoice --", ""));
            ddlInvoicesForLoad.SelectedIndex = 0;

            ShowGstRate();

            if (Session["type"].ToString() == "c")
            {
                lnkPayment.Visible = true;
            }

            var dtRecurringInvoices = GetRecurringInvoices();

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(dtRecurringInvoices.Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }
            if (dsC.Tables[0].Rows.Count > 0)
            {
                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }
        }

        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            lnkPayment.Visible = false;
        }

        HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("InvoicesTemplateForLoad");
        if(Request.QueryString["page"]!=null && Request.QueryString["page"].ToString()== "recurringinvoices")
        {
            Response.Redirect("recurringinvoices.aspx");
        }

        if (Session["type"].ToString() != "c")
        {
            if (Session["MSM"].ToString() != "TS")
            {
                if (Request.QueryString["o"] == null)
                {
                    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + "&o=1");
                }
            }
            else
            {
                Response.Redirect("invoices.aspx?fil=1");
            }
        }
        else
        {
            Response.Redirect("invoices.aspx?fil=1");
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
                mail.Title = txtSubject.Text.Trim();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }

                mail.FileName = "Invoice-" + Request.QueryString["uid"].ToString() + ".pdf";
                mail.attachmentBytes = ExportReportToPDF("");

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private byte[] GetReportAsAttachment()
    {
        byte[] buffer1 = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(GetInvoiceAsReport(GetRecurringInvoices()), stream, settings);
        buffer1 = stream.ToArray();

        return buffer1;
    }

    protected void lnkPayment_Click(object sender, EventArgs e)
    {
        if (Convert.ToDouble(objGen.IsNull(ViewState["amount"].ToString(), "0")) == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Amount can not be zero.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            return;
        }
        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
        string Auth = HttpContext.Current.Request.Url.Authority;
        if (!port)
        {
            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
        }
        string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
        string URL = Auth + webPath;
        string strQuery = "[{'inv':'" + Request.QueryString["uid"].ToString() + "','amt':'" + ViewState["amount"].ToString() + "'}]";
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<string, string>> lstInv = sr.Deserialize<List<Dictionary<string, string>>>(strQuery);
        Session["uidv"] = lstInv;
        string paymentscreen = System.Web.Configuration.WebConfigurationManager.AppSettings["PayGateway"].Trim();

        Response.Redirect("https://" + URL + paymentscreen);
    }

    protected void lnkMailReport_BK_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = GetRecurringInvoices();

            DataTable dt = dtInv.AsEnumerable()
                       .GroupBy(r => r.Field<int>("Loc"))
                       .Select(g => g.First())
                       .CopyToDataTable();

            string _fromEmail = WebBaseUtility.GetFromEmailAddress();

            List<string> lstLoc = new List<string>();
            string strLoc;
            bool isUnscuss = false;
            int mailCount = 0;
            foreach (DataRow _dr in dt.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Ref = _ref;
                DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                if (_dsCon.Tables[0].Rows.Count > 0)
                {
                    string _toEmail = "";
                    string _ccEmail = "";
                    if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                    {
                        #region Generate Report

                        int loc = Convert.ToInt32(_dr["Loc"]);
                        DataTable dtRecur = dtInv
                                .Select("Loc = " + loc)
                                .CopyToDataTable();

                        if (mailCount == 4)
                        {
                            Thread.Sleep(10000);
                            mailCount = 0;
                        }
                        #endregion

                        #region Email

                        _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                        {
                            _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                        }

                        List<string> _toEmaillst = new List<string>();
                        _toEmaillst = _toEmail.Split(';', ',').OfType<string>().ToList();
                        List<string> _ccEmaillst = new List<string>();
                        _ccEmaillst = _ccEmail.Split(';', ',').OfType<string>().ToList(); 

                        Mail mail = new Mail();
                        mail.From = _fromEmail;
                        mail.To = _toEmaillst;
                        mail.Cc = _ccEmaillst;

                        mail.Title = "Invoices - " + _dsCon.Tables[0].Rows[0]["ID"].ToString() + " " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                        if (txtBody.Text.Trim() != string.Empty)
                        {
                            mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                        }
                        else
                        {
                            mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                        }
                        mail.attachmentBytes = GetReportAsAttachmentFile(dtRecur);
                        mail.FileName = "Invoices_" + _todayDate + ".pdf";

                        mail.DeleteFilesAfterSend = true;
                        mail.RequireAutentication = false;
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                        mail.Send();
                        mailCount = mailCount + 1;

                        #endregion
                    }
                    else
                    {
                        lstLoc.Add(_dsCon.Tables[0].Rows[0]["Tag"].ToString());
                        isUnscuss = true;
                    }
                }
            }

            if (isUnscuss)
            {
                strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Send all emails successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }    
    }

    protected void lnkMailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = GetRecurringInvoices();

            DataTable dt = dtInv.AsEnumerable()
                       .GroupBy(r => r.Field<int>("Loc"))
                       .Select(g => g.First())
                       .CopyToDataTable();

            string _fromEmail = WebBaseUtility.GetFromEmailAddress();

            List<string> lstLoc = new List<string>();
            string strLoc;
            bool isUnscuss = false;
            
            int totalSentEmails = 0;
            int totalSendErr = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Email All";
            emailLog.Screen = "PrintInvoice";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();
            foreach (DataRow _dr in dt.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Ref = _ref;
                DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                if (_dsCon.Tables[0].Rows.Count > 0)
                {
                    emailLog.Ref = _ref;
                    string _toEmail = "";
                    string _ccEmail = "";
                    if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                    {
                        #region Generate Report

                        int loc = Convert.ToInt32(_dr["Loc"]);
                        DataTable dtRecur = dtInv
                                .Select("Loc = " + loc)
                                .CopyToDataTable();
                        #endregion

                        #region Email
                        _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                        {
                            _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                        }

                        List<string> _toEmaillst = new List<string>();
                        _toEmaillst = _toEmail.Split(';', ',').OfType<string>().ToList();
                        List<string> _ccEmaillst = new List<string>();
                        _ccEmaillst = _ccEmail.Split(';', ',').OfType<string>().ToList();

                        Mail mail = new Mail();
                        mail.From = _fromEmail;
                        mail.To = _toEmaillst;
                        mail.Cc = _ccEmaillst;

                        mail.Title = "Invoices - " + _dsCon.Tables[0].Rows[0]["ID"].ToString() + " " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                        if (txtBody.Text.Trim() != string.Empty)
                        {
                            mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                        }
                        else
                        {
                            mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                        }
                        mail.attachmentBytes = GetReportAsAttachmentFile(dtRecur);
                        mail.FileName = "Invoices_" + _todayDate + ".pdf";

                        mail.DeleteFilesAfterSend = true;
                        mail.RequireAutentication = false;
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

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
                                    totalSendErr++;
                                }
                            }
                            else
                            {
                                mimeSentMessages.Add(mimeMessage);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        lstLoc.Add(_dsCon.Tables[0].Rows[0]["Tag"].ToString());
                        isUnscuss = true;

                        totalSendErr++;
                        emailLog.To = string.Empty;
                        emailLog.Status = 0;
                        emailLog.UsrErrMessage = "Email address does not exist for this location";
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                }
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

            if (isUnscuss)
            {
                strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Send all emails successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        //Print invoices which are not emailed to locations.
        try
        {
            DataTable dtInv = GetRecurringInvoices();
            //var rows = dtInv.AsEnumerable()
            //    .Where(x => x.Field<int>("IsExistsEmail").Equals(0));

            if (dtInv != null)
            {
                if (dtInv.Rows.Count > 0)
                {
                    string fileName = string.Empty;
                    if (dtInv.Rows.Count == 1)
                    {
                        int _ref = Convert.ToInt32(dtInv.Rows[0]["Ref"]);
                        fileName = string.Format("Invoice{0}.pdf", _ref.ToString());
                    }
                    else
                    {
                        fileName = "Invoices.pdf";
                    }

                    List<byte[]> invoicesToPrint = new List<byte[]>();
                    invoicesToPrint = PrintInvoice(dtInv);

                    if (invoicesToPrint != null)
                    {
                        byte[] buffer1 = null;
                        buffer1 = concatAndAddContent(invoicesToPrint);

                        Response.Clear();
                        MemoryStream ms = new MemoryStream(buffer1);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                        Response.Buffer = true;
                        ms.WriteTo(Response.OutputStream);
                        Response.End();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] GetReportAsAttachmentFile(DataTable dtInvoices)
    {
        try
        {
            DataTable dtInv = dtInvoices;

            if (dtInv != null)
            {
                if (dtInv.Rows.Count > 0)
                {
                    List<byte[]> invoicesToPrint = new List<byte[]>();
                    invoicesToPrint = PrintInvoice(dtInv);

                    if (invoicesToPrint != null)
                    {
                        byte[] buffer1 = null;
                        buffer1 = concatAndAddContent(invoicesToPrint);

                        return buffer1;
                    }
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            return null;
        }
    }

    private DataTable GetRecurringInvoices()
    {
        DataTable dtResult = new DataTable();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        if (Session["MSM"].ToString() == "TS" && Session["type"].ToString() != "c")
        {
            objProp_Contracts.isTS = 1;
        }

        int _sInvID = Int32.Parse(Request.QueryString["uid"].ToString());
        int _eInvID = Int32.Parse(Request.QueryString["eid"].ToString());
        for (int i = _sInvID; i <= _eInvID; i++)
        {
            objProp_Contracts.InvoiceID = Convert.ToInt32(i);
            var ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            if (dtResult.Rows.Count > 0)
            {
                dtResult.Merge(ds.Tables[0], true);
            }
            else
            {
                dtResult = ds.Tables[0];
            }
        }

        return dtResult;
    }

    #endregion

    #region Custom functions

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
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

        return ms.ToArray();
    }

    private StiReport GetInvoiceAsReport(DataTable dtInvoices)
    {
        // Export to PDF
        List<StiReport> reports = new List<StiReport>();
        DataTable mergedInvoice = new DataTable();
        DataTable mergedInvoiceItems = new DataTable();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = dtInvoices;
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();
            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();

            cTable.Rows.Add(cRow);

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                _dtInvoice = ds.Tables[0];


                int _days = 0;
                for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                {

                    #region Determine Pay Terms
                    if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                    {
                        _days = 0;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                    {
                        _days = 10;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                    {
                        _days = 15;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                    {
                        _days = 45;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                    {
                        _days = 60;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                    {
                        _days = 90;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                    {
                        _days = 180;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                    {
                        _days = 0;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                    {
                        _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                        _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                    }
                }

                DataTable _dtInvItems1 = GetInvoiceItems(_ref);

                mergedInvoice.Merge(_dtInvoice);
                mergedInvoiceItems.Merge(_dtInvItems1);

                string reportPathStimul = string.Empty;
                if (Session["InvoicesTemplateForLoad"] != null && !string.IsNullOrEmpty(Session["InvoicesTemplateForLoad"].ToString()))
                {
                    string reportName = Session["InvoicesTemplateForLoad"].ToString();
                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + reportName + ".mrt");
                }
                else
                {
                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
                }

                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                report.RegData("Invoices", _dtInvoice);
                report.RegData("CompanyDetails", cTable);
                report.RegData("Invoice_dtInvoice", ds.Tables[0]);
                report.RegData("Ticket_Company", dsC.Tables[0]);
                report.RegData("InvoiceItems", _dtInvItems1);
                report.Dictionary.Synchronize();
                report.Render();
                reports.Add(report);
            }

            return AggregateMultipleReports(reports);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private StiReport AggregateMultipleReports(List<StiReport> reports)
    {
        StiReport singleReport = new StiReport();
        singleReport.NeedsCompiling = false;
        singleReport.IsRendered = true;
        singleReport.ReportCacheMode = StiReportCacheMode.On;
        singleReport.RenderedPages.CanUseCacheMode = true;
        singleReport.RenderedPages.CacheMode = true;
        singleReport.RenderedPages.Clear();

        Stimulsoft.Report.Units.StiUnit newUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(singleReport.ReportUnit);
        singleReport.RenderedPages.Clear();

        foreach (StiReport report in reports)
        {
            singleReport.RenderedPages.Add(report.RenderedPages[0]);
        }

        return singleReport;
    }

    private List<byte[]> PrintInvoice(DataTable dtInvoices)
    {
        // Export to PDF
        List<StiReport> reports = new List<StiReport>();
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        DataTable mergedInvoice = new DataTable();
        DataTable mergedInvoiceItems = new DataTable();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = dtInvoices;
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            foreach (DataRow _dr in dtNew.Rows)
            {
                int _ref = Convert.ToInt32(_dr["Ref"]);

                objProp_Contracts.InvoiceID = _ref;
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                _dtInvoice = ds.Tables[0];


                int _days = 0;
                for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                {

                    #region Determine Pay Terms
                    if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                    {
                        _days = 0;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                    {
                        _days = 10;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                    {
                        _days = 15;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                    {
                        _days = 45;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                    {
                        _days = 60;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                    {
                        _days = 30;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                    {
                        _days = 90;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                    {
                        _days = 180;
                    }
                    else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                    {
                        _days = 0;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                    {
                        _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                        _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                    }
                }

                DataTable _dtInvItems1 = GetInvoiceItems(_ref);

                mergedInvoice.Merge(_dtInvoice);
                mergedInvoiceItems.Merge(_dtInvItems1);

                string reportPathStimul = string.Empty;
                if (Session["InvoicesTemplateForLoad"] != null && !string.IsNullOrEmpty(Session["InvoicesTemplateForLoad"].ToString()))
                {
                    string reportName = Session["InvoicesTemplateForLoad"].ToString();
                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + reportName + ".mrt");
                }
                else
                {
                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
                }

                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                DataSet companyLogo = new DataSet();
                companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());

                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();
                cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
                cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
                cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
                cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();
                cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
                cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
                cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
                cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
                cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();

                cTable.Rows.Add(cRow);

                report.RegData("Invoices", _dtInvoice);
                report.RegData("CompanyDetails", cTable);
                report.RegData("Invoice_dtInvoice", ds.Tables[0]);
                report.RegData("Ticket_Company", dsC.Tables[0]);
                report.RegData("InvoiceItems", _dtInvItems1);
                report.Dictionary.Synchronize();
                report.Render();
                reports.Add(report);
            }

            for (int i = 0; i < reports.Count; i++)
            {
                byte[] buffer1 = null;
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(reports[i], stream, settings);
                buffer1 = stream.ToArray();
                invoicesAsBytes.Add(buffer1);
            }

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private DataTable GetInvoiceItems(int _refId)
    {
        DataTable _dtItem = new DataTable();
        try
        {
            objProp_Contracts.InvoiceID = _refId;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (_dsItemDetails.Tables[0].Rows.Count < 1)
            {
                _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
            }
            else
                _dtItem = _dsItemDetails.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
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

    private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
    {
        DataRow _dr = _dt.NewRow();
        _dr["Ref"] = _idRef;
        _dr["Acct"] = 0;
        _dr["Quan"] = 0;
        _dr["fDesc"] = string.Empty;
        _dr["Price"] = 0.00;
        _dr["Amount"] = 0.00;
        _dr["STax"] = 0.00;
        _dr["billcode"] = string.Empty;
        _dr["staxAmt"] = 0.00;
        _dr["balance"] = 0.00;
        _dr["amtpaid"] = 0.00;
        _dr["total"] = 0.00;
        _dt.Rows.Add(_dr);

        return _dt;
    }

    private byte[] ExportReportToPDF(string reportName)
    {
        byte[] buffer1 = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(StiWebViewerRecInvoices.Report, stream, settings);
        buffer1 = stream.ToArray();
        return buffer1;
    }

    private byte[] ExportReportToPDF(string reportName, StiWebViewer stiWebViewer)
    {
        byte[] buffer1 = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(stiWebViewer.Report, stream, settings);
        buffer1 = stream.ToArray();
        return buffer1;
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

    private void ShowGstRate()
    {
        // For canadian company show GST rate in Invoice template.
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CustomName = "Country";
        DataSet dsCustom = objBL_General.getCustomFields(objGeneral);

        if (dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            {
                IsGst = true;
            }
        }
        ViewState["IsGst"] = IsGst;
    }
    #endregion

    protected void StiWebViewerRecInvoices_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        DataTable dtRecur = GetRecurringInvoices();
        e.Report = GetInvoiceAsReport(dtRecur);
    }

    protected void StiWebViewerRecInvoices_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void ddlInvoicesForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["InvoicesTemplateForLoad"] = ddlInvoicesForLoad.SelectedItem.Text.Trim();
    }
}