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
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.IO;
using System.Net.Mail;
using ImapX;

public partial class testmail1 : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    Pop3Client pop3Client;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        
        if (Application["pop3"] == null)
        {
            Application["pop3"] = 0;
        }

        if ((int)Application["pop3"] == 0)
        {
            Application["pop3"] = 1;
            try
            {
                DataSet ds = new DataSet();
                objGeneral.ConnConfig = Session["config"].ToString();
                ds = objBL_General.GetEmailAccounts(objGeneral);

                DataSet dsEmail = objBL_General.getCRMEmails(objGeneral);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ////Thread email = new Thread(delegate()
                    ////   {
                    ////try
                    ////{

                    string host = dr["inserver"].ToString();
                    string user = dr["inusername"].ToString();
                    string pass = dr["inpassword"].ToString();
                    string port = dr["inport"].ToString();
                    int Userid = Convert.ToInt32(dr["Userid"]);
                    string LastFetch = dr["lastfetch"].ToString();
                    imapmails(host, user, pass, port, Userid, LastFetch, dsEmail);
                    //DownloadMails(host, user, pass, port, Userid);
                    ////}
                    ////catch(Exception ex)
                    ////{
                    ////    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
                    ////}
                    ////  });
                    ////email.IsBackground = true;
                    ////email.Start();
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
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: 'Mail download in progress by another user.',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        //GetMailsfromdb(0);
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

            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.AccountID = user.Trim();
            DataSet ds = objBL_General.GetMsgUID(objGeneral);
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
                        objGeneral.to = Convert.ToString(string.Join(",", objgn.toStringArray(unseenMessage.Headers.To)));
                        objGeneral.cc = Convert.ToString(string.Join(",", objgn.toStringArray(unseenMessage.Headers.Cc)));
                        objGeneral.bcc = Convert.ToString(string.Join(",", objgn.toStringArray(unseenMessage.Headers.Bcc)));
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
                        objGeneral.ConnConfig = Session["config"].ToString();
                        int success = objBL_General.AddEmails(objGeneral);

                        if (success == 1)
                        {
                            string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                            string savepath = savepathconfig + @"\mails\";
                            if (!Directory.Exists(savepath))
                            {
                                Directory.CreateDirectory(savepath);
                            }
                            string filename = AID.ToString() + ".eml";
                            FileInfo file = new FileInfo(savepath + filename);
                            unseenMessage.Save(file);
                        }
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

    private void imapmails(string hostname, string username, string password, string port, int userid, string lastfetch, DataSet CRMEmails)
    {

        //string hostname = "imap.gmail.com", username = "msmtryfor30@gmail.com", password = "ideavate@123";
        using (ImapClient client = new ImapClient(hostname, Convert.ToInt32( port), true, false))//993
        {
            if (client.Connect())
            {
                if (client.Login(username, password))
                {
                    DateTime dateSince = DateTime.Now.AddMonths(-1);
                    if (lastfetch.Trim() != string.Empty)
                        dateSince = Convert.ToDateTime(lastfetch.Trim());

                    dateSince = dateSince.AddDays(-1);
                    string strDate = String.Format("{0:d-MMM-yyyy}", dateSince);

                    foreach (DataRow dr in CRMEmails.Tables[0].Rows)
                    {
                        string strEmail=dr["email"].ToString();
                        IEnumerable<ImapX.Message> messages = client.Folders.All.Search(strDate + " OR (OR (FROM " + strEmail + ") (TO " + strEmail + ")) (CC " + strEmail + ")", ImapX.Enums.MessageFetchMode.Full, Int32.MaxValue);

                        foreach (ImapX.Message msg in messages)
                        {
                            var AID = System.Guid.NewGuid();
                            objGeneral.From = Convert.ToString(msg.From.Address);
                            objGeneral.to = Convert.ToString(string.Join(",",objgn.toStringArray(msg.To)));
                            objGeneral.cc = Convert.ToString(string.Join(",", objgn.toStringArray(msg.Cc)));
                            objGeneral.bcc = Convert.ToString(string.Join(",", objgn.toStringArray(msg.Bcc)));
                            objGeneral.subject = Convert.ToString(msg.Subject);
                            objGeneral.sentdate = msg.Date;
                            objGeneral.Attachments = msg.Attachments.Count();
                            objGeneral.msgid = Convert.ToString(msg.MessageId);
                            objGeneral.uid = msg.UId;
                            objGeneral.GUID = AID;
                            objGeneral.type = 0;
                            objGeneral.userid = userid;
                            objGeneral.AccountID = username.Trim();
                            objGeneral.ConnConfig = Session["config"].ToString();
                            int success = objBL_General.AddEmails(objGeneral);

                            if (success == 1)
                            {
                                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                                string savepath = savepathconfig + @"\mails\";
                                if (!Directory.Exists(savepath))
                                {
                                    Directory.CreateDirectory(savepath);
                                }
                                string filename = AID.ToString() + ".eml";
                                string str = msg.DownloadRawMessage();
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(savepath + filename, true))
                                {
                                    file.WriteLine(str);
                                }
                            }
                            
                        }
                    }                    
                }
            }            
        }
    }

}
