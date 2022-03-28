using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetInventoryActiveWarehouseViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Location { get; set; }
        public string Remarks { get; set; }
        public int Count { get; set; }
        public bool Multi { get; set; }
        public int En { get; set; }
        public string TypeName { get; set; }
        public string Company { get; set; }
        public int status { get; set; }
    }
}
