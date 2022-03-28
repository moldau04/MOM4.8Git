using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCategoryViewModel
    {
        public string type { get; set; }
        public bool Status { get; set; }
    }
}
