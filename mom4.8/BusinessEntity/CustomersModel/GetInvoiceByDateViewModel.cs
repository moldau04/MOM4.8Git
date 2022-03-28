using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoiceByDateViewModel
    {
        public DateTime InvDate { get; set; }
        public int Ref { get; set; }
        public string Description { get; set; }
        public double Balance { get; set; }
        public Int16 Dep { get; set; }
        public string Customer { get; set; }
        public string Tag { get; set; }
        public string Location { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public int Age { get; set; }
        public DateTime DueDate { get; set; }
        public int Retainage { get; set; }
        public int CurrentDay { get; set; }
        public int ThirtyDay { get; set; }
        public int SixtyDay { get; set; }
        public int NintyDay { get; set; }
        public int OverNintyDay { get; set; }
    }

}
