using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EmailSender : System.Web.UI.Page
{
    User objProp_User = new User();
    BL_User objBL_User = new BL_User();
    GeneralFunctions objgn = new GeneralFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetSMTPUser();
            BindAttchments();

            txtTo.Text = Convert.ToString(Session["SelectedEstimate_Email"]);
            txtBody.Text = Convert.ToString(Session["SelectedEstimate_Contact"]);

            #region Collection Screen
            if (Convert.ToString(Request.QueryString["pagetype"]) == "collection")
            {
                txtTo.Text = Convert.ToString(Session["CollectionSendMail"]);
                txtBody.Text = "";
            }

            #endregion
            LoadContent();
            FillDistributionList("0", "");
        }

    }

    private void GetSMTPUser()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getSMTPByUserID(objProp_User);
        if (ds.Tables[0].Rows.Count > 0)
        {
            String emailFrom = "";
            emailFrom = Convert.ToString(ds.Tables[0].Rows[0]["From"]);
            if (emailFrom == "")
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                //string from = section.From;
                //string host = section.Network.Host;
                //int port = section.Network.Port;
                //bool enableSsl = section.Network.EnableSsl;
                //string password = section.Network.Password;
                string user = section.Network.UserName;
                txtFrom.Text = user;
            }
            else
            {
                txtFrom.Text = emailFrom;
            }
            txtBCC.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCCEmail"]);
            //txtFrom.ReadOnly = true;
        }
    }

    private void BindAttchments()
    {
        List<BusinessEntity.MailSender> list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];

        if (list != null && list.Count > 0)
        {
            rptAttachments.DataSource = list;
            rptAttachments.DataBind();
        }
    }

    protected void rptAttachments_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete" && e.CommandArgument.ToString() != "")
        {
            List<BusinessEntity.MailSender> list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
            if (list != null && list.Count > 0)
            {
                Int32 ID = Convert.ToInt32(e.CommandArgument);

                var query = (list.Where(x => x.ID == ID)).FirstOrDefault();

                list.Remove(query);

                rptAttachments.DataSource = list;
                rptAttachments.DataBind();

                Session["SelectedEstimateTemplate"] = list;
            }
        }
    }

    protected void btnDiscard_Click(object sender, EventArgs e)
    {
        txtBCC.Text = "";
        txtCC.Text = "";
        txtTo.Text = "";
        txtSubject.Text = "";
        txtBody.Text = "";
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
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
                mail.Text = "";
            }

            if (Convert.ToString(Request.QueryString["pagetype"]) == "collection")
            {
                CollectionAttachmentFile(mail);
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
            }
            else
            {
                List<BusinessEntity.MailSender> list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
                mail.Attachments = new List<Attachment>();
                if (list != null)
                {
                    foreach (var data in list)
                    {
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(data.PDFFilePath);
                        attachment.Name = data.FileName;
                        mail.Attachments.Add(attachment);
                    }
                }

                Tuple<int, string, string> emailSendError = null;
                StringBuilder sbdSentError = new StringBuilder();

                EmailLog emailLog = new EmailLog();
                emailLog.ConnConfig = Session["config"].ToString();
                emailLog.Function = "Email Proposal";
                emailLog.Screen = "Estimate";
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
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    if (Request.QueryString["uid"] != null && Convert.ToString(Request.QueryString["uid"]) != "")
                    {
                        List<BusinessEntity.MailSender> list1 = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];

                        #region Update Values In EstimateTemplate Table
                        BL_EstimateForm objBL_EF = new BL_EstimateForm();
                        EstimateForm objEF = new EstimateForm();
                        objEF.ConnConfig = Session["config"].ToString();
                        List<BusinessEntity.MailSender> listEstimateTemplate = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
                        foreach (var data in list1)
                        {
                            objEF.Id = data.ID;
                            objBL_EF.UpdateEstimateForm(objEF, txtTo.Text, txtFrom.Text, Session["Username"].ToString());
                        }
                        #endregion
                    }
                }

                //WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                //mail.Send();
            }

            // ES-33:Task#2
            //WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
            //mail.Send();

            

            #region Delete Collection Files
            try
            {
                if (Convert.ToString(Request.QueryString["pagetype"]) == "collection")
                {
                    List<BusinessEntity.MailSender> list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
                    foreach (var data in list)
                    {
                        String InvoiceID = data.Name;
                        string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices_" + InvoiceID + ".pdf");
                        if (File.Exists(filename))
                            File.Delete(filename);
                    }
                }
            }
            catch (Exception)
            {

            }

            #endregion

            // Response.Redirect("addestimate.aspx?uid=" + Request.QueryString["uid"]);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'The mail has been sent successfully !', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddEstimate.aspx?uid=" + Convert.ToString(Request.QueryString["uid"]));
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
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
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

    protected void lnkFileNameDownload_Click(object sender, EventArgs e)
    {
        try
        {
            //EstimateForm objEF = new EstimateForm();
            //BL_EstimateForm objBL_EF = new BL_EstimateForm();
            //objEF.ConnConfig = Session["config"].ToString();
            //LinkButton btn = (LinkButton)sender;
            //objEF.Id = Convert.ToInt32("0" + btn.CommandArgument);
            //objBL_EF.GetEstimateFormById(objEF);
            //String newFileName = objEF.FileName.Split('.').FirstOrDefault() + "-" + objEF.Estimate + "-" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(objEF.AddedOn)).Replace("/", "") + "." + objEF.FileName.Split('.').LastOrDefault();
            //DownloadDocument(objEF.PdfFilePath, newFileName.Replace(".docx", ".pdf"));

            List<BusinessEntity.MailSender> list = new List<BusinessEntity.MailSender>();
            if (Session["SelectedEstimateTemplate"] != null)
            {
                list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
                LinkButton btn = (LinkButton)sender;
                foreach (MailSender item in list)
                {
                    if (item.ID == Convert.ToInt32("0" + btn.CommandArgument))
                    {
                        DownloadDocument(item.PDFFilePath, item.FileName);
                    }
                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void CollectionAttachmentFile(Mail mail)
    {
        try
        {
            #region Invoice
            List<BusinessEntity.MailSender> list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
            foreach (var data in list)
            {
                if (data.PDFFilePath == "Invoice")
                {
                    String InvoiceID = data.Name;

                    StiWebViewer rvInvoices = new StiWebViewer();
                    byte[] buffer = null;

                    List<byte[]> invoicesToPrint = PrintInvoicesForIndivudial(rvInvoices, Convert.ToInt32(InvoiceID));
                    string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices_" + InvoiceID + ".pdf");
                    if (invoicesToPrint != null)
                    {

                        buffer = concatAndAddContent(invoicesToPrint);
                        if (File.Exists(filename))
                            File.Delete(filename);
                        using (var fs = new FileStream(filename, FileMode.Create))
                        {
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }

                        System.IO.File.WriteAllBytes(filename, buffer);
                        mail.AttachmentFiles.Add(filename);
                    }
                }
            }
            #endregion

            #region  Customer Statement
            String CustID = Convert.ToString(Session["CollectionCustID"]);
            String LocID = Convert.ToString(Session["CollectionLocID"]);
            if (LocID != "")
            {
                Contracts objProp_Contracts = new Contracts();
                BL_Contracts objBL_Contracts = new BL_Contracts();

                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                foreach (var data in list)
                {
                    if (data.PDFFilePath == "CustomerStatement")
                    {
                        DataSet dsC = new DataSet();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        if (Session["MSM"].ToString() != "TS")
                        {
                            dsC = objBL_User.getControl(objPropUser);
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(LocID);
                            dsC = objBL_User.getControlBranch(objPropUser);
                        }


                        objProp_Contracts.ConnConfig = Session["config"].ToString();
                        objProp_Contracts.Loc = Convert.ToInt32(LocID);

                        DataSet ds = objBL_Contracts.GetCustomerStatementByLoc(objProp_Contracts);
                        DataTable dtLoc = ds.Tables[0];
                        ViewState["Invoices"] = dtLoc;
                        ViewState["CollectionLoc"] = LocID;
                        ReportViewer rvCs = new ReportViewer();

                        rvCs.LocalReport.DataSources.Clear();

                        rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                        rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
                        rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));

                        string reportPath;
                        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
                        {
                            reportPath = "Reports/CustomerStatementSouthern.rdlc";
                        }
                        else
                        {
                            reportPath = "Reports/CustomerStatement.rdlc";
                        }

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


                        mail.attachmentBytes = ExportReportToPDF1("", rvCs);
                        mail.FileName = "CustomerStatement.pdf";
                    }
                }
            }
            #endregion
        }
        catch (Exception)
        {

        }

    }

    private void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataTable dt = (DataTable)ViewState["Invoices"];
            int loc = Convert.ToInt32(ViewState["CollectionLoc"]);

            objProp_Contracts.Loc = loc;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
            {
                ds = objBL_Contracts.GetCustomerStatementInvoicesSouthern(objProp_Contracts, true);
            }
            else
            {
                ds = objBL_Contracts.GetCustomerStatementInvoices(objProp_Contracts, true);
            }


            if (dt.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dsInvoiceItem", ds.Tables[0]);

                e.DataSources.Add(rdsItems);
            }

        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
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
        //Return just before disposing
        return ms.ToArray();
    }

    private List<byte[]> PrintInvoicesForIndivudial(StiWebViewer rvInvoices, Int32 Ref)
    {

        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();

            int _ref = Ref;

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
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            //rvInvoices.LocalReport.DataSources.Clear();
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];

            DataTable _dtInvItems1 = GetInvoiceItems(Ref);


            string reportPathStimul = string.Empty;

            reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + Convert.ToString(Session["CollectionInvoiceTemplate"]).Trim() + ".mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            report.Compile();

            DataSet companyLogo = new DataSet();
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
            byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
            string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
            FileStream fs = new FileStream(strfn,
                              FileMode.CreateNew, FileAccess.Write);
            fs.Write(barrImg, 0, barrImg.Length);
            fs.Flush();
            fs.Close();


            System.Uri uri = new Uri(strfn);
            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["LogoURL"] = uri.AbsolutePath;
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

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";


            DataSet Invoices = new DataSet();
            DataTable dtInvoice1 = _dtInvoice.Copy();
            dtInvoice1.TableName = "Invoices";
            Invoices.Tables.Add(dtInvoice1.Copy());
            Invoices.DataSetName = "Invoices";

            DataSet InvoiceItems = new DataSet();
            DataTable dtIInvItems = _dtInvItems1.Copy();
            dtIInvItems.TableName = "InvoiceItems";
            InvoiceItems.Tables.Add(dtIInvItems);
            InvoiceItems.DataSetName = "InvoiceItems";


            DataSet Ticket_Company = new DataSet();
            DataTable dtTicketCompany = new DataTable();
            dtTicketCompany = dsC.Tables[0].Copy();
            Ticket_Company.Tables.Add(dtTicketCompany);
            dtTicketCompany.TableName = "Ticket_Company";
            Ticket_Company.DataSetName = "Ticket_Company";


            DataSet Invoice_dtInvoice = new DataSet();
            DataTable dtInvoice = new DataTable();
            dtInvoice = ds.Tables[0].Copy();
            Invoice_dtInvoice.Tables.Add(dtInvoice);
            dtInvoice.TableName = "Invoice_dtInvoice";
            Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

            report.RegData("Invoices", Invoices);
            report.RegData("CompanyDetails", CompanyDetails);

            report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

            report.RegData("Ticket_Company", Ticket_Company);
            report.RegData("InvoiceItems", InvoiceItems);
            report.Dictionary.Synchronize();
            report.Render();
            rvInvoices.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvInvoices.Report, stream, settings);
            buffer1 = stream.ToArray();
            invoicesAsBytes.Add(buffer1);

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
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();


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
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        return companyDetailsTable;
    }

    private void FillDistributionList(string searchType, string searchValue)
    {
        DataTable distributionList = new DataTable();
        DataTable distributionList1 = new DataTable();
        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            distributionList1.Columns.Add("MemberEmail");
            distributionList1.Columns.Add("MemberName");
            distributionList1.Columns.Add("GroupName");
            distributionList1.Columns.Add("Type");
            DataRow dr = distributionList1.NewRow();
            dr[0] = txtTo.Text;
            dr[1] = txtTo.Text;
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
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
        //UpdateSelectedRows
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // ddlSearch_SelectedIndexChanged(sender, e);
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //protected void radOpen_Click(object sender, EventArgs e)
    //{
    //    //business logic goes here

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "OPenPopupWindow", "OpenEmailsSelectionWindow();", true);
    //}
    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
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
            //FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //protected override void OnPreRender(EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "pageLoadHandler", startscript + endscript, false);

    //    base.OnPreRender(e);
    //}

    protected void RadGrid_Emails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
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

    private void LoadContent()
    {
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            //objProp_User.POID = Convert.ToInt32(Request.QueryString["id"]);
            #region Company Check
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            //if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            //{
            //    _objPO.EN = 1;
            //}
            //else
            //{
            //    _objPO.EN = 0;
            //}
            #endregion

            DataSet dsC = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objProp_User);
            }
            else
            {
                //objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objProp_User);
            }
            if (dsC.Tables[0].Rows.Count > 0)
            {
                string address = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
                {
                    address += dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
                {
                    address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
                {
                    address += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
                {
                    address += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
                {
                    address += dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
                {
                    address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
                {
                    address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
                {
                    if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                        address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine + "<br />";
                }

                address = Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CustomerName"]) && ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("AlternateElevator", StringComparison.InvariantCultureIgnoreCase))
                {
                    txtBody.Text = "Thank you for considering Alternate Elevator Sales & Services for your elevator repairs. </br>" +
                                "The estimate you requested is attached.Please review it and feel free to contact us if you have any questions.</br>" +
                                "We look forward to working with you.</br></br>" +
                                "Sincerely" + address;
                }
                else
                {
                    txtBody.Text = address;
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = flpFile.FileName;
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
        flpFile.SaveAs(fullpath);
        List<BusinessEntity.MailSender> list = new List<BusinessEntity.MailSender>();
        Int32 ID = new Random().Next();
        if (Session["SelectedEstimateTemplate"] != null)
        {
            list = (List<BusinessEntity.MailSender>)Session["SelectedEstimateTemplate"];
            if (list != null && list.Count > 0)
            {
                list.Add(new BusinessEntity.MailSender()
                {
                    ID = ID,
                    PDFFilePath = fullpath,
                    FileName = filename
                });

                rptAttachments.DataSource = list;
                rptAttachments.DataBind();

                Session["SelectedEstimateTemplate"] = list;
            }
            else
            {
                list = new List<BusinessEntity.MailSender>();
                list.Add(new BusinessEntity.MailSender()
                {
                    ID = ID,
                    PDFFilePath = fullpath,
                    FileName = filename
                });
            }
        }
        else
        {
            list = new List<BusinessEntity.MailSender>();
            list.Add(new BusinessEntity.MailSender()
            {
                ID = ID,
                PDFFilePath = fullpath,
                FileName = filename
            });
        }

        Session["SelectedEstimateTemplate"] = list;
        rptAttachments.DataSource = list;
        rptAttachments.DataBind();
        txtBody.Focus();
    }
}