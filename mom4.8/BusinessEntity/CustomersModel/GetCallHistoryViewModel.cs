using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCallHistoryViewModel
    {
        public string who { get; set; }
        public string CPhone { get; set; }
        public  int  lid { get; set; }
        public string locid { get; set; }
        public int assigned { get; set; }
        public string fulladdress { get; set; }
        public string city { get; set; }
        public string WorkOrder { get; set; }
        public double Reg { get; set; }
        public double OT { get; set; }
        public double NT { get; set; }
        public double DT { get; set; }
        public double TT { get; set; }
        public double BT { get; set; }
        public double Total { get; set; }
        public int ClearCheck { get; set; }
        public Int16 charge { get; set; }
        public string fDesc { get; set; }
        public DateTime TimeRoute { get; set; }
        public DateTime TimeSite { get; set; }
        public DateTime TimeComp { get; set; }
        public int comp { get; set; }
        public string dwork { get; set; }
        public string lastname { get; set; }
        public double hourlyrate { get; set; }
        public int ID { get; set; }
        public string customername { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string locname { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string Cat { get; set; }
        public DateTime edate { get; set; }
        public DateTime CDate { get; set; }
        public string descres { get; set; }
        public string assignname { get; set; }
        public double Est { get; set; }
        public double Tottime { get; set; }
        public double timediff { get; set; }
        public string workorder1 { get; set; }
        public double expenses { get; set; }
        public double zone { get; set; }
        public double toll { get; set; }
        public double othere { get; set; }
        public double extraexp { get; set; }
        public double mileagetravel { get; set; }
        public int mileage { get; set; }
        public int signatureCount { get; set; }
        public int DocumentCount { get; set; }
        public int workerid { get; set; }
        public string description { get; set; }
        public string fdescreason { get; set; }
        public int invoice { get; set; }
        public int Confirmed { get; set; }
        public string manualinvoice { get; set; }
        public string invoiceno { get; set; }
        public int ownerid { get; set; }
        public string QBinvoiceid { get; set; }
        public int TransferTime { get; set; }
        public string serviceitem { get; set; }
        public string PayrollItem { get; set; }
        public double RTOTTT { get; set; }
        public string WorkerLastName { get; set; }
        //public Byte[] timesign { get; set; }
        public int dispalert { get; set; }
        public int credithold { get; set; }
        public int high { get; set; }
        public int unitid { get; set; }
        public string unit { get; set; }
        public string unittype { get; set; }
        public string defaultworker { get; set; }
        public string defaultmech { get; set; }
        public string department { get; set; }
        public string bremarks { get; set; }
        public double laborexp { get; set; }
        //public Byte[] signature { get; set; }
        public string signature { get; set; }
        public string state { get; set; }
        public double mileagepr { get; set; }
        public bool afterhours { get; set; }
        public bool weekends { get; set; }
        public int EmailNotified { get; set; }
        public DateTime EmailTime { get; set; }
        public int Job { get; set; }
        public string ProjectDescription { get; set; }
        //public Byte Custom6 { get; set; }
        //public Byte Custom7 { get; set; }
        public string Custom6 { get; set; }
        public string Custom7 { get; set; }
        public string fBy { get; set; }
        public string WageCategory { get; set; }
    }
}
