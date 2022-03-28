using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class PayrollViewModel
    {
        public string CoCode { get; set; }
        public string BatchID { get; set; }
        public string EmpRef { get; set; }
        public int Shift { get; set; }
        public string TempDept { get; set; }
        public string RateCode { get; set; }
        public int RegHours { get; set; }
        public int OTHours { get; set; }
        public string Hour3Code { get; set; }
        public string Hours3Amount { get; set; }
    }
}
