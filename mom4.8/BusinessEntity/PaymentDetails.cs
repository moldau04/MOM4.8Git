using System;
using System.Data;

namespace BusinessEntity
{
    public class PaymentDetails
    {
        public int ID;
        public int ReceivedPaymentID;
        public int TransID;
        public int Loc;
        public int Rol;
        public int InvoiceID;
        public int Owner;
        
        private DataSet _ds;
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
        public string strInvoiceId;
    }

    public class WriteOff
    {
        public string ConnConfig { get; set; }       
        public int ID { get; set; }
        public int Acct { get; set; }
        public string Desc { get; set; }
        public DateTime fDate { get; set; }
        public string CreateBy { get; set; }
        public int TransID { get; set; }
        public String ListInvoice { get; set; }
        public string WriteoffDesc { get; set; }
        public string CheckNo { get; set; }
        public double WriteOffAmount { get; set; }
    }

    public class GetInvoiceNosChangeParam
    {
        public string strInvoiceId { get; set; }
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetInvoicesByReceivedPayParam
    {
        public int ReceivedPaymentID { get; set; }
        public int Rol { get; set; }
        public int Loc { get; set; }
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class writeOffInvoiceMultiParam
    {
        public string ConnConfig { get; set; }
        public int Acct { get; set; }
        public String ListInvoice { get; set; }
        public string Desc { get; set; }
        public DateTime fDate { get; set; }
        public string CreateBy { get; set; }
        public string WriteoffDesc { get; set; }
        public string CheckNo { get; set; }
        public double WriteOffAmount { get; set; }
    }

    public class writeOffInvoiceParam
    {
        public int ID { get; set; }
        public int Acct { get; set; }
        public string Desc { get; set; }
        public DateTime fDate { get; set; }
        public string CreateBy { get; set; }
        public string ConnConfig { get; set; }
        public string WriteoffDesc { get; set; }
        public string CheckNo { get; set; }

    }
}
