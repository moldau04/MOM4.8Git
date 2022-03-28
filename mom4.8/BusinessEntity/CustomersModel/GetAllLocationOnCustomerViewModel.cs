using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetAllLocationOnCustomerViewModel
    {
        public int Loc { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
    }
}
