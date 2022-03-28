using System.Data;

namespace BusinessEntity
{
    public class TransBankAdj
    {
        public int ID;
        public int Batch;
        public int Bank;
        public bool IsRecon;
        public double Amount;
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
    }
}
