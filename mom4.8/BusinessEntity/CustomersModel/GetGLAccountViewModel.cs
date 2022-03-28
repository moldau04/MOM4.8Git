using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetGLAccountViewModel
    {
        public string Acct { get; set; }
        public string fDesc { get; set; }
        public Int16 Type { get; set; }
        public int ID { get; set; }
        public string CAlias { get; set; }
        public string Sub { get; set; }
    }
}
