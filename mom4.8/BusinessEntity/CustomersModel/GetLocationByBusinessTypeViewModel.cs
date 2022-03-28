using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetLocationByBusinessType
    {
        public List<GetLocationByBusinessTypeTable> lstTable { get; set; }
        public List<GetLocationByBusinessTypeTable1> lstTable1 { get; set; }
        public List<GetLocationByBusinessTypeTable2> lstTable2 { get; set; }
        public List<GetLocationByBusinessTypeTable3> lstTable3 { get; set; }
        public List<GetLocationByBusinessTypeTable4> lstTable4 { get; set; }

    }

    [Serializable]
    public class GetLocationByBusinessTypeTable
    {
        public string ID { get; set; }
        public int Loc { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Type { get; set; }
        public int Terr { get; set; }
        public int SMan { get; set; }
        public string Salesperson { get; set; }
        public string Unit { get; set; }
        public int Elevs { get; set; }
        public string BusinessType { get; set; }
    }

    [Serializable]
    public class GetLocationByBusinessTypeTable1
    {
        public string BusinessType { get; set; }
        public int LocCount { get; set; }
    }

    [Serializable]
    public class GetLocationByBusinessTypeTable2
    {
        public string BusinessType { get; set; }
        public int Terr { get; set; }
        public string Salesperson { get; set; }
        public int LocCount { get; set; }
        
    }

    [Serializable]
    public class GetLocationByBusinessTypeTable3
    {
        public string BusinessType { get; set; }
        public int ElevCount { get; set; }
    }

    [Serializable]
    public class GetLocationByBusinessTypeTable4
    {
        public string BusinessType { get; set; }
        public int Terr { get; set; }
        public string Salesperson { get; set; }
        public int ElevCount { get; set; }
    }
}
