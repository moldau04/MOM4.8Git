using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using Stimulsoft.Report;
using System.IO;
using System.Net.Configuration;
using System.Configuration;
using Telerik.Web.UI;
using System.Collections;
using System.Text;
using System.Web;
using AjaxControlToolkit;
using Stimulsoft.Report.Web;
using Stimulsoft.Report.Export;
using System.Web.UI.HtmlControls;

public partial class PreviewInvoice : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    GeneralFunctions objgn = new GeneralFunctions();
    BL_User objBL_User = new BL_User();
    BL_Report bL_Report = new BL_Report();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();
    BL_General objBL_General = new BL_General();
    General objGenerals = new General();
    Owner _objOwner = new Owner();

    protected DataTable dtBillingCodeData = new DataTable();
    protected DataTable dtProjectCodeData = new DataTable();

    GeneralFunctions objGeneral = new GeneralFunctions();

    Inv _objInv = new Inv();
    Transaction _objTrans = new Transaction();

    Journal _objJournal = new Journal();
    BL_JournalEntry _objBL_Journal = new BL_JournalEntry();

    Invoices _objInvoices = new Invoices();
    BL_Invoice objBL_Invoice = new BL_Invoice();

    Chart _objChart = new Chart();
    BL_Chart _objBL_Chart = new BL_Chart();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");

            LoadMailContent();
            // Fill data for selection emails form
            ddlSearch.SelectedValue = "0";
            txtSearch.Text = "";
            DataTable distributionList = WebBaseUtility.GetContactListOnExchangeServer();

            GetSMTPUser();

            if (ConfigurationManager.AppSettings["CustomerName"] == "PCE")
            {
                divShowItems.Visible = true;

                if (Request.QueryString["showItems"] != null)
                {
                    chkShowItems.Checked = Convert.ToBoolean(Request.QueryString["showItems"]);
                }
            }
        }
        else
        {
            var ss = hdnActiveDiv.Value;
            if (hdnActiveDiv.Value == "0")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "activeDiv", "cancel();", true);
            }
            else if (hdnActiveDiv.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "activeDiv", "OpenSendEmailWindow();", true);
            }
        }
    }

    private void LoadMailContent()
    {
        txtEmailFrom.Text = WebBaseUtility.GetFromEmailAddress();
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
        ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }
        DataTable cTable = (DataTable)Session["CompanyTable"];
        txtSubject.Text = "";// "Invoice from " + cTable.Rows[0]["CompanyName"].ToString();

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();
        txtEmailFrom.Text = objBL_User.getUserEmail(objPropUser);

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

        BL_General bL_General = new BL_General();
        EmailTemplate emailTemplate = new EmailTemplate();
        emailTemplate.ConnConfig = Session["config"].ToString();
        emailTemplate.Screen = "Invoice";
        emailTemplate.FunctionName = "Preview Invoice";
        string mailContent = bL_General.GetEmailTemplate(emailTemplate);


        StringBuilder content = new StringBuilder();
        // We will keep all previous as hard code things
        if (string.IsNullOrEmpty(mailContent))
        {
            content.AppendFormat("{0}", ds.Tables[0].Rows[0]["customerName"].ToString());
            content.AppendLine();
            content.AppendLine();
            content.Append("</br></br>");
            content.AppendLine("Thank you for giving us the opportunity to serve you. Attached to this email");
            content.AppendFormat("is invoice {0}.", Request.QueryString["uid"].ToString());
            content.AppendLine();
            content.AppendLine();
            content.Append("</br></br>");
            content.Append("Kind Regards,");
            content.AppendLine();
            content.AppendLine();
            content.Append("</br></br>");
        }
        else
        {
            mailContent = mailContent.Replace("{Invoice_CustomerName}", ds.Tables[0].Rows[0]["customerName"].ToString())
                .Replace("{Invoice_No}", Request.QueryString["uid"].ToString());
            content.Append(mailContent);
        }

        if (dsC.Tables[0].Rows.Count > 0)
        {
            if (Session["MSM"].ToString() != "TS")
            {
                if (txtEmailFrom.Text.Trim() == string.Empty)
                {
                    txtEmailFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                }
            }

            var address = WebBaseUtility.GetSignature();

            content.Append(address);

            //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
            //{
            //    User objPropUser = new User();
            //    BL_User objBL_User = new BL_User();
            //    objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            //    objPropUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            //    //var signature = objBL_User.GetDefaultUserEmailSignature(objPropUser);

            //    //if (string.IsNullOrEmpty(signature))
            //    //{
            //    //    var phone = "905.305.0195";
            //    //    if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
            //    //    {
            //    //        phone = dsC.Tables[0].Rows[0]["Phone"].ToString();
            //    //    }

            //    //    content.Append("<br/>Notes:  Please do not reply to this email with questions or follow up communication since this queue is not monitored by human operators.");
            //    //    content.Append("<br/>Call at " + phone + " and contact your account manager for further communication.");
            //    //}
            //}

            //address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + "<br />" + "<br />" + address;
            ViewState["company"] = content.ToString();
            ////txtBody.Content = address;
            txtBodyCKE.Text = content.ToString();
        }

        if (txtEmailFrom.Text.Trim() == string.Empty)
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            string username = mailSettings.Smtp.Network.UserName;
            txtEmailFrom.Text = username;
            ////txtFrom.ReadOnly = true;
        }
        bool IsGst = false;
        objGenerals.ConnConfig = Session["config"].ToString();
        objGenerals.CustomName = "Country";
        DataSet dsCustom = objBL_General.getCustomFields(objGenerals);

        if (dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            {
                IsGst = true;
            }
        }
        string subject = string.Empty;

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
        DataSet dsloc = new DataSet();
        dsloc = objBL_User.getLocationByID(objPropUser);
        if (dsloc.Tables[0].Rows.Count > 0)
        {
            if (Session["MSM"].ToString() != "TS")
            {
                txtEmail.Text = dsloc.Tables[0].Rows[0]["custom12"].ToString();
                txtEmailCc.Text = dsloc.Tables[0].Rows[0]["custom13"].ToString();
            }
            subject = dsloc.Tables[0].Rows[0]["tag"].ToString();
            //lblAttachmentName.Text = "Invoice.pdf";

        }
        //if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c" && Session["MSM"].ToString() != "TS")
        //{
        //    objPropUser.ConnConfig = Session["config"].ToString();
        //    objPropUser.Username = Session["username"].ToString();
        //    string strTo = objBL_User.getUserEmail(objPropUser);
        //    if (txtTo.Text.Trim() == string.Empty && strTo != string.Empty)
        //    {
        //        txtTo.Text = strTo;
        //    }
        //    else if (txtTo.Text.Trim() != string.Empty && strTo != string.Empty)
        //    {
        //        txtTo.Text += "," + strTo;
        //    }
        //}
        ViewState["subject"] = subject;
        txtSubject.Text = "Invoice " + Request.QueryString["uid"].ToString() + " - " + subject;

        string FileName = string.Format("Invoice{0}.pdf", Request.QueryString["uid"].ToString().Trim());
        ArrayList lstPath = new ArrayList();
        // TODO: Thomas: need to discuss with Harsh in this case
        // just comment for fixing ES-1253
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(FileName);
        }
        else
        {
            lstPath.Add(FileName);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        hdnFirstAttachement.Value = FileName;

    }

    protected void StiWebViewerInvoice_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = PrintInvoices();
    }

    protected void StiWebViewerInvoice_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

        // e.Report = PrintInvoices();
    }

    protected void StiWebViewerInvoice_ExportReport(object sender, StiExportReportEventArgs e)
    {
        if (e.Format == StiExportFormat.Pdf)
        {
            StiPdfExportSettings pdfSettings = e.Settings as StiPdfExportSettings;
            pdfSettings.ImageQuality = 100;
            pdfSettings.ImageResolution = 100;
            pdfSettings.ImageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;

        }
    }

    private StiReport PrintInvoices()
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsEquip = new DataSet();
            DataTable dtInstallationItems = new DataTable();

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            int _ref = Convert.ToInt32(Request.QueryString["uid"].ToString());

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            _dtInvoice = ds.Tables[0];
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

            int refId = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataTable _dtInvItems = GetInvoiceItems(refId);

            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["Logo"] = companyLogo.Tables[0].Rows[0]["Logo"];
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();
            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();
            cRow["GSTreg"] = companyLogo.Tables[0].Rows[0]["GSTreg"].ToString();
            cTable.Rows.Add(cRow);
            string configRepostName = "";
            // Template name
            if (ConfigurationManager.AppSettings["CustomerName"].ToString() == "Brock")
            {
                configRepostName = "InvoicesNewPreviewDefault-Brock.mrt";
            }
            else
            {
                configRepostName = ConfigurationManager.AppSettings["InvoicePreviewReport"].ToString();
            }


            // Load for Port City Installation template
            bool pceInstallation = false;

            if (ConfigurationManager.AppSettings["CustomerName"] == "PCE")
            {
                if (_dtInvoice.Rows.Count > 0)
                {
                    var _objUser = new User();
                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.SearchBy = string.Empty;

                    _objUser.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);
                    _objUser.InstallDate = string.Empty;
                    _objUser.ServiceDate = string.Empty;
                    _objUser.Price = string.Empty;
                    _objUser.Manufacturer = string.Empty;
                    _objUser.Status = -1;
                    _objUser.building = "All";

                    dsEquip = objBL_User.getElev(_objUser);

                    bool showItems = false;
                    if (Request.QueryString["showItems"] != null)
                    {
                        showItems = Convert.ToBoolean(Request.QueryString["showItems"]);
                    }

                    if (_dtInvoice.Rows[0]["TypeName"] != null && _dtInvoice.Rows[0]["TypeName"].ToString() == "Installation" && !showItems)
                    {
                        pceInstallation = true;
                        configRepostName = "InstallationInvoicesPreviewDefault-PortCity.mrt";
                        dtInstallationItems = GetPCEInstallationInvoiceItems(_dtInvoice, _dtInvItems);
                    }

                    var cBilling = _dtInvItems.Select("billcode like '%-C'");
                    if (cBilling.Count() > 0)
                    {
                        cTable.Rows[0]["CompanyAddress"] = "2652 Bonds Avenue, Suite 101";
                        cTable.Rows[0]["City"] = "North Charleston";
                        cTable.Rows[0]["State"] = "SC";
                        cTable.Rows[0]["Zip"] = "29405";
                        cTable.Rows[0]["Phone"] = "843-729-1006";
                    }
                }
            }

            // Load template
            string reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Invoices/{0}", configRepostName));
            StiReport report = new StiReport();

            report.Load(reportPathStimul);
            report.RegData("CompanyDetails", cTable);
            report.RegData("Invoices", _dtInvoice);
            report.RegData("Invoice_dtInvoice", ds.Tables[0]);
            report.RegData("Ticket_Company", dsC.Tables[0]);

            if (pceInstallation)
            {
                report.RegData("InvoiceItems", dtInstallationItems);
            }
            else
            {
                report.RegData("InvoiceItems", _dtInvItems);
            }

            if (report.DataSources.Contains("Equipments") && dsEquip.Tables.Count > 0)
            {
                report.RegData("Equipments", dsEquip.Tables[0]);
            }

            report.CacheAllData = true;
            report.Render();
            StiWebViewerInvoice.Report = report;

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataTable GetPCEInstallationInvoiceItems(DataTable dtInvoice, DataTable dtInvItems)
    {
        DataTable dtInstallationItems = dtInvItems.Clone();

        var positiveAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") > 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (positiveAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = dtInvoice.Rows[0]["fDesc"];
            dr["Amount"] = positiveAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        var negativeAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") < 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (negativeAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = "Deposit";
            dr["Amount"] = negativeAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        return dtInstallationItems;
    }

    private byte[] GetReportAsAttachment()
    {
        byte[] buffer1 = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(PrintInvoices(), stream, settings);
        buffer1 = stream.ToArray();
        return buffer1;
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

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("Logo");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        companyDetailsTable.Columns.Add("GSTreg");
        return companyDetailsTable;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        //
        if (Request.QueryString["redirect"] == null)
        {
            if (Session["type"].ToString() == "c")
            {
                Response.Redirect("invoices.aspx?fil=1");
            }
            else
            {
                Response.Redirect("AddInvoice.aspx?uid=" + Request.QueryString["uid"].ToString());
            }
        }
        else
        {
            Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
        }

    }

    protected void chkShowItems_OnCheckedChanged(object sender, EventArgs e)
    {
        var url = "PreviewInvoice.aspx?uid=" + Request.QueryString["uid"] + "&showItems=" + chkShowItems.Checked;

        if (Request.QueryString["redirect"] != null)
        {
            url += "&redirect=" + Request.QueryString["redirect"];
        }

        Response.Redirect(url);
    }

    protected void lnkEmail_Click(object sender, EventArgs e)
    {
        //LoadMailContent();
    }

    protected void LnkSend_Click(object sender, EventArgs e)
    {
        if (txtEmail.Text.Trim() != string.Empty)
        {
            try
            {
                var invoiceFileName = string.Format("Invoice{0}.pdf", Request.QueryString["uid"].ToString().Trim());

                DataTable cTable = (DataTable)Session["CompanyTable"];
                Mail mail = new Mail();
                mail.From = txtEmailFrom.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();

                StringBuilder sbdInValidEmails = new StringBuilder();

                foreach (var item in mail.To.ToList())
                {
                    if (!WebBaseUtility.IsValidEmailAddress1(item))
                    {
                        mail.To.Remove(item);
                        sbdInValidEmails.AppendFormat("{0}</br>", item);
                    }
                }

                if (txtEmailCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtEmailCc.Text.Split(';', ',').OfType<string>().ToList();
                    foreach (var item in mail.Cc.ToList())
                    {
                        if (!WebBaseUtility.IsValidEmailAddress1(item))
                        {
                            mail.Cc.Remove(item);
                            sbdInValidEmails.AppendFormat("{0}</br>", item);
                        }
                    }
                }
                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                    foreach (var item in mail.Bcc.ToList())
                    {
                        if (!WebBaseUtility.IsValidEmailAddress1(item))
                        {
                            mail.Bcc.Remove(item);
                            sbdInValidEmails.AppendFormat("{0}</br>", item);
                        }
                    }
                }

                mail.Title = txtSubject.Text;
                mail.Text = txtBodyCKE.Text;
                //mail.FileName = "Invoice.pdf";
                mail.FileName = invoiceFileName;
                mail.RequireAutentication = false;

                //mail.attachmentBytes = GetReportAsAttachment();
                // TODO: Thomas need to discuss with Harsh in this case
                // just comment for fixing ES-1253
                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = GetReportAsAttachment();
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];

                    foreach (string strpath in lst)
                    {
                        //if (strpath != "Invoice.pdf")
                        if (strpath != invoiceFileName)
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }
                // ES-33:Task#2
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                //mail.Send();

                Tuple<int, string, string> emailSendError = null;
                StringBuilder sbdSentError = new StringBuilder();

                EmailLog emailLog = new EmailLog();
                emailLog.ConnConfig = Session["config"].ToString();
                emailLog.Function = "Preview Invoice";
                emailLog.Screen = "Invoice";
                emailLog.Username = Session["Username"].ToString();
                emailLog.SessionNo = Guid.NewGuid().ToString();
                emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);

                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
                if (emailSendError != null && emailSendError.Item1 == 1)
                {
                    sbdSentError.Append(emailSendError.Item2);
                }
                else
                {
                    emailSendError = mail.SendSingleMailWithLog(mimeMessage, true, emailLog);
                    if (emailSendError != null)
                    {
                        if (emailSendError.Item1 == 1)
                        {
                            sbdSentError.Append(emailSendError.Item2);
                        }
                        else
                        {
                            sbdSentError.Append(emailSendError.Item2);
                        }
                    }
                }

                if (sbdSentError.Length > 0)
                {
                    string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue: true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);

                }

                if (sbdInValidEmails.Length > 0)
                {
                    sbdInValidEmails.Insert(0, "There is some invalid email address:</br>");
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
                }
            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
        else
        {
            string str = "To email address cannot be empty";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCloseMail_Click(object sender, EventArgs e)
    {
        Response.Redirect("PreviewInvoice.aspx?uid=" + Request.QueryString["uid"].ToString());
    }

    private void FillDistributionList(string searchType, string searchValue)
    {
        DataTable distributionList = new DataTable();
        DataTable distributionList1 = new DataTable();
        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            distributionList1.Columns.Add("MemberEmail");
            distributionList1.Columns.Add("MemberName");
            distributionList1.Columns.Add("GroupName");
            distributionList1.Columns.Add("Type");
            DataRow dr = distributionList1.NewRow();
            dr[0] = txtEmail.Text;
            dr[1] = txtEmail.Text;
            dr[2] = "";
            dr[3] = "";
            distributionList1.Rows.InsertAt(dr, 0);
        }
        distributionList = WebBaseUtility.GetContactListOnExchangeServer();
        distributionList.Merge(distributionList1);
        IEnumerable<DataRow> rowSources;

        var emailList = distributionList.Clone();
        switch (searchType)
        {
            case "1":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            case "2":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "3":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "4":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            default:
                //distributionList = distributionList.AsEnumerable().Distinct().OrderBy(e=>e.Field<string>("GroupName")).CopyToDataTable();
                rowSources = (from myRow in distributionList.AsEnumerable()
                              where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                              select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
        }

        if (rowSources.Any())
        {
            emailList = rowSources.CopyToDataTable();
        }
        else
        {
            emailList = distributionList.Clone();
        }

        lblRecordCount.Text = emailList.Rows.Count + " Record(s) found";
        RadGrid_Emails.DataSource = emailList;
        RadGrid_Emails.VirtualItemCount = emailList.Rows.Count;
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // ddlSearch_SelectedIndexChanged(sender, e);
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        //RadGrid_Emails.DataBind();
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression_PreviewInvoice"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression_PreviewInvoice"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression_PreviewInvoice"] != null && Convert.ToString(Session["Emails_FilterExpression_PreviewInvoice"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression_PreviewInvoice"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);

    }

    private void GetSMTPUser()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getSMTPByUserID(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            String emailFrom = "";
            emailFrom = Convert.ToString(ds.Tables[0].Rows[0]["From"]);
            if (emailFrom == "")
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                string user = section.Network.UserName;
                txtEmailFrom.Text = user;
            }
            else
            {
                txtEmailFrom.Text = emailFrom;
            }
            txtEmailBCC.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCCEmail"]);
            //txtEmailFrom.ReadOnly = true;


        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = FileUpload1.FileName;
        string fullpath = savepath + filename;

        if (File.Exists(fullpath))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
            fullpath = savepath + filename;
        }

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        FileUpload1.SaveAs(fullpath);


        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fullpath);
        }
        else
        {
            lstPath.Add(fullpath);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        txtBodyCKE.Focus();

    }

    protected void imgDelAttach_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        if (hdnFirstAttachement.Value == path)
        {
            hdnFirstAttachement.Value = "-1";
        }
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
        PrintInvoices();
    }
    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        //MoveFileEx(filepath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);

        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch //(System.IO.IOException e)
            {
                //Console.WriteLine(e.Message);
                //return;
            }
        }
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            string FileName = string.Format("Invoice{0}.pdf", Request.QueryString["uid"].ToString().Trim());
            if (DownloadFileName == FileName)
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(PrintInvoices(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
    //    //Customer objProp_Customer = new Customer();
    //    //BL_Customer objBL_Customer = new BL_Customer();

    //    //DataSet ds = new DataSet();
    //    //if (contextKey != string.Empty)
    //    //{
    //    //    objProp_Customer.ROL = Convert.ToInt32(contextKey);
    //    //}
    //    //objProp_Customer.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    //ds = objBL_Customer.getContactByRolID(objProp_Customer);

    //    //DataTable dt = ds.Tables[0];

    //    //List<string> txtItems = new List<string>();
    //    //String dbValues;

    //    //foreach (DataRow row in dt.Rows)
    //    //{
    //    //    dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString() + "(" + row["email"].ToString() + ")", row["email"].ToString());
    //    //    txtItems.Add(dbValues);
    //    //}

    //    //return txtItems.ToArray();
    //    //DataTable dt = (DataTable)HttpContext.Current.Session["DistributionList"];
    //    DataTable dt = WebBaseUtility.GetContactListOnExchangeServer();

    //    List<string> txtItems = new List<string>();
    //    String dbValues;

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["MemberName"].ToString() + "(" + row["MemberEmail"].ToString() + ")", row["MemberEmail"].ToString());
    //        txtItems.Add(dbValues);
    //    }

    //    return txtItems.ToArray();
    //}

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