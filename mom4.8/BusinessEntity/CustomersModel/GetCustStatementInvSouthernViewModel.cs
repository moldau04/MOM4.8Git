using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCustStatementInvSouthernViewModel
    {
        public int Loc { get; set; }
        public DateTime fDate { get; set; }
        public string Type { get; set; }
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Balance { get; set; }
        public int Days { get; set; }
    }
}
