using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoicesByReceivedPayMultiViewModel
    {
        public int Owner { get; set; }
        public string OwnerName { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public double Amount { get; set; }
        public double AmountDue { get; set; }
        public int Invoice { get; set; }
        public int Loc { get; set; }
    }
}
