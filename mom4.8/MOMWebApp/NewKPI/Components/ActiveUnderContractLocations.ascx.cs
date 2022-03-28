using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Demo_Components_ActiveUnderContractLocations : System.Web.UI.UserControl
{
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ActiveUnderContractLocationsChart.DataSource = GetLocationsStatus();
            ActiveUnderContractLocationsChart.DataBind();
        }
    }

    public BarChartDTO GetLocationsStatus()
    {
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
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
    
        return result;
    }
}