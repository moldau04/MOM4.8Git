using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAPAging360ByDateViewModel
    {
        public int PJID { get; set; }
        public int VendorID { get; set; }
        public string Vendor { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public int TRID { get; set; }
        public double Original { get; set; }
        public double Paid { get; set; }
        public double Total { get; set; }
        public int DueIn { get; set; }
        public double ThirtyDay { get; set; }
        public double NintyDay { get; set; }
        public double ThreeSixtyDay { get; set; }
        public double OverThreeSixtyDay { get; set; }
    }
}
