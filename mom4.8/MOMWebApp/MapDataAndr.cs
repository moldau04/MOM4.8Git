using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using Newtonsoft.Json;



/// <summary>
/// Summary description for MapDataAndr
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[System.Web.Script.Services.ScriptService]
public class MapDataAndr : System.Web.Services.WebService
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMData = new MapData();

    General objgeneral = new General();
    BL_General objBL_General = new BL_General();

    public MapDataAndr()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string MapGeoLoc(List<Dictionary<string, object>> geoData)//(string geoData)
    {
        string connstr = "";

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
        string strGPSping = string.Empty;

        try
        {
            strGPSping = objBL_General.GetGPSInterval(objgeneral);

            dictionary1 = geoData[0];

            deviceId = dictionary1["deviceId"].ToString();
            object[] locList = (object[])dictionary1["data"];
            DataRow row;

            foreach (Dictionary<string, object> a in locList)
            {
                row = dtData.NewRow();
                row["deviceId"] = deviceId;
                row["latitude"] = a["lat"].ToString();
                row["longitude"] = a["long"].ToString();
                row["date"] = Convert.ToDateTime(a["date"].ToString().Replace(".", string.Empty));
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

            objMData.ConnConfig = connstr;
            objMData.LocData = new DataTable();
            objMData.LocData = dtData;

            objBL_MapData.AddMapData(objMData);

            dictionary2.Add("Success", "1");
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary2.Add("GPSinterval", strGPSping);
            dictionary.Add(dictionary2);

            str = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary2.Add("Success", "0");
            dictionary2.Add("ErrMsg", ex.Message);
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary2.Add("GPSinterval", strGPSping);
            dictionary.Add(dictionary2);
            str = sr.Serialize(dictionary);

            ////dictionary1 = geoData[0];
            ////deviceId = dictionary1["deviceId"].ToString();
            //string reportname = System.DateTime.Now.Ticks + ".txt";
            //string strLogpath = System.Web.Configuration.WebConfigurationManager.AppSettings["GPSLogPath"].Trim();
            //string filename = Path.Combine(strLogpath, reportname);           
            //string filePath = filename;
            //StreamWriter w;
            //w = File.CreateText(filePath);
            //w.WriteLine(sr.Serialize(geoData));            
            //w.Flush();
            //w.Close();            
        }
        return str;
    }




    ///Create New Method For Mobile Device 
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
   // public string MapGeoLocNew(List<Dictionary<string, object>> geoData)//(string geoData)
    public string MapGeoLocNew(string geoData)//(string geoData)
    {
        string connstr = "";

        string json = geoData;
        Dictionary<string, string> dictionary11 = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        //Console.WriteLine(values.Count);
        // 2
        //Console.WriteLine(values["key1"]);

        JavaScriptSerializer sr = new JavaScriptSerializer();

        //Dictionary<string, object> dictionary1 = new Dictionary<string, object>();

        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        List<Dictionary<string, object>> dictionary = new List<Dictionary<string, object>>();

        DataTable dtData = new DataTable();
        dtData.Columns.Add("deviceId", typeof(string));
        dtData.Columns.Add("latitude", typeof(string));
        dtData.Columns.Add("longitude", typeof(string));
        dtData.Columns.Add("date", typeof(DateTime));
        dtData.Columns.Add("fake", typeof(int));
        dtData.Columns.Add("accuracy", typeof(string));
        dtData.Columns.Add("fUser", typeof(string));
        dtData.Columns.Add("userId", typeof(string));


        string deviceId,fake ,str;
        string date;
        string strGPSping = string.Empty;

        try
        {
            strGPSping = objBL_General.GetGPSInterval(objgeneral);

            //dictionary1 = geoData[0];

            deviceId = dictionary11["deviceId"].ToString();
            date = dictionary11["date"].ToString();
            fake = dictionary11["fake"].ToString();
       

            //object[] locList = (object[])sr.Deserialize<object>(json);
            DataRow row;
            row = dtData.NewRow();
            row["deviceId"] = deviceId;
            row["latitude"] = dictionary11["latitude"].ToString();
            row["longitude"] = dictionary11["longitude"].ToString();

            //row["date"] = Convert.ToDateTime(date.Substring(0,10).Replace(".", string.Empty));
            row["date"] = Convert.ToDateTime(date.Replace("T", " ").Replace("Z", " "));

            if (dictionary11.ContainsKey("fake"))
            {
                row["fake"] = fake.ToString();
            }
            else
            {
                row["fake"] = "0";
            }

            if (dictionary11.ContainsKey("accuracy"))
            {
                row["accuracy"] = dictionary11["accuracy"].ToString();
            }
            else
            {
                row["accuracy"] = "0";
            }
            row["fUser"] = dictionary11["fUser"].ToString();
            row["userId"] = dictionary11["userId"].ToString();
            dtData.Rows.Add(row);

            //foreach (Dictionary<string, object> a in locList)
            //{
            //    row = dtData.NewRow();
            //    row["deviceId"] = deviceId;
            //    row["latitude"] = a["lat"].ToString();
            //    row["longitude"] = a["long"].ToString();
            //    row["date"] = Convert.ToDateTime(a["date"].ToString().Replace(".", string.Empty));

            //    if (a.ContainsKey("fake"))
            //    {
            //        row["fake"] = a["fake"].ToString();
            //    }
            //    else
            //    {
            //        row["fake"] = "0";
            //    }

            //    if (a.ContainsKey("accuracy"))
            //    {
            //        row["accuracy"] = a["accuracy"].ToString();
            //    }
            //    else
            //    {
            //        row["accuracy"] = "0";
            //    }
            //    row["fUser"] = a["fUser"].ToString();
            //    row["userId"] = a["userId"].ToString();
            //    dtData.Rows.Add(row);
            //}

            objMData.ConnConfig = connstr;
            objMData.LocData = new DataTable();
            objMData.LocData = dtData;

            objBL_MapData.AddMapNewData(objMData);

            dictionary2.Add("Success", "1");
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary2.Add("GPSinterval", strGPSping);
            dictionary.Add(dictionary2);

            str = sr.Serialize(dictionary);
           // str = sr.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            dictionary2.Add("Success", "0");
            dictionary2.Add("ErrMsg", ex.Message);
            dictionary2.Add("timestamp", System.DateTime.Now.ToString());
            dictionary2.Add("GPSinterval", strGPSping);
            dictionary.Add(dictionary2);
            str = sr.Serialize(dictionary);

            ////dictionary1 = geoData[0];
            ////deviceId = dictionary1["deviceId"].ToString();
            //string reportname = System.DateTime.Now.Ticks + ".txt";
            //string strLogpath = System.Web.Configuration.WebConfigurationManager.AppSettings["GPSLogPath"].Trim();
            //string filename = Path.Combine(strLogpath, reportname);           
            //string filePath = filename;
            //StreamWriter w;
            //w = File.CreateText(filePath);
            //w.WriteLine(sr.Serialize(geoData));            
            //w.Flush();
            //w.Close();            
        }
        return str;
    }
}

