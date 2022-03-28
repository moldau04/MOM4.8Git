using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetInvoiceByCustomerID
    {
        public List<GetInvoiceByCustomerIDTable1> lstTable1 { get; set; }
        public List<GetInvoiceByCustomerIDTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetInvoiceByCustomerIDTable1
    {
        public int Ref { get; set; }
        public int Owner { get; set; }
        public DateTime fDate { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public double Amount { get; set; }
        public double STax { get; set; }
        public double Total { get; set; }
        public Int16 StatusID { get; set; }
        public string manualInv { get; set; }
        public int TransID { get; set; }
        public double PrevDueAmount { get; set; }
        public double paymentAmt { get; set; }
        public int PaymentID { get; set; }
        public double DueAmount { get; set; }
        public double OrigAmount { get; set; }
        public string status { get; set; }
        public string PO { get; set; }
        public int loc { get; set; }
        public string customername { get; set; }
        public string type { get; set; }
        public double balance { get; set; }
    }

    [Serializable]
    public class GetInvoiceByCustomerIDTable2
    {
        public int ID { get; set; }
        public double Balance { get; set; }
    }
}
