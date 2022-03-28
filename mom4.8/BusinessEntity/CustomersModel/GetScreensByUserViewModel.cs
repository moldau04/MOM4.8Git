using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetScreensByUserViewModel
    {
        public bool access { get; set; }
        public bool edit { get; set; }
        public bool VIEW { get; set; }
        public bool add { get; set; }
        public bool DELETE { get; set; }
    }
}
