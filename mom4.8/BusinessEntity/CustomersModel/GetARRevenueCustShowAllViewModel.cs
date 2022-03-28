using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetARRevenueCustShowAll
    {
        public List<GetARRevenueCustShowAllTable1> lstTable1 { get; set; }
        public List<GetARRevenueCustShowAllTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetARRevenueCustShowAllTable1
    {
        public string CustName { get; set; }
        public int ID { get; set; }
        public DateTime fDate { get; set; }
        public int Ref { get; set; }
        public string LocName { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public double Credits { get; set; }
        public double Balance { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int linkTo { get; set; }
        public int owner { get; set; }
        public int LocID { get; set; }
        public string Link { get; set; }
        public int TransID { get; set; }
    }

    [Serializable]
    public class GetARRevenueCustShowAllTable2
    {
        public double PrevRunTotal { get; set; }
    }
}
