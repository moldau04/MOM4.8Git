using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetActiveBillingCodeViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public Int16 TYPE { get; set; }
        public double Hand { get; set; }
        public string BillType { get; set; }
        public Int16 Status { get; set; }
        public double Price1 { get; set; }
        public Int16 AStatus { get; set; }
        public int SAcct { get; set; }

    }
}
