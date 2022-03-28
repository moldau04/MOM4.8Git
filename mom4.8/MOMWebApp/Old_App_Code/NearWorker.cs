using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using BusinessLayer;
using BusinessEntity;
using System.Data;

/// <summary>
/// Summary description for NearWorker
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class NearWorker : System.Web.Services.WebService
{

    public NearWorker()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetNearWorker(string lat, string lng, string worker)
    {
        BL_MapData objBL_MapData = new BL_MapData();
        MapData objMapData = new MapData();
        GeneralFunctions objGeneral = new GeneralFunctions();

        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objMapData.Lat = lat;
        objMapData.Lng = lng;
        objMapData.Worker = worker;
        objMapData.ConnConfig = HttpContext.Current.Session["config"].ToString(); ;
        ds = objBL_MapData.GetNearWorkersByTime(objMapData);
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);
        return str;
    }

}

