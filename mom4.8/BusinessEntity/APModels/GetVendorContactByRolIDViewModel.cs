using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetVendorContactByRolIDViewModel
    {
        public Int32 contactid { get; set; }
        //public Int32 Rol { get; set; }
        public string name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Title { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public bool EmailRecPO { get; set; }
    }
}
