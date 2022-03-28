using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetPOWeeklyViewModel
    {
        public int PO { get; set; }
        public DateTime Post { get; set; }
        public DateTime Due { get; set; }
        public string fDesc { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        public double Amount { get; set; }
        public Int16 Status { get; set; }
        public string StatusName { get; set; }
        public int WeekCount { get; set; }
        public DateTime WeekDate { get; set; }
        
    }
}
