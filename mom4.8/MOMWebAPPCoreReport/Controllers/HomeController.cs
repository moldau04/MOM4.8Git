using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using MOMWebAPPCoreReport.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using MOMWebAPPCoreReport.ReportsBL;
using BusinessEntity;
using BusinessLayer;
using Microsoft.AspNetCore.Http;

namespace MOMWebAPPCoreReport.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        Auth authser = new Auth();
        private readonly ILogger<HomeController> _logger;
        readonly ReqParams Req = new ReqParams();
        BL_User objBL_User = new BL_User();

        static HomeController()
        {
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHlgHN5p4wFaNqaDgehHwPI3o2yZv8P5AgvHzgWQKWodhZsOug" +
         "YiZkrdLmU/XqA+/UPfNtByhqBpYDCZm2EbsThXjQo40gEU0m917ZQzsJtK7AmfdxbOBZekTJy+tmOpqEwJo0oU7KhG" +
         "5nbAJRF3Bs7EUXHmXUHVgBQrxck4F8gW5WgnAKszvQsAvqterBx6rX50nUB+f/CVvWAXC8TUNpxHYa5yEofvIjwcfi" +
         "07Kb6e8NWpWmy+9CvgF0q5C1ywoq0GEgewrIHnRrI/ri/LRJCEgfLh+xPNynFpV3wL6EAikEN5pOqO3lgBtO8lZ+F3" +
         "ipjXUzD83APniT86vodkEq5I6sEl77wT5mrfQa8Xe2UHsMs2GONyid+QkaMmCAgEet+Fpy2Q/hYwsAeHx2LwLM8xP9" +
         "vZiPovsQql55C6wHgXBLWVn7tdrVyBnxz9uNu84HSH6ipr+IFHn35qjtgpya/7Jak3uOVMtA/zm9uFxQEja9zlHbaq" +
         "mfuhd/8RFlmMHv61brrXkwl51xVJ6jTBDheJQGhlIyvKEbb+WZ11xzWRVw==";
        }
        public HomeController(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public  IActionResult Index()
        {

            if (Request.Form.Count > 0)
            {

                string data = Request.Form["Data"];
                HttpContext.Session.SetString("Data", data);
                Req.Token = Request.Form["Token"];
                Req.DecrpytToken = authser.Decrypt(Req.Token);
                Req.company = Req.DecrpytToken.Split("|||")[1].ToString();
                Req.Domain_Name = Req.DecrpytToken.Split("|||")[2].ToString();
                Req.User_Id = Convert.ToInt32(Req.DecrpytToken.Split("|||")[3].ToString());
                HttpContext.Session.SetString("ID", Req.DecrpytToken.Split("|||")[3].ToString());
                Req.Constring = _Configuration.GetConnectionString("DevConnection");
                Req.startdate = data.Split("|||")[0].ToString();
                Req.EndDate = data.Split("|||")[1].ToString();
                Req.ddlDeprt = data.Split("|||")[2].ToString();
                Req.Screen = data.Split("|||")[3].ToString();
                Req.ticketFilter = Request.Form["ticketFilter"];
                Req.types = data.Split("|||")[7].ToString();
                Req.Defalutcmp = data.Split("|||")[4].ToString();
                Req.ReportName = data.Split("|||")[5].ToString();
                Req.username = data.Split("|||")[6].ToString();
                Req.AppValue = _Configuration.GetConnectionString("CustomerName");
                bool auth = authser.Checkuser(Req);
                if (auth.Equals(true))
                {
                    return Redirect("~/Home/Report");
                }
                else
                {
                    return Redirect("~/Home/Error");

                }

            }
            return RedirectToAction("target");


        }

        public IActionResult Report()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult GetReport()
        {

            StiReport report = new StiReport();
            ReqParams Reqs = new ReqParams();
            SetValues( Reqs);

            try
            {
                string reportPathStimul = StiNetCoreHelper.MapPath(this, "Reports/TimeRecapReport.mrt");
                if (!string.IsNullOrEmpty(Reqs.ddlDeprt) && (Reqs.types) == "all")
                {
                    reportPathStimul = StiNetCoreHelper.MapPath(this, "Reports/TimeRecapAllHoursReport.mrt");
                }
                
                report.Load(reportPathStimul);
                
                // Company info
                DataSet dsC = new DataSet();
                User objPropUser = new User();
                objPropUser.ConnConfig = _Configuration.GetConnectionString("DevConnection");

                 dsC = objBL_User.getControl(objPropUser);

                report.RegData("dsCompany", dsC.Tables[0]);
                  
                //Get data
                DataSet ServiceCallHistoryReportDataSet = new DataSet();
                ServiceCallHistoryReportDataSet = authser.GetReportData(Reqs);
                DataTable CompletedTickets = new DataTable();
                CompletedTickets = ServiceCallHistoryReportDataSet.Tables[0].Copy();

                report.RegData("dsTimeRecap", CompletedTickets);
               
                report.Dictionary.Variables["paramUsername"].Value = Reqs.username;
                report.Dictionary.Variables["paramSDate"].Value = Reqs.startdate;
                report.Dictionary.Variables["paramEDate"].Value = Reqs.EndDate;
                return StiNetCoreViewer.GetReportResult(this, report);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

                return null;
            }

        }

        public IActionResult ViewerEvent()
        {
            return StiNetCoreDesigner.DesignerEventResult(this);
        }

        //public IActionResult ViewerInteraction()

        //{

        //    StiRequestParams requestParams = StiNetCoreViewer.GetRequestParams(this);

        //    if (requestParams.Action == StiAction.Variables)

        //    {

        //        DataSet data = new DataSet();





        //        StiReport report = StiNetCoreViewer.GetReportObject(this);

        //        report.RegData("Demo", data);



        //        return StiNetCoreViewer.InteractionResult(this, report);

        //    }



        //    return StiNetCoreViewer.InteractionResult(this);

        //}

        public IActionResult DesignerEvent()

        {

            return StiNetCoreDesigner.DesignerEventResult(this);

        }

        public DataSet GetReportData()
        {

            // print GetReport// 
            // DataSet dsC = new DataSet();
            //report.Load(StiNetCoreHelper.MapPath(this, "Reports/Empt.mrt"));
            //return StiNetCoreViewer.GetReportResult(this, report);
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter adapter;

                using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DevConnection")))
                {
                    con.Open();
                    string query = "select * from Emp";
                    adapter = new SqlDataAdapter(query, con);
                    //fill the dataset
                    adapter.Fill(ds);
                    con.Close();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetValues(ReqParams Reqs)
        {
            string  Data= HttpContext.Session.GetString("Data");
            Reqs.Constring = _Configuration.GetConnectionString("DevConnection");
            Reqs.startdate =Data.Split("|||")[0].ToString();
            Reqs.EndDate = Data.Split("|||")[1].ToString();
            Reqs.ddlDeprt = Data.Split("|||")[2].ToString();
            Reqs.Screen = Data.Split("|||")[3].ToString();
            Reqs.User_Id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            //Reqs.ticketFilter = Request.Form["ticketFilter"];
            //Reqs.type = Request.Form["type"];
            Reqs.Defalutcmp = Data.Split("|||")[4].ToString();
            Reqs.ReportName = Data.Split("|||")[5].ToString();
            Reqs.username = Data.Split("|||")[6].ToString();
            Reqs.types = Data.Split("|||")[7].ToString();
        }
    }
}
