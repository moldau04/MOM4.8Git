using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class PaidViewModel
    {
        public int PITR{ get; set; }
        public DateTime fDate{ get; set; }
        public Int16 Type{ get; set; }
        public Int16 Line{ get; set; }
        public string fDesc{ get; set; }
        public double Original{ get; set; }
        public double Balance{ get; set; }
        public double Disc{ get; set; }
        public double Paid1{ get; set; }
        public int TRID{ get; set; }
        public string Ref{ get; set; }
        public string ConnConfig{ get; set; }

        private DataSet _ds;
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public int PJID{ get; set; }
        public int Batch{ get; set; }
        public double TBalance{ get; set; }

    }
}
