using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class JobIViewModel
    {
        public int Job{ get; set; }
        public Int16 Phase{ get; set; }
        public DateTime fDate{ get; set; }
        public string Ref{ get; set; }
        public string fDesc{ get; set; }
        public double Amount{ get; set; }
        public int TransID{ get; set; }
        public Int16 Type{ get; set; }
        public Int16 Labor{ get; set; }
        public int Billed{ get; set; }
        public int Invoice{ get; set; }
        public int UseTax{ get; set; }
        public int vAPTicket{ get; set; }
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
        public int APTicket{ get; set; }
    }
}
