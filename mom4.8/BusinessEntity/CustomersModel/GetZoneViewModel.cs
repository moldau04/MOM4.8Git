using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetZoneViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Surcharge { get; set; }
        public double Bonus { get; set; }
        public int Count { get; set; }
        public string Remarks { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public double Price4 { get; set; }
        public double Price5 { get; set; }
        public double IDistance { get; set; }
        public double ODistance { get; set; }
        public Int16 Color { get; set; }
        public string fDesc { get; set; }
        public Int16 Tax { get; set; }
    }
}
