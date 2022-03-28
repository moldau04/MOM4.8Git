using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetEmailDetailByLocViewModel
    {
        public int Loc { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public string custom12 { get; set; }
        public string custom13 { get; set; }
    }
}
