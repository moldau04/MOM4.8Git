using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetTerritoryViewModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string SDesc { get; set; }
    }
}
