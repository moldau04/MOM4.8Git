using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocContactByRolIDViewModel
    {
        public int contactid { get; set; }
        public string name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public bool EmailTicket { get; set; }
        public bool EmailRecInvoice { get; set; }
        public bool ShutdownAlert { get; set; }
        public bool EmailRecTestProp { get; set; }
    }
}
