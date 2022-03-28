using System.Data;

namespace BusinessEntity
{
    public class State
    {
        public string Name;
        public string fDesc;
        public string Country;
        private DataSet _dsState;
        public string ConnConfig;
        public string geoCode;          //geoCode for vertex payroll
        public DataSet DsState
        {
            get { return _dsState; }
            set { _dsState = value; }
        }
    }

    public class GetStatesParam
    {
        public string ConnConfig;
    }
}
