using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetARAgingByLocTypeViewModel
    {
        public string LocName { get; set; }
        public double Balance { get; set; }
        public double CurrentDay { get; set; }
        public double ThirtyDay { get; set; }
        public double SixtyDay { get; set; }
        public double NintyDay { get; set; }
        public double OverNintyDay { get; set; }
        public double OneTwentyDay { get; set; }
        public string LocType { get; set; }
    }
}
