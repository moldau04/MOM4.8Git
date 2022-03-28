using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessEntity
{
    public class ReceivedPayment
    {
        public int ID;
        public int Loc;
       
        public double Amount;
        public DateTime PaymentReceivedDate;
        public Int16 PaymentMethod;
        public string CheckNumber;
        public double AmountDue;
        public string fDesc;
        public int DepID;
        public int Status;
        private DataSet _ds;
        private DataSet _dsID;
        private DataTable _dtPay;
        private string _ConnConfig;
        public int Rol;
        public DateTime StartDate;
        public DateTime EndDate;
        public int UserID { get; set; }
        public string MOMUSer { get; set; }
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
            get { return _dsID; }
            set { _dsID = value; }
        }
        public DataTable DtPay
        {
            get { return _dtPay; }
            set { _dtPay = value; }
        }
        public String page { get; set; }

    }

    public class DeletePaymentParam
    {
        public int ID; 
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAllReceivePaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RetainFilter> filters { get; set; }
        public int intEN { get; set; }
    }

    public class UpdateReceivePaymentParam
    {
        private string _ConnConfig;
        private DataTable _dtPay;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
        public int Loc { get; set; }
        public DataTable DtPay
        {
            get { return _dtPay; }
            set { _dtPay = value; }
        }
        public double Amount { get; set; }
        public string CheckNumber { get; set; }
        public double AmountDue { get; set; }
        public string fDesc { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public Int16 PaymentMethod { get; set; }
        public string MOMUSer { get; set; }
    }

    public class AddReceivePaymentParam
    {
        private string _ConnConfig;
        private DataTable _dtPay;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Rol { get; set; }
        public int Loc { get; set; }
        public DataTable DtPay
        {
            get { return _dtPay; }
            set { _dtPay = value; }
        }
        public double Amount { get; set; }
        public string CheckNumber { get; set; }
        public double AmountDue { get; set; }
        public string fDesc { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public Int16 PaymentMethod { get; set; }
        public string MOMUSer { get; set; }
    }

    public class GetReceivePaymentByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }

    }

    public class GetReceivePaymentLogsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
    }

    public class GetCustomerUnAppliedCreditParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int userId { get; set; }
        public int filter { get; set; }

    }

    public class TransferPaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string strRef { get; set; }
        public int newLoc { get; set; }
    }

    public class UnapplyPaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }
    }

    public class AddBatchReceivePaymentParam
    {
        private string _ConnConfig;
        private DataTable _dtPay;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable DtPay
        {
            get { return _dtPay; }
            set { _dtPay = value; }
        }
        public DateTime PaymentReceivedDate { get; set; }
        public Int16 PaymentMethod { get; set; }
        public string MOMUSer { get; set; }
        public Boolean createDeposit { get; set; }
        public int bank { get; set; }
    }

    public class GetReceivedPaymentByDepParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int DepID { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class GetInvoiceByListParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string invoiceId { get; set; }
        public String checkNumber { get; set; }
        public Boolean isSeparate { get; set; }
    }

    public class GetInvoicesByReceivedPayMultiParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int owner { get; set; }
        public string loc { get; set; }
        public string invoice { get; set; }
    }

    public class GetReceivePaymentReportParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetAllReceivePaymentForDepParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime PaymentReceivedDate { get; set; }
    }

    public class UpdateReceivedPayStatusParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Status { get; set; }
        public int ID { get; set; }
    }
}
