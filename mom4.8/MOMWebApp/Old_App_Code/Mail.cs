using BusinessEntity;
using BusinessLayer;
using HtmlAgilityPack;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;

/// <summary>
/// Send mail with attachement.
/// </summary>
public class Mail
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string From { get; set; }

    public string SenderAddress { get; set; }
    public bool RequireAutentication { get; set; }
    public bool DeleteFilesAfterSend { get; set; }

    public List<string> To { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public List<string> AttachmentFiles { get; set; }
    public byte[] attachmentBytes { get; set; }
    public List<Attachment> Attachments { get; set; }
    public string FileName { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string SMTPHost { get; set; }
    public int SMTPPort { get; set; }

    public string InUsername { get; set; }
    public string InPassword { get; set; }
    public string InHost { get; set; }
    public int InPort { get; set; }

    string[] CommonSentFolderNames = { "sent items", "sent mail", "sent", "sent messages"/* maybe add some translated names */ };

    public bool TakeASentEmailCopy { get; set; }

    public bool IsBodyHtml { get; set; }
    public MailPriority Priority { get; set; }

    public bool SSL { get; set; }

    public bool IsIncludeSignature { get; set; }

    MailKit.IMailFolder GetSentFolder(MailKit.Net.Imap.ImapClient client, CancellationToken cancellationToken)
    {
        var personal = client.GetFolder(client.PersonalNamespaces[0]);

        string commonSentFolderNamesFromWebConfig = System.Configuration.ConfigurationManager.AppSettings["CommonSentFolderNames"];
        string[] commonSentFolderNamesFromWebConfigArr;
        if (!string.IsNullOrEmpty(commonSentFolderNamesFromWebConfig))
        {
            commonSentFolderNamesFromWebConfigArr = commonSentFolderNamesFromWebConfig.ToLower().Split(',').Select(e => e.Trim()).ToArray();
            CommonSentFolderNames = CommonSentFolderNames.Union(commonSentFolderNamesFromWebConfigArr).ToArray<string>();
        }

        foreach (var folder in personal.GetSubfolders(false, cancellationToken))
        {
            foreach (var commonName in CommonSentFolderNames)
            {
                if (folder.Name.Equals(commonName, StringComparison.InvariantCultureIgnoreCase))
                    return folder;
            }
        }

        return null;
    }

    public Mail()
    {
        To = new List<string>();
        Cc = new List<string>();
        Bcc = new List<string>();
        AttachmentFiles = new List<string>();
        From = "";
        attachmentBytes = null;
        FileName = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        SMTPHost = string.Empty;
        SMTPPort = 0;
        TakeASentEmailCopy = false;
        IsBodyHtml = true;
        Priority = MailPriority.High;
        SSL = false;
        IsIncludeSignature = false;
    }

    public void SendEx()
    {
        var message = new MimeKit.MimeMessage();
        message.From.Add(new MimeKit.MailboxAddress("Joey Tribbiani", "thomas@perrydomain.ddns.net"));

        message.To.Add(new MimeKit.MailboxAddress("Mrs. Chanandler Bong", "thomas@enclave.vn"));
        //message.Sender = new MailboxAddress("thomas@perrydomain.ddns.net");
        //message.From.Clear();
        //message.From.Add(new MailboxAddress(From));
        //message.ReplyTo.Clear();
        //message.ReplyTo.Add(new MailboxAddress(From));
        //message.Subject = "How you doin'?";

        message.Body = new MimeKit.TextPart("plain")
        {
            Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                client.Connect("smtp.perrydomain.ddns.net", 587, SecureSocketOptions.StartTls);
            }
            catch(Exception ex)
            {
                client.Connect("smtp.perrydomain.ddns.net", 587, SecureSocketOptions.None);
            }

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.AuthenticationMechanisms.Remove("NTLM");

            // Note: only needed if the SMTP server requires authentication
            //client.Authenticate("thomashle@outlook.com", "leho@ngth@o");
            client.Authenticate("thomas@perrydomain.ddns.net", "test@123");

            client.Send(message);
            client.Disconnect(true);
        }

        //var imap = new MailKit.Net.Imap.ImapClient();

        //imap.ServerCertificateValidationCallback = (s, c, h, ef) => true;


        //imap.Connect("imap.office365.com", 993, true);
        //imap.Authenticate("thomashle@outlook.com", "leho@ngth@o");
        //MailKit.IMailFolder folder = null;

        //if ((imap.Capabilities & (MailKit.Net.Imap.ImapCapabilities.SpecialUse | MailKit.Net.Imap.ImapCapabilities.XList)) != 0)
        //{
        //    // ...
        //    folder = imap.GetFolder(MailKit.SpecialFolder.Sent);
        //}
        //else
        //{
        //    folder = GetSentFolder(imap, CancellationToken.None);
        //}
        //folder.Open(MailKit.FolderAccess.ReadWrite);
        //folder.Append(message);
    }

    public void Send(string strDefaultCompanySignature = "")
    {
        // Updating email setting in case user 
        if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SMTPHost) || string.IsNullOrEmpty(Password))
        {
            WebBaseUtility.UpdateMailConfigurationFromLoginUser(this);
        }

        if (string.IsNullOrEmpty(Username))
        {
            if (string.IsNullOrEmpty(From))
                throw new Exception("Sending Email Error: Sender's address cannot be blank");
            else
                Username = From;
        }

        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();

        //if (emailValidation.IsValid(Username))
        if(WebBaseUtility.IsValidEmailAddress(Username))
        {
            SenderAddress = Username;
        }
        else
        {
            SenderAddress = WebBaseUtility.GetFromEmailAddress();//From;
            if (string.IsNullOrEmpty(SenderAddress))
            {
                SenderAddress = From;
            }
        }

        //SenderAddress = Username;
        var message = new MailMessage
        {
            //Sender = new MailAddress(SenderAddress, From),
            //From = new MailAddress(From, From),
            //ReplyTo = new MailAddress(From, From)
            Sender = new MailAddress(SenderAddress),
            From = new MailAddress(From),
            ReplyTo = new MailAddress(From)
        };

        To.RemoveAll(item => item == string.Empty);
        Cc.RemoveAll(item => item == string.Empty);
        //if(!string.Equals(SenderAddress,From,StringComparison.CurrentCultureIgnoreCase))
        //{
        //    message.CC.Add(new MailAddress(From));
        //}
        //AddDestinataryToList(To, message.To);
        //AddDestinataryToList(Cc, message.CC);
        //AddDestinataryToList(Bcc, message.Bcc);

        message.Subject = Title;
        message.IsBodyHtml = IsBodyHtml;

        //if (IsIncludeSignature)
        //{
        //    User _objGeneral = new User();
        //    _objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //    _objGeneral.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //    BL_User _objBL_General = new BL_User();
        //    //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
        //    var signature = _objBL_General.GetDefaultUserEmailSignature(_objGeneral);

        //    Text = Text + signature;
        //}
        if (IsIncludeSignature)
        {
            User _objUser = new User();
            _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            BL_User _objBL_User = new BL_User();
            //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
            var signature = _objBL_User.GetDefaultUserEmailSignature(_objUser);

            if (!string.IsNullOrEmpty(signature))
            {
                Text = Text + signature;
            }
            else
            {
                Text = Text + strDefaultCompanySignature;
            }
        }
        else
        {
            Text = Text + strDefaultCompanySignature;
        }

        if (IsBodyHtml)
        {
            message.Body = Text;
        }
        else
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Text);
            string result = htmlDoc.DocumentNode.InnerText;
            message.Body = result;
        }
        message.Priority = Priority;//MailPriority.High;

        if (attachmentBytes != null)
        {
            MemoryStream ms = new MemoryStream(attachmentBytes);
            message.Attachments.Add(new Attachment(ms, FileName));
        }

        foreach (var attachment in AttachmentFiles.Select(file => new Attachment(file)))
        {
            message.Attachments.Add(attachment);
        }

        if (Attachments != null && Attachments.Count > 0)
        {
            foreach (var attachment in Attachments)
            {
                message.Attachments.Add(attachment);
            }
        }

        var sender = new MailboxAddress(From, SenderAddress);
        var fromer = new MailboxAddress(From, From);
        //if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
        //{
        //    fromer.Name = SenderAddress + " on behalf of " + From;
        //}

        // Updating email sender and from
        var mimeMessage = new MimeKit.MimeMessage();
        mimeMessage = (MimeKit.MimeMessage)message;
        mimeMessage.Sender = sender;
        mimeMessage.From.Clear();
        mimeMessage.From.Add(fromer);
        mimeMessage.ReplyTo.Clear();
        mimeMessage.ReplyTo.Add(new MailboxAddress(From));

        AddDestinataryToList(To, mimeMessage.To);
        AddDestinataryToList(Cc, mimeMessage.Cc);
        AddDestinataryToList(Bcc, mimeMessage.Bcc);

        if(mimeMessage.To.Count == 0)
        {
            throw new Exception("Sending Email Error: To email address is invalid");
        }

        using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        {
            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.StartTls);
            }
            catch (Exception)
            {
                try
                {
                    smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.None);
                }
                catch (Exception ex)
                {
                    var err = "Sending Email - Connection Error: Please check your SMTP host and port before trying again";
                    var eee = new Exception(err, ex);
                    throw eee;
                }
                
            }


            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //smtpClient.AuthenticationMechanisms.Remove("NTLM");

            // Note: only needed if the SMTP server requires authentication
            if (RequireAutentication)
            {
                //SenderAddress = Username;
                //ICredentials ss = new MailKit.Security.

                try
                {
                    smtpClient.Authenticate(Username, Password);
                }
                catch (Exception ex)
                {
                    //errorMessages[0] = "Sending Email - Authentication Error: Please check your username and password before trying again";
                    //errorMessages[1] = ex.Message;
                    //return errorMessages;
                    var err = "Sending Email - Authentication Error: Please check your username and password before trying again";
                    var eee = new Exception(err, ex);
                    throw eee;

                }
            }
            // Thomas: add this code to re-send email in case of QAE email (@kqaelevator.ca)
            try
            {
                smtpClient.Send(mimeMessage);
            }
            catch (Exception ex)
            {
                // re-send if the error come from the difference of from address and sender address
                if(!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Updated from address by sender address for re-send
                    mimeMessage.From.Clear();
                    mimeMessage.From.Add(sender);
                    try
                    {
                        smtpClient.Send(mimeMessage);
                    }
                    catch (Exception ex1)
                    {
                        var err = "Sending Email Error: Please contact your support for more information";
                        var eee = new Exception(err, ex1);
                        throw eee;
                    }
                    
                }
                else
                {
                    var err = "Sending Email Error: Please contact your support for more information";
                    var eee = new Exception(err, ex);
                    throw eee;
                }
            }
            // Commented old code
            //smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);
        }

        if (TakeASentEmailCopy)
        {
            //if (!string.IsNullOrEmpty(InHost) && InPort != 0 && !string.IsNullOrEmpty(InUsername))
            {
                try
                {
                    var imapClient = new MailKit.Net.Imap.ImapClient();

                    imapClient.ServerCertificateValidationCallback = (s, c, h, ef) => true;
                    if (SSL)
                    {
                        try
                        {
                            imapClient.Connect(InHost, InPort, SecureSocketOptions.SslOnConnect);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (!imapClient.IsConnected)
                    {
                        try
                        {
                            imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                        }
                        catch
                        {
                            imapClient.Connect(InHost, InPort, SecureSocketOptions.None);
                        }
                    }
                    //imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                    //imapClient.AuthenticationMechanisms.Remove("NTLM");
                    imapClient.Authenticate(InUsername, InPassword);
                    MailKit.IMailFolder folder = null;

                    if ((imapClient.Capabilities & (MailKit.Net.Imap.ImapCapabilities.SpecialUse | MailKit.Net.Imap.ImapCapabilities.XList)) != 0)
                    {
                        // ...
                        folder = imapClient.GetFolder(MailKit.SpecialFolder.Sent);
                    }
                    else
                    {
                        folder = GetSentFolder(imapClient, CancellationToken.None);
                    }

                    if (folder != null)
                    {
                        folder.Open(MailKit.FolderAccess.ReadWrite);
                        folder.Append(mimeMessage);
                    }
                    else
                    {
                        throw new Exception("Get SentItems Error: Cannot access \"Sent Items\" folder.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Get SentItems Error: " + ex.Message + " Please contact your support to check your settings!", ex);
                }
            }
        }
    }

    public void SendTest(string userid = "")
    {
        // Updating email setting in case user 
        if (WebBaseUtility.IsValidEmailAddress(Username))
        {
            SenderAddress = Username;
        }
        else
        {
            SenderAddress = From;
        }

        var message = new MailMessage
        {
            Sender = new MailAddress(SenderAddress),
            From = new MailAddress(From),
            ReplyTo = new MailAddress(From)
        };

        To.RemoveAll(item => item == string.Empty);
        Cc.RemoveAll(item => item == string.Empty);
        
        message.Subject = Title;
        message.IsBodyHtml = IsBodyHtml;

        if (IsIncludeSignature)
        {
            if (!string.IsNullOrEmpty(userid))
            {
                User _objGeneral = new User();
                _objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _objGeneral.UserID = Convert.ToInt32(userid);
                BL_User _objBL_General = new BL_User();
                //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
                var signature = _objBL_General.GetDefaultUserEmailSignature(_objGeneral);

                Text = Text + signature;
            }
        }

        if (IsBodyHtml)
        {
            message.Body = Text;
        }
        else
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Text);
            string result = htmlDoc.DocumentNode.InnerText;
            message.Body = result;
        }
        message.Priority = Priority;//MailPriority.High;

        var sender = new MailboxAddress(From, SenderAddress);
        var fromer = new MailboxAddress(From, From);

        // Updating email sender and from
        var mimeMessage = new MimeKit.MimeMessage();
        mimeMessage = (MimeKit.MimeMessage)message;
        mimeMessage.Sender = sender;
        mimeMessage.From.Clear();
        mimeMessage.From.Add(fromer);
        mimeMessage.ReplyTo.Clear();
        mimeMessage.ReplyTo.Add(new MailboxAddress(From));

        AddDestinataryToList(To, mimeMessage.To);
        AddDestinataryToList(Cc, mimeMessage.Cc);
        AddDestinataryToList(Bcc, mimeMessage.Bcc);

        using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //client.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.StartTls);
            }
            catch (Exception ex)
            {
                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.None);
            }


            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //smtpClient.AuthenticationMechanisms.Remove("NTLM");

            // Note: only needed if the SMTP server requires authentication
            if (RequireAutentication)
            {
                //SenderAddress = Username;
                //ICredentials ss = new MailKit.Security.
                smtpClient.Authenticate(Username, Password);
            }
            // Thomas: add this code to re-send email in case of QAE email (@kqaelevator.ca)
            try
            {
                smtpClient.Send(mimeMessage);
            }
            catch (Exception ex)
            {
                // re-send if the error come from the difference of from address and sender address
                if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Updated from address by sender address for re-send
                    mimeMessage.From.Clear();
                    mimeMessage.From.Add(sender);
                    smtpClient.Send(mimeMessage);
                }
                else
                {
                    throw ex;
                }
            }
            smtpClient.Disconnect(true);
        }

        if (TakeASentEmailCopy)
        {
            try
            {
                var imapClient = new MailKit.Net.Imap.ImapClient();

                imapClient.ServerCertificateValidationCallback = (s, c, h, ef) => true;
                if (SSL)
                {
                    try
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.SslOnConnect);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (!imapClient.IsConnected)
                {
                    try
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                    }
                    catch
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.None);
                    }
                }
                imapClient.Authenticate(InUsername, InPassword);
                MailKit.IMailFolder folder = null;

                if ((imapClient.Capabilities & (MailKit.Net.Imap.ImapCapabilities.SpecialUse | MailKit.Net.Imap.ImapCapabilities.XList)) != 0)
                {
                    // ...
                    folder = imapClient.GetFolder(MailKit.SpecialFolder.Sent);
                }
                else
                {
                    folder = GetSentFolder(imapClient, CancellationToken.None);
                }

                if (folder != null)
                {
                    folder.Open(MailKit.FolderAccess.ReadWrite);
                    folder.Append(mimeMessage);
                }
                else
                {
                    throw new Exception("Cannot access \"Sent Items\" folder.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get SentItems Error: " + ex.Message + " Please contact your support to check your settings!", ex);
            }
        }
    }

    public Tuple<int, string, string> CompletingMessage(ref MimeMessage mimeMessage, bool IsLog = true, EmailLog emailLog = null, string strDefaultCompanySignature = "")
    {
        Tuple<int, string, string> error = null;
        var userErr = string.Empty;
        // Updating email setting in case user 
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(SMTPHost) || string.IsNullOrEmpty(Password))
        {
            WebBaseUtility.UpdateMailConfigurationFromLoginUser(this);
        }

        if (string.IsNullOrEmpty(Username))
        {
            if (string.IsNullOrEmpty(From))
                //userErr = "Sending Email Error: Sender address should not be empty.";
                //error.Item1 = 1;
                error = new Tuple<int, string, string>(1, "Sending Email Error: Sender's address cannot be blank", "");
            else
                Username = From;
        }

        //var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();

        //if (emailValidation.IsValid(Username))
        if (WebBaseUtility.IsValidEmailAddress(Username))
        {
            SenderAddress = Username;
        }
        else
        {
            SenderAddress = WebBaseUtility.GetFromEmailAddress();//From;
            if (string.IsNullOrEmpty(SenderAddress))
            {
                SenderAddress = From;
            }
        }

        if (string.IsNullOrEmpty(SenderAddress))
        {
            error = new Tuple<int, string, string>(1, "Sending Email Error: Sender's address cannot be blank", "");
            return error;
        }

        var message = new MailMessage
        {
            Sender = new MailAddress(SenderAddress),
            From = new MailAddress(From),
            ReplyTo = new MailAddress(From)
        };

        To.RemoveAll(item => item == string.Empty);
        Cc.RemoveAll(item => item == string.Empty);

        message.Subject = Title;
        message.IsBodyHtml = IsBodyHtml;

        if (IsIncludeSignature)
        {
            User _objUser = new User();
            _objUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _objUser.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            BL_User _objBL_User = new BL_User();
            //DataSet dsEmailacc = _objBL_General.GetEmailAccounts(_objGeneral);
            var signature = _objBL_User.GetDefaultUserEmailSignature(_objUser);

            if (!string.IsNullOrEmpty(signature))
            { 
                Text = Text + signature; 
            }
            else
            {
                Text = Text + strDefaultCompanySignature;
            }
        }
        else
        {
            Text = Text + strDefaultCompanySignature;
        }

        if (IsBodyHtml)
        {
            message.Body = Text;
        }
        else
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Text);
            string result = htmlDoc.DocumentNode.InnerText;
            message.Body = result;
        }
        message.Priority = Priority;//MailPriority.High;

        if (attachmentBytes != null)
        {
            MemoryStream ms = new MemoryStream(attachmentBytes);
            message.Attachments.Add(new Attachment(ms, FileName));
        }

        foreach (var attachment in AttachmentFiles.Select(file => new Attachment(file)))
        {
            message.Attachments.Add(attachment);
        }

        if (Attachments != null && Attachments.Count > 0)
        {
            foreach (var attachment in Attachments)
            {
                message.Attachments.Add(attachment);
            }
        }

        var sender = new MailboxAddress(From, SenderAddress);
        var fromer = new MailboxAddress(From, From);
        //if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
        //{
        //    fromer.Name = SenderAddress + " on behalf of " + From;
        //}

        // Updating email sender and from
        if(mimeMessage == null)
            mimeMessage = new MimeKit.MimeMessage();
        mimeMessage = (MimeKit.MimeMessage)message;
        mimeMessage.Sender = sender;
        mimeMessage.From.Clear();
        mimeMessage.From.Add(fromer);
        mimeMessage.ReplyTo.Clear();
        mimeMessage.ReplyTo.Add(new MailboxAddress(From));

        AddDestinataryToList(To, mimeMessage.To);
        AddDestinataryToList(Cc, mimeMessage.Cc);
        AddDestinataryToList(Bcc, mimeMessage.Bcc);

        if (IsLog && emailLog != null && error != null)
        {
            // Emailing logs: testing only
            emailLog.EmailDate = DateTime.Now;
            emailLog.From = mimeMessage.From.ToString();
            emailLog.Sender = mimeMessage.Sender.ToString();
            emailLog.To = mimeMessage.To.ToString();
            emailLog.Status = 0;
            emailLog.SysErrMessage = error.Item3;
            emailLog.UsrErrMessage = error.Item2;
            BL_EmailLog bL_EmailLog = new BL_EmailLog();
            bL_EmailLog.AddEmailLog(emailLog);
        }

        return error;
    }


    public Tuple<int, string, string> Send(MimeMessage mimeMessage, bool IsLog = true, EmailLog emailLog = null)
    {
        //string[] errorMessages = new string[2];
        Tuple<int, string, string> error = null;
        using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.StartTls);
            }
            catch
            {
                try
                {
                    smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.None);
                }
                catch (Exception ex)
                {
                    //errorMessages[0] = "Sending Email - Connection Error: Please check your SMTP host and port before trying again";
                    //errorMessages[1] = ex.Message;
                    //return errorMessages;
                    error = new Tuple<int, string, string>(1
                        , "Sending Email - Connection Error: Please check your SMTP host and port before trying again"
                        , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }

                    return error;
                }
            }


            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //smtpClient.AuthenticationMechanisms.Remove("NTLM");

            // Note: only needed if the SMTP server requires authentication
            if (RequireAutentication)
            {
                //SenderAddress = Username;
                //ICredentials ss = new MailKit.Security.
                try
                {
                    smtpClient.Authenticate(Username, Password);
                }
                catch (Exception ex)
                {
                    //errorMessages[0] = "Sending Email - Authentication Error: Please check your username and password before trying again";
                    //errorMessages[1] = ex.Message;
                    //return errorMessages;
                    error = new Tuple<int, string, string>(1
                        , "Sending Email - Authentication Error: Please check your username and password before trying again"
                        , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                    return error;
                }
                
            }
            // Thomas: add this code to re-send email in case of QAE email (@kqaelevator.ca)
            try
            {
                smtpClient.Send(mimeMessage);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = mimeMessage.To.ToString();
                    emailLog.Status = 1;
                    emailLog.SysErrMessage = string.Empty;
                    emailLog.UsrErrMessage = string.Empty;
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
            }
            catch (Exception ex)
            {
                // re-send if the error come from the difference of from address and sender address
                if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Updated from address by sender address for re-send
                    mimeMessage.From.Clear();
                    mimeMessage.From.Add(mimeMessage.Sender);
                    try
                    {
                        smtpClient.Send(mimeMessage);
                        if (IsLog && emailLog != null)
                        {
                            emailLog.EmailDate = DateTime.Now;
                            emailLog.From = mimeMessage.From.ToString();
                            emailLog.Sender = mimeMessage.Sender.ToString();
                            emailLog.To = mimeMessage.To.ToString();
                            emailLog.Status = 1;
                            emailLog.SysErrMessage = string.Empty;
                            emailLog.UsrErrMessage = string.Empty;
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                        }
                    }
                    catch (Exception ex1)
                    {
                        //errorMessages[0] = "Sending Email Error: Please contact your support for more infomation";
                        //errorMessages[1] = ex1.Message;
                        //return errorMessages;
                        error = new Tuple<int, string, string>(2
                            , "Sending Email Error: Please contact your support for more information"
                            , ex1.Message);
                        if (IsLog && emailLog != null)
                        {
                            emailLog.EmailDate = DateTime.Now;
                            emailLog.From = mimeMessage.From.ToString();
                            emailLog.Sender = mimeMessage.Sender.ToString();
                            emailLog.To = mimeMessage.To.ToString();
                            emailLog.Status = 0;
                            emailLog.SysErrMessage = error.Item3;
                            emailLog.UsrErrMessage = error.Item2;
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                        }
                        return error;
                    }
                    
                }
                else
                {
                    //errorMessages[0] = "Sending Email Error: Please contact your support for more information";
                    //errorMessages[1] = ex.Message;
                    //return errorMessages;
                    error = new Tuple<int, string, string>(2
                            , "Sending Email Error: Please contact your support for more information"
                            , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                    return error;
                }
            }
            smtpClient.Disconnect(true);
        }

        return error;
    }

    /// <summary>
    /// Send single email with log. For sending serveral we should use the "public Tuple<int, string, string> Send(MimeMessage mimeMessage, bool IsLog = true, EmailLog emailLog = null)"
    /// </summary>
    /// <param name="mimeMessage"></param>
    /// <param name="IsLog"></param>
    /// <param name="emailLog"></param>
    /// <returns></returns>
    public Tuple<int, string, string> SendSingleMailWithLog(MimeMessage mimeMessage, bool IsLog = true, EmailLog emailLog = null)
    {
        Tuple<int, string, string> error = null;
        using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        {
            try
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.StartTls);
            }
            catch
            {
                try
                {
                    smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.None);
                }
                catch (Exception ex)
                {
                    error = new Tuple<int, string, string>(1
                        , "Sending Email - Connection Error: Please check your SMTP host and port before trying again"
                        , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }

                    return error;
                }
            }


            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            //smtpClient.AuthenticationMechanisms.Remove("NTLM");

            // Note: only needed if the SMTP server requires authentication
            if (RequireAutentication)
            {
                //SenderAddress = Username;
                //ICredentials ss = new MailKit.Security.
                try
                {
                    smtpClient.Authenticate(Username, Password);
                }
                catch (Exception ex)
                {
                    error = new Tuple<int, string, string>(1
                        , "Sending Email - Authentication Error: Please check your username and password before trying again"
                        , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                    return error;
                }

            }
            // Thomas: add this code to re-send email in case of QAE email (@kqaelevator.ca)
            try
            {
                smtpClient.Send(mimeMessage);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = mimeMessage.To.ToString();
                    emailLog.Status = 1;
                    emailLog.SysErrMessage = string.Empty;
                    emailLog.UsrErrMessage = string.Empty;
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
            }
            catch (Exception ex)
            {
                // re-send if the error come from the difference of from address and sender address
                if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Updated from address by sender address for re-send
                    mimeMessage.From.Clear();
                    mimeMessage.From.Add(mimeMessage.Sender);
                    try
                    {
                        smtpClient.Send(mimeMessage);
                        if (IsLog && emailLog != null)
                        {
                            emailLog.EmailDate = DateTime.Now;
                            emailLog.From = mimeMessage.From.ToString();
                            emailLog.Sender = mimeMessage.Sender.ToString();
                            emailLog.To = mimeMessage.To.ToString();
                            emailLog.Status = 1;
                            emailLog.SysErrMessage = string.Empty;
                            emailLog.UsrErrMessage = string.Empty;
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                        }
                    }
                    catch (Exception ex1)
                    {
                        //errorMessages[0] = "Sending Email Error: Please contact your support for more infomation";
                        //errorMessages[1] = ex1.Message;
                        //return errorMessages;
                        error = new Tuple<int, string, string>(2
                            , "Sending Email Error: Please contact your support for more information"
                            , ex1.Message);
                        if (IsLog && emailLog != null)
                        {
                            emailLog.EmailDate = DateTime.Now;
                            emailLog.From = mimeMessage.From.ToString();
                            emailLog.Sender = mimeMessage.Sender.ToString();
                            emailLog.To = mimeMessage.To.ToString();
                            emailLog.Status = 0;
                            emailLog.SysErrMessage = error.Item3;
                            emailLog.UsrErrMessage = error.Item2;
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                        }
                        return error;
                    }

                }
                else
                {
                    //errorMessages[0] = "Sending Email Error: Please contact your support for more information";
                    //errorMessages[1] = ex.Message;
                    //return errorMessages;
                    error = new Tuple<int, string, string>(2
                            , "Sending Email Error: Please contact your support for more information"
                            , ex.Message);
                    if (IsLog && emailLog != null)
                    {
                        emailLog.EmailDate = DateTime.Now;
                        emailLog.From = mimeMessage.From.ToString();
                        emailLog.Sender = mimeMessage.Sender.ToString();
                        emailLog.To = mimeMessage.To.ToString();
                        emailLog.Status = 0;
                        emailLog.SysErrMessage = error.Item3;
                        emailLog.UsrErrMessage = error.Item2;
                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
                        bL_EmailLog.AddEmailLog(emailLog);
                    }
                    return error;
                }
            }
            smtpClient.Disconnect(true);
        }

        //return error;
        if (TakeASentEmailCopy)
        {
            var imapClient = new MailKit.Net.Imap.ImapClient();

            imapClient.ServerCertificateValidationCallback = (s, c, h, ef) => true;
            try
            {
                if (SSL)
                {
                    try
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.SslOnConnect);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (!imapClient.IsConnected)
                {
                    //imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                    try
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                    }
                    catch
                    {
                        imapClient.Connect(InHost, InPort, SecureSocketOptions.None);
                    }
                }
            }
            catch (Exception ex)
            {
                //error = new Tuple<int, string, string>(3
                //                , "Get SentItems - Connection Error: Please check your IMAP host and port before trying again"
                //                , ex.Message);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = string.Empty;
                    emailLog.Status = 2;
                    emailLog.SysErrMessage = ex.Message;
                    emailLog.UsrErrMessage = "Get SentItems - Connection Error: Please check your IMAP host and port before trying again";
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
                return error;
            }

            try
            {
                //imapClient.AuthenticationMechanisms.Remove("NTLM");
                imapClient.Authenticate(InUsername, InPassword);
            }
            catch (Exception ex)
            {
                //error = new Tuple<int, string, string>(3
                //                , "Get SentItems - Authentication Error: Please check your username and password before trying again"
                //                , ex.Message);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = string.Empty;
                    emailLog.Status = 2;
                    emailLog.SysErrMessage = ex.Message;
                    emailLog.UsrErrMessage = "Get SentItems - Authentication Error: Please check your username and password before trying again";
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
                return error;
            }

            MailKit.IMailFolder folder = null;

            try
            {

                if ((imapClient.Capabilities & (MailKit.Net.Imap.ImapCapabilities.SpecialUse | MailKit.Net.Imap.ImapCapabilities.XList)) != 0)
                {
                    // ...
                    folder = imapClient.GetFolder(MailKit.SpecialFolder.Sent);
                }
                else
                {
                    folder = GetSentFolder(imapClient, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                //error = new Tuple<int, string, string>(3
                //            , "Get SentItems Folder Error: Please contact your support for more infomation"
                //            , ex.Message);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = string.Empty;
                    emailLog.Status = 2;
                    emailLog.SysErrMessage = ex.Message;
                    emailLog.UsrErrMessage = "Get SentItems Folder Error: Please contact your support for more infomation";
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
                return error;
            }

            if (folder != null)
            {
                folder.Open(MailKit.FolderAccess.ReadWrite);
            }
            else
            {
                //error = new Tuple<int, string, string>(3
                //            , "Get SentItems Error: Cannot access \"Sent Items\" folder."
                //            , string.Empty);
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = string.Empty;
                    emailLog.Status = 2;
                    emailLog.SysErrMessage = "";
                    emailLog.UsrErrMessage = "Get SentItems Error: Cannot access \"Sent Items\" folder.";
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
                return error;
            }

            StringBuilder userErr = new StringBuilder();
            StringBuilder sysErr = new StringBuilder();
            //foreach (var mimeMessage in mimeMessages)
            {
                try
                {
                    folder.Append(mimeMessage);
                }
                catch (Exception ex)
                {
                    userErr.AppendFormat("Get SentItems Error: {0}: Error on append this email to Sent Items folder.", mimeMessage.Subject);
                    userErr.AppendLine();
                    //userErr.AppendLine("Get SentItems Error: Cant append sent email to Sent Items folder.");
                    sysErr.AppendLine(ex.Message);
                    //Thread.Sleep(5000);
                }
            }

            if (userErr.Length > 0)
            {
                //error = new Tuple<int, string, string>(4
                //                , userErr.ToString()
                //                , sysErr.ToString());
                if (IsLog && emailLog != null)
                {
                    emailLog.EmailDate = DateTime.Now;
                    emailLog.From = mimeMessage.From.ToString();
                    emailLog.Sender = mimeMessage.Sender.ToString();
                    emailLog.To = string.Empty;
                    emailLog.Status = 2;
                    emailLog.SysErrMessage = sysErr.ToString();
                    emailLog.UsrErrMessage = userErr.ToString();
                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
                    bL_EmailLog.AddEmailLog(emailLog);
                }
                return error;
            }

            return error;
        }

        return error;
    }

    //public Tuple<int, string, string> Send(List<int, MimeMessage> mimeMessages
    //    , List<MimeKit.MimeMessage> mimeSentMessages
    //    , List<MimeKit.MimeMessage> mimeErrorMessages
    //    , bool IsLog = true, EmailLog emailLog = null)
    //{
    //    //string[] errorMessages = new string[2];
    //    Tuple<int, string, string> error = null;
    //    using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
    //    {
    //        try
    //        {
    //            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
    //            smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

    //            smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.StartTls);
    //        }
    //        catch
    //        {
    //            try
    //            {
    //                smtpClient.Connect(SMTPHost, SMTPPort, SecureSocketOptions.None);
    //            }
    //            catch (Exception ex)
    //            {
    //                //errorMessages[0] = "Sending Email - Connection Error: Please check your SMTP host and port before trying again";
    //                //errorMessages[1] = ex.Message;
    //                //return errorMessages;
    //                error = new Tuple<int, string, string>(1
    //                    , "Sending Email - Connection Error: Please check your SMTP host and port before trying again"
    //                    , ex.Message);
    //                if (IsLog && emailLog != null)
    //                {
    //                    emailLog.EmailDate = DateTime.Now;
    //                    emailLog.From = mimeMessages[0].From.ToString();
    //                    emailLog.Sender = mimeMessages[0].Sender.ToString();
    //                    emailLog.To = string.Empty;
    //                    emailLog.Status = 0;
    //                    emailLog.SysErrMessage = error.Item3;
    //                    emailLog.UsrErrMessage = error.Item2;
    //                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                    bL_EmailLog.AddEmailLog(emailLog);
    //                }

    //                return error;
    //            }
    //        }


    //        // Note: since we don't have an OAuth2 token, disable
    //        // the XOAUTH2 authentication mechanism.
    //        smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
    //        //smtpClient.AuthenticationMechanisms.Remove("NTLM");

    //        // Note: only needed if the SMTP server requires authentication
    //        if (RequireAutentication)
    //        {
    //            //SenderAddress = Username;
    //            //ICredentials ss = new MailKit.Security.
    //            try
    //            {
    //                smtpClient.Authenticate(Username, Password);
    //            }
    //            catch (Exception ex)
    //            {
    //                //errorMessages[0] = "Sending Email - Authentication Error: Please check your username and password before trying again";
    //                //errorMessages[1] = ex.Message;
    //                //return errorMessages;
    //                error = new Tuple<int, string, string>(1
    //                    , "Sending Email - Authentication Error: Please check your username and password before trying again"
    //                    , ex.Message);
    //                if (IsLog && emailLog != null)
    //                {
    //                    emailLog.EmailDate = DateTime.Now;
    //                    emailLog.From = mimeMessages[0].From.ToString();
    //                    emailLog.Sender = mimeMessages[0].Sender.ToString();
    //                    emailLog.To = string.Empty;
    //                    emailLog.Status = 0;
    //                    emailLog.SysErrMessage = error.Item3;
    //                    emailLog.UsrErrMessage = error.Item2;
    //                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                    bL_EmailLog.AddEmailLog(emailLog);
    //                }
    //                return error;
    //            }

    //        }

    //        if (mimeErrorMessages == null) mimeErrorMessages = new List<MimeMessage>();
    //        if (mimeSentMessages == null) mimeSentMessages = new List<MimeMessage>();
    //        // Thomas: add this code to re-send email in case of QAE email (@kqaelevator.ca)
    //        StringBuilder sbdUserErr = new StringBuilder();
    //        foreach (var mimeMessage in mimeMessages)
    //        {
    //            try
    //            {
    //                smtpClient.Send(mimeMessage);
    //                mimeSentMessages.Add(mimeMessage);
    //                if (IsLog && emailLog != null)
    //                {
    //                    emailLog.EmailDate = DateTime.Now;
    //                    emailLog.From = mimeMessage.From.ToString();
    //                    emailLog.Sender = mimeMessage.Sender.ToString();
    //                    emailLog.To = mimeMessage.To.ToString();
    //                    emailLog.Status = 1;
    //                    emailLog.SysErrMessage = string.Empty;
    //                    emailLog.UsrErrMessage = string.Empty;
    //                    BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                    bL_EmailLog.AddEmailLog(emailLog);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                // re-send if the error come from the difference of from address and sender address
    //                if (!SenderAddress.Equals(From, StringComparison.InvariantCultureIgnoreCase))
    //                {
    //                    // Updated from address by sender address for re-send
    //                    mimeMessage.From.Clear();
    //                    mimeMessage.From.Add(mimeMessage.Sender);
    //                    try
    //                    {
    //                        smtpClient.Send(mimeMessage);
    //                        mimeSentMessages.Add(mimeMessage);
    //                        if (IsLog && emailLog != null)
    //                        {
    //                            emailLog.EmailDate = DateTime.Now;
    //                            emailLog.From = mimeMessage.From.ToString();
    //                            emailLog.Sender = mimeMessage.Sender.ToString();
    //                            emailLog.To = mimeMessage.To.ToString();
    //                            emailLog.Status = 1;
    //                            emailLog.SysErrMessage = string.Empty;
    //                            emailLog.UsrErrMessage = string.Empty;
    //                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                            bL_EmailLog.AddEmailLog(emailLog);
    //                        }
    //                    }
    //                    catch (Exception ex1)
    //                    {
    //                        //errorMessages[0] = "Sending Email Error: Please contact your support for more infomation";
    //                        //errorMessages[1] = ex1.Message;
    //                        //return errorMessages;
    //                        //error = new Tuple<int, string, string>(2
    //                        //    , "Sending Email Error: Please contact your support for more infomation"
    //                        //    , ex1.Message);
    //                        mimeErrorMessages.Add(mimeMessage);
    //                        if (IsLog && emailLog != null)
    //                        {
    //                            emailLog.EmailDate = DateTime.Now;
    //                            emailLog.From = mimeMessage.From.ToString();
    //                            emailLog.Sender = mimeMessage.Sender.ToString();
    //                            emailLog.To = mimeMessage.To.ToString();
    //                            emailLog.Status = 0;
    //                            emailLog.SysErrMessage = ex1.Message;
    //                            emailLog.UsrErrMessage = "Sending Email Error: Please contact your support for more infomation";
    //                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                            bL_EmailLog.AddEmailLog(emailLog);
    //                        }
    //                        sbdUserErr.
    //                        //return error;
    //                    }

    //                }
    //                else
    //                {
    //                    //errorMessages[0] = "Sending Email Error: Please contact your support for more infomation";
    //                    //errorMessages[1] = ex.Message;
    //                    //return errorMessages;
    //                    //error = new Tuple<int, string, string>(2
    //                    //        , "Sending Email Error: Please contact your support for more infomation"
    //                    //        , ex.Message);
    //                    mimeErrorMessages.Add(mimeMessage);
    //                    if (IsLog && emailLog != null)
    //                    {
    //                        emailLog.EmailDate = DateTime.Now;
    //                        emailLog.From = mimeMessage.From.ToString();
    //                        emailLog.Sender = mimeMessage.Sender.ToString();
    //                        emailLog.To = mimeMessage.To.ToString();
    //                        emailLog.Status = 0;
    //                        emailLog.SysErrMessage = ex.Message;
    //                        emailLog.UsrErrMessage = "Sending Email Error: Please contact your support for more infomation";
    //                        BL_EmailLog bL_EmailLog = new BL_EmailLog();
    //                        bL_EmailLog.AddEmailLog(emailLog);
    //                    }
    //                    //return error;
    //                }
    //            }
    //        }


    //        smtpClient.Disconnect(true);
    //    }

    //    return error;
    //}

    public Tuple<int, string, string> GetSentItems(List<MimeMessage> mimeMessages, bool IsLog = true, EmailLog emailLog = null)
    {
        Tuple<int, string, string> error = null;
        //var errorMessages = new string[2];
        var imapClient = new MailKit.Net.Imap.ImapClient();

        imapClient.ServerCertificateValidationCallback = (s, c, h, ef) => true;
        try
        {
            if (SSL)
            {
                try
                {
                    imapClient.Connect(InHost, InPort, SecureSocketOptions.SslOnConnect);
                }
                catch (Exception)
                {
                }
            }

            if (!imapClient.IsConnected)
            {
                //imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                try
                {
                    imapClient.Connect(InHost, InPort, SecureSocketOptions.Auto);
                }
                catch
                {
                    imapClient.Connect(InHost, InPort, SecureSocketOptions.None);
                }
            }
        }
        catch (Exception ex)
        {
            //errorMessages[0] = "Get SentItems - Connection Error: Please check your IMAP host and port before trying again";
            //errorMessages[1] = ex.Message;
            //return errorMessages;
            error = new Tuple<int, string, string>(3
                            , "Get SentItems - Connection Error: Please check your IMAP host and port before trying again"
                            , ex.Message);
            if (IsLog && emailLog != null)
            {
                emailLog.EmailDate = DateTime.Now;
                emailLog.From = mimeMessages[0].From.ToString();
                emailLog.Sender = mimeMessages[0].Sender.ToString();
                emailLog.To = string.Empty;
                emailLog.Status = 2;
                emailLog.SysErrMessage = error.Item3;
                emailLog.UsrErrMessage = error.Item2;
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            return error;
        }

        try
        {
            //imapClient.AuthenticationMechanisms.Remove("NTLM");
            imapClient.Authenticate(InUsername, InPassword);
        }
        catch (Exception ex)
        {
            //errorMessages[0] = "Get SentItems - Authentication Error: Please check your username and password before trying again";
            //errorMessages[1] = ex.Message;
            //return errorMessages;
            error = new Tuple<int, string, string>(3
                            , "Get SentItems - Authentication Error: Please check your username and password before trying again"
                            , ex.Message);
            if (IsLog && emailLog != null)
            {
                emailLog.EmailDate = DateTime.Now;
                emailLog.From = mimeMessages[0].From.ToString();
                emailLog.Sender = mimeMessages[0].Sender.ToString();
                emailLog.To = string.Empty;
                emailLog.Status = 2;
                emailLog.SysErrMessage = error.Item3 + string.Format(" - Username: {0}, Password: {1}", InUsername, InPassword);
                emailLog.UsrErrMessage = error.Item2;
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            return error;
        }
        
        MailKit.IMailFolder folder = null;

        try
        {

            if ((imapClient.Capabilities & (MailKit.Net.Imap.ImapCapabilities.SpecialUse | MailKit.Net.Imap.ImapCapabilities.XList)) != 0)
            {
                // ...
                folder = imapClient.GetFolder(MailKit.SpecialFolder.Sent);
            }
            else
            {
                folder = GetSentFolder(imapClient, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            //errorMessages[0] = "Get SentItems Folder Error: Please contact your support for more infomation";
            //errorMessages[1] = ex.Message;
            //return errorMessages;
            error = new Tuple<int, string, string>(3
                            , "Get SentItems Folder Error: Please contact your support for more infomation"
                            , ex.Message);
            if (IsLog && emailLog != null)
            {
                emailLog.EmailDate = DateTime.Now;
                emailLog.From = mimeMessages[0].From.ToString();
                emailLog.Sender = mimeMessages[0].Sender.ToString();
                emailLog.To = string.Empty;
                emailLog.Status = 2;
                emailLog.SysErrMessage = error.Item3;
                emailLog.UsrErrMessage = error.Item2;
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            return error;
        }

        if (folder != null)
        {
            folder.Open(MailKit.FolderAccess.ReadWrite);
            //folder.Append(mimeMessage);
        }
        else
        {
            //errorMessages[0] = "Get SentItems Error: Cannot access \"Sent Items\" folder.";
            //errorMessages[1] = string.Empty;
            //return errorMessages;
            error = new Tuple<int, string, string>(3
                            , "Get SentItems Error: Cannot access \"Sent Items\" folder."
                            , string.Empty);
            if (IsLog && emailLog != null)
            {
                emailLog.EmailDate = DateTime.Now;
                emailLog.From = mimeMessages[0].From.ToString();
                emailLog.Sender = mimeMessages[0].Sender.ToString();
                emailLog.To = string.Empty;
                emailLog.Status = 2;
                emailLog.SysErrMessage = error.Item3;
                emailLog.UsrErrMessage = error.Item2;
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            return error;
        }

        StringBuilder userErr = new StringBuilder();
        StringBuilder sysErr = new StringBuilder();
        foreach (var mimeMessage in mimeMessages)
        {
            try
            {
                folder.Append(mimeMessage);
            }
            catch (Exception ex)
            {
                userErr.AppendFormat("Get SentItems Error: {0}: Error on append this email to Sent Items folder.", mimeMessage.Subject);
                userErr.AppendLine();
                //userErr.AppendLine("Get SentItems Error: Cant append sent email to Sent Items folder.");
                sysErr.AppendLine(ex.Message);
                Thread.Sleep(5000);
            }
        }

        if(userErr.Length > 0)
        {
            //errorMessages[0] = userErr.ToString();
            //errorMessages[1] = sysErr.ToString();
            error = new Tuple<int, string, string>(4
                            , userErr.ToString()
                            , sysErr.ToString());
            if (IsLog && emailLog != null)
            {
                emailLog.EmailDate = DateTime.Now;
                emailLog.From = mimeMessages[0].From.ToString();
                emailLog.Sender = mimeMessages[0].Sender.ToString();
                emailLog.To = string.Empty;
                emailLog.Status = 2;
                emailLog.SysErrMessage = error.Item3;
                emailLog.UsrErrMessage = error.Item2;
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            return error;
        }

        return error;
    }

    public void SendOld()
    {
        var Mailsettings = System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp") as System.Net.Configuration.SmtpSection;
        SenderAddress = Mailsettings.Network.UserName;
        //bool enablessl = Mailsettings.Network.EnableSsl;

        var client = new SmtpClient();
        //{
        //    EnableSsl = true
        //};

        if (RequireAutentication)
        {
            var credentials = new NetworkCredential(Username, Password);
            client.Credentials = credentials;
            client.Host = SMTPHost;
            client.Port = SMTPPort;
            SenderAddress = Username;
            client.EnableSsl = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        var message = new MailMessage
        {
            //Sender = new MailAddress(SenderAddress, From),
            //From = new MailAddress(From, From),
            //ReplyTo = new MailAddress(From, From)
            Sender = new MailAddress(SenderAddress),
            From = new MailAddress(SenderAddress),
            ReplyTo = new MailAddress(From)
        };

        To.RemoveAll(item => item == string.Empty);
        Cc.RemoveAll(item => item == string.Empty);
        //if(!string.Equals(SenderAddress,From,StringComparison.CurrentCultureIgnoreCase))
        //{
        //    message.CC.Add(new MailAddress(From));
        //}
        AddDestinataryToList(To, message.To);
        AddDestinataryToList(Cc, message.CC);
        AddDestinataryToList(Bcc, message.Bcc);

        message.Subject = Title;
        message.Body = Text;
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;

        if (attachmentBytes != null)
        {
            MemoryStream ms = new MemoryStream(attachmentBytes);
            message.Attachments.Add(new Attachment(ms, FileName));
        }

        foreach (var attachment in AttachmentFiles.Select(file => new Attachment(file)))
        {
            message.Attachments.Add(attachment);
        }

        if (Attachments != null && Attachments.Count > 0)
        {
            foreach (var attachment in Attachments)
            {
                message.Attachments.Add(attachment);
            }
        }

        //////ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        client.Send(message);
    }

    public void SaveEML(string EMLPath, string filename)
    {
        //var client = new SmtpClient();
        var message = new MailMessage
        {
            //Sender = new MailAddress(SenderAddress, From),
            //From = new MailAddress(From, From),
            //ReplyTo = new MailAddress(From, From)
            Sender = new MailAddress(SenderAddress),
            From = new MailAddress(SenderAddress),
            ReplyTo = new MailAddress(From)
        };

        To.RemoveAll(item => item == string.Empty);
        Cc.RemoveAll(item => item == string.Empty);
        //if (!string.Equals(SenderAddress, From, StringComparison.CurrentCultureIgnoreCase))
        //{
        //    message.CC.Add(new MailAddress(From));
        //}
        AddDestinataryToList(To, message.To);
        AddDestinataryToList(Cc, message.CC);
        AddDestinataryToList(Bcc, message.Bcc);

        message.Subject = Title;
        message.Body = Text;
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;

        foreach (var attachment in AttachmentFiles.Select(file => new Attachment(file)))
        {
            message.Attachments.Add(attachment);
        }

        if (Attachments != null && Attachments.Count > 0)
        {
            foreach (var attachment in Attachments)
            {
                message.Attachments.Add(attachment);
            }
        }

        if (!Directory.Exists(EMLPath))
        {
            Directory.CreateDirectory(EMLPath);
        }
        message.Save(EMLPath + filename);
        //client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
        //client.PickupDirectoryLocation = EMLPath;
        //client.Send(message);
    }

    private void AddDestinataryToList(IEnumerable<string> from, ICollection<MailAddress> mailAddressCollection)
    {
        foreach (var destinatary in from)
        {
            var temp = destinatary.Trim();
            if (!string.IsNullOrEmpty(temp))
            {
                mailAddressCollection.Add(new MailAddress(temp));
            }
        }
    }

    private void AddDestinataryToList(IEnumerable<string> from, InternetAddressList mailAddressCollection)
    {
        foreach (var destinatary in from)
        {
            var temp = destinatary.Trim();
            if (!string.IsNullOrEmpty(temp))
            {
                mailAddressCollection.Add(new MailboxAddress(temp));
            }
        }
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
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

    }
}
/*
public static class SmtpClientExtensions
{
    static System.IO.StreamWriter sw = null;
    static System.Net.Sockets.TcpClient tcpc = null;
    static System.Net.Security.SslStream ssl = null;
    static string path;
    static int bytes = -1;
    static byte[] buffer;
    static System.Text.StringBuilder sb = new System.Text.StringBuilder();
    static byte[] dummy;

    /// <summary>
    /// Communication with server
    /// </summary>
    /// <param name="command">The command beeing sent</param>
    private static void SendCommandAndReceiveResponse(string command)
    {
        try
        {
            if (command != "")
            {
                if (tcpc.Connected)
                {
                    dummy = System.Text.Encoding.ASCII.GetBytes(command);
                    ssl.Write(dummy, 0, dummy.Length);
                }
                else
                {
                    throw new System.ApplicationException("TCP CONNECTION DISCONNECTED");
                }
            }
            ssl.Flush();

            buffer = new byte[2048];
            bytes = ssl.Read(buffer, 0, 2048);
            sb.Append(System.Text.Encoding.ASCII.GetString(buffer));

            sw.WriteLine(sb.ToString());
            sb = new System.Text.StringBuilder();
        }
        catch (System.Exception ex)
        {
            throw new System.ApplicationException(ex.Message);
        }
    }

    /// <summary>
    /// Saving a mail message before beeing sent by the SMTP client
    /// </summary>
    /// <param name="self">The caller</param>
    /// <param name="imapServer">The address of the IMAP server</param>
    /// <param name="imapPort">The port of the IMAP server</param>
    /// <param name="userName">The username to log on to the IMAP server</param>
    /// <param name="password">The password to log on to the IMAP server</param>
    /// <param name="sentFolderName">The name of the folder where the message will be saved</param>
    /// <param name="mailMessage">The message being saved</param>
    public static void SendAndSaveMessageToIMAP(this System.Net.Mail.SmtpClient self, System.Net.Mail.MailMessage mailMessage, string imapServer, int imapPort, string userName, string password, string sentFolderName)
    {
        try
        {
            //path = System.Environment.CurrentDirectory + "\\emailresponse.txt";
            path = "c:\\temp\\emailresponse.txt";
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            sw = new System.IO.StreamWriter(System.IO.File.Create(path));

            tcpc = new System.Net.Sockets.TcpClient(imapServer, imapPort);

            ssl = new System.Net.Security.SslStream(tcpc.GetStream());
            ssl.AuthenticateAsClient(imapServer);
            SendCommandAndReceiveResponse("");

            SendCommandAndReceiveResponse(string.Format("$ LOGIN {1} {2}  {0}", System.Environment.NewLine, userName, password));

            using (var m = mailMessage.RawMessage())
            {
                m.Position = 0;
                var sr = new System.IO.StreamReader(m);
                var myStr = sr.ReadToEnd();
                SendCommandAndReceiveResponse(string.Format("$ APPEND {1} (\\Seen) {{{2}}}{0}", System.Environment.NewLine, sentFolderName, myStr.Length));
                SendCommandAndReceiveResponse(string.Format("{1}{0}", System.Environment.NewLine, myStr));
            }
            SendCommandAndReceiveResponse(string.Format("$ LOGOUT{0}", System.Environment.NewLine));
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("error: " + ex.Message);
        }
        finally
        {
            if (sw != null)
            {
                sw.Close();
                sw.Dispose();
            }
            if (ssl != null)
            {
                ssl.Close();
                ssl.Dispose();
            }
            if (tcpc != null)
            {
                tcpc.Close();
            }
        }

        self.Send(mailMessage);
    }
}
*/
