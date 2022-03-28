using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetReceivePaymentByIDViewModel
    {
        public int ID { get; set; }
        public int Owner { get; set; }
        public string RolName { get; set; }
        public int Loc { get; set; }
        public string Tag { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public Int16 PaymentMethod { get; set; }
        public string CheckNumber { get; set; }
        public double AmountDue { get; set; }
        public string fDesc { get; set; }
        public Int16 Status { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public int DepID { get; set; }
    }
}
