using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetTemplateItemByIDViewModel
    {
        public int ID { get; set; }
        public int EquipT { get; set; }
        public int Elev { get; set; }
        public string fDesc { get; set; }
        public int Line { get; set; }
        public DateTime Lastdate { get; set; }
        public DateTime NextDateDue { get; set; }
        public int Frequency { get; set; }
        public string Code { get; set; }
        public string section { get; set; }
        public int PrimarySyncID { get; set; }
        public string Notes { get; set; }
        public Int16 LeadEquip { get; set; }
        public string name { get; set; }

    }
}
