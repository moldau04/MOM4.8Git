using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for KPIWebServiceGetSixtyPlusAR
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 

[System.Web.Script.Services.ScriptService]
public class KPIWebService : System.Web.Services.WebService
{
    #region Variables

    static Contracts objContract = new Contracts();
    static BL_Contracts objBLContracts = new BL_Contracts();
    public SQLNotifier Notifier { get; set; }
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    static DateTime? GetFullDate(object dt)
    {
        try
        {
            var dtString = dt.ToString();
            var year = int.Parse(dtString.Substring(0, 4));
            var month = int.Parse(dtString.Substring(4, 2));
            return new DateTime(year, month, 1);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    #endregion

    public KPIWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod(EnableSession = true)]
    public BarChartDTO ConvertedEstimatesBySalespersonAverageDays()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var result = new BarChartDTO();

        List<ActualvsBudgetData> actualvsBudgetDataList = new List<ActualvsBudgetData>();

        var dataTables = objBL_User.ConvertedEstimatesBySalespersonAverageDays(objPropUser);

        var currentDuration = GetLeadPersonAvg(dataTables[0]);
        //  var previousDuration = GetLeadPersonAvg(dataTables[1]);

        var ArrLocationStatus = new LocationStatus[1];
        ArrLocationStatus[0] = new LocationStatus();
        //  ArrLocationStatus[1] = new LocationStatus();


        ArrLocationStatus[0].StatusName = "Current Period";
        //ArrLocationStatus[1].StatusName = "";


        ArrLocationStatus[0].Count = new double[currentDuration.Count()];
        //ArrLocationStatus[1].Count = new double[currentDuration.Count()];

        result.Categories = new string[Math.Max(1, currentDuration.Count())];
        result.Categories[0] = "";


        for (int i = 0; i < currentDuration.Count(); i++)
        {
            result.Categories[i] = currentDuration[i].SalesPerson == "" || currentDuration[i].SalesPerson == null ? "Unassigned" : currentDuration[i].SalesPerson;

            ArrLocationStatus[0].Count[i] = currentDuration[i].Avg;
            // ArrLocationStatus[1].Count[i] = previousDuration[i].Avg;
        }
        result.LocationStatus = ArrLocationStatus;
        result.Max = ArrLocationStatus[0].Count.Length > 0 ? ArrLocationStatus[0].Count.Max() : 0;
        if (result.Max == 0)
        {
            result.Max = 1;
        }
        return result;


    }


    [WebMethod(EnableSession = true)]
    public bool getPermissions(string permissionType)
    {
        bool permission = false;
        if (permissionType == "FinanceStatement")
        {
            if ((bool)Session["FinanceStatement"].Equals(true) && Session["FinanceStatement"] != null)
            {
                permission = true;
            }
            else
            {
                permission = false;
            }
        }
        else if (permissionType == "Sales")
        {
            if (Session["type"].ToString() != "am")
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["userinfo"];
                string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);
                if (Sales == "Y")
                {
                    permission = true;
                }
                else
                {
                    permission = false;
                }
            }
        }
        return permission;
    }

    [WebMethod(EnableSession = true)]
    public ArrayList AvgEstimate()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var avg = objBL_User.GetAvgEstimate(objPropUser);
        double avgFirstYear = double.Parse(avg[0].ToString());
        double avgSecondYear = double.Parse(avg[1].ToString());

        string incBy;
        if (avgFirstYear > avgSecondYear)
        {
            var diff = avgFirstYear - avgSecondYear;
            var up = diff == 0 ? 0 : diff * 100 / avgFirstYear;
            incBy = "Up By " + up + "%";
        }
        else {
            var diff = avgSecondYear - avgFirstYear;
            var down = diff == 0 ? 0 : diff * 100 / avgSecondYear;
            incBy = "Down By " + down + "%";
        }
        avg.Add(incBy);
        return avg;
    }

    [WebMethod(EnableSession = true)]
    public List<string> GetBudgetNames()
    {
        var result = new List<string>();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var ds = objBL_User.GetListBudgetName(objPropUser);

        var dt = ds.Tables[0];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            result.Add(dt.Rows[i]["Budget"].ToString());
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public BarChartDTO GetActualvsBudgetedRevenue(string selectedBudget)
    {
        var result = new BarChartDTO();

        List<ActualvsBudgetData> actualvsBudgetDataList = new List<ActualvsBudgetData>();

        var dSet = objBL_User.Get12MonthActualvsBudgetData(objPropUser, selectedBudget);

        var dtTable = dSet.Tables[0];

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            if (dtTable.Rows[row]["Period"] != null)
            {
                var actualvsBudgetData = new ActualvsBudgetData() { NTotal = double.Parse(dtTable.Rows[row]["NTotal"].ToString()), NBudget = double.Parse(dtTable.Rows[row]["NBudget"].ToString()), NMonth = dtTable.Rows[row]["NMonth"].ToString().Substring(0, 3), TypeValue = int.Parse(dtTable.Rows[row]["Type"].ToString()), Period = GetFullDate(dtTable.Rows[row]["Period"]) };
                actualvsBudgetDataList.Add(actualvsBudgetData);
            }
        }

        var actualvsBudgetDataListGroup = actualvsBudgetDataList.Where(x => x.TypeValue == 3).OrderBy(x => x.Period).GroupBy(x => x.NMonth).Select(g => new ActualvsBudgetData()
        {
            NMonth = g.Key,
            NBudget = g.Sum(x => x.NBudget),
            NTotal = g.Sum(x => x.NTotal)
        }).ToList<ActualvsBudgetData>();

        var ArrLocationStatus = new LocationStatus[2];
        ArrLocationStatus[0] = new LocationStatus();
        ArrLocationStatus[1] = new LocationStatus();


        ArrLocationStatus[0].StatusName = "Actual";
        ArrLocationStatus[1].StatusName = selectedBudget;

        ArrLocationStatus[0].Count = new double[actualvsBudgetDataListGroup.Count];
        ArrLocationStatus[1].Count = new double[actualvsBudgetDataListGroup.Count];
        int i = 0;
        result.Categories = new string[actualvsBudgetDataListGroup.Count];
        foreach (var obj in actualvsBudgetDataListGroup)
        {
            var currentMonth = obj.NMonth;
            result.Categories[i] = currentMonth;
            ArrLocationStatus[0].Count[i] = obj.NTotal;
            ArrLocationStatus[1].Count[i] = obj.NBudget;
            i++;
        }
        result.LocationStatus = ArrLocationStatus;
        result.Max = ArrLocationStatus[0].Count.Length > 0 ? ArrLocationStatus[0].Count.Max() : 1;
        return result;
    }

    [WebMethod(EnableSession = true)]
    public EquipmentType[] GetEquipmentTypeCount()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var dSet = objBL_User.GetEquipmentTypeCount(objPropUser);
        DataTable dt = dSet.Tables[0];

        var result = new EquipmentType[dt.Rows.Count];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            result[i] = new EquipmentType { value = double.Parse(dt.Rows[i]["Total"].ToString()), category = dt.Rows[i]["TypeName"].ToString() };
        }
        var max = result.Max(x => x.value);
        foreach (var item in result)
        {
            if (item.value == max)
            {
                item.color = "#ADD8E6";
            }
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public EquipmentType[] GetEquipmentBuildingCount()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var dSet = objBL_User.GetEquipmentBuildingCount(objPropUser);
        DataTable dt = dSet.Tables[0];

        var result = new EquipmentType[dt.Rows.Count];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            result[i] = new EquipmentType { value = double.Parse(dt.Rows[i]["Total"].ToString()), category = dt.Rows[i]["Building"].ToString() };
        }
        var max = 0;
        if (result.Count() > 0)
        {
            result.Max(x => x.value);
        }
        foreach (var item in result)
        {
            if (item.value == max)
            {
                item.color = "#ADD8E6";
            }
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public BarChartDTO GetLocationsStatus()
    {
        //objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var result = new BarChartDTO();
        result.Categories = new string[6];

        List<LocationsStatus> LocationsStatusList = new List<LocationsStatus>();

        var dSet = objBL_User.GetLocationsStatus(objPropUser);
        var dtTable = dSet.Tables[0];

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            var locationsStatus = new LocationsStatus() { location = int.Parse(dtTable.Rows[row]["loc"].ToString()), status = int.Parse(dtTable.Rows[row]["status"].ToString()), BStart = DateTime.Parse(dtTable.Rows[row]["bStart"].ToString()) };
            LocationsStatusList.Add(locationsStatus);
        }

        var ArrLocationStatus = new LocationStatus[2];
        ArrLocationStatus[0] = new LocationStatus();
        ArrLocationStatus[1] = new LocationStatus();

        ArrLocationStatus[0].StatusName = "Total";
        ArrLocationStatus[1].StatusName = "Active";

        ArrLocationStatus[0].Count = new double[6];
        ArrLocationStatus[1].Count = new double[6];

        for (var i = 5; i >= 0; i--)
        {
            var currentMonth = DateTime.Now.AddMonths(-1 * (i)).Month;
            result.Categories[i] = DateTime.Now.AddMonths(-1 * (i)).ToString("MMM", CultureInfo.InvariantCulture);
            ArrLocationStatus[0].Count[i] = LocationsStatusList.Where(x => x.status == 0 && x.BStart.Month == currentMonth).Count();
            ArrLocationStatus[1].Count[i] = LocationsStatusList.Where(x => x.status != 0 && x.BStart.Month == currentMonth).Count();
        }
        result.LocationStatus = ArrLocationStatus;
        result.Max = ArrLocationStatus[0].Count.Length > 0 ? ArrLocationStatus[0].Count.Max() : 1;
        if (dtTable.Rows.Count == 0)
        {
            result.Max = 1;
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public int[] GetTicketStatus()
    {
        var result = objBL_User.GetTicketStatus(objPropUser);

        return result;
    }

    [WebMethod(EnableSession = true)]
    public double[] GetSixtyPlusAR()
    {
        double[] result = new double[2];
        objContract.ConnConfig = HttpContext.Current.Session["config"].ToString();

        objContract.Date = Convert.ToDateTime(DateTime.Now.AddMonths(-1));

        var ds = objBLContracts.GetARAging(objContract);

        var dt = ds.Tables[0];
        double PreviousMonthtotal = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            PreviousMonthtotal += double.Parse(dt.Rows[i]["NintyDay"].ToString());
        }

        objContract.Date = Convert.ToDateTime(DateTime.Now);

        ds = objBLContracts.GetARAging(objContract);

        dt = ds.Tables[0];
        double currentMonthtotal = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            currentMonthtotal += double.Parse(dt.Rows[i]["NintyDay"].ToString());
        }
        result[0] = PreviousMonthtotal;
        result[1] = currentMonthtotal;
        return result;
    }

    [WebMethod(EnableSession = true)]
    public double[] GetNinetyPlusAR()
    {
        double[] result = new double[2];
        objContract.ConnConfig = HttpContext.Current.Session["config"].ToString();

        objContract.Date = Convert.ToDateTime(DateTime.Now.AddMonths(-1));

        var ds = objBLContracts.GetARAging(objContract);

        var dt = ds.Tables[0];
        double PreviousMonthtotal = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            PreviousMonthtotal += double.Parse(dt.Rows[i]["NintyOneDay"].ToString());
        }

        objContract.Date = Convert.ToDateTime(DateTime.Now);

        ds = objBLContracts.GetARAging(objContract);

        dt = ds.Tables[0];
        double currentMonthtotal = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            currentMonthtotal += double.Parse(dt.Rows[i]["NintyOneDay"].ToString());
        }
        result[0] = PreviousMonthtotal;
        result[1] = currentMonthtotal;
        return result;
    }

    [WebMethod(EnableSession = true)]
    public BarChartDTO GetEstimatedAndTotalHoursCompleted()
    {
        var result = new BarChartDTO();


        List<LocationsStatus> LocationsStatusList = new List<LocationsStatus>();

        var dataTables = objBL_User.GetEstimatedAndTotalHoursCompleted(objPropUser);

        var totalHours = getRouteHour(dataTables[0]);
        var completedHours = getRouteHour(dataTables[1]);
        var openHours = getRouteHour(dataTables[2]);

        var AllRoutes = new HashSet<string>();

        foreach (var obj in totalHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
            else
                AllRoutes.Add("");
        }

        foreach (var obj in completedHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
            else
                AllRoutes.Add("");
        }

        foreach (var obj in openHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
            else
                AllRoutes.Add("");
        }


        var ArrLocationStatus = new LocationStatus[3];
        ArrLocationStatus[0] = new LocationStatus();
        ArrLocationStatus[1] = new LocationStatus();
        ArrLocationStatus[2] = new LocationStatus();

        ArrLocationStatus[0].StatusName = "Total Hours";
        ArrLocationStatus[1].StatusName = "Completed";
        ArrLocationStatus[2].StatusName = "Open";

        ArrLocationStatus[0].Count = new double[AllRoutes.Count];
        ArrLocationStatus[1].Count = new double[AllRoutes.Count];
        ArrLocationStatus[2].Count = new double[AllRoutes.Count];

        result.Categories = new string[Math.Max(1, AllRoutes.Count)];
        result.Categories[0] = "";

        int i = 0;
        foreach (var obj in AllRoutes)
        {
            result.Categories[i] = obj == "" ? "Unassigned" : obj;

            var routeHour = totalHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                ArrLocationStatus[0].Count[i] = routeHour.FirstOrDefault().Total;
            }
            else
            {
                ArrLocationStatus[0].Count[i] = 0;
            }
            routeHour = completedHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                ArrLocationStatus[1].Count[i] = routeHour.FirstOrDefault().Total;
            }
            else
            {
                ArrLocationStatus[1].Count[i] = 0;
            }

            routeHour = openHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                ArrLocationStatus[2].Count[i] = routeHour.FirstOrDefault().Total;
            }
            else
            {
                ArrLocationStatus[2].Count[i] = 0;
            }

            i++;
        }
        result.LocationStatus = ArrLocationStatus;
        result.Max = Math.Max(ArrLocationStatus[0].Count.Length > 0 ? ArrLocationStatus[0].Count.Max() : 0, Math.Max(ArrLocationStatus[0].Count.Length > 0 ? ArrLocationStatus[1].Count.Max() : 0, ArrLocationStatus[2].Count.Length > 0 ? ArrLocationStatus[0].Count.Max() : 0));
        if (result.Max == 0)
        {
            result.Max = 1;
        }
        return result;
    }

    private List<RouteHour> getRouteHour(DataTable table)
    {
        var result = new List<RouteHour>();

        if (table != null)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                result.Add(new RouteHour() { Total = double.Parse(table.Rows[i]["Total"].ToString()), Route = table.Rows[i]["Route"].ToString() });
            }
        }
        return result;
    }

    private List<LeadAverage> GetLeadPersonAvg(DataTable table)
    {
        var result = new List<LeadAverage>();

        if (table != null)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                result.Add(new LeadAverage() { Avg = double.Parse(table.Rows[i]["Avg"].ToString()), SalesPerson = table.Rows[i]["SalesPerson"].ToString() });

            }
        }
        return result;
    }
}





#region

public class LeadAverage
{
    public string SalesPerson { get; set; }
    public double Avg { get; set; }
}
public class RouteHour
    {
        public double Total { get; set; }
        public string Route { get; set; }
    }
    public class EquipmentType
    {
        public string category { get; set; }
        public double value { get; set; }
        public string color { get; set; }
    }
    public class BarChartDTO
    {
        public LocationStatus[] LocationStatus { get; set; }
        public double Max { get; set; }
        public string[] Categories { get; set; }
    }
    public class LocationStatus
    {
        public string StatusName { get; set; }
        public double[] Count { get; set; }
    }
    public class LocationsStatus
    {
        public int location { get; set; }
        public int status { get; set; }
        public DateTime BStart { get; set; }
    }
    public class ActualvsBudgetData
    {
        public double NTotal { get; set; }
        public double NBudget { get; set; }
        public string NMonth { get; set; }
        public int TypeValue { get; set; }
        public DateTime? Period { get; set; }

    }
    public class BudgetList
    {
        public int BudgetID { get; set; }
        public string Budget { get; set; }
    }

    public class RecurringHoursRemaining
    {
        public string Category { get; set; }
        public double TotalHours { get; set; }
        public double Completed { get; set; }
        public double Open { get; set; }
    }
    #endregion



