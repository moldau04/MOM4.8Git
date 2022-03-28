using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class POModel
    {
        public int POID{ get; set; }
        public DateTime fDate{ get; set; }
        public string fDesc{ get; set; }
        public double Amount{ get; set; }
        public int Vendor{ get; set; }
        public Int16 Status{ get; set; }
        public DateTime Due{ get; set; }
        public string ShipVia{ get; set; }
        public Int16 Terms{ get; set; }
        public string FOB{ get; set; }
        public string ShipTo{ get; set; }
        public Int16 Approved{ get; set; }
        public string Custom1{ get; set; }
        public string Custom2{ get; set; }
        public string ApprovedBy{ get; set; }
        public int ReqBy{ get; set; }
        public string fBy{ get; set; }
        private DataSet _ds;
        private DataTable _POdt;
        private string _ConnConfig;
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
        public string POReasonCode{ get; set; }
        public string CourrierAcct{ get; set; }
        public string PORevision{ get; set; }
        public double Quan{ get; set; }
        public double SelectedQuan{ get; set; }
        public double BalanceQuan{ get; set; }
        public double ReceivedQuan{ get; set; }
        public bool IsClosed{ get; set; }
        public int jobID{ get; set; }
        public int ItemID{ get; set; }
        public String WarehouseID { get; set; }
        public int LocationID { get; set; }
        public int EN { get; set; }
        public int BatchID { get; set; }
        public int UserID { get; set; }

        public Int16 ApprovalStatus { get; set; }
        public DataTable PODt
        {
            get { return _POdt; }
            set { _POdt = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int RID{ get; set; }
        public string Ref{ get; set; }
        public string WB{ get; set; }
        public string Comments{ get; set; }
        public double Balance{ get; set; }
        public double Selected{ get; set; }
        public Int16 Line{ get; set; }
        public int ReceivePOId{ get; set; }
        public int IsReceiveIssued { get; set; }
        public String SearchValue { get; set; }

        public String SearchBy { get; set; }
        public String ReceiveStartDate { get; set; }
        public String ReceiveEndDate { get; set; }

        public ApprovePOStatus _ApprovePOStatus;
        public string MOMUSer { get; set; }
        public string RequestedBy { get; set; }

        public string Name { get; set; }

        public Int16 Days { get; set; }
        public Int32 PO { get; set; }
    }

}
