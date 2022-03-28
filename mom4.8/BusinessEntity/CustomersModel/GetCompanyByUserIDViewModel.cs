using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCompanyByUserIDViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int OfficeID { get; set; }
        public bool IsSel { get; set; }
    }
}
