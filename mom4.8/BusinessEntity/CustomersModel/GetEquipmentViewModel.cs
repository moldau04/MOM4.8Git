using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEquipmentViewModel
    {
        public string state { get; set; }
        public string cat { get; set; }
        public string category { get; set; }
        public string Classification { get; set; }
        public string manuf { get; set; }
        public double price { get; set; }
        public DateTime last { get; set; }
        public DateTime since { get; set; }
        public DateTime Install { get; set; }
        public int id { get; set; }
        public string unit { get; set; }
        public string type { get; set; }
        public string fdesc { get; set; }
        public string Status { get; set; }
        public string shut_down { get; set; }
        public string ShutdownReason { get; set; }
        public string building { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string name { get; set; }
        public string locid { get; set; }
        public string tag { get; set; }
        public string address { get; set; }
        public int Loc { get; set; }
        public int unitid { get; set; }
    }
}
