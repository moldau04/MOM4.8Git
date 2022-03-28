using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_RecurringHoursRemaining : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        var data = GetEstimatedAndTotalHoursCompleted();
        RecurringHoursRemainingChart.DataSource = data.OrderBy(x => x.Category);
        RecurringHoursRemainingChart.DataBind();
    }

    public List<RecurringHoursRemaining> GetEstimatedAndTotalHoursCompleted()
    {
        List<RecurringHoursRemaining> result = new List<RecurringHoursRemaining>();

        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var dataTables = objBL_User.GetEstimatedAndTotalHoursCompleted(objPropUser);

        var totalHours = getRouteHour(dataTables[0]);
        var completedHours = getRouteHour(dataTables[1]);
        var openHours = getRouteHour(dataTables[2]);

        var AllRoutes = new HashSet<string>();

        foreach (var obj in totalHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
        }

        foreach (var obj in completedHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
        }

        foreach (var obj in openHours)
        {
            if (!AllRoutes.Contains(obj.Route))
                AllRoutes.Add(obj.Route);
        }


        foreach (var obj in AllRoutes)
        {
            var item = new RecurringHoursRemaining();
            item.Category = obj == "" ? "Unassigned" : obj;

            var routeHour = totalHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                item.TotalHours = routeHour.Sum(x => x.Total);
            }
            else
            {
                item.TotalHours = 0;
            }

            routeHour = completedHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                item.Completed = routeHour.Sum(x => x.Total);
            }
            else
            {
                item.Completed = 0;
            }

            routeHour = openHours.Where(x => x.Route == obj);
            if (routeHour.FirstOrDefault() != null)
            {
                item.Open = routeHour.Sum(x => x.Total);
            }
            else
            {
                item.Open = 0;
            }

            result.Add(item);
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
}