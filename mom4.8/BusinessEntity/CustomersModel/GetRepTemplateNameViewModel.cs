using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetRepTemplateNameViewModel
    {
        public int ID { get; set; }
        public string fdesc { get; set; }
        public int CBcheckStatus { get; set; }
    }
}
