using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetRouteActiveViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Mech { get; set; }
        public int Loc { get; set; }
        public int Elev { get; set; }
        public double Hour { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public Int16 Symbol { get; set; }
        public int EN { get; set; }
        public string Color { get; set; }
        public bool Status { get; set; }
        public string MechName { get; set; }
    }
}
