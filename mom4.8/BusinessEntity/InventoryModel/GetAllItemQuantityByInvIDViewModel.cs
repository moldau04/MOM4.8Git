using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetAllItemQuantityByInvIDViewModel
    {
        public double Hand { get; set; }
        public double fOrder { get; set; }
        public double Committed { get; set; }
        public double Available { get; set; }
        public double IssuesToOpenJobs { get; set; }
        public double OOValue { get; set; }
        public double OHValue { get; set; }
        public double CommittedValue { get; set; }
        public double UnitCost { get; set; }
    }
}
