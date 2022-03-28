using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationTypeViewModel
    {
        public string Type { get; set; }
        public string Remarks { get; set; }
        public int Count { get; set; }
    }
}
