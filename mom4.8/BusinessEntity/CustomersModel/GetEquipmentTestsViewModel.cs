using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEquipmentTestsViewModel
    {
        public int idUnit { get; set; }
        public int idTestItem { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Last { get; set; }
        public DateTime Next { get; set; }
        public int Ticketed { get; set; }
        public int Ticket { get; set; }
    }
}
