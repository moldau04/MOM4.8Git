using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetActiveServiceTypeViewModel
    {
        public int RT { get; set; }
        public int OT { get; set; }
        public int NT { get; set; }
        public int DT { get; set; }
        public string type { get; set; }
        public string fdesc { get; set; }
        public string remarks { get; set; }
        public int Count { get; set; }
        public int InvID { get; set; }
        public string Name { get; set; }
        public Int16 Status { get; set; }
        public string StatusLabel { get; set; }
    }
}
