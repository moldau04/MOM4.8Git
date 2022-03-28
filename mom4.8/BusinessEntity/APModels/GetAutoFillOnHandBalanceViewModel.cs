using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAutoFillOnHandBalanceViewModel
    {
        public int ID { get; set; }
        public Decimal Hand { get; set; }
        public Decimal Balance { get; set; }
    }
}
