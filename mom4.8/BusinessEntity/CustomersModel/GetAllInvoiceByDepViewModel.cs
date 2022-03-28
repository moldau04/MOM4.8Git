using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetAllInvoiceByDep
    {
        public List<GetAllInvoiceByDepTable1> lstTable1 { get; set; }
        public List<GetAllInvoiceByDepTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetAllInvoiceByDepTable1
    {
        public int Owner { get; set; }
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int Rol { get; set; }
        public string customerName { get; set; }
        public int loc { get; set; }
        public string Tag { get; set; }
        public int En { get; set; }
        public string Company { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public string fDesc { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodID { get; set; }
        public string CheckNumber { get; set; }
        public double AmountDue { get; set; }
        public bool isChecked { get; set; }
        public int OrderNo { get; set; }
        public int Type { get; set; }
    }

    [Serializable]
    public class GetAllInvoiceByDepTable2
    {
        public int TransID { get; set; }
        public double Amount { get; set; }
        public string fDesc { get; set; }
        public string Acct { get; set; }
        public string fTitle { get; set; }
        public int ID { get; set; }
        public int ischecked { get; set; }
        public string Ref { get; set; }
        public int orderNo { get; set; }
        public int Loc { get; set; }
        public string Tag { get; set; }

    }
}
