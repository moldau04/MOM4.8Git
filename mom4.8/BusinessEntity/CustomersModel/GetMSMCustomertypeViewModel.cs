using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetMSMCustomertypeViewModel
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public string Remarks { get; set; }
        public string QBCustomerTypeID { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
