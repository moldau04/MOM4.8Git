using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAllCDViewModel
    {
        public string Company { get; set; }
        public int EN { get; set; }
        public Int64 RowNumber { get; set; }
        public int ID { get; set; }
        public int TransID { get; set; }
        public DateTime fDate { get; set; }
        public Int64 Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        public int Bank { get; set; }
        public string BankName { get; set; }
        public int Batch { get; set; }
        public Int16 Type { get; set; }
        public Int16 ACH { get; set; }
        public Int16 Status { get; set; }
        public string French { get; set; }
        public string Memo { get; set; }
        public string VoidR { get; set; }
        public int Sel { get; set; }
        public string TypeName { get; set; }
        public string StatusName { get; set; }
        public int TotalRow { get; set; }
    }
}
