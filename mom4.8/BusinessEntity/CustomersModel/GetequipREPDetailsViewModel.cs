using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetequipREPDetailsViewModel
    {
        public int comp { get; set; }
        public string fwork { get; set; }
        public string Template { get; set; }
        public DateTime Lastdate { get; set; }
        public DateTime NextDateDue { get; set; }
        public int ticketID { get; set; }
        public string Code { get; set; }
        public string fDesc { get; set; }
        public string freq { get; set; }
        public string equip { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
        public string section { get; set; }
    }
}
