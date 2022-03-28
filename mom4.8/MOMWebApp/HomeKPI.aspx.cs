using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Newtonsoft.Json;
using System.Configuration;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;

public partial class HomeKPI : System.Web.UI.Page
{    
 
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }
        //////
            /////--->  Condition for Customers Portal 
        ////
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("portalhome.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Call Web API 
        List<LeadAverageResponse> _lstLeadAverageResponse = new List<LeadAverageResponse>();
        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
        if (IsAPIIntegrationEnable == "YES")
        {
            //objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            //List<ResponseTable1> lst = new List<ResponseTable1>();
            //RecurringChartResponse obj = new RecurringChartResponse();
            //string APINAME = "DashBoardAPI/DashBoard_GetRecurringHours";
            ////APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, objPropUser);
            //APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, null);
            //_lstLeadAverageResponse = (new JavaScriptSerializer()).Deserialize<List<LeadAverageResponse>>(_APIResponse.ResponseData);
            //int count = _lstLeadAverageResponse.Count();
            //ClientScript.RegisterStartupScript(this.GetType(), "updateRecurringHoursChart", "updateRecurringHoursChart('" + _APIResponse.ResponseData + "');", true);
        }
        // Call Web API End       
    }    
}