using System;
using System.Data;

namespace BusinessEntity
{
    public class PO
    {
        public int POID;
        public DateTime fDate;
        public string fDesc;
        public double Amount;
        public int Vendor;
        public Int16 Status;
        public DateTime Due;
        public string ShipVia;
        public Int16 Terms;
        public string FOB;
        public string ShipTo;
        public Int16 Approved;
        public string Custom1;
        public string Custom2;
        public string ApprovedBy;
        public int ReqBy;
        public string fBy;
        private DataSet _ds;
        private DataTable _POdt;
        private string _ConnConfig;
        public DateTime StartDate;
        public DateTime EndDate;
        public string SalesOrderNo;
        public string POReasonCode;
        public string CourrierAcct;
        public string PORevision;
        public double Quan;
        public double SelectedQuan;
        public double BalanceQuan;
        public double ReceivedQuan;
        public bool IsClosed;
        public int jobID;
        public int ItemID;
        public bool IsPOClose;
        public bool IsAddReceivePO;
        public DataTable _dt;
        public String WarehouseID { get; set; }
        public int LocationID { get; set; }
        public int EN { get; set; }
        public int BatchID { get; set; }
        public int UserID { get; set; }

        public Int16 ApprovalStatus { get; set; }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
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
        public int RID;
        public string Ref;
        public string WB;
        public string Comments;
        public double Balance;
        public double Selected;
        public Int16 Line;
        public int ReceivePOId;
        public int IsReceiveIssued { get; set; }
        public String SearchValue { get; set; }

        public String SearchBy { get; set; }
        public String ReceiveStartDate { get; set; }
        public String ReceiveEndDate { get; set; }

        public ApprovePOStatus _ApprovePOStatus;
        public string MOMUSer { get; set; }
        public string RequestedBy { get; set; }
    }

    public class issueDrpValue
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ApprovePOStatus
    {
        public int ID { get; set; }
        public int POID { get; set; }
        public int? UserID { get; set; }
        public Int16? Status { get; set; }
        public string Comments { get; set; }
        public byte[] Signature { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ConnConfig { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime? ApproveFrom { get; set; }
        public DateTime? ApproveTo { get; set; }
        public string POIDs { get; set; }

        
    }

    public enum POStatus
    {
        /// <summary>
        /// WHEN 0 THEN 'Open'  
        /// </summary>
        Open = 0,
        /// <summary>
        /// WHEN 1 THEN 'Closed'
        /// </summary>
        Closed = 1,
        /// <summary>
        /// WHEN 2 THEN 'Void'
        /// </summary>
        Void = 2,
        /// <summary>
        /// WHEN 3 THEN 'Partial-Quantity'
        /// </summary>
        PartialQuantity = 3,
        /// <summary>
        /// WHEN 4 THEN 'Partial-Amount' 
        /// </summary>
        PartialAmount = 4,
        /// <summary>
        /// WHEN 5 THEN 'Closed At Receive PO'
        /// </summary>
        ClosedAtReceivePO = 5
    }

    public class GetOutStandingPOByIdParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID { get; set; }
    }

    public class GetReceivePOListParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;
        public int Vendor;
        public int EN { get; set; }

    }

    public class AddReceivePOItemParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int POID;
        public int Vendor;
        public double SelectedQuan;
        public double BalanceQuan;
        public double Amount;
        public Int16 Line;
        public int ReceivePOId;
        public double Quan;
        public int IsReceiveIssued { get; set; }
        public double Balance;
        public double Selected;
    }

    public class UpdatePOItemBalanceParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;
        public Int16 Line;
        public double Selected;
        public double Balance;

    }
    public class UpdatePOItemQuanParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;
        public double SelectedQuan;
        public double BalanceQuan;
        public Int16 Line;

    }
    public class AddEditReceivePOParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;
        public Int16 Line;
        public int ReceivePOId;
        public double Quan;
        public double Amount;
        public int RID;
        public string Ref;
        public string WB;
        public string Comments;
        public DateTime fDate;
        public DateTime Due;
        public int BatchID { get; set; }
        public string MOMUSer { get; set; }
        public int IsReceiveIssued { get; set; }

    }
    public class updateJobCommParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int jobID;
    }

    public class GetPOReceivePOByIdParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int RID;
    }

    public class UpdatePOStatusParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;

        public Int32 Status;
    }

    public class UpdateReceivePOStatusByPOIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;

        public Int32 Status;
        public int RID;
    }

    public class UpdateReceivePOStatusParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;

        public Int32 Status;
        public int RID;
    }

    public class IsExistPOParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;

        public Int32 Status;
        public int RID;
    }

    public class GetClosePOCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;

        public Int32 Status;
        public int RID;
    }

    public class GetMaxReceivePOIdParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        //public int POID;

        //public Int32 Status;
        //public int RID;
    }

    public class UpdatePOItemWarehouseLocationParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int POID;
        public String WarehouseID { get; set; }
        public int LocationID { get; set; }
        public Int16 Line;
        public Int32 Status;
        public int RID;
    }


    public class GetPOWeeklyParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DateTime StartDate;
        public DateTime EndDate;
    }
}
