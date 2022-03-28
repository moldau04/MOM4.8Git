using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using BusinessEntity;
using BusinessLayer;

//public class Distance
//{
//    public string text { get; set; }
//    public int value { get; set; }
//}

//public class Duration
//{
//    public string text { get; set; }
//    public int value { get; set; }
//}

//public class Element
//{
//    public Distance distance { get; set; }
//    public Duration duration { get; set; }
//    public string status { get; set; }
//}

//public class Row
//{
//    public List<Element> elements { get; set; }
//}

//public class RootObject
//{
//    public List<Row> rows { get; set; }
//    public string[] originAddresses { get; set; }
//    public string[] destinationAddresses { get; set; }
//}

public partial class Maptest : System.Web.UI.Page
{

    Customer objCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();


    protected void Page_Load(object sender, EventArgs e)
    {
        //OptimizeDirections();
        FillRoute();
    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);

        lstWorker.DataSource = ds.Tables[0];
        lstWorker.DataTextField = "Name";
        lstWorker.DataValueField = "ID";
        lstWorker.DataBind();        
    }

    private void GetLoc()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getLocCoordinates(objCustomer);

        double lat1=Convert.ToDouble( hdnCenter.Value.Split(',')[0]);
        double long1=Convert.ToDouble( hdnCenter.Value.Split(',')[1]);
        double radius = Convert.ToDouble(hdnRadius.Value);

        List<int> lstLoc = new List<int>();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["lat"] != DBNull.Value || dr["lat"].ToString() != string.Empty)
            {
                double lat2 = Convert.ToDouble(dr["lat"]);
                double long2 = Convert.ToDouble(dr["lng"]);
                double dist= GetDistanceBetweenPoints(lat1, long1, lat2, long2);
                if (dist <= radius)
                {
                    lstLoc.Add(Convert.ToInt32( dr["loc"]));
                }
            }
        }
    }


    public double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
    {
        double distance = 0;

        double dLat = (lat2 - lat1) / 180 * Math.PI;
        double dLong = (long2 - long1) / 180 * Math.PI;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                    + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        //Calculate radius of earth
        // For this you can assume any of the two points.
        double radiusE = 6378135; // Equatorial radius, in metres
        double radiusP = 6356750; // Polar Radius

        //Numerator part of function
        double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
        //Denominator part of the function
        double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                        + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
        double radius = Math.Sqrt(nr / dr);

        //Calaculate distance in metres.
        distance = radius * c;
        return distance;
    }

    private void OptimizeDirections()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getLocCoordinates(objCustomer);

        ////string concatString = ConcatString(ds.Tables[0]);
        ////string input = GetDistanceMatrix(concatString, concatString);
        ////var deserializer = new JavaScriptSerializer();
        ////RootObject distanceMatrix = deserializer.Deserialize<RootObject>(input);
        //RootObject distanceMatrix = getDistanceMatrixFromCode(ds.Tables[0]);


        //int nodeCount = distanceMatrix.rows.Count;
        ////int nodeCount = distanceMatrix.rows.Length;
        //List<short> arrayOfNodes = new List<short>();
        ////for (short i = 0; i < nodeCount; i++)
        //for (short i = 1; i < nodeCount; i++)
        //{
        //    arrayOfNodes.Add(i);
        //}
        //List<List<short>> combinations = GenerateAllCombinations(arrayOfNodes);

        //double shortestPath = double.MaxValue;
        ////int shortestPath = Int32.MaxValue;
        //List<short> shortestPathCombination = null;
        //foreach (List<short> combination in combinations)
        //{
        //    combination.Insert(0, 0);
        //    //combination.Insert(nodeCount, 0);

        //    double pathLength = GetPathLength(combination, distanceMatrix);
        //    if (pathLength < shortestPath)
        //    {
        //        shortestPathCombination = combination;
        //        shortestPath = pathLength;
        //    }
        //}

        //DataTable dtFinal = ds.Tables[0].Clone();
        //foreach (var li in shortestPathCombination)
        //{
        //    dtFinal.ImportRow(ds.Tables[0].Rows[li]);            
        //}

        DataTable dtFinal = ds.Tables[0].Clone();
        for (int i = 0; i < 5; i++)
        {
            dtFinal.ImportRow(ds.Tables[0].Rows[i]);   
        }

        GridView1.DataSource = dtFinal;
        GridView1.DataBind();
        rptMarkers.DataSource = dtFinal;
        rptMarkers.DataBind();        
    }

    private string ConcatString(DataTable dt)
    {
        var concatString = "";
        int inc = 0;
        foreach (DataRow dr in dt.Rows)
        {
            concatString += dr["coordinates"].ToString();

            if (inc != dt.Rows.Count - 1)
            {
                concatString += "|";
            }

            inc++;
        }
        return concatString;
    }

    public string GetDistanceMatrix(string origins, string destinations)
    {
        WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/distancematrix/json?origins=" + origins + "&destinations=" + destinations + "&sensor=false");
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
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //return serializer.Deserialize<GeoJsonData>(result);
        return result;
    }

    private List<List<T>> GenerateAllCombinations<T>(List<T> array)
    {
        List<List<T>> allLists = new List<List<T>>();
        List<T> initialList = new List<T>();
        initialList.Add(array[0]);
        allLists.Add(initialList);

        //i is the next element to be inserted.  
        for (short i = 1; i < array.Count; i++)
        {
            List<List<T>> newAllLists = new List<List<T>>();

            foreach (List<T> existingList in allLists)
            {
                List<List<T>> tempAllLists = new List<List<T>>();
                for (int j = 0; j <= existingList.Count; j++)
                {
                    List<T> newList = new List<T>();
                    newList.AddRange(existingList);
                    newList.Insert(j, array[i]);
                    tempAllLists.Add(newList);
                }
                newAllLists.AddRange(tempAllLists);
                allLists = newAllLists;
            }

            GC.Collect();
        }
        return allLists;
    }

    private double GetPathLength(List<short> combination, RootObject distanceMatrix)
    {
        double length = 0;

        for (int i = 0; i < combination.Count; i++)
        //for (int i = 0; i < combination.Count - 1; i++)
        {
            int source = combination[i];
            int destination = -1;
            int inc = i + 1;
            //int destination = i + 1;
            //if (destination == combination.Count)
            ////if (destination == combination.Count - 1)
            //{
            //    destination = combination[0];
            //}
            //else
            if (inc < combination.Count )
            {
                destination = combination[inc];
            }
            
            if (destination != -1)
            {
                length = length + distanceMatrix.rows[source].elements[destination].distance.value;
            }
        }

        return length;
    }

    /// <summary>
    /// Generate Distance Matrix for coordinates by mathematical claculations.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    #region Generate Distance Matrix programatically
    private RootObject getDistanceMatrixFromCode(DataTable dt)
    {
        RootObject ro = new RootObject();
        List<Row> rowsList = new List<Row>();
        foreach (DataRow dr in dt.Rows)
        {
            Row r = new Row();
            List<Element> eleList = new List<Element>();
            foreach (DataRow drin in dt.Rows)
            {
                Element el = new Element();
                Distance dist = new Distance();
                double dis = distance(Convert.ToDouble(dr["lat"].ToString()), Convert.ToDouble(dr["lng"].ToString()), Convert.ToDouble(drin["lat"].ToString()), Convert.ToDouble(drin["lng"].ToString()), Convert.ToChar("M"));
                dist.value = Convert.ToInt32(Math.Round(dis));
                //dist.value = dis;
                el.distance = dist;
                eleList.Add(el);
            }
            r.elements = eleList;
            rowsList.Add(r);
        }
        ro.rows = rowsList;

        return ro;       
    }

    private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
    {
        //double theta = lon1 - lon2;
        //double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
        //dist = Math.Acos(dist);
        //dist = rad2deg(dist);
        //dist = dist * 60 * 1.1515;
        //if (unit == 'K')
        //{
        //    dist = dist * 1.609344;
        //}
        //else if (unit == 'N')
        //{
        //    dist = dist * 0.8684;
        //}
        //return (dist);

        int R = 6371;

        double rLat1 = deg2rad(lat1);
        double rLat2 = deg2rad(lat2);

        double dLat = rLat2 - rLat1;
        double dLon = deg2rad(lon2 - lon1);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
            Math.Pow(Math.Sin(dLon / 2), 2) *
            Math.Cos(rLat1) * Math.Cos(rLat2);

        double b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * b;
    }

    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180);
    }

    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
    #endregion    

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        GetLoc();
        ModalPopupExtender1.Hide();
    }
}
