using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationByCustomerIDViewModel
    {
        public string locid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Int16 Status { get; set; }
        public int Elevs { get; set; }
        public double Balance { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int loc { get; set; }
        public int roleid { get; set; }
    }
}
