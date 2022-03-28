using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class InventoryTransactionByInvIDViewModel
    {
        public int Ref { get; set; }
        public string URLref { get; set; }
        public DateTime fDate { get; set; }
        public string TType { get; set; }
        public string MDesc { get; set; }
        public int INVID { get; set; }
        public string Quan { get; set; }
        public double Amount { get; set; }
        public double Charges { get; set; }
        public double Credits { get; set; }
        public double Balance { get; set; }
    }
}
