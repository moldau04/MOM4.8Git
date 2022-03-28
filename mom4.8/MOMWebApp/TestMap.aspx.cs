using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;

public partial class TestMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getmap()
    {
        GeneralFunctions genfunc = new GeneralFunctions();

        BL_MapData objBL_MapData = new BL_MapData();
        MapData objpropMapData = new MapData();        

        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objpropMapData.Date =Convert.ToDateTime("1/31/2013");
        objpropMapData.Tech = "KUNAL";

            DataSet ds = new DataSet();
            ds = objBL_MapData.GetTimestmpLocation(objpropMapData);
            JavaScriptSerializer js = new JavaScriptSerializer();
           string resp=js.Serialize( genfunc.RowsToDictionary(ds.Tables[0]));
           return resp;
    }
}
