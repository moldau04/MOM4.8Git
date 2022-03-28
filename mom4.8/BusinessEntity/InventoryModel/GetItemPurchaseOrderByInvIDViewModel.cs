using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetItemPurchaseOrderByInvIDViewModel
    {
        public DateTime LastPurchaseDate { get; set; }
        public DateTime NextPODate { get; set; }
        public string VendorName { get; set; }
        public double LastPurchasePrice { get; set; }
        public DateTime LastReceiptDate { get; set; }

    }
}
