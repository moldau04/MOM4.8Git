using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetAlertTypeViewModel
    {
        public int ID { get; set; }
        public string AlertName { get; set; }
        public string Code { get; set; }
    }
}
