using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetElevContractViewModel
    {
        public int Elev { get; set; }
        public double Price { get; set; }
        public double hours { get; set; }
    }
}
