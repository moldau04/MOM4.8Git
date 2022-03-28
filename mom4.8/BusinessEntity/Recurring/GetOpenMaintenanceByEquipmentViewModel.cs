using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetOpenMaintenanceByEquipmentViewModel
    {
        public int ID { get; set; }
        public DateTime CDate { get; set; }
        public DateTime EDate { get; set; }
        public int fWork { get; set; }
        public int Job { get; set; }
        public int Loc { get; set; }
        public int Elev { get; set; }
        public Int16 Type { get; set; }
        public string fDesc { get; set; }
        public string Cat { get; set; }
        public string DWork { get; set; }
        public string CustomerName { get; set; }
        public string LocName { get; set; }
        public string LocAddress { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentDesc { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentCat { get; set; }
        public string EquipmentUnique { get; set; }
        public string TicketFreq { get; set; }
    }
}
