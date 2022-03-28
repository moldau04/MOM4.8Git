using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCompletedTicketViewModel
    {
        public int ID { get; set; }
        public DateTime CDate { get; set; }
        public DateTime DDate { get; set; }
        public DateTime EDate { get; set; }
        public int fWork { get; set; }
        public int Job { get; set; }
        public int Loc { get; set; }
        public int Elev { get; set; }
        public Int16 Type { get; set; }
        public string fDesc { get; set; }
        public string DescRes { get; set; }
        public double Total { get; set; }
        public double Reg { get; set; }
        public double OT { get; set; }
        public double DT { get; set; }
        public double TT { get; set; }
        public double Zone { get; set; }
        public double Toll { get; set; }
        public double OtherE { get; set; }
        public Int16 Status { get; set; }
        public int Invoice { get; set; }
        public Int16 Level { get; set; }
        public double Est { get; set; }
        public string Cat { get; set; }
        public string Who { get; set; }
        public string fBy { get; set; }
        public int fLong { get; set; }
        public int Latt { get; set; }
        public int WageC { get; set; }
        public Int16 Phase { get; set; }
        public int Car { get; set; }
        public Int16 CallIn { get; set; }
        public double Mileage { get; set; }
        public double NT { get; set; }
        public int CauseID { get; set; }
        public string CauseDesc { get; set; }
        public string fGroup { get; set; }
        public int PriceL { get; set; }
        public string WorkOrder { get; set; }
        public DateTime TimeRoute { get; set; }
        public DateTime TimeSite { get; set; }
        public DateTime TimeComp { get; set; }
        public string JobType { get; set; }
        public string Mech { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Assignname { get; set; }
        public string Unit { get; set; }
        //public byte[] Signature { get; set; }
        public string Signature { get; set; }
    }
}
