using System;
using System.Data;

namespace BusinessEntity
{
    public class OpenAP
    {
        public int Vendor;
        public DateTime fDate;
        public DateTime Due;
        public Int16 Type;
        public string fDesc;
        public double Original;
        public double Balance;
        public double Selected;
        public double Disc;
        public int PJID;
        public int TRID;
        public string Ref;
        public DateTime SearchDate;
        public Int16 SearchValue;

        private DataSet _ds;
        private string _ConnConfig;

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
        public int IsSelected;
        public string Company;
    }

    public class GetOpenAPByPJIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int PJID;
    }

    public class AddOpenAPParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int PJID;
        public int Vendor;
        public DateTime fDate;
        public DateTime Due;
        public Int16 Type;
        public string fDesc;
        public double Original;
        public double Balance;
        public double Selected;
        public double Disc;
        public int TRID;
        public string Ref;
    }

    public class UpdateOpenAPBalanceParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int PJID;
        public double Balance;
        public double Selected;
        public int IsSelected;
    }

    public class GetBillsByVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Vendor;
        public Int16 SearchValue;
        public DateTime SearchDate;
        public string Company;
    }

    public class UpdateWriteCheckOpenAPpaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int PJID;
        public double Balance;
        public string Ref;
        public double Disc;
        public int IsSelected;
    }
    public class GetPurchaseJournalParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime Due;
    }
}
