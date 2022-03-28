using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetDepositListByDateViewModel
    {
        public int DepID { get; set; }
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public string customerName { get; set; }
        public int loc { get; set; }
        public string Tag { get; set; }
        public string Company { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public string fDesc { get; set; }
        public double AmountDue { get; set; }
        public int OrderNo { get; set; }
        public int Type { get; set; }
        public string Department { get; set; }
        public string LocID { get; set; }
        public string DefaultSalePerson { get; set; }
        public string CheckNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string AccountChart { get; set; }
        public DateTime DepDate { get; set; }
        public string Bank { get; set; }
        public string ProjectID { get; set; }
    }
}
