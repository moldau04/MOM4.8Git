using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCustomersViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string fLogin { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public double Balance { get; set; }
        public string type { get; set; }
        public string city { get; set; }
        public string State { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public string cellular { get; set; }
        public int loc { get; set; }
        public int equip { get; set; }
        public int opencall { get; set; }
        public string sageid { get; set; }
        public string qbcustomerid { get; set; }
    }
}
