using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetBuildingElevViewModel
    {
        public string EDesc { get; set; }
        public string Label { get; set; }
        public int Count { get; set; }
    }
}
