using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetARAging360ByAsOfDate
    {
        public List<GetARAging360ByAsOfDateTable1> lstTable1 { get; set; }
        public List<GetARAging360ByAsOfDateTable2> lstTable2 { get; set; }
        public List<GetARAging360ByAsOfDateTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetARAging360ByAsOfDateTable1
    {
        public int TransID { get; set; }
        public string Type { get; set; }
        public string LocType { get; set; }
        public string cid { get; set; }
        public string CustomerName { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public string Custom1 { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public double Original { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public string fDesc { get; set; }
        public int Ref { get; set; }
        public int DueIn { get; set; }
        public double ZeroToThirtyDay { get; set; }
        public double ThirtyDayToNinety { get; set; }
        public double NinetyTo360 { get; set; }
        public double Over360 { get; set; }
        public string Status { get; set; }
    }

    [Serializable]
    public class GetARAging360ByAsOfDateTable2
    {
        public double Column1 { get; set; }
    }

    [Serializable]
    public class GetARAging360ByAsOfDateTable3
    {
        public int TransID { get; set; }
        public Int16 type { get; set; }
        public string cid { get; set; }
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
        public Int64 Ref { get; set; }
        public int DueIn { get; set; }
        public double ZeroToThirtyDay { get; set; }
        public double ThirtyDayToNinety { get; set; }
        public double NinetyTo360 { get; set; }
        public double Over360 { get; set; }
        public Int16 Sel { get; set; }
    }
}
