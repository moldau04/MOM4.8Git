using System;
using System.Collections.Generic;
using System.Web.UI;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.Script.Serialization;

public partial class maptest1 : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objpropMapData = new MapData();

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadMap();
    }
    private void BindMarkers(DataTable dt)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["latitude"] != DBNull.Value && dr["latitude"].ToString().Trim() != string.Empty)
            {
                row = new Dictionary<string, object>();
                row.Add("parsedate", Convert.ToDateTime(dr["date"]).ToShortTimeString());
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
        }
        string serialised = serializer.Serialize(rows);

        //hdnMarkers.Value = serialised;

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize('"+serialised+"');", true);


    }

    private void LoadMap()
    {
        string connstr = Session["config"].ToString();
        DateTime Date = Convert.ToDateTime("10/22/2012");
        string tech = "JOHN";
        objpropMapData.ConnConfig = connstr;
        objpropMapData.Date = Date;
        objpropMapData.Tech = tech;

        DataSet ds = new DataSet();
        ds = objBL_MapData.GetTimestmpLocation(objpropMapData);

        BindMarkers(ds.Tables[0]);
    }
}
