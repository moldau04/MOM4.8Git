using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;

namespace MOMWebApp
{
    /// <summary>
    /// Summary description for WS_Scheduler
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WS_Scheduler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        //Web Service code
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]

        public string AppointmentMove(string TicketId, string TimeSlotIndex, string StartTime, string workername)
        {
            BL_MapData objBL_MapData = new BL_MapData();
            string str = string.Empty;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            MapData objpropMapData = new MapData();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            try
            {

                string MoveTicketId = TicketId;
                string MoveNewTimeSlotIndex = TimeSlotIndex;
                var NewTimeSlot = StartTime;
                var MoveNewTimeSlot = (DateTime.TryParseExact(NewTimeSlot, "M/d/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dt)) ? dt : null as DateTime?;

                if (MoveTicketId != "" && MoveNewTimeSlotIndex != "")
                {
                    #region Simulation of database update                    
                    objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
                    objpropMapData.Tech = workername;
                    objpropMapData.TicketID = Convert.ToInt32(MoveTicketId);
                    objpropMapData.Date = Convert.ToDateTime(MoveNewTimeSlot);
                    objpropMapData.Assigned = 1;
                    objBL_MapData.UpdateTicket(objpropMapData);

                    dictionary.Add("Success", "1");
                    str = sr.Serialize(dictionary);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                dictionary.Add("Success", "0");
                dictionary.Add("ErrMsg", ex.Message);
                str = sr.Serialize(dictionary);
            }
            return str;
        }

        //Web Service code
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AppointmentResize(string TicketId, string ResizeNewStartDate, string ResizeNewEndDate)
        {
            BL_MapData objBL_MapData = new BL_MapData();
            string str = string.Empty;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            MapData objpropMapData = new MapData();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            try
            {
                DateTime Start = ConvertInTOLocaldate(ResizeNewStartDate);
                DateTime End = ConvertInTOLocaldate(ResizeNewEndDate);
                if (End > Start)
                {
                    TimeSpan duration = End - Start;


                    #region Simulation of database update

                    objpropMapData.ConnConfig = Session["config"].ToString();
                    objpropMapData.TicketID = Convert.ToInt32(TicketId);
                    objpropMapData.Date = Convert.ToDateTime(Start);
                    objpropMapData.Resize = duration.TotalHours;
                    objBL_MapData.UpdateTicketResize(objpropMapData);

                    #endregion

                }

            }
            catch (Exception ex)
            {
                dictionary.Add("Success", "0");
                dictionary.Add("ErrMsg", ex.Message);
                str = sr.Serialize(dictionary);
            }
            return str;
        }

        private DateTime ConvertInTOLocaldate(string strDate)
        {
            DateTime _DateTime;

            int day = Convert.ToInt32((strDate.Split(' '))[2]), year = Convert.ToInt32((strDate.Split(' '))[3]);

            string time = strDate.Split(' ')[4];
            int hour = Convert.ToInt32(time.Split(':')[0]), min = Convert.ToInt32(time.Split(':')[1]), sec = Convert.ToInt32(time.Split(':')[0]);

            switch (strDate.Split(' ')[1])
            {
                case "Jan":
                    {
                        _DateTime = new DateTime(year, 1, day, hour, min, sec);
                    }
                    break;

                case "Feb":
                    {
                        _DateTime = new DateTime(year, 2, day, hour, min, sec);
                    }
                    break;


                case "Mar":
                    {
                        _DateTime = new DateTime(year, 3, day, hour, min, sec);
                    }
                    break;


                case "Apr":
                    {
                        _DateTime = new DateTime(year, 4, day, hour, min, sec);
                    }
                    break;

                case "May":
                    {
                        _DateTime = new DateTime(year, 5, day, hour, min, sec);
                    }
                    break;

                case "Jun":
                    {
                        _DateTime = new DateTime(year, 6, day, hour, min, sec);
                    }
                    break;

                case "Jul":
                    {
                        _DateTime = new DateTime(year, 7, day, hour, min, sec);
                    }
                    break;

                case "Sep":
                    {
                        _DateTime = new DateTime(year, 9, day, hour, min, sec);
                    }
                    break;

                case "Aug":
                    {
                        _DateTime = new DateTime(year, 8, day, hour, min, sec);
                    }
                    break;

                case "Oct":
                    {
                        _DateTime = new DateTime(year, 10, day, hour, min, sec);
                    }
                    break;

                case "Nov":
                    {
                        _DateTime = new DateTime(year, 11, day, hour, min, sec);
                    }
                    break;

                case "Dec":
                    {

                        _DateTime = new DateTime(year, 12, day, hour, min, sec);

                    }
                    break;
                default:
                    {
                        _DateTime = new DateTime();
                    }
                    break;


            }
            return _DateTime;

        }
    }
}
