using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{

    [Serializable]
    public class ListGetARAgingByTerritory
    {
        public List<GetARAgingByTerritoryTable1> lstTable1 { get; set; }
        public List<GetARAgingByTerritoryTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetARAgingByTerritoryTable1
    {
        public int TransID { get; set; }
        public string Salesperson { get; set; }
        public string DefaultSalesperson { get; set; }
        public int Type { get; set; }
        public string CID { get; set; }
        public string CustomerName { get; set; }
        public string Custom1 { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public double Original { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public string fDesc { get; set; }
        public int Ref { get; set; }
        public int DueIn { get; set; }
        public double CurrentDay { get; set; }
        public double ThirtyDay { get; set; }
        public double SixtyDay { get; set; }
        public double SixtyOneDay { get; set; }
        public double NintyDay { get; set; }
        public double OverNintyDay { get; set; }
        public double OverOneTwentyDay { get; set; }
        public double OnetwentyDay { get; set; }
        public string Status { get; set; }
    }

    [Serializable]
    public class GetARAgingByTerritoryTable2
    {
        public int TransID { get; set; }
        public string Salesperson { get; set; }
        public string DefaultSalesperson { get; set; }
        public int Type { get; set; }
        public string CID { get; set; }
        public string CustomerName { get; set; }
        public string Custom1 { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public double Original { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public string fDesc { get; set; }
        public int Ref { get; set; }
        public int DueIn { get; set; }
        public double ThirtyDay { get; set; }
        public double SixtyDay { get; set; }
        public double NintyoneDay { get; set; }
        public double OnetwentyDay { get; set; }
        public double OverOneTwentyDay { get; set; }
    }
}
