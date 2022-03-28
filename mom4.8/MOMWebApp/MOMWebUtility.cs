using BusinessEntity.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MOMWebApp
{
    public class MOMWebUtility
    {
        public static string BaseApiUrl { get; set; }

        public static string API_Token { get; set; }

        public static string ReportBaseApiUrl { get; set; }
        public MOMWebUtility()
        {
            BaseApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["BaseApiUrl"].Trim();
            ReportBaseApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportBaseApiUrl"].Trim();
            API_Token = HttpContext.Current.Session["API_Token"].ToString();
        }
        public APIResponse CallMOMWebAPI(string APIName  , object obj,bool IsReportAPI=false)
        {

            string JsonParameters = JsonConvert.SerializeObject(obj); 

            APIRequest _APIRequest = new APIRequest();

            _APIRequest.Token = HttpContext.Current.Session["API_Token"].ToString();

            _APIRequest.Param = SSTCryptographer.Encrypt(JsonParameters,"core");

            string _APIRequestJson = JsonConvert.SerializeObject(_APIRequest);

            WebClient client = new WebClient();

            client.Headers["Content-type"] = "application/json";

            client.Encoding = Encoding.UTF8;
                
            string APIResponsJson=   client.UploadString(IsReportAPI == false? BaseApiUrl + APIName : ReportBaseApiUrl + APIName, _APIRequestJson);

            //APIResponse _APIResponse = (new JavaScriptSerializer()).Deserialize<APIResponse>(APIResponsJson);

            // Change for avoid "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property. Parameter name: input" this Error //

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            APIResponse _APIResponse = serializer.Deserialize<APIResponse>(APIResponsJson);

            _APIResponse.ResponseData = SSTCryptographer.Decrypt(_APIResponse.ResponseData,"core");

            return _APIResponse;
        }

    }
}