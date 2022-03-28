using BusinessEntity;
using BusinessLayer;
using BusinessLayer.Billing;
using BusinessLayer.Schedule;
using MobilePushNotification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
//using Telerik.Web.UI;

/// <summary>
/// Summary description for MapFill
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MapFill : System.Web.Services.WebService
{
    GeneralFunctions objGeneral = new GeneralFunctions();

    Wage _objWage = new Wage();
    BL_User _objBLUser = new BL_User();
    BL_Customer _objBLCustomer = new BL_Customer();
    BL_Job _objBLJob = new BL_Job();

    public MapFill()
    {

    }




    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getlocationAddress(string Tech, string Date, string Time, string TicketID, string timestamp)
    {
        string strResponse = "Not Available";
        BL_MapData objBL_MapData = new BL_MapData();
        MapData objpropMapData = new MapData();

        GeneralFunctions genFunction = new GeneralFunctions();

        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objpropMapData.Tech = Tech;
        objpropMapData.Date = Convert.ToDateTime(Date);
        objpropMapData.CallDate = Convert.ToDateTime(Time);
        objpropMapData.TicketID = Convert.ToInt32(TicketID);
        objpropMapData.TempId = timestamp;

        DataSet dsLoc = objBL_MapData.getlocationAddress(objpropMapData);

        if (dsLoc.Tables[0].Rows.Count > 0)
        {
            string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();

            GeoJsonData g = genFunction.GeoRequest(dsLoc.Tables[0].Rows[0]["latitude"].ToString(), dsLoc.Tables[0].Rows[0]["longitude"].ToString(), mapsAPIKey);

            if (g.results.Length > 0)
            {
                strResponse = g.results[0].formatted_address;
            }
            else
            {
                strResponse = "Not Available";
            }
        }

        return strResponse;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetMapData(string fUser, string date ,string cat , string iscall )
    {
        //List<MapData> resMap = new List<MapData> { };
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();        
        DateTime Date = Convert.ToDateTime(date);
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString(); 
        objpropMapData.Date = Date;
        objpropMapData.Tech = fUser;
        objpropMapData.Category = cat;
        objpropMapData.TicketID = Convert.ToInt32(iscall);
        BL_MapData objBL_MapData = new BL_MapData();
        ds = objBL_MapData.GetTimestmpLocationTest(objpropMapData);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetOpenTicket(string fUser, string date)
    {
        //List<MapData> resMap = new List<MapData> { };
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        DateTime Date = Convert.ToDateTime(date);
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objpropMapData.Date = Date;
        objpropMapData.Tech = fUser;
        BL_MapData objBL_MapData = new BL_MapData();
        ds = objBL_MapData.GetOpenTicket(objpropMapData);
        //DataTable dtTableNew = ds.Tables[0].Copy();
        //foreach (DataRow drtableOld in dtTableNew.Rows)
        //{}
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        // GeneralFunctions objGeneral = new GeneralFunctions();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetTechCurrentLocationNew()
    {
        //List<MapData> resMap = new List<MapData> { };
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        BL_MapData objBL_MapData = new BL_MapData();
        ds = objBL_MapData.GetTechCurrentLocationNew(objpropMapData);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetTokenAndDeviceType(string fUser)
    {
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        objpropMapData.Tech = fUser;
        BL_MapData objBL_MapData = new BL_MapData();
        objpropMapData.fuser = fUser;
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_MapData.GetTokenAndDeviceType(objpropMapData);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLogData(string fUser, string date, string database)
    {
        //List<MapData> resMap = new List<MapData> { };
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        DateTime Date = Convert.ToDateTime(date);
        objpropMapData.Date = Date;
        objpropMapData.Tech = fUser;
        objpropMapData.Database = database;
        BL_MapData objBL_MapData = new BL_MapData();
        ds = objBL_MapData.GetLogData(objpropMapData);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod(EnableSession = true)]    
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string GetOpenCallsMapScreen(string fUser,string date, string  CategoryName)
    {
        string str;
        DataSet ds = new DataSet();
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.FieldEmp = fUser;
        objProp_User.Edate = Convert.ToDateTime(date);
        if(CategoryName != "NoCat")
         objProp_User.CategoryName = GetSelectedCategory(CategoryName); 
        else
            objProp_User.CategoryName = CategoryName;
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        if (Convert.ToString((HttpContext.Current.Session["CmpChkDefault"].ToString())) == "1")
        {
            objProp_User.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
        }        
        ds = objBL_User.getOpenCallsOnMap(objProp_User);
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string getUpdate(string fUser,string DeviceType)
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        objGeneral.fuser = fUser;
        objGeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();
        DataSet dspingResponse = objBL_General.getPingNew(objGeneral);
        string strResponse = "0";
        string strRunning = "";
        string strGPS = "";
        if (dspingResponse.Tables[0].Rows.Count > 0)
        {
            if (DeviceType.ToLower() == "ios")
            {
                strRunning = "Location Service : OFF";
                if (dspingResponse.Tables[0].Rows[0]["IsGPSEnabled"].ToString() == "1")
                {
                    strRunning = "Location Service : ON";
                }

                string strLocationAccess = "Location Access : Never";

                if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "0")
                {
                    strLocationAccess = "Location Access : User has not yet made a choice with regards to this application";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "1")
                {
                    strLocationAccess = "Location Access : This application is not authorized to use location services.";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "2")
                {
                    strLocationAccess = "Location Access : User has explicitly denied authorization for this application, or location services are disabled in Settings.";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "3")
                {
                    strLocationAccess = "Location Access : Always";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "4")
                {
                    strLocationAccess = "Location Access : User has granted authorization to use their location only when your app is visible to them";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "5")
                {
                    strLocationAccess = "Location Access : User has authorized this application to use location services.";
                }


                string strBackgroundRefresh = "Background Refresh : NA";
                if (dspingResponse.Tables[0].Rows[0]["backgroundRefresh"].ToString() == "0")
                {
                    strBackgroundRefresh = "Background Refresh : OFF";
                }
                else if (dspingResponse.Tables[0].Rows[0]["backgroundRefresh"].ToString() == "1")
                {
                    strBackgroundRefresh = "Background Refresh : ON";
                }

                strResponse = strRunning + "</BR>" + strLocationAccess + "</BR>" + strBackgroundRefresh;
            }
            else
            {
                strRunning = "GPS Tracking : OFF";
                if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "1")
                {
                    strRunning = "GPS Tracking : ON";
                }

                strGPS = "GPS : OFF";
                if (dspingResponse.Tables[0].Rows[0]["IsGPSEnabled"].ToString() == "1")
                {
                    strGPS = "GPS : ON";
                }

                strResponse = strRunning + "</BR>" + strGPS;
            }
        }

        return strResponse;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public  string SendPushNoti(string Username)
    {
        string ConnConfig = HttpContext.Current.Session["config"].ToString();
        JavaScriptSerializer js = new JavaScriptSerializer();
        string strResp = string.Empty;
        string[] strResponse = new string[4];
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        GeneralFunctions genFunction = new GeneralFunctions();
        AndroidPushNotification theAndroidPn = new AndroidPushNotification();
        IOSPushNotification theIOSPn = new IOSPushNotification();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.Username = Username;
        objGeneral.DeviceID = objBL_User.getUserDeviceID(objProp_User);
        strResponse[0] = objGeneral.DeviceID;

        string tokenID = objBL_General.GetDeviceTokenID(objGeneral);

        string DeviceType = "";

        DeviceType = objBL_General.GetDeviceType(objGeneral);
       // DeviceType = "APA91bEe2suFXKo6XcsQPNeGw5r5snhoir5a2_e3lTf1IquhCz4E-BAvfcXCbEmJmYbcKLBmMTaFyiuDqvtyZn_1_IsZf1cIw59cGa0yZzEaTtU7UGDpEmY";
        string strRandom = genFunction.generateRandomString(10);
        strResponse[1] = strRandom;

        if (tokenID != "")
        {

            if (DeviceType == "iOS")
            {
                string message = "Requesting your location..";
                string CertificatePath = (WebConfigurationManager.AppSettings["IOSPushNPath"].Trim());
                string notificationType = "1";
                string sResponseFromServer = "";
                String Hostname = WebConfigurationManager.AppSettings["IOSPushNHostName"].Trim();
                sResponseFromServer = theIOSPn.PushToiPhone(tokenID, message, CertificatePath, notificationType, Hostname, strRandom);

                if (sResponseFromServer.Contains("Error") == false)
                {
                    strResponse[2] = "1";
                }
                else
                {
                    strResponse[2] = "Error pinging the device : " + sResponseFromServer;
                }
            }

            else
            {
                DeviceType = "Android";
                string sResponseFromServer = "";
                sResponseFromServer = theAndroidPn.PushToAndroid(WebConfigurationManager.AppSettings["AndroidPNServerUrl"].Trim(),
                                                 strRandom,
                                                  WebConfigurationManager.AppSettings["GoogleAppID"].Trim(),
                                                  WebConfigurationManager.AppSettings["SENDER_ID"].Trim(),
                                                  DateTime.UtcNow.ToString(),
                                                  tokenID);

                if (sResponseFromServer.Contains("Error") == false)
                {
                    strResponse[2] = "1";
                }
                else
                {
                    strResponse[2] = "Error pinging the device : " + sResponseFromServer;
                }

            }
        }
        else
        {
            strResponse[2] = "Device not registered for ping.";

        }
        strResponse[3] = DeviceType;
        strResp = js.Serialize(strResponse);

        return strResp;
    }




    private  string GetSelectedCategory(string labels)
{
        string selectedvals = string.Empty;
        string[] split = labels.Split(',');
        foreach (string item in split)
        {
            selectedvals += "'" + item + "'" + ',';
        }

        return selectedvals.TrimEnd(',');
}

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetMapDataTest(string fUser, string date, string CategoryName)
    {
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        DateTime Date = Convert.ToDateTime(date);
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objpropMapData.Date = Date;
        objpropMapData.Tech = fUser;
        objpropMapData.Category = CategoryName;
        BL_MapData objBL_MapData = new BL_MapData();
        ds = objBL_MapData.GetTimestmpLocationTest1(objpropMapData);
        //for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //{
        //    if (i == 150)
        //    {
        //        ds.Tables[0].Rows[i]["timestm"] = "2";
        //    }
        //}
        JavaScriptSerializer sr = new JavaScriptSerializer();
        //List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        // GeneralFunctions objGeneral = new GeneralFunctions();
        //dictListEval = objGeneral.RowsToDictionary(ds);
        //str = sr.Serialize(dictListEval);
        str  = objGeneral.GetJson(ds);        
        return str;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GeoRequest(string address)
    {    
        string str;
        MapData objpropMapData = new MapData();
        DataSet ds = new DataSet();
        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        BL_MapData objBL_MapData = new BL_MapData();
        //ds = objBL_MapData.GetTimestmpLocationTest1(objpropMapData);
    
        string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();
        GeneralFunctions genFunction = new GeneralFunctions();
        GeoJsonData g = genFunction.GeoRequest(address, mapsAPIKey);
        if (g.results.Length > 0)
        {
            var p = g.results[0].geometry.location;
            string lat = p.lat.ToString();
            string lng = p.lng.ToString();
        }
        var json = new JavaScriptSerializer().Serialize(g);
        str = json.ToString();
        //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //Test routes_list =
        //       (Test)json_serializer.DeserializeObject("{ \"lat\":\"some data\" }");

        //JavaScriptSerializer sr = new JavaScriptSerializer();
        //str = objGeneral.GetJson(ds);
        return str;
    }


    class Test
    {

        String lat;
        String lng;
      

    }
    private string GetSelectedCategory1(object[] labels)
    {
        string selectedvals = string.Empty;
        foreach (object item in labels)
        {
            selectedvals += "'" + item + "'" + ',';
        }
       
        return selectedvals.TrimEnd(',');
    }
    public class CheckListBoxItem
    {
        public string Text { get; set; }
    }
}
