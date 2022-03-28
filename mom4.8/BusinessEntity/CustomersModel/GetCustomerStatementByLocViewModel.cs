using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetCustomerStatementByLocViewModel
    {
        public int Owner { get; set; }
        public int Loc { get; set; }
        public string LocID { get; set; }
        public string locname { get; set; }
        public string locAddress { get; set; }
        public string customerName { get; set; }
        public string custAddress { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Terr { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public double Total { get; set; }
        public double Selected { get; set; }
        public double ZeroDay { get; set; }
        public double ThirtyOneDay { get; set; }
        public double SixtyOneDay { get; set; }
        public double NintyOneDay { get; set; }
        public int IsExistsEmail { get; set; }
        public string Custom12 { get; set; }
        public string Custom13 { get; set; }
    }
}
