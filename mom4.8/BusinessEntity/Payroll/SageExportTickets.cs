using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace BusinessEntity.payroll
{
    [Serializable]
    public class SageExportTickets
    {
        public string DC { get; set; }
        public string SageJob { get; set; }
        public string extra { get; set; }
        public double costcode { get; set; }
        public string Category { get; set; }
        public int trantype { get; set; }
        public string trandate { get; set; }
        public string accdate { get; set; }
        public string description { get; set; }
        public double units { get; set; }
        public double unitcost { get; set; }
        public double amount { get; set; }
        public string debitacc { get; set; }
        public string creditacc { get; set; }
        public int ticket { get; set; }
    }
}
