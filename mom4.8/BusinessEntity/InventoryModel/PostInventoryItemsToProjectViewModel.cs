using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class ListPostInventoryItemsToProject
    {
        public List<PostInventoryItemsToProjectTable> lstTable { get; set; }
        public List<PostInventoryItemsToProjectTable1> lstTable1 { get; set; }
        public List<PostInventoryItemsToProjectTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class PostInventoryItemsToProjectTable
    {
        public int Ticket { get; set; }
        public Int16 Line { get; set; }
        public int Item { get; set; }
        public double Quan { get; set; }
        public string fDesc { get; set; }
        public Int16 Charge { get; set; }
        public double Amount { get; set; }
        public string Phase { get; set; }
        public string AID { get; set; }
        public int TypeID { get; set; }
        public string WarehouseID { get; set; }
        public int LocationID { get; set; }
        public string PhaseName { get; set; }
    }

    [Serializable]
    public class PostInventoryItemsToProjectTable1
    {
        public int Column1 { get; set; }
    }

    [Serializable]
    public class PostInventoryItemsToProjectTable2
    {
        public string Column1 { get; set; }
    }
}
