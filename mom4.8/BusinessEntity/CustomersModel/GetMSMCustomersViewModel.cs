using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetMSMCustomersViewModel
    {
        public int ID { get; set; }
        public string QBCustomerID { get; set; }
        public string Address { get; set; }
        public string Cellular { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string Country { get; set; }
        public string EMail { get; set; }
        public string Fax { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Remarks { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Int16 Status { get; set; }
        public double balance { get; set; }
        public string QBCustomertypeID { get; set; }
    }
}
