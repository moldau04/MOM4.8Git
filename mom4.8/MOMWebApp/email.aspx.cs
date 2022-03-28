using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using OpenPop.Mime;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Text;
using HtmlAgilityPack;
using AjaxControlToolkit;
using System.Collections;
using System.Net.Configuration;
using System.Configuration;

public partial class email : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {

            ViewState["pathmailatt"] = null;
            if (Request.QueryString["aid"] != null)
            {
                ShowMsgBody(Request.QueryString["aid"].ToString());
            }
            else
            {
                pnlBody.Visible = false;
                replyHeader.Visible = false;
                pnlCompose.Visible = true;
                composeHeader.Visible = true;
                string RefID = string.Empty;
                if (Request.QueryString["op"] != null)
                {
                    RefID = "[OP-" + Request.QueryString["op"].ToString() + "] ";
                    txtBody.Text = "[***************PLEASE DO NOT EDIT THE SUBJECT WHILE REPLYING**************]";
                }
                txtSubject.Text = RefID;
                //txtEmailFrom.Text = WebBaseUtility.GetFromEmailAddress();
                GetSMTPUser();
                if (Request.QueryString["to"] != null)
                    txtEmail.Text = Request.QueryString["to"].ToString();
            }
            if (Request.QueryString["rol"] != null)
                hdnRol.Value = Request.QueryString["rol"].ToString();
            LoadContent();
        }
    }

    private void ShowMsgBody(string AID)
    {
        try
        {
            ifbody.Attributes["src"] = "mailbody.ashx?aid=" + AID;

            ViewState["AID"] = AID;
            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
            string filepath = savepathconfig + @"\mails\" + AID.ToString() + ".eml";
            FileInfo file = new FileInfo(filepath);
            Message message = Message.Load(file);
            lblFrom.Text = Convert.ToString(message.Headers.From.Address);
            lblTo.Text = Convert.ToString(string.Join(",", objgn.toStringArray(message.Headers.To)));
            lblCC.Text = Convert.ToString(string.Join(",", objgn.toStringArray(message.Headers.Cc)));
            lblSub.Text = Convert.ToString(message.Headers.Subject);
            lblSent.Text = Convert.ToString(message.Headers.DateSent);

            MessagePart selectedMessagePart = message.MessagePart;
            List<MessagePart> lstAttachments = message.FindAllAttachments();
            dlAttachments.DataSource = lstAttachments;
            dlAttachments.DataBind();

            #region body
            //string bodytext = string.Empty;
            //bodytext = "<BR/><BR/><hr>From: " + lblFrom.Text + "<BR/>";
            //bodytext += "To: " + lblTo.Text + "<BR/>";
            //if (lblCC.Text != string.Empty)
            //    bodytext += "CC: " + lblCC.Text + "<BR/>";
            //bodytext += "Sent: " + lblSent.Text + "<BR/>";
            //bodytext += "Subject: <B>" + lblSub.Text + "</B><BR/><BR/>";

            //if (selectedMessagePart.IsText)
            //{
            //    //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(selectedMessagePart.Body);
            //    bodytext += Encoding.UTF8.GetString(selectedMessagePart.Body);
            //}
            //else
            //{
            //    MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
            //    MessagePart plainTextPart = message.FindFirstPlainTextVersion();

            //    if (HTMLTextPart != null)
            //    {
            //        ////////ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(HTMLTextPart.Body);
            //        //bodytext += Encoding.UTF8.GetString(HTMLTextPart.Body);

            //        HtmlDocument doc = new HtmlDocument();
            //        doc.Load(new MemoryStream(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), HTMLTextPart.Body)));
            //        HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@src]");
            //        if (imgs != null)
            //        {
            //            foreach (HtmlNode img in imgs)
            //            {
            //                if (img.Attributes["src"].Value.StartsWith("CID:", StringComparison.CurrentCultureIgnoreCase))
            //                {
            //                    HtmlAttribute src = img.Attributes["src"];
            //                    string cid = src.Value.Split(':')[1];

            //                    foreach (MessagePart m in message.FindAllAttachments())
            //                    {
            //                        if (m.ContentId.Equals(cid, StringComparison.CurrentCultureIgnoreCase))
            //                        {
            //                            img.Attributes["src"].Value = "data:text/html;base64," + Convert.ToBase64String(m.Body);
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        bodytext += doc.DocumentNode.OuterHtml;  
            //    }
            //    else if (plainTextPart != null)
            //    {
            //        //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(plainTextPart.Body);
            //        bodytext += Encoding.UTF8.GetString(plainTextPart.Body);
            //    }
            //    else
            //    {
            //        List<MessagePart> textVersions = message.FindAllTextVersions();
            //        if (textVersions.Count >= 1)
            //        {
            //            //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(textVersions[0].Body);
            //            bodytext += Encoding.UTF8.GetString(textVersions[0].Body);
            //        }
            //        else
            //        {
            //            //ifbody.InnerText = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
            //        }
            //    }
            //}
            //ViewState["body"] = bodytext;
            #endregion

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void imgbtnAttachment_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = sender as LinkButton;
            string LBQ = btn.Text;
            string AID = ViewState["AID"].ToString();
            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
            string filepath = savepathconfig + @"\mails\" + AID.ToString() + ".eml";
            FileInfo file = new FileInfo(filepath);
            Message message = Message.Load(file);
            MessagePart selectedMessagePart = message.MessagePart;
            List<MessagePart> lstAttachments = message.FindAllAttachments();

            foreach (MessagePart msg in lstAttachments)
            {
                if (msg.FileName.Equals(LBQ))
                {
                    DownloadDocument(msg.Body, msg.ContentType.MediaType, msg.FileName);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DownloadDocument(byte[] data, string type, string filename)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = type;
            Response.AddHeader("content-disposition", "attachment;filename=\"" + filename + "\"");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(data);
            Response.End();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkReply_Click(object sender, EventArgs e)
    {
        ReplyMail(true);
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        if (txtEmail.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtEmailFrom.Text;

                // ES-33:Task#2
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
                if (txtEmailCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtEmailCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text.Trim();
                mail.Text = txtBody.Text;

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        mail.AttachmentFiles.Add(strpath);
                    }
                }

                mail.Send();

                var AID = System.Guid.NewGuid();

                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\mails\";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                string filename = AID.ToString() + ".eml";
                mail.SaveEML(savepath, filename);

                //foreach (string strpath in lst)
                //{
                //    DeleteFile(strpath);
                //}
                objGeneral.ConnConfig = Session["config"].ToString();
                objGeneral.userid = Convert.ToInt32(Session["UserID"]);
                objGeneral.From = txtEmailFrom.Text.Trim();
                objGeneral.to = txtEmail.Text.Trim();
                objGeneral.cc = txtEmailCc.Text.Trim();
                objGeneral.bcc = txtEmailBCC.Text.Trim();
                objGeneral.subject = txtSubject.Text.Trim();
                objGeneral.sentdate = System.DateTime.Now;
                //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
                objGeneral.Attachments = lst.Count;
                objGeneral.msgid = string.Empty;
                objGeneral.uid = 0;
                objGeneral.GUID = AID;
                objGeneral.type = 1;
                objGeneral.userid = Convert.ToInt32(Session["userid"]);
                objGeneral.AccountID = txtEmailFrom.Text.Trim();
                objGeneral.ConnConfig = Session["config"].ToString();
                objBL_General.AddEmails(objGeneral);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

                //objgn.ResetFormControlValues(pnlCompose);
                txtEmailBCC.Text = string.Empty;
                txtEmailCc.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtBody.Text = string.Empty;
                txtSubject.Text = string.Empty;

                ViewState["pathmailatt"] = null;

                if (Request.QueryString["aid"] != null)
                {
                    pnlCompose.Visible = false;
                }
                else
                {
                    string RefID = string.Empty;
                    if (Request.QueryString["op"] != null)
                    {
                        RefID = "[OP-" + Request.QueryString["op"].ToString() + "] ";
                        txtBody.Text = "[***************PLEASE DO NOT EDIT THE SUBJECT WHILE REPLYING**************]" + txtBody.Text;
                    }
                    txtSubject.Text = RefID;
                    dlAttachmentsDelete.DataSource = null;
                    dlAttachmentsDelete.DataBind();

                }
            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

    }
    protected void lnkFwd_Click(object sender, EventArgs e)
    {
        ReplyMail(false);
    }

    private void ReplyMail(bool IsReply)
    {
        objgn.ResetFormControlValues(pnlCompose);
        ViewState["pathmailatt"] = null;
        dlAttachmentsDelete.DataSource = null;
        dlAttachmentsDelete.DataBind();
        pnlCompose.Visible = true;

        txtBody.Text = replymailbody(ViewState["AID"].ToString());

        string RefID = string.Empty;
        if (Request.QueryString["op"] != null)
        {
            RefID = "[OP-" + Request.QueryString["op"].ToString() + "]";
            if (lblSub.Text.Contains(RefID))
            {
                RefID = string.Empty;
            }
            else
            {
                RefID += " ";
            }
            txtBody.Text = "[***************PLEASE DO NOT EDIT THE SUBJECT WHILE REPLYING**************]" + txtBody.Text;
        }

        if (Request.QueryString["pl"] != null)
        {
            RefID = "[PL-" + Request.QueryString["pl"].ToString() + "]";
            if (lblSub.Text.Contains(RefID))
            {
                RefID = string.Empty;
            }
            else
            {
                RefID += " ";
            }
            txtBody.Text = "[***************PLEASE DO NOT EDIT THE SUBJECT WHILE REPLYING**************]" + txtBody.Text;
        }

        if (IsReply)
        {
            txtEmail.Text = lblFrom.Text;
            txtEmailCc.Text = lblCC.Text;
            string subject = lblSub.Text;
            if (subject.Contains("RE:") || subject.Contains("FW:"))
            {
                subject = subject.Replace("RE:", string.Empty);
                subject = subject.Replace("FW:", string.Empty);
            }
            txtSubject.Text = "RE:" + RefID + subject;
        }
        else
        {
            txtEmail.Text = string.Empty;
            txtEmailCc.Text = string.Empty;
            string subject = lblSub.Text;
            if (subject.Contains("RE:") || subject.Contains("FW:"))
            {
                subject = subject.Replace("RE:", string.Empty);
                subject = subject.Replace("FW:", string.Empty);
            }
            txtSubject.Text = "FW:" + RefID + subject;
            FwdAttachment();
        }

        //txtBody.Text = ViewState["body"].ToString();        
        txtBody.Focus();
        txtEmailFrom.Text = WebBaseUtility.GetFromEmailAddress();
    }

    //private void GetFromEmailAddress()
    //{
    //    objGeneral.ConnConfig = Session["config"].ToString();
    //    objGeneral.userid = Convert.ToInt32(Session["UserID"]);
    //    DataSet dsEmailacc = objBL_General.GetEmailAcc(objGeneral);
    //    if (dsEmailacc.Tables[0].Rows.Count > 0)
    //    {
    //        txtEmailFrom.Text = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //    }
    //    else
    //    {
    //        System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
    //        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //        string username = mailSettings.Smtp.Network.UserName;
    //        txtEmailFrom.Text = username;
    //    }
    //}

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
        txtBody.Focus();

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

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    protected void imgDelAttach_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
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

    private void FwdAttachment()
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();

        ArrayList lstPath = new ArrayList();
        string AID = ViewState["AID"].ToString();
        string filepath = savepathconfig + @"\mails\" + AID.ToString() + ".eml";
        FileInfo file = new FileInfo(filepath);
        Message message = Message.Load(file);
        MessagePart selectedMessagePart = message.MessagePart;
        List<MessagePart> lstAttachments = message.FindAllAttachments();

        string savepath = savepathconfig + @"\mailattach\" + System.DateTime.Now.Ticks.ToString() + @"\";
        foreach (MessagePart msg in lstAttachments)
        {
            string filename = msg.FileName;
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
            File.WriteAllBytes(fullpath, msg.Body);

            if (ViewState["pathmailatt"] != null)
            {
                lstPath = (ArrayList)ViewState["pathmailatt"];
                lstPath.Add(fullpath);
            }
            else
            {
                lstPath.Add(fullpath);
            }
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
    }

    private string replymailbody(string AID)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string filepath = savepathconfig + @"\mails\" + AID.ToString() + ".eml";
        FileInfo file = new FileInfo(filepath);
        Message message = Message.Load(file);
        MessagePart selectedMessagePart = message.MessagePart;

        string bodytext = string.Empty;
        bodytext = "<BR/><BR/><hr>From: " + lblFrom.Text + "<BR/>";
        bodytext += "To: " + lblTo.Text + "<BR/>";
        if (lblCC.Text != string.Empty)
            bodytext += "CC: " + lblCC.Text + "<BR/>";
        bodytext += "Sent: " + lblSent.Text + "<BR/>";
        bodytext += "Subject: <B>" + lblSub.Text + "</B><BR/><BR/>";

        if (selectedMessagePart.IsText)
        {
            //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(selectedMessagePart.Body);
            bodytext += Encoding.UTF8.GetString(selectedMessagePart.Body);
        }
        else
        {
            MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
            MessagePart plainTextPart = message.FindFirstPlainTextVersion();

            if (HTMLTextPart != null)
            {
                ////////ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(HTMLTextPart.Body);
                //bodytext += Encoding.UTF8.GetString(HTMLTextPart.Body);

                HtmlDocument doc = new HtmlDocument();
                doc.Load(new MemoryStream(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), HTMLTextPart.Body)));
                HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@src]");
                if (imgs != null)
                {
                    foreach (HtmlNode img in imgs)
                    {
                        if (img.Attributes["src"].Value.StartsWith("CID:", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HtmlAttribute src = img.Attributes["src"];
                            string cid = src.Value.Split(':')[1];

                            foreach (MessagePart m in message.FindAllAttachments())
                            {
                                if (m.ContentId.Equals(cid, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    //img.Attributes["src"].Value = "data:image/jpeg;base64," + Convert.ToBase64String(m.Body);

                                    //string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                                    string savepath = savepathconfig + @"\Email_Embedded\";
                                    var ImgAID = System.Guid.NewGuid();
                                    string filename = ImgAID.ToString() + ".jpg";
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
                                    File.WriteAllBytes(fullpath, m.Body);

                                    string imageurladdress = System.Web.Configuration.WebConfigurationManager.AppSettings["imageurladdress"].Trim();
                                    img.Attributes["src"].Value = "http://" + imageurladdress + "/MobileServiceDocs/Email_Embedded/" + filename;
                                    //img.Attributes["src"].Value = "http://" + HttpContext.Current.Request.Url.Authority + "/MobileServiceDocs/Email_Embedded/" + filename;
                                    //img.Attributes["src"].Value = "email_embedimg.ashx?imgid=" + filename;
                                }
                            }
                        }
                    }
                }

                bodytext += doc.DocumentNode.OuterHtml;
            }
            else if (plainTextPart != null)
            {
                //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(plainTextPart.Body);
                bodytext += Encoding.UTF8.GetString(plainTextPart.Body);
            }
            else
            {
                List<MessagePart> textVersions = message.FindAllTextVersions();
                if (textVersions.Count >= 1)
                {
                    //ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(textVersions[0].Body);
                    bodytext += Encoding.UTF8.GetString(textVersions[0].Body);
                }
                else
                {
                    //ifbody.InnerText = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                }
            }
        }
        return bodytext;
    }

    //protected void File_Upload(object sender, AjaxFileUploadEventArgs e)
    //{
    //    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
    //    string savepath = savepathconfig + @"\mailattach\";
    //    string filename = e.FileName;
    //    string fullpath = savepath + filename;

    //    if (File.Exists(fullpath))
    //    {
    //        filename = objgn.generateRandomString(4) + "_" + filename;
    //        fullpath = savepath + filename;
    //    }

    //    if (!Directory.Exists(savepath))
    //    {
    //        Directory.CreateDirectory(savepath);
    //    }

    //    if (Session["pathmailatt"] != null)
    //    {
    //        ArrayList lstPath = (ArrayList)Session["pathmailatt"];
    //        lstPath.Add(fullpath);
    //        Session["pathmailatt"] = lstPath;
    //    }
    //    else
    //    {
    //        ArrayList lstPath = new ArrayList();
    //        lstPath.Add(fullpath);
    //        Session["pathmailatt"] = lstPath;           
    //    }

    //    //AjaxFileUpload1.SaveAs(fullpath);
    //}   
    protected void btnDiscard_Click(object sender, EventArgs e)
    {
        ArrayList lst = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lst = (ArrayList)ViewState["pathmailatt"];
        }
        foreach (string strpath in lst)
        {
            DeleteFile(strpath);
        }

        //objgn.ResetFormControlValues(pnlCompose);
        txtEmailBCC.Text = string.Empty;
        txtEmailCc.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtBody.Text = string.Empty;
        txtSubject.Text = string.Empty;

        ViewState["pathmailatt"] = null;

        if (Request.QueryString["aid"] != null)
        {
            pnlCompose.Visible = false;
        }
        else
        {
            string RefID = string.Empty;
            if (Request.QueryString["op"] != null)
            {
                RefID = "[OP-" + Request.QueryString["op"].ToString() + "] ";
                txtBody.Text = "[***************PLEASE DO NOT EDIT THE SUBJECT WHILE REPLYING**************]" + txtBody.Text;
            }
            txtSubject.Text = RefID;
            dlAttachmentsDelete.DataSource = null;
            dlAttachmentsDelete.DataBind();

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

    // ES-33:Task#2: new function
    //private void GetMailConfigurationFromUser(Mail mail)
    //{
    //    if(mail == null)
    //        mail = new Mail();
    //    mail.RequireAutentication = false;
    //    General _objGeneral = new General();
    //    _objGeneral.ConnConfig = Session["config"].ToString();
    //    _objGeneral.userid = Convert.ToInt32(Session["UserID"]);
    //    BL_General _objBL_General = new BL_General();
    //    DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
    //    if (dsEmailacc.Tables[0].Rows.Count > 0)
    //    {
    //        mail.RequireAutentication = true;
    //        mail.Username = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //        mail.Password = dsEmailacc.Tables[0].Rows[0]["OutPassword"].ToString();
    //        mail.SMTPHost = dsEmailacc.Tables[0].Rows[0]["OutServer"].ToString();
    //        mail.SMTPPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["OutPort"].ToString());
    //        mail.From = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //    }
    //}

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

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddEstimate.aspx?uid=" + Convert.ToString(Request.QueryString["uid"]));
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
    private void LoadContent()
    {
        try
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            //objProp_User.POID = Convert.ToInt32(Request.QueryString["id"]);
            #region Company Check
            objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
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
            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                //objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
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

                txtBody.Text = address;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

