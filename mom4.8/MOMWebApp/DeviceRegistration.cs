using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using BusinessLayer;
using BusinessEntity;

/// <summary>
/// Summary description for DeviceRegistration
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class DeviceRegistration : System.Web.Services.WebService
{

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    public DeviceRegistration()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string RegisterDevice(string deviceId, string tokenID)
    {



        String userAgent;
        userAgent = Context.Request.UserAgent;

        string deviceType = userAgent.Contains("GPSTracker") ? "iOS" : "Android";

        string strOut;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

        try
        {
            objGeneral.DeviceType = deviceType;
            objGeneral.DeviceID = deviceId;
            objGeneral.RegID = tokenID;
            objBL_General.RegisterDevice(objGeneral);
            dictionary.Add("Success", "1");
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }
        return strOut;
    }

   

    /// <summary>
    ///  For Ios
    /// </summary>
    /// <param name = "deviceId" ></ param >
    /// < param name="tokenID"></param>
    /// <param name = "deviceType" ></ param >
    /// < returns ></ returns >
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

    public string RegisterDeviceWithType(string deviceId, string tokenID, string deviceType)
    {
        string strOut;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

        try
        {
            objGeneral.DeviceType = deviceType;
            objGeneral.DeviceID = deviceId;
            objGeneral.RegID = tokenID;
            objBL_General.RegisterDevice(objGeneral);
            dictionary.Add("Success", "1");
            dictionary.Add("timestamp", DateTime.Now.ToString());
            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }
        return strOut;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string PingResponseFromDevice(string deviceId, string randomId, string IsRunning, string IsGPSEnabled)
    {
        string strOut;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

        try
        {
            objGeneral.DeviceID = deviceId;
            objGeneral.RegID = randomId;
            objGeneral.IsRunning = Convert.ToInt32(IsRunning);
            objGeneral.IsGPSEnabled = Convert.ToInt32(IsGPSEnabled);
            //objGeneral.IsGPSEnabled = Convert.ToInt32(0);
            objGeneral.backgroundRefresh = 0;
            objBL_General.PingResponse(objGeneral);

            dictionary.Add("Success", "1");
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }
        return strOut;
    }

  

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string iOSPingResponseFromDevice(string deviceId, string randomId, string IsRunning, string IsGPSEnabled, string backgroundRefresh)
    {
        string strOut;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

        try
        {
            objGeneral.DeviceID = deviceId;
            objGeneral.RegID = randomId;
            objGeneral.IsRunning = Convert.ToInt32(IsRunning);
            objGeneral.IsGPSEnabled = Convert.ToInt32(IsGPSEnabled);
            objGeneral.backgroundRefresh = Convert.ToInt32(backgroundRefresh);

            //objGeneral.IsGPSEnabled = Convert.ToInt32(0);
            objBL_General.PingResponse(objGeneral);

            dictionary.Add("Success", "1");
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);

            strOut = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary.Add("Success", "0");
            dictionary.Add("ErrMsg", ex.Message);
            dictionary.Add("timestamp", System.DateTime.Now.ToString());
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
        }
        return strOut;
    }




}

