using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAPGLRegViewModel
    {
        public int ID;
        public int Acct;
        public string GLAcct;
        public DateTime fDate;//datetime
        public string Ref;
        public int PO;
        public int ReceivePO;
        public string fDesc;
        public double Amount;
        public string VendorName;
    }
}
