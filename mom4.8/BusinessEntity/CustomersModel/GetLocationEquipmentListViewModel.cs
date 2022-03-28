using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationEquipmentListViewModel
    {
        public string ID { get; set; }
        public int Loc { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string RolName { get; set; }
        public string RolAddress { get; set; }
        public string RolCity { get; set; }
        public string RolState { get; set; }
        public string RolZip { get; set; }
        public string Contact { get; set; }
        public string Unit { get; set; }
    }
}
