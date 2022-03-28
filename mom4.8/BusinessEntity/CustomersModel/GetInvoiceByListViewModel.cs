using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoiceByListViewModel
    {
        public int Owner { get; set; }
        public string OwnerName { get; set; }
        public string LocID { get; set; }
        public string LocationName { get; set; }
        public double STax { get; set; }
        public double Total { get; set; }
        public double PrevDueAmount { get; set; }
        public double Amount { get; set; }
        public double AmountDue { get; set; }
        public double paymentAmt { get; set; }
        public string Invoice { get; set; }
        public string CheckNumber { get; set; }
        public string BatchReceive { get; set; }
        public int OrderNo { get; set; }
        public Int16 isChecked { get; set; }
    }
}
