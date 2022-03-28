using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetHistoryTransactionViewModel
    {
        public int line { get; set; }
        public DateTime fDate { get; set; }
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string LinkTo { get; set; }
    }
}
