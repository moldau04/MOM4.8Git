using System;
using System.Linq;
using System.Web.Services;
using BusinessLayer;
using BusinessEntity;
using System.Threading;

/// <summary>
/// Summary description for IndexTables
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class IndexTables : System.Web.Services.WebService
{
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    public IndexTables()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void IndexMapdata()
    {
        
        try
        {
            objBL_MapData.IndexMapdata(objMapData);
        }
        catch (Exception ex)
        {
            string error = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace + Environment.NewLine;
            mail(error);
            throw ex;
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
                 string from = "index@mom.webserv";

                 if (to.Trim() != string.Empty && from.Trim() != string.Empty)
                 {
                     try
                     {
                         Mail mail = new Mail();
                         mail.From = from;
                         mail.To = to.Split(';', ',').OfType<string>().ToList();
                         mail.Title = "Indexing Error";
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

