using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetSingleConsultantViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Rol { get; set; }
        public int Count { get; set; }
        public Int16 API { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
    }
}
