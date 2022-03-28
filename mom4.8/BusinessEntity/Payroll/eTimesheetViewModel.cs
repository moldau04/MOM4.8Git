using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.payroll
{
    [Serializable]
    public class eTimesheetViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string fDesc { get; set; }

        public double reg { get; set; }
        public double OT { get; set; }
        public double DT { get; set; }
        public double TT { get; set; }
        public double NT { get; set; }
        public double Zone { get; set; }
        public double MileageRate { get; set; }

        public int Mileage { get; set; }
        public int MileRate { get; set; }

        public double extra { get; set; }
        public double Toll { get; set; }
        public double OtherE { get; set; }
        public int pay { get; set; }
        public int holiday { get; set; }
        public int vacation { get; set; }
        public int sicktime { get; set; }
        public int reimb { get; set; }
        public int bonus { get; set; }
        public int paymethod { get; set; }
        public int pmethod { get; set; }

        public int userid { get; set; }
        public string usertype { get; set; }
        public double total { get; set; }
        public double phour { get; set; }
        public double salary { get; set; }

        public double HourlyRate { get; set; }
        public int Customtick1 { get; set; }
        public int Customtick2 { get; set; }
        public int Customtick3 { get; set; }
        public int dollaramount { get; set; }
        public int Reg1 { get; set; }

        public int OT1 { get; set; }
        public int DT1 { get; set; }
        public int TT1 { get; set; }
        public int NT1 { get; set; }
        public int Zone1 { get; set; }

        public int Mileage1 { get; set; }

        public int Extra1 { get; set; }

        public int Misc1 { get; set; }

        public int Toll1 { get; set; }

        public int HourRate1 { get; set; }
        public string signature { get; set; }
        public int ref1 {get;set;}

        public int custom { get; set; }

        public int countDetail { get; set; }

        public int Processed { get; set; }
        public DateTime Date { get; set; }
        public int TicketID { get; set; }












    }
}
