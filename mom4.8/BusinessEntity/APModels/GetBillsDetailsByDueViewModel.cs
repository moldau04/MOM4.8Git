using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBillsDetailsByDueViewModel
    {
        public int ID { get; set; }
        public DateTime fDate { get; set; }//datetime 
        public DateTime Due{ get; set; }//datetime
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Total { get; set; }
        public int VendorID { get; set; }
        public string Vendor { get; set; }
        public Int16 Status { get; set; }
        public string StatusName { get; set; }
        public int Batch { get; set; }
        public Int16 Terms { get; set; }
        public int PO { get; set; }
        public int TRID { get; set; }
        public Int16 Spec { get; set; }
        public DateTime IDate { get; set; }//datetime
        public double UseTax { get; set; }
        public double Disc { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public int ReqBy { get; set; }
        public string VoidR { get; set; }
        public Int32 DueIn { get; set; }
        public double Amount { get; set; }
        public double SevenDay { get; set; }
        public double ThirtyDay { get; set; }
        public double SixtyDay { get; set; }
        public double NintyDay { get; set; }
        public double NintyOneDay { get; set; }
        public double SevenDay2 { get; set; }
        public double ThirtyDay2 { get; set; }
        public double SixtyDay2 { get; set; }
        public double SixtyOneDay { get; set; }
    }
}
