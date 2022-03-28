using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetElevByTicketIDsViewModel
    {
        public int TicketID { get; set; }
        public int elev_id { get; set; }
        public bool labor_percentage { get; set; }
        public string Unit { get; set; }
        public string Serial { get; set; }
        public string State { get; set; }
        public int Owner { get; set; }
    }
}
