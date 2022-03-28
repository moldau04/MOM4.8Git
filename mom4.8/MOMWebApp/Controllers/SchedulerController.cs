using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MOMWebApp.Controllers
{
    public class SchedulerController : ApiController
    {
      

        [Route("Map/GetGPSMapData")]
        [HttpPost]
        public string GetGPSMapData([FromBody] GPS_LiveData _GPS_LiveData)
        {

            string _webconfig = SSTCryptographer.Decrypt(_GPS_LiveData.webconfig, "webconfig");

            GeneralFunctions objGeneral = new GeneralFunctions();

            //List<MapData> resMap = new List<MapData> { };
            //string str;
            //MapData objpropMapData = new MapData();
            //DataSet ds = new DataSet();
            //DateTime Date = Convert.ToDateTime(_GPS_LiveData.date);
            //objpropMapData.ConnConfig = _webconfig;
            //objpropMapData.Date = Date;
            //objpropMapData.Tech = _GPS_LiveData.fUser;
            //objpropMapData.Category = _GPS_LiveData.category;
            //objpropMapData.ISTicketD = Convert.ToInt16(_GPS_LiveData.Iscall);
            //BL_MapData objBL_MapData = new BL_MapData();
            //ds = objBL_MapData.GetTimestmpLocationLatest(objpropMapData);
            //JavaScriptSerializer sr = new JavaScriptSerializer();
            //List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
            //dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            //str = sr.Serialize(dictListEval);
            //return str;


            //List<MapData> resMap = new List<MapData> { };
            string str;
            MapData objpropMapData = new MapData();
            DataSet ds = new DataSet();
            DateTime Date = Convert.ToDateTime(_GPS_LiveData.date);
            objpropMapData.ConnConfig = _webconfig;
            objpropMapData.Date = Date;
            objpropMapData.Tech = _GPS_LiveData.fUser;
            BL_MapData objBL_MapData = new BL_MapData();
            ds = objBL_MapData.GetTimestmpLocationTest(objpropMapData);
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
            dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = sr.Serialize(dictListEval);
            return str;
        }



        [Route("Map/GetTokenbyuser")]
        [HttpPost]
        public string GetTokenbyuser([FromBody] PingM _PingM)
        {

            string _webconfig =  SSTCryptographer.Decrypt(_PingM.webconfig, "webconfig");

            string strOut;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();            
            string tokenID = new BL_General().GetDeviceTokenbyuser(_PingM.workername, _webconfig) ;


            dictionary.Add("Success", tokenID.Length > 1 ? "200":"500");


            dictionary.Add("token", tokenID);
       
            dictionaryList.Add(dictionary);
            strOut = sr.Serialize(dictionary);
            return strOut; 

        }


        [Route("Map/GetpingResponse")]
        [HttpPost]
        public  string GetpingResponse([FromBody] PingM _PingM)
        {


            string _webconfig = SSTCryptographer.Decrypt(_PingM.webconfig, "webconfig");

            DataSet dspingResponse = new BL_General().GetPingResponse(_webconfig, _PingM.workername, _PingM.Randomid);
            string strResponse = "0";
            string strRunning = "";
            string strGPS = "";
            if (dspingResponse.Tables[0].Rows.Count > 0)
            {
                if (dspingResponse.Tables[0].Rows[0]["DeviceType"].ToString().ToLower() == "ios")
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


        public class  PingM
        { 
            public string Randomid { get; set; } 
            public string workername { get; set; }
            public string webconfig { get; set; }

        }


        public class GPS_LiveData
        {
           

            public string fUser { get; set; }
            public string date { get; set; }
            public string category { get; set; } 
            public string Iscall { get; set; }
            public string webconfig { get; set; }

        }

    }
}