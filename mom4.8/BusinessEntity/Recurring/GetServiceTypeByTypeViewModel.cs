using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetServiceTypeByTypeViewModel
    {
        public string type { get; set; }
        public string fdesc { get; set; }
        public string remarks { get; set; }
        public int Count { get; set; }
        public int InvID { get; set; }
        public string Sacct { get; set; }
        public string GLAcct { get; set; }
    }
}
