using System;
using System.Data;

namespace BusinessEntity
{
    public class Dep
    {
        public int Ref;
        public DateTime fDate;
        public int Bank;
        public string fDesc;
        public double Amount;
        public int TransID;
        public int EN;
        public int ReceivedPaymentID;
        public bool IsRecon;
        public int fDateYear;
        public DateTime StartDate;
        public DateTime EndDate;
        public int UserID { get; set; }
        private DataSet _ds;
        private DataSet _dsId;
        private string _ConnConfig;
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
        public DataSet DsID
        {
            get { return _dsId; }
            set { _dsId = value; }
        }
        public DataTable _dtReceipt;
        public DataTable _dtGlAccount;
        public Boolean isDeleteReceive;
    }

    public class GetDepByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }
    }

    public class GetDepHeadByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }

    }

    public class GetAllDepositsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EN { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserID { get; set; }
    }

    public class DeleteDepositParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }
        public Boolean isDeleteReceive { get; set; }
    }

    public class UpdateDepositParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int depId { get; set; }
        public DataTable dtDelete { get; set; }
        public DataTable dtNew { get; set; }
        public DataTable dtDeleteGL { get; set; }
        public DataTable dtNewGL { get; set; }
        public String UpdatedBy { get; set; }
    }

    public class DepositInfor_UpdateDepositParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }
        public DateTime fDate { get; set; }
        public int Bank { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
    }


    public class GetAllInvoiceByDepParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int depId { get; set; }
    }

    public class AddDepositWithGLParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime fDate{ get; set; }
        public int Bank{ get; set; }
        public DataTable _dtReceipt{ get; set; }
        public DataTable _dtGlAccount{ get; set; }
        public string fDesc{ get; set; }
        public double Amount{ get; set; } 
        public int Ref{ get; set; }
    }

    public class GetDepositListByDateParam
    {
        private string _ConnConfig;
        private DataSet _ds;
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool incZeroAmount { get; set; }
    }
}
