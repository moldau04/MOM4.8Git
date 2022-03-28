using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetGCCustomerViewModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string EMail { get; set; }
        public string Country { get; set; }
        public string remarks { get; set; }
        public string Cellular { get; set; }
        public int rol { get; set; }
        public string type { get; set; }
    }
}
