using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBillHistoryPaymentViewModel
    {
        public string Type { get; set; }
        public int ReceivedPaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public string link { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
    }
}
