using System;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Data;
using BusinessLayer;

public partial class UpdateCoordinates : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
    }

    private void UpdateCoordinate()
    {
        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        ds = objBL_User.getLocations(objProp_User);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (string.IsNullOrEmpty(dr["lat"].ToString()))            
            {
                string address = dr["address"].ToString() +" "+ dr["city"].ToString() +" "+dr["state"].ToString() +" "+dr["zip"].ToString();

                GeoJsonData g = GeoRequest(address.Replace(".",""));

                if (g.results.Length > 0)
                {
                    var p = g.results[0].geometry.location;
                    double latitude = p.lat;
                    double longitude = p.lng;

                    update(Convert.ToInt32(dr["rol"]),latitude.ToString(),longitude.ToString());
                }                
            }
        }
    }

    private void update(int rolid, string Lat, string Lng)
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.RolId = rolid;
        objProp_User.Lat = Lat;
        objProp_User.Lng = Lng;

        objBL_User.UpdateRolCoordinates(objProp_User);
    }

    private GeoJsonData GeoRequest(string address)
    {
        WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&sensor=false");
        request.Method = "GET";
        var response = request.GetResponse();
        string result;
        using (var stream = response.GetResponseStream())
        {
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<GeoJsonData>(result);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //UpdateCoordinate();
    }
}
