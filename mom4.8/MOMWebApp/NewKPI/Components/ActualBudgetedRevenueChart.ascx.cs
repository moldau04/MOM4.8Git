using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_ActualBudgetedRevenueChart : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BL_Report objBL_Report = new BL_Report();
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SelectBudgetDd.Items.Count == 0)
        {
            SelectBudgetDd.DataSource = GetBudgetNames();
            SelectBudgetDd.DataTextField = "Value";
            SelectBudgetDd.DataValueField = "Id";
            SelectBudgetDd.DataBind();
        }

        int? budgetId = string.IsNullOrEmpty(SelectBudgetDd.SelectedValue) ? null : (int?)Convert.ToInt32(SelectBudgetDd.SelectedValue);
        var data = GetActualvsBudgetedRevenue(budgetId);
        ActualBudgetedRevenueChart.DataSource = data;
        ActualBudgetedRevenueChart.DataBind();
    }

    protected void SelectBudgetDd_SelectedIndexChanged(object sender, EventArgs e)
    {
        int? budgetId = string.IsNullOrEmpty(SelectBudgetDd.SelectedValue) ? null : (int?)Convert.ToInt32(SelectBudgetDd.SelectedValue);
        var data = GetActualvsBudgetedRevenue(budgetId);
        ActualBudgetedRevenueChart.DataSource = data;
        ActualBudgetedRevenueChart.DataBind();
    }


    public DataTable GetBudgetNames()
    {
        var dtResult = new DataTable("BudgetSelect");
        dtResult.Columns.Add("Id", Type.GetType("System.String"));
        dtResult.Columns.Add("Value", Type.GetType("System.String"));
        dtResult.Rows.Add("", "Select");

        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        var ds = objBL_User.GetListBudgetName(objPropUser);

        var dt = ds.Tables[0];

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            dtResult.Rows.Add(dt.Rows[i]["BudgetID"].ToString(), dt.Rows[i]["Budget"].ToString());
        }

        return dtResult;
    }

    private List<ActualvsBudgetData> GetActualvsBudgetedRevenue(int? budgetID)
    {
        var result = new List<ActualvsBudgetData>();
        List<ActualvsBudgetData> actualvsBudgetDataList = new List<ActualvsBudgetData>();

        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        var currentDate = DateTime.Now;
        var fiscalYearMonth = objBL_Report.GetFiscalYearData(objPropUser);
        var endMonth = fiscalYearMonth + 1;
        var startMonth = endMonth == 12 ? 1 : endMonth + 1;

        var fiscalYear = currentDate.Year;
        if (DateTime.Now.Month > endMonth)
        {
            fiscalYear = currentDate.Year + 1;
            objPropUser.FStart = new DateTime(currentDate.Year, startMonth, 1);
            objPropUser.FEnd = new DateTime(currentDate.Year + 1, endMonth, DateTime.DaysInMonth(currentDate.Year + 1, endMonth));
        }
        else
        {
            if (startMonth == 1)
            {
                objPropUser.FStart = new DateTime(currentDate.Year, startMonth, 1);
            }
            else
            {
                fiscalYear = currentDate.Year - 1;
                objPropUser.FStart = new DateTime(currentDate.Year - 1, startMonth, 1);
            }
            objPropUser.FEnd = new DateTime(currentDate.Year, endMonth, DateTime.DaysInMonth(currentDate.Year, endMonth));
        }

        var dSet = objBL_User.Get12MonthActualvsBudgetGraphData(objPropUser, budgetID, fiscalYear);

        var dtTable = dSet.Tables[0];

        for (int row = 0; row < dtTable.Rows.Count; row++)
        {
            var actualvsBudgetData = new ActualvsBudgetData()
            {
                NTotal = double.Parse(dtTable.Rows[row]["NTotal"].ToString()),
                NBudget = double.Parse(dtTable.Rows[row]["NBudget"].ToString()),
                NMonth = dtTable.Rows[row]["NMonth"].ToString()
            };

            actualvsBudgetDataList.Add(actualvsBudgetData);
        }

        var listMonth = GetMonthLegend(objPropUser.FStart, objPropUser.FEnd);

        foreach (var month in listMonth)
        {
            ActualvsBudgetData data = new ActualvsBudgetData();
            var temp = actualvsBudgetDataList.FirstOrDefault(x => x.NMonth == month);
            data.NMonth = month;
            data.NBudget = temp != null ? temp.NBudget : 0;
            data.NTotal = temp != null ? temp.NTotal : 0;

            result.Add(data);
        }

        return result;
    }

    private IEnumerable<string> GetMonthLegend(DateTime startDate, DateTime endDate)
    {
        List<string> listMonth = new List<string>();

        var start = new DateTime(startDate.Year, startDate.Month, 1);
        while (start < endDate)
        {
            listMonth.Add(start.Date.ToString("MMM"));
            start = start.AddMonths(1);
        }

        return listMonth;
    }
}