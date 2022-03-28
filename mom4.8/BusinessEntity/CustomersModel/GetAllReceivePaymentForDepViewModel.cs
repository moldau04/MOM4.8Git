using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetAllReceivePaymentForDepViewModel
    {
        public int owner { get; set; }
        public int ID { get; set; }
        public int Rol { get; set; }
        public string customerName { get; set; }
        public int loc { get; set; }
        public string Tag { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public string fDesc { get; set; }
        public string PaymentMethod { get; set; }
        public string CheckNumber { get; set; }
        public double AmountDue { get; set; }
    }
}
