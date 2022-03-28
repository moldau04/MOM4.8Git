using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCollectionsViewModel
    {
        public int TransID { get; set; }
        public string Department { get; set; }
        public int Type { get; set; }
        public string cid { get; set; }
        public int Owner { get; set; }
        public int Loc { get; set; }
        public int credit { get; set; }
        public string CustomerName { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public string LocIID { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public double Original { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public string fDesc { get; set; }
        public int Ref { get; set; }
        public string RefURL { get; set; }
        public int DueIn { get; set; }
        public double CurrentDay { get; set; }
        public double CurrSevenDay { get; set; }
        public double ThirtyDay { get; set; }
        public double SixtyDay { get; set; }
        public double NintyDay { get; set; }
        public double NintyOneDay { get; set; }
        public double OneTwentyDay { get; set; }
        public int sel { get; set; }
        public string Status { get; set; }

    }
}
