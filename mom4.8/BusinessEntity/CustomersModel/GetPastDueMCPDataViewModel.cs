using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetPastDueMCPDataViewModel
    {
        public string LocationName { get; set; }
        public string Unit { get; set; }
        public string DefaultWorker { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public int EquipT { get; set; }
        public string Code { get; set; }
        public string Section { get; set; }
        public string fDesc { get; set; }
        public DateTime Lastdate { get; set; }
        public DateTime NextDateDue { get; set; }
        public int Frequency { get; set; }
        public string FrequencyName { get; set; }
    }
}
