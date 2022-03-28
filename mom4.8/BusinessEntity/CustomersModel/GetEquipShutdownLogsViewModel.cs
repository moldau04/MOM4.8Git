using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEquipShutdownLogsViewModel
    {
        public int id { get; set; }
        public string ticket_id { get; set; }
        public string status { get; set; }
        public int elev_id { get; set; }
        public DateTime created_on { get; set; }
        public string worker { get; set; }
        public string reason { get; set; }
        public string longdesc { get; set; }
    }
}
