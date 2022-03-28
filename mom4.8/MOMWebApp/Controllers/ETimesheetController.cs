using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;
using BusinessEntity.payroll;
using System.Text;

namespace MOMWebApp.Controllers
{
    public class ETimesheetController : ApiController
    {
        BL_User objBL_User = new BL_User();
        User objPropUser = new User();
        // POST api/<controller>
        [Route("MOM/GetTimesheetEmp")]
        [HttpPost]

        public string GetTimesheetEmp([FromBody] TimesheetEmp _TimesheetEmp)
        {
            string _webconfig = SSTCryptographer.Decrypt(_TimesheetEmp.webconfig, "webconfig");
            //string _webconfig = _TimesheetEmp.webconfig;
            GeneralFunctions objGeneral = new GeneralFunctions();
            string str;

            //string strOut = "";
            //JavaScriptSerializer sr = new JavaScriptSerializer();
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();


            string stdate = _TimesheetEmp.stDate + " 00:00:00";
            string enddate = _TimesheetEmp.edDate + " 23:59:59";
            objPropUser.ConnConfig = _webconfig;
            objPropUser.Startdt = Convert.ToDateTime(stdate);
            objPropUser.Enddt = Convert.ToDateTime(enddate);
            double weeks = (objPropUser.Startdt - objPropUser.Enddt).TotalDays / 7;
            DataSet dsSaved = objBL_User.getSavedTimesheet(objPropUser);
            if (dsSaved.Tables[0].Rows.Count > 0)
            {
                //GetSavedTimesheetEmp();
                
                objPropUser.ConnConfig = _webconfig;
                DataSet dset = new DataSet();
                objPropUser.Startdt = Convert.ToDateTime(stdate);
                objPropUser.Enddt = Convert.ToDateTime(enddate);
                objPropUser.DepartmentID = Convert.ToInt32(_TimesheetEmp.departmentid);
                objPropUser.Supervisor = _TimesheetEmp.supervisor;
                dset = objBL_User.getSavedTimesheetEmp(objPropUser);

                //return dset.Tables[0];
                JavaScriptSerializer srr = new JavaScriptSerializer();
                List<Dictionary<object, object>> dictListEvals = new List<Dictionary<object, object>>();
                dictListEvals = objGeneral.RowsToDictionary(dset.Tables[0]);
                str = srr.Serialize(dictListEvals);
                return str;

            }
            else
            {
                //GetTimesheetEmpTicket();
                objPropUser.ConnConfig = _webconfig;
                DataSet ds = new DataSet();
                objPropUser.Startdt = Convert.ToDateTime(stdate);
                objPropUser.Enddt = Convert.ToDateTime(enddate);
                objPropUser.DepartmentID = Convert.ToInt32(_TimesheetEmp.departmentid);
                if (_TimesheetEmp.supervisor == null)
                {
                    objPropUser.Supervisor = "";
                }
                else
                {
                    objPropUser.Supervisor = _TimesheetEmp.supervisor;
                }
                objPropUser.WorkId = Convert.ToInt32(_TimesheetEmp.workid);
                int Etimesheet = Convert.ToInt32(_TimesheetEmp.etimesheet);
                #region Company Check
                objPropUser.UserID = Convert.ToInt32(_TimesheetEmp.userid);
                objPropUser.EN = Convert.ToInt32(_TimesheetEmp.en);

                #endregion
                ds = objBL_User.getTimesheetEmp(objPropUser, Etimesheet);
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
                dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
                str = sr.Serialize(dictListEval);
                return str;


                //int count = ds.Tables[0].Rows.Count;
                //string strJson = Dataset2Json(ds, count);
                //return strJson;
            }


            //string stdate = _TimesheetEmp.stDate + " 00:00:00";
            //string enddate = _TimesheetEmp.edDate + " 23:59:59";
            

        }

        // POST api/<controller>
        [Route("MOM/GetTimesheetTicketsByEmp")]
        [HttpPost]

        public string GetTimesheetTicketsByEmp([FromBody] TimesheetEmp _TimesheetEmp)
        {
            string _webconfig = SSTCryptographer.Decrypt(_TimesheetEmp.webconfig, "webconfig");
            //string _webconfig = _TimesheetEmp.webconfig;
            GeneralFunctions objGeneral = new GeneralFunctions();
            string str;


            string stdate = _TimesheetEmp.stDate + " 00:00:00";
            string enddate = _TimesheetEmp.edDate + " 23:59:59";
            objPropUser.ConnConfig = _webconfig;
            DataSet ds = new DataSet();
            objPropUser.Startdt = Convert.ToDateTime(stdate);
            objPropUser.Enddt = Convert.ToDateTime(enddate);
            objPropUser.EmpId = _TimesheetEmp.EmpID;
            //objPropUser.saved = (int)ViewState["saved"];
            //objPropUser.unsaved = (int)ViewState["update"];
            objPropUser.saved = 0;
            objPropUser.unsaved = 0;
            int Etimesheet = Convert.ToInt32(_TimesheetEmp.etimesheet);
            ds = objBL_User.GetTimesheetTicketsByEmp(objPropUser, Etimesheet);
            //return ds.Tables[0];


            JavaScriptSerializer srr = new JavaScriptSerializer();
            List<Dictionary<object, object>> dictListEvals = new List<Dictionary<object, object>>();
            dictListEvals = objGeneral.RowsToDictionary(ds.Tables[0]);
            str = srr.Serialize(dictListEvals);
            return str;



        }

        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder("");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("[");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    //jsonBuilder.Append(dt.Columns[j].ColumnName);
                    //jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                if (dt.Columns.Count > 0)
                {
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                }
                jsonBuilder.Append("],");
            }
            if (dt.Rows.Count > 0)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }

            return jsonBuilder.ToString();
        }
        public static string Dataset2Json(DataSet ds, int total)
        {
            StringBuilder json = new StringBuilder();
            int Dwn = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //{"total":5,"rows":[
                json.Append("{\"draw\":");
                json.Append(Dwn);
                json.Append(",\"recordsTotal\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"recordsFiltered\":");
                if (total == -1)
                {
                    json.Append(dt.Rows.Count);
                }
                else
                {
                    json.Append(total);
                }
                json.Append(",\"data\":[");
                json.Append(DataTable2Json(dt));
                json.Append("]}");
            }
            return json.ToString();
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class TimesheetEmp
    {
        public string webconfig { get; set; }
        public string stDate { get; set; }
        public string edDate { get; set; }
        public string departmentid { get; set; }
        public string supervisor { get; set; }
        public string workid { get; set; }
        public string userid { get; set; }
        public string en { get; set; }
        public string etimesheet { get; set; }
        public int EmpID { get; set; }



    }
}
