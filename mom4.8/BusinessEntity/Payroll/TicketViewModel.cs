using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Payroll
{
    [Serializable]
    public class TicketViewModel
    {
    
         public int ID { get; set; }
        public string TWokrOrder { get; set; }
        public DateTime TCDate { get; set; }
        public DateTime TDDate { get; set; }
        public DateTime TEDate { get; set; }
        public int JType { get; set; }
        public string WfDesc { get; set; }
        public string TfDesc { get; set; }
        public int TTotal { get; set; }
        public int WIReg { get; set; }
        public int Loc { get; set; }
        public string LocName { get; set; }



    }
}
