using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAllVenderAjaxSearchModel
    {
        public Int64 RowNumber { get; set; }
        public int TotalRow { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public int ID { get; set; }
        public int Rol { get; set; }
        public string Name { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string Acct { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double Balance { get; set; }

    }
}
