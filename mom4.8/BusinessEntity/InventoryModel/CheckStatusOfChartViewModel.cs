using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class CheckStatusOfChartViewModel
    {
        public int ID { get; set; }
        public string fDesc { get; set; }
        public int status { get; set; }
    }
}
