using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetGCandHowerLocID
    {
        public List<GetGCandHowerLocIDTable1> lstTable1 { get; set; }
        public List<GetGCandHowerLocIDTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetGCandHowerLocIDTable1
    {
        public string RolName { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string cellular { get; set; }
        public string rolRemarks { get; set; }
        public int LocContactType { get; set; }
        public int RolID { get; set; }
        public string Address { get; set; }
    }

    [Serializable]
    public class GetGCandHowerLocIDTable2
    {
        public string RolName { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string cellular { get; set; }
        public string rolRemarks { get; set; }
        public int LocContactType { get; set; }
        public int RolID { get; set; }
        public string Address { get; set; }
    }
}
