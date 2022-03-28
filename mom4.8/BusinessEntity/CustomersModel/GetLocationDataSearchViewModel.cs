using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationDataSearchViewModel
    {
        public int Loc { get; set; }
        public string locid { get; set; }
        public string Name { get; set; }
        public Int16 Status { get; set; }
        public Int16 Elevs { get; set; }
        public double Balance { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public Int16 credit { get; set; }
        public int opencall { get; set; }
        public string qblocid { get; set; }
        public string Customer { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public bool NoCustomerStatement { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string Salesperson { get; set; }
        public string Salesperson2 { get; set; }
        public string RouteName { get; set; }
        public string ConsultantName { get; set; }
        public string OwnerName { get; set; }
        public string SageID { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string Zone { get; set; }
        public int BusinessType { get; set; }
        public string BusinessTypeName { get; set; }
        public int CusID { get; set; }
        public string locStatus { get; set; }

    }
}
