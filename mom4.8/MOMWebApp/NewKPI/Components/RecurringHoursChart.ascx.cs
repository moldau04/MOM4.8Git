using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_RecurringHoursChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        RecurringHoursChart.DataSource = ConvertedEstimatesBySalespersonAverageDays().OrderBy(x => x.SalesPerson);
        RecurringHoursChart.DataBind();
    }

    private List<LeadAverage> ConvertedEstimatesBySalespersonAverageDays()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var dataTables = objBL_User.ConvertedEstimatesBySalespersonAverageDays(objPropUser);
        var currentDuration = GetLeadPersonAvg(dataTables[0]);

        return currentDuration;
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