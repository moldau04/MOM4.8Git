using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCustomTemplateViewModel
    {
        public int ID { get; set; }
        public string fDesc { get; set; }
        public int Count { get; set; }
        public string Remarks { get; set; }
        public int PrimarySyncID { get; set; }
    }
}
