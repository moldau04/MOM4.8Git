using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetElevByTicketViewModel
    {
        public int ticketid { get; set; }
        public string unit { get; set; }
        public int elev_id { get; set; }
        public bool labor_percentage { get; set; }
        public string serial { get; set; }
        public string state { get; set; }
        public int owner { get; set; }
    }
}
