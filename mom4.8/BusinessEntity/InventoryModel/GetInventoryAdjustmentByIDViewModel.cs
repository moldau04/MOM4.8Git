using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetInventoryAdjustmentByIDViewModel
    {
        public int AdjID { get; set; }
        public double Quantity { get; set; }
        public DateTime fDate { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public int TransID { get; set; }
        public int Batch { get; set; }
        public int Acct { get; set; }
        public string WarehouseID { get; set; }
        public int LocationID { get; set; }
        public int ChartID { get; set; }
        public string Chart { get; set; }
        public double Hand { get; set; }
        public double Balance { get; set; }
        public int InvID { get; set; }
        public string ItemName { get; set; }
        public string InvDesc { get; set; }
        public string WarehouseName { get; set; }
        public string LocationName { get; set; }
        public string Company { get; set; }
        public int EN { get; set; }
    }
}
