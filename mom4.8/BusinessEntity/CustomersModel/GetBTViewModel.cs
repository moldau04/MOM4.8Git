using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetBTViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public int Count { get; set; }
    }
}
