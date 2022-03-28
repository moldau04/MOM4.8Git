using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetPJDetailByIDViewModel
    {
        public int ID { get; set; }
        public DateTime fDate { get; set; }//datetime
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public Int16 Status { get; set; }
        public int Batch { get; set; }
        public Int16 Terms { get; set; }
        public int PO { get; set; }
        public int TRID { get; set; }
        public Int16 Spec { get; set; }
        public int IfPaid { get; set; }
        public DateTime IDate { get; set; }//datetime
        public double UseTax { get; set; }
        public double Disc { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public int ReqBy { get; set; }
        public string VoidR { get; set; }
        public string STaxName { get; set; }
        public int STaxGL { get; set; }
        public Int16 STaxType { get; set; }
        public double STaxRate { get; set; }
        public string VendorName { get; set; }
        public int ReceivePO { get; set; }
        public DateTime Due { get; set; }//datetime
        public int VendorID { get; set; }
        public string State { get; set; }
        public double ReceivedAmount { get; set; }
        public double POAmount { get; set; }
        public double Paid { get; set; }
        public double CrPaid { get; set; }
        public int Vendor { get; set; }
    }
}
