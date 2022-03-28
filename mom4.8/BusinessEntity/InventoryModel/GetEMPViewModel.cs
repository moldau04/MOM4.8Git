using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetEMPViewModel
    {
        public string fDesc { get; set; }
        public int id { get; set; }
        public Int16 Status { get; set; }
    }
}
