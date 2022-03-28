using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MOMWebApp.Controllers
{

     
    
    public class ReactNativeController : ApiController
    {

        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
       
       

        // POST api/<controller>
        [Route("MS/RegisterDevice")]
        [HttpPost]
   
        public string RegisterDevice([FromBody] RegisterDevice registerDevice)
        {
            //String userAgent;
            //userAgent = Context.Request.UserAgent;
            //string deviceType = userAgent.Contains("GPSTracker") ? "iOS" : "Android";
            string strOut = "";
            JavaScriptSerializer sr = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

            try
            {
                objGeneral.DeviceType = registerDevice.devicetype;
                objGeneral.DeviceID = registerDevice.deviceid;
                objGeneral.RegID = registerDevice.tokenid;
                objGeneral.fuser = registerDevice.fuser;
                objGeneral.userid = Convert.ToInt32(registerDevice.userid);
                objGeneral.database = registerDevice.database;
                objBL_General.RegisterDeviceNew(objGeneral);
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


        [Route("MS/PingResponseFromDevice")]
        [HttpPost]

        public string PingResponseFromDevice([FromBody] PingResponse pingResponse)
        {
            string strOut;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();

            try
            {
                objGeneral.DeviceID = pingResponse.deviceid;
                objGeneral.RegID = pingResponse.randomid;
                objGeneral.IsRunning = Convert.ToInt32(pingResponse.isRunning);
                objGeneral.IsGPSEnabled = Convert.ToInt32(pingResponse.IsGPSEnabled);
                objGeneral.backgroundRefresh = 0;
                objGeneral.fuser = pingResponse.fuser;
                objGeneral.userid = Convert.ToInt32(pingResponse.userid);
                objGeneral.database = pingResponse.database;
                objGeneral.sentdate = DateTime.Now;
                objBL_General.PingResponseNew(objGeneral);
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

    /// <summary>
    /// 
    /// </summary>
    public class RegisterDevice
    { 
        public string deviceid { get; set; }
        public string tokenid { get; set; }
        public string fuser { get; set; }
        public string userid { get; set; }
        public string devicetype { get; set; } 
        public string database { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class PingResponse
    {
 

        public string deviceid { get; set; }
        public string randomid { get; set; }
        public string isRunning { get; set; }
        public string IsGPSEnabled { get; set; }
        public string fuser { get; set; }
        public string userid { get; set; } 
        public string database { get; set; }

    }


}