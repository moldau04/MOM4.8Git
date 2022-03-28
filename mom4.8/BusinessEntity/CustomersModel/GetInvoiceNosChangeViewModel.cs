using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoiceNosChangeViewModel
    {
        public int Ref { get; set; }
        public int Loc { get; set; }
        public int Owner { get; set; }
        public string OwnerName { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public Int16 Status { get; set; }
    }
}
