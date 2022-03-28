using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class BillReportDetails
    {
        public string PostingDate { get; set; }
        public string InvoiceDate { get; set; }
        public string  Ref { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public double UseTax { get; set; }
        public string VendorName { get; set; }
    }
}
