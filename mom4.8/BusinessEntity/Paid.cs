using System;
using System.Data;

namespace BusinessEntity
{
    public class Paid
    {
        public int PITR;
        public DateTime fDate;
        public Int16 Type;
        public Int16 Line;
        public string fDesc;
        public double Original;
        public double Balance;
        public double Disc;
        public double Paid1;
        public int TRID;
        public string Ref;
        public string ConnConfig;

        private DataSet _ds;
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

    }

    public class GetPaidDetailByIDParam
    {
        public int PITR;
        public string ConnConfig;
    }

    public class UpdatePaidOnVoidCheckParam
    {
        public int PITR;
        public string ConnConfig;
        public int TRID;
    }
    public class GetRecurrBillDetailByIDParam
    {
        public int PITR;
        public string ConnConfig;
    }

}
