using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using OpenPop.Common.Logging;
using Message = OpenPop.Mime.Message;
using System.Data;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using BusinessLayer;
using AjaxControlToolkit;
using System.Threading;

public partial class testmail : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    Pop3Client pop3Client;
    protected void Page_Load(object sender, EventArgs e)
    {
        //ReceiveMails();
        if (!IsPostBack)
        {
            ////txtHost.Text = "pop.gmail.com";
            ////txtPort.Text = "995";
            ////txtUsername.Text = "msmtryfor30@gmail.com";
            ////txtPass.Text = "ideavate@123";

            //GetMailsfromdb(0);
        }
    }

    private void DownloadMails(string host, string user, string pass, string port, int userid)
    {       
        pop3Client = new Pop3Client();

        try
        {
            if (pop3Client.Connected)
                pop3Client.Disconnect();

            pop3Client.Connect(host.Trim(), int.Parse(port.Trim()), true);
            pop3Client.Authenticate(user.Trim(), pass.Trim());

            int count = pop3Client.GetMessageCount();
            List<string> uids = pop3Client.GetMessageUids();

            DataSet ds = GetMsgUID(user.Trim());
            List<string> seenUids = ds.Tables[0].AsEnumerable()
                                   .Select(r => r.Field<string>("UID"))
                                   .ToList();

            for (int i = 0; i < uids.Count; i++)
            {
                string currentUidOnServer = uids[i];
                if (!seenUids.Contains(currentUidOnServer))
                {
                    try
                    {
                        Message unseenMessage = pop3Client.GetMessage(i + 1);

                        var AID = System.Guid.NewGuid();
                        objGeneral.From = Convert.ToString(unseenMessage.Headers.From.Address);
                        objGeneral.to = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.To)));
                        objGeneral.cc = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.Cc)));
                        objGeneral.bcc = Convert.ToString(string.Join(",", toStringArray(unseenMessage.Headers.Bcc)));
                        objGeneral.subject = Convert.ToString(unseenMessage.Headers.Subject);
                        objGeneral.sentdate = unseenMessage.Headers.DateSent;
                        //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
                        objGeneral.Attachments = unseenMessage.FindAllAttachments().Count();
                        objGeneral.msgid = Convert.ToString(unseenMessage.Headers.MessageId);
                        objGeneral.uid =Convert.ToInt32( currentUidOnServer);
                        objGeneral.GUID = AID;
                        objGeneral.type = 0;
                        objGeneral.userid = userid;
                        objGeneral.AccountID = user.Trim();

                        AddEmails();

                        //string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                        string savepath = @"C:\mails\in\";
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }
                        string filename = AID.ToString() + ".eml";
                        FileInfo file = new FileInfo(savepath + filename);
                        unseenMessage.Save(file);
                       
                    }
                    catch (Exception ex)
                    {
                        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                        throw ex;
                    }
                }
            }
        }
        catch (InvalidLoginException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
            throw new Exception("The server did not accept the user credentials!");
        }
        catch (PopServerNotFoundException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
            throw new Exception("The server could not be found");
        }
        catch (PopServerLockedException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
            throw new Exception("The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?");
        }
        catch (LoginDelayException)
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
            throw new Exception("Login not allowed. Server enforces delay between logins. Have you connected recently?");
        }
        catch (Exception ex)
        {
            throw ex;
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetMailsfromdb(int type)
    {
        //try
        //{
        //    objGeneral.type = type;
        //    DataSet ds = GetMails();
        //    gvmail.DataSource = ds.Tables[0];
        //    gvmail.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    
    private void ShowMsgBody(string AID)
    {
        try
        {
            ViewState["AID"] = AID;
            string filepath = @"C:\mails\in\" + AID.ToString() + ".eml";
            FileInfo file = new FileInfo(filepath);
            Message message = Message.Load(file);
            lblFrom.Text = Convert.ToString(message.Headers.From.Address);
            lblTo.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.To)));
            lblCC.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Cc)));
            lblBcc.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Bcc)));
            lblSub.Text = Convert.ToString(message.Headers.Subject);
            MessagePart selectedMessagePart = message.MessagePart;

            List<MessagePart> lstAttachments = message.FindAllAttachments();

            dlAttachments.DataSource = lstAttachments;
            dlAttachments.DataBind();

            if (selectedMessagePart.IsText)
            {
                ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(selectedMessagePart.Body);
            }
            else
            {
                MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
                MessagePart plainTextPart = message.FindFirstPlainTextVersion();

                if (HTMLTextPart != null)
                {
                    ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(HTMLTextPart.Body);
                }
                else if (plainTextPart != null)
                {
                    ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(plainTextPart.Body);
                }
                else
                {
                    List<MessagePart> textVersions = message.FindAllTextVersions();
                    if (textVersions.Count >= 1)
                        ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(textVersions[0].Body);
                    else
                        ifbody.InnerText = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                }
            }
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

            string filepath = @"C:\mails\in\" + AID.ToString() + ".eml";
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


    /*
    private void ReceiveMails()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Subject");
        dt.Columns.Add("From");
        dt.Columns.Add("To");
        //dt.Columns.Add("Cc");
        //dt.Columns.Add("Bcc");
        //dt.Columns.Add("Sent");
        dt.Columns.Add("Date");
        //dt.Columns.Add("Sender");
        dt.Columns.Add("msgid");
        //dt.Columns.Add("Attachments");

        pop3Client = new Pop3Client();

        try
        {
            if (pop3Client.Connected)
                pop3Client.Disconnect();

            pop3Client.Connect(txtHost.Text.Trim(), int.Parse(txtPort.Text.Trim()), true);
            pop3Client.Authenticate(txtUsername.Text.Trim(), txtPass.Text.Trim());

            int count = pop3Client.GetMessageCount();
            List<string> uids = pop3Client.GetMessageUids();

            for (int i = count; i >= 1; i -= 1)
            {
                try
                {
                    MessageHeader Headers = pop3Client.GetMessageHeaders(i);
                    string msgid = Headers.MessageId;

                    if (msgid.Contains(""))
                    {
                        DataRow dr = dt.NewRow();
                        dr["subject"] = Convert.ToString(Headers.Subject);
                        dr["from"] = Convert.ToString(Headers.From.Address);
                        dr["to"] = Convert.ToString(string.Join(",", toStringArray(Headers.To)));
                        //dr["cc"] = Convert.ToString(string.Join(",", toStringArray(Headers.Cc)));
                        //dr["bcc"] = Convert.ToString(string.Join(",", toStringArray(Headers.Bcc)));
                        //dr["sent"] = Convert.ToString(Headers.DateSent);
                        dr["date"] = Convert.ToString(Headers.Date);
                        //dr["sender"] = Convert.ToString(Headers.Sender);
                        dr["msgid"] = Convert.ToString(i); //Convert.ToString(Headers.MessageId); //message.MessagePart.GetBodyAsText().ToString();
                        //dr["attachments"] = Convert.ToString(message.FindAllAttachments().Count());

                        dt.Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                }
                //progressBar.Value = (int)(((double)(count - i) / count) * 100);
            }
            gvmail.DataSource = dt;
            gvmail.DataBind();
        }
        catch (InvalidLoginException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
        }
        catch (PopServerNotFoundException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
        }
        catch (PopServerLockedException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
        }
        catch (LoginDelayException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    private void ReceiveMessage(int msgid)
    {
        ViewState["msgid"] = msgid;
        pop3Client = new Pop3Client();

        try
        {
            if (pop3Client.Connected)
                pop3Client.Disconnect();

            pop3Client.Connect(txtHost.Text.Trim(), int.Parse(txtPort.Text.Trim()), true);
            pop3Client.Authenticate(txtUsername.Text.Trim(), txtPass.Text.Trim());

            int success = 0;
            int fail = 0;

            try
            {
                Message message = pop3Client.GetMessage(msgid);
                //FileInfo file = new FileInfo("someFile.eml");
                //Message message = Message.Load(file);
                lblFrom.Text = Convert.ToString(message.Headers.From.Address);
                lblTo.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.To)));
                lblCC.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Cc)));
                lblBcc.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Bcc)));
                lblSub.Text = Convert.ToString(message.Headers.Subject);
                MessagePart selectedMessagePart = message.MessagePart;

                List<MessagePart> lstAttachments = message.FindAllAttachments();

                dlAttachments.DataSource = lstAttachments;
                dlAttachments.DataBind();

                //FileInfo file = new FileInfo("someFile.eml");
                //message.Save(file);

                if (selectedMessagePart.IsText)
                {
                    //lblBody.Text = selectedMessagePart.GetBodyAsText();

                    ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(selectedMessagePart.Body);
                }
                else
                {
                    //// We are not able to show non-text MessageParts (MultiPart messages, images, pdf's ...)
                    //lblBody.Text = "<<OpenPop>> Cannot show this part of the email. It is not text <<OpenPop>>";

                    MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
                    MessagePart plainTextPart = message.FindFirstPlainTextVersion();

                    if (HTMLTextPart != null)
                    {
                        //// The message had a html/plain version - show that one
                        //lblBody.Text = HTMLTextPart.GetBodyAsText();
                        ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(HTMLTextPart.Body);
                    }
                    else if (plainTextPart != null)
                    {
                        // The message had a text/plain version - show that one
                        //lblBody.Text = plainTextPart.GetBodyAsText();

                        ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(plainTextPart.Body);
                    }
                    else
                    {
                        // Try to find a body to show in some of the other text versions
                        List<MessagePart> textVersions = message.FindAllTextVersions();
                        if (textVersions.Count >= 1)
                            //lblBody.Text = textVersions[0].GetBodyAsText();
                            ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(textVersions[0].Body);
                        else
                            ifbody.InnerText = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                    }
                }

                success++;
                //}
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

                fail++;
            }
        }
        catch (InvalidLoginException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
        }
        catch (PopServerNotFoundException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
        }
        catch (PopServerLockedException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
        }
        catch (LoginDelayException)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    //private void downloadclick()
    //{
    //    pop3Client = new Pop3Client();

    //    try
    //    {
    //        if (pop3Client.Connected)
    //            pop3Client.Disconnect();

    //        pop3Client.Connect(txtHost.Text.Trim(), int.Parse(txtPort.Text.Trim()), true);
    //        pop3Client.Authenticate(txtUsername.Text.Trim(), txtPass.Text.Trim());

    //        try
    //        {
    //            Message message = pop3Client.GetMessage(Convert.ToInt32(ViewState["msgid"]));
    //            MessagePart selectedMessagePart = message.MessagePart;
    //            List<MessagePart> lstAttachments = message.FindAllAttachments();

    //            LinkButton btn = sender as LinkButton;
    //            string LBQ = btn.Text;

    //            foreach (MessagePart msg in lstAttachments)
    //            {
    //                if (msg.FileName.Equals(LBQ))
    //                {
    //                    DownloadDocument(msg.Body, msg.ContentType.MediaType, msg.FileName);
    //                }
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //        }
    //    }
    //    catch (InvalidLoginException)
    //    {
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server did not accept the user credentials!", true);
    //    }
    //    catch (PopServerNotFoundException)
    //    {
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The server could not be found", true);
    //    }
    //    catch (PopServerLockedException)
    //    {
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", true);
    //    }
    //    catch (LoginDelayException)
    //    {
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "Login not allowed. Server enforces delay between logins. Have you connected recently?", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

   */

    protected void Button1_Click(object sender, EventArgs e)
    {
        //ReceiveMails();
        GetMailsfromdb(0);
    }
    protected void lnkSub_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnk.NamingContainer;
        Label lblMsgID = (Label)row.FindControl("lnkMsgID");
        ShowMsgBody(lblMsgID.Text);
        //ReceiveMessage(Convert.ToInt32( lblMsgID.Text.Trim()));
        pnlCompose.Visible = false;
        pnlBody.Visible = true;
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
    public static Message SaveAndLoadFullMessage(Message message)
    {
        // FileInfo about the location to save/load message
        FileInfo file = new FileInfo("someFile.eml");

        // Save the full message to some file
        message.Save(file);

        // Now load the message again. This could be done at a later point
        Message loadedMessage = Message.Load(file);

        // use the message again
        return loadedMessage;
    }
    private string[] toStringArray(List<RfcMailAddress> lst)
    {
        RfcMailAddress[] strRFC = lst.ToArray();
        string[] str = new string[lst.Count];
        int i = 0;
        foreach (RfcMailAddress rfc in lst)
        {
            str[i] = Convert.ToString(rfc.Address);
            i++;
        }
        return str;
    }


    /**********test********
    private void mailsend()
    {
        string to = "anandm1012@gmail.com";
        Mail mail = new Mail();
        mail.From = "anandm1012@gmail.com";
        mail.To = to.Split(';', ',').OfType<string>().ToList();
        mail.Title = "testeml";
        mail.Text = "emltest";
        mail.AttachmentFiles.Add(@"C:\payment.png");
        mail.DeleteFilesAfterSend = true;
        mail.RequireAutentication = true;

        var AID = System.Guid.NewGuid();
        string path = @"C:\inetpub\wwwroot\mailtest\";
        string filename = AID.ToString() + ".eml";
        mail.SaveEML(path, filename);
        mail.Send();
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        mailsend();
    }

    protected void btnEml_Click(object sender, EventArgs e)
    {
        FileInfo file = new FileInfo(@"C:\inetpub\wwwroot\mailtest\testeml.eml");
        Message message = Message.Load(file);

        lblFrom.Text = Convert.ToString(message.Headers.From.Address);
        lblTo.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.To)));
        lblCC.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Cc)));
        lblBcc.Text = Convert.ToString(string.Join(",", toStringArray(message.Headers.Bcc)));
        lblSub.Text = Convert.ToString(message.Headers.Subject);
        MessagePart selectedMessagePart = message.MessagePart;

        List<MessagePart> lstAttachments = message.FindAllAttachments();

        dlAttachments.DataSource = lstAttachments;
        dlAttachments.DataBind();

        if (selectedMessagePart.IsText)
        {
            ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(selectedMessagePart.Body);
        }
        else
        {
            MessagePart HTMLTextPart = message.FindFirstHtmlVersion();
            MessagePart plainTextPart = message.FindFirstPlainTextVersion();

            if (HTMLTextPart != null)
            {
                ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(HTMLTextPart.Body);
            }
            else if (plainTextPart != null)
            {
                ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(plainTextPart.Body);
            }
            else
            {
                List<MessagePart> textVersions = message.FindAllTextVersions();
                if (textVersions.Count >= 1)
                    ifbody.Attributes["src"] = "data:text/html;base64," + Convert.ToBase64String(textVersions[0].Body);
                else
                    ifbody.InnerText = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
            }
        }
    }

*/



    /**********SQL***********/
    public DataSet GetMsgUID(string user)
    {
        DataSet ds = new DataSet();
        try
        {
            return ds = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, "select UID from tblemail where accountid = '" + user + "' and type = 0");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetMails()
    {
        DataSet ds = new DataSet();
        try
        {
            return ds = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, "select * from tblemail where isnull(type,0)= " + objGeneral.type + " order by recdate desc");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void AddEmails()
    {
        try
        {
            SqlHelper.ExecuteNonQuery(Session["config"].ToString(), "spADDEmail", objGeneral.From, objGeneral.to, objGeneral.cc, objGeneral.bcc, objGeneral.subject, objGeneral.sentdate, objGeneral.Attachments, objGeneral.msgid, objGeneral.uid, objGeneral.GUID, objGeneral.type, objGeneral.userid, objGeneral.AccountID);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = GetEmailAccounts();
        
        if (Application["pop3"] == null)
        {
             Application["pop3"] = 0;
        }

        if ((int)Application["pop3"] == 0)
        {
            Application["pop3"] = 1;
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //Thread email = new Thread(delegate()
                    //   {
                    //try
                    //{
                    string host = dr["inserver"].ToString();
                    string user = dr["inusername"].ToString();
                    string pass = dr["inpassword"].ToString();
                    string port = dr["inport"].ToString();
                    int Userid = Convert.ToInt32(dr["Userid"]);
                    DownloadMails(host, user, pass, port, Userid);
                    //}
                    //catch(Exception ex)
                    //{
                    //    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
                    //}
                    //  });
                    //email.IsBackground = true;
                    //email.Start();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            }
            finally
            {
                Application["pop3"] = 0;
            }
        }
        else
        {
            
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'Mails already download',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);

        }

        GetMailsfromdb(0);
    }

    private void log(String message)
    {
        DateTime datetime = DateTime.Now;
        string savepath = Server.MapPath(Request.ApplicationPath) + "/logs/";
        String oFileName = savepath + "MOM_MailDown" + datetime.ToString("dd_MM_yyyy") + ".log";
        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        if (!File.Exists(oFileName))
        {
            System.IO.FileStream f = File.Create(oFileName);
            f.Close();
        }

        try
        {
            System.IO.StreamWriter writter = File.AppendText(oFileName);
            writter.WriteLine(datetime.ToString("MM-dd hh:mm") + " > " + message);
            writter.Flush();
            writter.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
    protected void lnkCompose_Click(object sender, EventArgs e)
    {
        pnlBody.Visible = false;
        pnlCompose.Visible = true;
    }

    protected void htmlEditorExtender1_ImageUploadComplete(object sender, AjaxFileUploadEventArgs e)
    {
        // Generate file path
        string filePath = "~/tempimg/" + e.FileName;

        // Save uploaded file to the file system
        var ajaxFileUpload = (AjaxFileUpload)sender;
        ajaxFileUpload.SaveAs(MapPath(filePath));

        // Update client with saved image path
        e.PostedUrl = Page.ResolveUrl(filePath);
    }

    protected void btnSendMAil_Click(object sender, EventArgs e)
    {
        if (txtEmail.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtEmailFrom.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
                if (txtEmailCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtEmailCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text.Trim();
                mail.Text = txtBody.Text;
                mail.RequireAutentication = true;
                mail.Send();


                var AID = System.Guid.NewGuid();

                string savepath = @"C:\mails\in\";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                string filename = AID.ToString() + ".eml";
                mail.SaveEML(savepath, filename);


                objGeneral.From = txtEmailFrom.Text.Trim();
                objGeneral.to = txtEmail.Text.Trim();
                objGeneral.cc = txtEmailCc.Text.Trim();
                objGeneral.bcc = string.Empty;
                objGeneral.subject = txtSubject.Text.Trim();
                objGeneral.sentdate = System.DateTime.Now;
                //objGeneral.date = Convert.ToString(unseenMessage.Headers.Date);
                objGeneral.Attachments = 0;
                objGeneral.msgid = string.Empty;
                objGeneral.uid = 0;
                objGeneral.GUID = AID;
                objGeneral.type = 1;
                objGeneral.AccountID = txtEmailFrom.Text.Trim();

                objBL_General.AddEmails(objGeneral);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    protected void lnkOutbox_Click(object sender, EventArgs e)
    {
        GetMailsfromdb(1);
    }
    protected void lnkInbox_Click(object sender, EventArgs e)
    {
        GetMailsfromdb(0);
    }

    public DataSet GetEmailAccounts()
    {
        try
        {
            return SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, "select ea.* from tblEmailAccounts ea inner join tbluser u on u.id=ea.userid where u.status=0 and u.emailaccount=1");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
