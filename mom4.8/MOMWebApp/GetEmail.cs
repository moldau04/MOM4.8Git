using System;
using System.Linq;
using System.Web.Services;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Threading;

/// <summary>
/// Summary description for GetEmail
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class GetEmail : System.Web.Services.WebService
{
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    public GetEmail()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void GetEmailsFromServer()
    {
        if (Application["pop3"] == null)
        {
            Application["pop3"] = 0;
        }

        if ((int)Application["pop3"] == 0)
        {
            Application["pop3"] = 1;
            //Thread email = new Thread(delegate()
            //   {
                   try
                   {
                       DataSet ds = new DataSet();
                       objGeneral.ConnConfig = objGeneralFunctions.ConnectionStr(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailDatabase"].Trim());
                       ds = objBL_General.GetEmailAccounts(objGeneral);
                       DataSet dsEmail = objBL_General.getCRMEmails(objGeneral);
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           string host = dr["inserver"].ToString();
                           string user = dr["inusername"].ToString();
                           string pass = dr["inpassword"].ToString();
                           string port = dr["inport"].ToString();
                           int Userid = Convert.ToInt32(dr["Userid"]);
                           string LastFetch = dr["lastfetch"].ToString();
                           objGeneral.AccountID = user;
                           int MAXUID = objBL_General.GetMAXEmailUID(objGeneral);
                           objGeneralFunctions.DownloadMailsIMAP(host, user, pass, port, Userid, objGeneralFunctions.ConnectionStr(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailDatabase"].Trim()), MAXUID, dsEmail);
                       }
                   }
                   catch (Exception ex)
                   {
                       string error = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace + Environment.NewLine;
                       SaveError(error);
                       throw ex;
                   }
                   finally
                   {
                       Application["pop3"] = 0;
                   }
               //});
               // email.IsBackground = true;
               // email.Start();
        }
    }

    private void SaveError(string statusMessage)
    {
        try
        {
            objGeneral.QBapi = "GetEmailsFromServer";
            objGeneral.QBStatusMessage = statusMessage;
            objGeneral.ConnConfig = objGeneralFunctions.ConnectionStr(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailDatabase"].Trim());
            objBL_General.AddQBErrorLog(objGeneral);

            mail(statusMessage);
        }
        catch
        {

        }
    }

    private void mail(string message)
    {
        string to = System.Web.Configuration.WebConfigurationManager.AppSettings["ErrorEmail"].Trim();
        if (!string.IsNullOrEmpty(to))
        {
            Thread email = new Thread(delegate()
            {
                //string to = "@gmail.com";
                string from = "mailsync@mom.webserv";

                if (to.Trim() != string.Empty && from.Trim() != string.Empty)
                {
                    try
                    {
                        Mail mail = new Mail();
                        mail.From = from;
                        mail.To = to.Split(';', ',').OfType<string>().ToList();
                        mail.Title = "IMAP Error";
                        mail.Text = message;
                        mail.RequireAutentication = false;
                        mail.Send();
                    }
                    catch { }
                }
            });
            email.IsBackground = true;
            email.Start();
        }
    }

}

