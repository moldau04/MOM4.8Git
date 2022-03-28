using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class PJViewModel
    {
        public int ID { get; set; }
        public DateTime fDate { get; set; }//datetime
        public DateTime PostDate { get; set; }//datetime
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public int Vendor { get; set; }
        public Int16 Status { get; set; }
        public int Batch{ get; set; }
        public Int16 Terms{ get; set; }
        public int PO{ get; set; }
        public int TRID{ get; set; }
        public Int16 Spec{ get; set; }
        public int IfPaid{ get; set; }
        public DateTime IDate{ get; set; }//datetime
        public double UseTax{ get; set; }
        public double Disc{ get; set; }
        public string Custom1{ get; set; }
        public string Custom2{ get; set; }
        public int ReqBy{ get; set; }
        public string VoidR{ get; set; }
        public string UtaxName{ get; set; }
        public int GL{ get; set; }
        private DataSet _ds;
        private string _ConnConfig;
        public DateTime StartDate{ get; set; }//datetime
        public DateTime EndDate{ get; set; }//datetime
        public Int16 SearchValue{ get; set; }
        public DateTime SearchDate{ get; set; }//datetime
        public DataTable _dt;
        public int ReceivePo{ get; set; }
        public DateTime Due{ get; set; }//datetime
        public int ProjectNumber{ get; set; }
        public string vendorName { get; set; }
        public bool IsRecurring{ get; set; }
        public int Frequency{ get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string MOMUSer { get; set; }
        public string Custom{ get; set; }

        public string STaxName{ get; set; }
        public string UTaxName{ get; set; }
        public int STaxGL{ get; set; }
        public Int16 STaxType { get; set; }
        public int UTaxGL{ get; set; }
        public double STax{ get; set; }
        public double STaxRate{ get; set; }
        public double UTax{ get; set; }
        public double UTaxRate{ get; set; }
        public string StatusName { get; set; }
        public double ReceivedAmount { get; set; }
        public double POAmount { get; set; }
        public double RunTotal { get; set; }
        public string Type { get; set; }
        public Int32 Count{ get; set; }
        public Int32 DueIn{ get; set; }
        public double SevenDay{ get; set; }
        public double SevenDay2{ get; set; }
        public double ThirtyDay{ get; set; }
        public double ThirtyDay2{ get; set; }
        public double SixtyDay{ get; set; }
        public double SixtyDay2{ get; set; }
        public double NintyDay{ get; set; }
        public double NintyOneDay{ get; set; }
        public double SixtyOneDay{ get; set; }
        public double WeekCount{ get; set; }
        public DateTime WeekDate{ get; set; }
        public string RolName{ get; set; }
        public int TransBatch{ get; set; }
        public DateTime TransfDate{ get; set; }
        public string TransfDesc{ get; set; }
        public double TransAmount{ get; set; }
        public int JobID{ get; set; }
        public string LocationName{ get; set; }
        public string State{ get; set; }
        public string TaxDesc{ get; set; }
        public string LineItemDesc{ get; set; }
        public string TransType{ get; set; }
        public int Acct{ get; set; }
        public string GLAcct{ get; set; }
        public int PJID{ get; set; }
        public double Original{ get; set; }
        public double Paid{ get; set; }
        public double CrPaid { get; set; }
        public string PJRef{ get; set; }
        public double PJItemAmount{ get; set; }
        public string JobType{ get; set; }
        public string Descp{ get; set; }
        public DateTime PJfDate{ get; set; }
        public DateTime Post{ get; set; }
        public string vendor{ get; set; }
        public int PJBatch{ get; set; }
        public double Total{ get; set; }
        public int VendorID{ get; set; }
        public int isIssueDate{ get; set; }

        public double Column1 { get; set; }
        
    }
}
