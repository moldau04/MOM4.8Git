using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;

/// <summary>
/// Summary description for IOSLog
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class IOSLog : System.Web.Services.WebService
{
    General objgeneral = new General();
    BL_General objBL_General = new BL_General();

    public IOSLog()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string IOSGetLog(string date, string deviceid, string doc, string username, string DeviceInfo)
    {
        JavaScriptSerializer sr = new JavaScriptSerializer();

        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionary = new List<Dictionary<string, object>>();
        string str;
        username = username.Replace('|', '_');
        string reportname = username + "_" + DeviceInfo + "_" + deviceid + "_" + System.DateTime.Now.Ticks + ".txt";

        try
        {
            byte[] bytes = Convert.FromBase64String(doc);
            //string filename = Path.Combine(Server.MapPath(this.Context.Request.ApplicationPath) + "/IOSLog", reportname);
            string strLogpath = System.Web.Configuration.WebConfigurationManager.AppSettings["GPSLogPath"].Trim();
            string filename = Path.Combine(strLogpath, reportname);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

            dictionary2.Add("Success", "1");
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary.Add(dictionary2);

            str = sr.Serialize(dictionary);

        }
        catch (Exception ex)
        {
            dictionary2.Add("Success", "0");
            dictionary2.Add("ErrMsg", ex.Message);
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary.Add(dictionary2);
            str = sr.Serialize(dictionary);

            string ServiceName = "IOSGetLog";
            string Error = ex.Message + " --- Stack Trace : " + ex.StackTrace + " --- Inner Exception : " + ex.InnerException;
            objgeneral.ServiceName = ServiceName;
            objgeneral.Error = Error;
            objBL_General.LogError(objgeneral);

        }
        return str;
    }
}

