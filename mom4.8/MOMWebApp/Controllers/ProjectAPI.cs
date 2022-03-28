 
using System.Collections.Generic; 
using System.Web.Http;
 

namespace MOMWebApp.Controllers
{
    public class ProjectAPIController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public  IHttpActionResult Get(string JobID, string Status,string WebConfig)
        {

            JobID = "";

            WebConfig = "";

            List<ProjectTicketTap> _ProjectTicketTap = new List<ProjectTicketTap>();

            _ProjectTicketTap.Add(new ProjectTicketTap
            {
                Assigned = "4",
                ID = "1",
                Comp = "1",
                dwork = "John Maxwell",
                Cat = "Time Card",
                 description = "Time Card",
                assignname = "Time Card",
                edate = "Time Card",
                Est = "Time Card",
                Tottime = "Time Card",
                Reg = "Time Card",
                OT = "Time Card",
                NT = "Time Card",
                DT = "Time Card",
                TT = "Time Card",
                laborexp = "Time Card",
                expenses = "Time Card"

            });

            _ProjectTicketTap.Add(new ProjectTicketTap
            {
                Assigned = "4",
                ID = "2",
                Comp = "1",
                dwork = "John Maxwell",
                Cat = "Time Card",
                description = "Time Card",
                assignname = "Time Card",
                edate = "Time Card",
                Est = "Time Card",
                Tottime = "Time Card",
                Reg = "Time Card",
                OT = "Time Card",
                NT = "Time Card",
                DT = "Time Card",
                TT = "Time Card",
                laborexp = "Time Card",
                expenses = "Time Card"
            });
            _ProjectTicketTap.Add(new ProjectTicketTap
            {
                Assigned = "4",
                ID = "3",
                Comp = "1",
                dwork = "John Maxwell",
                Cat = "Time Card",
                description = "Time Card",
                assignname = "Time Card",
                edate = "Time Card",
                Est = "Time Card",
                Tottime = "Time Card",
                Reg = "Time Card",
                OT = "Time Card",
                NT = "Time Card",
                DT = "Time Card",
                TT = "Time Card",
                laborexp = "Time Card",
                expenses = "Time Card"
            });
            _ProjectTicketTap.Add(new ProjectTicketTap
            {
                Assigned = "4",
                ID = "4",
                Comp = "1",
                dwork = "John Maxwell",
                Cat = "Time Card",
                description = "Time Card",
                assignname = "Time Card",
                edate = "Time Card",
                Est = "Time Card",
                Tottime = "Time Card",
                Reg = "Time Card",
                OT = "Time Card",
                NT = "Time Card",
                DT = "Time Card",
                TT = "Time Card",
                laborexp = "Time Card",
                expenses = "Time Card"
            });
            _ProjectTicketTap.Add(new ProjectTicketTap
            {
                Assigned = "4",
                ID = "5",
                Comp = "1",
                dwork = "John Maxwell",
                Cat = "Time Card",
                description = "Time Card",
                assignname = "Time Card",
                edate = "Time Card",
                Est = "Time Card",
                Tottime = "Time Card",
                Reg = "Time Card",
                OT = "Time Card",
                NT = "Time Card",
                DT = "Time Card",
                TT = "Time Card",
                laborexp = "Time Card",
                expenses = "Time Card"
            });

            return Json(_ProjectTicketTap); 

        }


       
    }

    public class ProjectTicketTap
    {  
        public string Assigned { get; set; }
        public string ID { get; set; }
        public string Comp { get; set; }
        public string dwork { get; set; }
        public string Cat { get; set; }
        public string description { get; set; }
        public string assignname { get; set; }
        public string edate { get; set; }
        public string Est { get; set; }
        public string Tottime { get; set; }
        public string Reg { get; set; }
        public string OT { get; set; }
        public string NT { get; set; }
        public string DT { get; set; }
        public string TT { get; set; }
        public string laborexp { get; set; }
        public string expenses { get; set; }

    }

}