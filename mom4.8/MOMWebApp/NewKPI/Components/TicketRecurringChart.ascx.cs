using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewKPI_Components_TicketRecurringChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        var data = GetTicketRecurringOpenAndCompleted();
        TicketRecurringChart.DataSource = data.OrderBy(x => x.Category);
        TicketRecurringChart.DataBind();
    }

    public List<RecurringHoursRemaining> GetTicketRecurringOpenAndCompleted()
    {
        List<RecurringHoursRemaining> result = new List<RecurringHoursRemaining>();

        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1" || Convert.ToString(Session["chkCompanyName"]) != "")
        {
            objPropUser.EN = 1;
        }
        else
        {
            objPropUser.EN = 0;
        }

        var dataTables = objBL_User.GetTicketRecurringOpenAndCompleted(objPropUser);

        var openHours = getRecurringHour(dataTables[0]);
        var completedHours = getRecurringHour(dataTables[1]);

        var allRoutes = new HashSet<string>();

        foreach (var obj in openHours)
        {
            if (!allRoutes.Contains(obj.Item1))
                allRoutes.Add(obj.Item1);
        }

        foreach (var obj in completedHours)
        {
            if (!allRoutes.Contains(obj.Item1))
                allRoutes.Add(obj.Item1);
        }

        foreach (var obj in allRoutes)
        {
            var item = new RecurringHoursRemaining();
            item.Category = obj == "" ? "Unassigned" : obj;

            var complete = completedHours.Where(x => x.Item1 == obj);
            if (complete != null && complete.Count() > 0)
            {
                item.Completed = complete.Sum(x => x.Item2);
            }
            else
            {
                item.Completed = 0;
            }

            var open = openHours.Where(x => x.Item1 == obj);
            if (open != null && open.Count() > 0)
            {
                item.Open = open.Sum(x => x.Item2);
            }
            else
            {
                item.Open = 0;
            }

            result.Add(item);
        }

        return result;
    }

    private List<Tuple<string, double>> getRecurringHour(DataTable table)
    {
        var result = new List<Tuple<string, double>>();

        if (table != null)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                result.Add(Tuple.Create(table.Rows[i]["DWork"].ToString(), double.Parse(table.Rows[i]["Total"].ToString())));
            }
        }
        return result;
    }
}