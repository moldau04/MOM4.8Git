using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBillsDetails360ByDueViewModel
    {
        public int ID { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Total { get; set; }
        public int VendorID { get; set; }
        public string Vendor { get; set; }
        public Int16 Status { get; set; }
        public Int16 Status1 { get; set; }
        public string StatusName { get; set; }
        public int Batch { get; set; }
        public Int16 Terms { get; set; }
        public int PO { get; set; }
        public int TRID { get; set; }
        public Int16 Spec { get; set; }
        public DateTime IDate { get; set; }
        public double UseTax { get; set; }
        public double Disc { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public int ReqBy { get; set; }
        public string VoidR { get; set; }
        public string DueIn { get; set; }
        public double Amount { get; set; }
        public double ThirtyDay { get; set; }
        public double NintyDay { get; set; }
        public double ThreeSixtyDay { get; set; }
        public double OverThreeSixtyDay { get; set; }

    }
}
