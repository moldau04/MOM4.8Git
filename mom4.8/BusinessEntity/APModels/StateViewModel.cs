using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class StateViewModel
    {
        public string Name { get; set; }
        public string fDesc { get; set; }
        public string Country { get; set; }
        private DataSet _dsState;
        public string ConnConfig;
        public DataSet DsState
        {
            get { return _dsState; }
            set { _dsState = value; }
        }
    }
}
