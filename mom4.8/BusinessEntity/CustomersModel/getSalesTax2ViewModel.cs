using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class getSalesTax2ViewModel
    {
        public string Name { get; set; }
        public string fDesc { get; set; }
        public double Rate { get; set; }
        public string State { get; set; }
        public string Remarks { get; set; }
        public int Count { get; set; }
        public int GL { get; set; }
        public Int16 Type { get; set; }
        public Int16 UType { get; set; }
        public string PSTReg { get; set; }
        public string QBStaxID { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsTaxable { get; set; }
        public string AcctDesc { get; set; }
    }
}
