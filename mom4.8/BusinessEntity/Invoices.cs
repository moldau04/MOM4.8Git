using System;
using System.Data;

namespace BusinessEntity
{
    public class Invoices
    {
        public int Ref;
        public DateTime fDate;
        public string fDesc;
        public double Amount;
        public double STax;
        public double Total;
        public string TaxRegion;
        public double TaxRate;
        public double TaxFactor;
        public double Taxable;
        public Int16 Type;
        public int Job;
        public int Loc;
        public Int16 Terms;
        public string PO;
        public Int16 Status;
        public int Batch;
        public string Remarks;
        public int TransID;
        public double GTax;
        public int Mech;
        public Int16 Pricing;
        public string TaxRegion2;
        public double TaxRate2;
        public Int16 BillToOpt;
        public string BillTo;
        public string Custom1;
        public string Custom2;
        public DateTime IDate;
        public string fUser;
        public string Custom3;
        public string QBInvoiceID;
        public DateTime LastUpdateDate;
        private DataSet _ds;
        private string _ConnConfig;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        private DataSet _dsID;
    }

    public class GetUTaxLocReportParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
