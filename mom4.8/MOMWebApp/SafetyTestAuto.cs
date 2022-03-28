
using System.Collections.Generic;
using System.Web.Services;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using System.Web.Script.Serialization;
using System.Linq;
using BusinessEntity.Recurring;
using System;
using System.Web;
using System.Web.Script.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SafetyTestAuto : System.Web.Services.WebService
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    GeneralFunctions objGeneral = new GeneralFunctions();

    public SafetyTestAuto()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string ServiceSearchTicketInLocation(string prefixText, int loc, int ticketYear)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = loc;
        ds = objBL_MapData.SearchTicketInLocation(Session["config"].ToString(), loc, prefixText.Trim(), ticketYear);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            dictListEval = objGeneral.RowsToDictionary(dt);
            str = sr.Serialize(dictListEval);
            return str;
        }
        else
        {
            return null;

        }


    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public string ServiceSearchTicketByID(string prefixText, int loc, int ticketYear)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = loc;
        ds = objBL_MapData.SearchTicketInLocation(Session["config"].ToString(), loc, prefixText.Trim(), ticketYear);


        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Select("ID="+ prefixText.Trim()).Count() > 0)
            {
                DataTable dt = ds.Tables[0].Select("ID="+ prefixText.Trim()).CopyToDataTable();
                dictListEval = objGeneral.RowsToDictionary(dt);
                str = sr.Serialize(dictListEval);
                return str;
            }
            else
            {
                return null;

            }
          
        }
        else
        {
            return null;

        }


    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string AssignTicketToTest(String LID, String TicketID, String TestYear,String OldTicket)
    {
        try
        {
            BL_SafetyTest objtestbl = new BL_SafetyTest();
            SafetyTest obj = new SafetyTest();
            obj.LID = Convert.ToInt32(LID);
            obj.Ticket = Convert.ToInt32(TicketID);
            obj.PriceYear = Convert.ToInt32(TestYear);
            obj.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
            obj.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());

           return objtestbl.AssignTicketToTest(obj, Convert.ToInt32(OldTicket));
           
        }
        catch(Exception ex)
        {
            return ex.ToString();
        }
       
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string UpdateTestSchedule(String LID, String ScheduleDate, String ScheduleYear,String ScheduleStatus,String ScheduleWorker)
    {
        try
        {
            BL_SafetyTest objtestbl = new BL_SafetyTest();
            SafetyTest obj = new SafetyTest();
            obj.LID = Convert.ToInt32(LID);
            obj.ScheduleDate = ScheduleDate;
            obj.PriceYear =Convert.ToInt32( ScheduleYear);
            obj.ScheduleStatusID = Convert.ToInt32(ScheduleStatus);
            obj.ScheduleWorker = ScheduleWorker;
            obj.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
            obj.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());

            return objtestbl.UpdateTestScheduled(obj);

        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string UpdateTestService(String LID, String ServiceDate, String ServiceYear, String ServiceStatus, String ServiceWorker)
    {
        try
        {
            BL_SafetyTest objtestbl = new BL_SafetyTest();
            SafetyTest obj = new SafetyTest();
            obj.LID = Convert.ToInt32(LID);
            obj.ServiceDate = ServiceDate;
            obj.PriceYear = Convert.ToInt32(ServiceYear);
            obj.ServiceStatusID = Convert.ToInt32(ServiceStatus);
            obj.ServiceWorker = ServiceWorker;
            obj.UserName = Convert.ToString(HttpContext.Current.Session["Username"].ToString());
            obj.ConnConfig = Convert.ToString(HttpContext.Current.Session["config"].ToString());

            return objtestbl.UpdateTestService(obj);

        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

    }
}

