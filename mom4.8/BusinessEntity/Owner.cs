using System.Data;

namespace BusinessEntity
{
    public class Owner
    {
        public int ID;
        public int Locs;
        public double Balance;
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
    }
}
