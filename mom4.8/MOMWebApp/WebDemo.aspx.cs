using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using Telerik.Web.UI;
//using Ext.Net;

namespace MOMWebApp
{
    public partial class WebDemo : System.Web.UI.Page
    {
        private static string TOOLTIP_TEMPLATE = @"
            <div class=""leftCol"">
                <div class=""flag flag-{0}""></div>
            </div>
            <div class=""rightCol"">
                <div class=""country"">{1}</div>
                <div class=""city"">{2}</div>
                <div class=""address"">{3}</div>
            </div>";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    RadMap1.DataSource = GetData();
            //    RadMap1.DataBind();
            //}
        }
        //private DataSet GetData()
        //{
        //    DataSet ds = new DataSet("TelerikOffices");

        //    DataTable dt = new DataTable("TelerikOfficesTable");
        //    dt.Columns.Add("Shape", Type.GetType("System.String"));
        //    dt.Columns.Add("Country", Type.GetType("System.String"));
        //    dt.Columns.Add("City", Type.GetType("System.String"));
        //    dt.Columns.Add("Address", Type.GetType("System.String"));
        //    dt.Columns.Add("Latitude", Type.GetType("System.Decimal"));
        //    dt.Columns.Add("Longitude", Type.GetType("System.Decimal"));

        //    dt.Rows.Add("PinTarget", "United States", "Palo Alto", "169 University Ave.<br />Palo Alto 94301", 37.444610, -122.163283);
        //    dt.Rows.Add("PinTarget", "United States", "Boston, MA", "201 Jones Rd Waltham<br />Boston MA 02451", 42.375067, -71.272233);
        //    dt.Rows.Add("PinTarget", "Denmark", "Copenhagen", "Vesterbrogade 149<br />Copenhagen DK-1620 Copenhagen V", 55.670312, 12.538266);
        //    dt.Rows.Add("PinTarget", "Australia", "Sydney", "Suite 705, 80 Mount St<br>Sydney North Sydney, NSW 2060", -33.838707, 151.207959);
        //    dt.Rows.Add("PinTarget", "United States", "Austin, TX", "221 W 6th Street Suite 850<br />Austin TX 78701", 30.268162, -97.744873);
        //    dt.Rows.Add("PinTarget", "Bulgaria", "Sofia", "33 Alexander Malinov Blvd.<br />Sofia 1729", 42.650613, 23.379025);
        //    dt.Rows.Add("PinTarget", "India", "Gurgaon", "Unit No 505, Tower A Spaze iTech<br />Park Gurgaon Sohna Road Sector 49<br />Gurgaon Haryana. 122002", 28.410139, 77.042439);
        //    dt.Rows.Add("PinTarget", "United Kingdom", "London", "14 Austin Friars<br />London EC2N 2HE", 51.515986, -0.085798);
        //    dt.Rows.Add("PinTarget", "Germany", "Munich", "Balanstrasse 73<br />Munich 81541 Munich", 48.117227, 11.601990);

        //    ds.Tables.Add(dt);
        //    return ds;
        //}
        //protected void RadMap1_ItemDataBound(object sender, UI.Map.MapItemDataBoundEventArgs e)
        //{
        //    MapMarker marker = e.Item as MapMarker;
        //    if (marker != null)
        //    {
        //        DataRowView item = e.DataItem as DataRowView;
        //        string country = item.Row["Country"] as string;
        //        string city = item.Row["City"] as string;
        //        string address = item.Row["Address"] as string;
        //        marker.TooltipSettings.Content = String.Format(TOOLTIP_TEMPLATE, country.ToLower().Replace(" ", string.Empty), country, city, address);
        //    }
        //}
    }
}