using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEquipmentShutdownForReportViewModel
    {
        public int id { get; set; }
        public string Location { get; set; }
        public string Equipment { get; set; }
        public int Ticket { get; set; }
        public DateTime Date { get; set; }
        public string Mechanic { get; set; }
        public string Planned { get; set; }
        public string reason { get; set; }
        public string longdesc { get; set; }
        public string Status { get; set; }
        public string Supervisor { get; set; }
        public string WorkCompleted { get; set; }
        public string Worker { get; set; }
        public int Row { get; set; }
    }
}
