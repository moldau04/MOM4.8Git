using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCustomersLogsViewModel
    {
        public string fUser { get; set; }
        public string Screen { get; set; }
        public int Ref { get; set; }
        public string Field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime CreatedStamp { get; set; }
        public DateTime fDate { get; set; }
        public DateTime fTime { get; set; }
    }
}
