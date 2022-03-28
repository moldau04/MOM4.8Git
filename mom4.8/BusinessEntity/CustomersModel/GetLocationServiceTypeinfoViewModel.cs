using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationServiceTypeinfoViewModel
    {
        public string ServiceTypeName { get; set; }
        public int ServiceTypeCount { get; set; }
        public int ProjectPerDepartmentCount { get; set; }
        public string ProjectaregoingtoUpdate { get; set; }
    }
}
