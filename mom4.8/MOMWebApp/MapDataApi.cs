using System;
using System.Collections;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.IO;

/// <summary>
/// Summary description for MapDataApi
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MapDataApi : System.Web.Services.WebService
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMData = new MapData();

    public MapDataApi()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string MapGeoLocData()//(string geoData)
    //public string MapGeoLocData(Dictionary<string, object> geoData)//(string geoData)
    {
        Context.Request.ContentType = "application/json";
        Context.Request.ContentEncoding = System.Text.Encoding.UTF8;
        Context.Request.InputStream.Position = 0;
        var paramStr = string.Empty;
        StreamReader reader = new StreamReader(Context.Request.InputStream);
        paramStr = reader.ReadToEnd();
        var geoData = new Dictionary<string, object>();
        JavaScriptSerializer srtest = new JavaScriptSerializer();
        geoData = srtest.Deserialize<Dictionary<string, object>>(paramStr);

        JavaScriptSerializer sr = new JavaScriptSerializer();

        Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionary = new List<Dictionary<string, object>>();

        DataTable dtData = new DataTable();
        dtData.Columns.Add("deviceId", typeof(string));
        dtData.Columns.Add("latitude", typeof(string));
        dtData.Columns.Add("longitude", typeof(string));
        dtData.Columns.Add("date", typeof(DateTime));
        dtData.Columns.Add("fake", typeof(int));
        dtData.Columns.Add("accuracy", typeof(string));

        string deviceId, str;

        try
        {
            dictionary1 = geoData;

            deviceId = dictionary1["deviceId"].ToString();
            ArrayList locList = (ArrayList)dictionary1["data"];
            DataRow row;

            foreach (Dictionary<string, object> a in locList)
            {
                row = dtData.NewRow();
                row["deviceId"] = deviceId;
                row["latitude"] = a["lat"].ToString();
                row["longitude"] = a["long"].ToString();
                row["date"] = Convert.ToDateTime(a["date"].ToString());

                if (a.ContainsKey("fake"))
                {
                    row["fake"] = a["fake"].ToString();
                }
                else
                {
                    row["fake"] = "0";
                }

                if (a.ContainsKey("accuracy"))
                {
                    row["accuracy"] = a["accuracy"].ToString();
                }
                else
                {
                    row["accuracy"] = "0";
                }


                dtData.Rows.Add(row);
            }

            objMData.LocData = new DataTable();
            objMData.LocData = dtData;

            objBL_MapData.AddMapData(objMData);

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
        }
        return str;
    }
}

