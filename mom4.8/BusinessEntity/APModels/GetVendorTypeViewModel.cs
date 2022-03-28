using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetVendorTypeViewModel
    {
        public string Type { get; set; }
        public string Remarks { get; set; }
        public Int64 Count { get; set; }
    }
}
