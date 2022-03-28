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
using BusinessEntity.Payroll;
using MOMWebApp;
using BusinessEntity.Utility;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;

public partial class CustomerStatementCollectionReport : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objContract = new Contracts();

    BL_Report objBL_Report = new BL_Report();

    BL_User objBL_User = new BL_User();
    User objPropUser = new User();

    DataSet _dsCom = new DataSet();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
    GetCustStatementInvByLocationParam _GetCustStatementInvByLocation = new GetCustStatementInvByLocationParam();
    AddEmailLogParam _AddEmailLog = new AddEmailLogParam();
    GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices = new GetCustomerStatementInvoicesParam();
    GetCustomerStatementCollectionParam _GetCustomerStatementCollection = new GetCustomerStatementCollectionParam();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["page"] != null && Request.QueryString["page"] == "invoices")
                {
                    HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
                }
                else
                {
                    HighlightSideMenu("cstmMgr", "lnkCollections", "cstmMgrSub");
                }

                if (Request.QueryString["includeCredit"] != null)
                {
                    chkIncludeCredit.Checked = Convert.ToBoolean(Request.QueryString["includeCredit"]);
                }

                if (Request.QueryString["includeCustomerCredit"] != null)
                {
                    chkIncludeCustomerCredit.Checked = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
                }

                objPropUser.ConnConfig = Session["config"].ToString();

                _dsCom = objBL_User.getControl(objPropUser);

                if (_dsCom.Tables[0].Rows.Count > 0)
                {
                    string address = _dsCom.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                    address += _dsCom.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                    address += _dsCom.Tables[0].Rows[0]["city"].ToString() + ", " + _dsCom.Tables[0].Rows[0]["state"].ToString() + ", " + _dsCom.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                    address += "Phone: " + _dsCom.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                    address += "Fax: " + _dsCom.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                    address += "Email: " + _dsCom.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                    address = "Please review the attached customer statement from: " + Environment.NewLine + Environment.NewLine + address;
                    ViewState["company"] = address;
                    txtBody.Text = address;
                }
            }
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
            var pageUrl = Request.QueryString["page"].ToString() + ".aspx";
            if (Request.QueryString["uid"] != null)
            {
                pageUrl += "?uid=" + Request.QueryString["uid"];
            }

            Response.Redirect(pageUrl);
        }
        else
        {
            Response.Redirect("iCollections.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var url = "CustomerStatementCollectionReport.aspx?includeCredit=" + chkIncludeCredit.Checked + "&includeCustomerCredit=" + chkIncludeCustomerCredit.Checked;

        Response.Redirect(url);
    }

    protected void StiWebViewerStatement_GetReport(object sender, StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    protected void StiWebViewerStatement_GetReportData(object sender, StiReportDataEventArgs e)
    {

    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/CustomerStatement.mrt");

            string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
            if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
            {
                reportPathStimul = Server.MapPath($"StimulsoftReports/{reportCustom}");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objContract.ConnConfig = connString;

            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"]);
            }

            #region Company Check
            objContract.UserID = Session["UserID"].ToString();
            _GetCustomerStatementCollection.UserID = Session["UserID"].ToString();

            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objContract.EN = 1;
                _GetCustomerStatementCollection.EN = 1;
            }
            else
            {
                objContract.EN = 0;
                _GetCustomerStatementCollection.EN = 0;
            }
            #endregion

            var includeCredit = true;
            var includeCustomerCredit = true;

            if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
            {
                includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["includeCustomerCredit"]))
            {
                includeCustomerCredit = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
            }

            objContract.LocationIDs = Convert.ToString(Session["ColLocationIds"]);

            DataSet ds = objBL_Contracts.GetCustomerStatementCollection(objContract, includeCredit, includeCustomerCredit);

            if (ds != null)
            {
                report.RegData("Invoices", ds.Tables[0]);

                DataSet dsItem = objBL_Contracts.GetCustomerStatementInvoicesByLocation(objContract, includeCredit);

                if (dsItem != null)
                {
                    report.RegData("InvoiceItem", dsItem.Tables[0]);
                }
            }

            report.CacheAllData = true;
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
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

    protected void lnkMailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");

            objContract.ConnConfig = Session["config"].ToString();
            _GetCustomerStatementCollection.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"]);
            }

            #region Company Check
            objContract.UserID = Session["UserID"].ToString();
            _GetCustomerStatementCollection.UserID = Session["UserID"].ToString();

            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objContract.EN = 1;
                _GetCustomerStatementCollection.EN = 1;
            }
            else
            {
                objContract.EN = 0;
                _GetCustomerStatementCollection.EN = 0;
            }
            #endregion

            var includeCredit = true;
            var includeCustomerCredit = true;

            if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
            {
                includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["includeCustomerCredit"]))
            {
                includeCustomerCredit = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
            }

            objContract.LocationIDs = Convert.ToString(Session["ColLocationIds"]);
            _GetCustomerStatementCollection.LocationIDs = Convert.ToString(Session["ColLocationIds"]);

            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetCustomerStatementCollectionViewModel> _lstGetCustomerStatementCollection = new List<GetCustomerStatementCollectionViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsReport_GetCustomerStatementCollection";

                _GetCustomerStatementCollection.includeCredit = includeCredit;
                _GetCustomerStatementCollection.includeCustomerCredit = includeCustomerCredit;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementCollection);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCustomerStatementCollection = serializer.Deserialize<List<GetCustomerStatementCollectionViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCustomerStatementCollectionViewModel>(_lstGetCustomerStatementCollection);
            }
            else
            {
                ds = objBL_Contracts.GetCustomerStatementCollection(objContract, includeCredit, includeCustomerCredit);
            }

            DataTable dtInv = ds.Tables[0];

            DataTable dt = dtInv.AsEnumerable()
                       .GroupBy(r => r.Field<int>("Loc"))
                       .Select(g => g.First())
                       .CopyToDataTable();

            string _fromEmail = WebBaseUtility.GetFromEmailAddress();

            int totalLocInList = dt != null ? dt.Rows.Count : 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
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

            _AddEmailLog.ConnConfig = Session["config"].ToString();
            _AddEmailLog.Function = "Customer Statement Report: Email All";
            _AddEmailLog.Screen = "Collections";
            _AddEmailLog.Username = Session["Username"].ToString();
            _AddEmailLog.SessionNo = Guid.NewGuid().ToString();


            //List<string> lstLoc = new List<string>();
            //string strLoc;
            //bool isUnscuss = false;
            //int mailCount = 0;
            foreach (DataRow _dr in dt.Rows)
            {
                string _toEmail = "";
                string _ccEmail = "";
                int loc = Convert.ToInt32(_dr["Loc"]);
                emailLog.Ref = loc;
                _AddEmailLog.Ref = loc;
                if (!string.IsNullOrEmpty(_dr["custom12"].ToString()))
                {
                    #region Generate Report

                    
                    DataTable dtInvoice = dtInv
                            .Select("Loc = " + loc)
                            .CopyToDataTable();
                    
                    //if (mailCount == 4)
                    //{
                    //    Thread.Sleep(10000);
                    //    mailCount = 0;
                    //}
                    #endregion

                    #region Email

                    _toEmail = _dr["custom12"].ToString();

                    if (!string.IsNullOrEmpty(_dr["custom13"].ToString()))
                    {
                        _ccEmail = _dr["custom13"].ToString();
                    }

                    List<string> _toEmaillst = new List<string>();
                    _toEmaillst = _toEmail.Split(';', ',').OfType<string>().ToList();
                    List<string> _ccEmaillst = new List<string>();
                    _ccEmaillst = _ccEmail.Split(';', ',').OfType<string>().ToList();

                    Mail mail = new Mail();
                    mail.From = _fromEmail;
                    mail.To = _toEmaillst;
                    mail.Cc = _ccEmaillst;

                    mail.Title = "Customer Statement - " + _dr["LocID"].ToString() + " " + _dr["locname"].ToString();

                    if (txtBody.Text.Trim() != string.Empty)
                    {
                        mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                    }
                    else
                    {
                        mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                    }
                    mail.attachmentBytes = GetReportAsAttachmentFile(dtInvoice);
                    mail.FileName = "Customer_Statement" + _dr["LocID"].ToString() + "_" + _todayDate + ".pdf";

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                    //mail.Send();
                    //mailCount = mailCount + 1;
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
                    //lstLoc.Add(_dr["locname"].ToString());
                    //isUnscuss = true;
                    totalSendErr++;
                    emailLog.To = string.Empty;
                    emailLog.Status = 0;
                    emailLog.UsrErrMessage = "Email address does not exist for this location";

                    _AddEmailLog.To = string.Empty;
                    _AddEmailLog.Status = 0;
                    _AddEmailLog.UsrErrMessage = "Email address does not exist for this location";

                    BL_EmailLog bL_EmailLog = new BL_EmailLog();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                    }
                    else
                    {
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                }
            }

            //if (isUnscuss)
            //{
            //    strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Send all emails successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //}

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

            if (sbdSentError.Length > 0)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true}});", true);
                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true}});", true);
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
                        var errMess = "There were " + totalSendErr + " could not be sent";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }

                    
                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out";
                    //if (totalSendErr > 0)
                    //{
                    //    str += "<br>Total " + totalSendErr + " failed";
                    //}
                    
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
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
                    invoicesToPrint = PrintStatement(dtInv);

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

    private List<byte[]> PrintStatement(DataTable dtInvoices)
    {
        // Export to PDF
        List<StiReport> reports = new List<StiReport>();
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        DataTable mergedInvoiceItems = new DataTable();

        string reportPathStimul = Server.MapPath("StimulsoftReports/CustomerStatement.mrt");
        string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
        if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
        {
            reportPathStimul = Server.MapPath($"StimulsoftReports/{reportCustom}");
        }

        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objContract.ConnConfig = Session["config"].ToString();
           
            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objContract.isTS = 1;
            }

            foreach (DataRow dr in dtInvoices.Rows)
            {
                int loc = Convert.ToInt32(dr["Loc"]);

                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                objPropUser.ConnConfig = Session["config"].ToString();
                var dsCom = objBL_User.getControl(objPropUser);

                objContract.ConnConfig = Session["config"].ToString();
                objContract.Loc = loc;

                _GetCustomerStatementInvoices.ConnConfig = Session["config"].ToString();
                _GetCustomerStatementInvoices.Loc = loc;

                var includeCredit = true;
                if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
                {
                    includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
                }

                DataSet dsItem = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustomerStatementInvoices = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoices";

                    _GetCustomerStatementInvoices.includeCredit = includeCredit;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementInvoices);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustomerStatementInvoices = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    dsItem = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustomerStatementInvoices);
                }
                else
                {
                    dsItem = objBL_Contracts.GetCustomerStatementInvoices(objContract, includeCredit);
                }

                report.CacheAllData = true;
                report.RegData("CompanyDetails", dsCom.Tables[0]);
                report.RegData("Invoices", dtInvoices);
                report.RegData("InvoiceItem", dsItem.Tables[0]);
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
}