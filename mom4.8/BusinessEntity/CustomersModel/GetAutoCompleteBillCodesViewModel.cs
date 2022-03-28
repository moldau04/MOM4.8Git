using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetAutoCompleteBillCodesViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public Int16 type { get; set; }
        public double Hand { get; set; }
        public string BillType { get; set; }
        public int Status { get; set; }
        public double Price1 { get; set; }
        public int AStatus { get; set; }
    }
}
