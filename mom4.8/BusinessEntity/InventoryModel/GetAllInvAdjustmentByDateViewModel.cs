using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetAllInvAdjustmentByDateViewModel
    {
         public int ID { get; set; }
         public double Quan { get; set; }
         public DateTime fDate { get; set; }
         public string fDesc { get; set; }
         public double Amount { get; set; }
         public string Name { get; set; }
         public string Itemsfdesc { get; set; }
         public string WarehouseID { get; set; }
         public string Company { get; set; }
         public int EN { get; set; }
         public string WHName { get; set; }
         public string WHLoc { get; set; }
    }
}
