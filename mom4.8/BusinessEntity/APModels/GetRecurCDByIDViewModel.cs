using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetRecurCDByIDViewModel
    {

        public int ID{ get; set; }
        public int Vendor{ get; set; }
        public string fDesc{ get; set; }
        public int Bank{ get; set; }
        public string Memo{ get; set; } 
        public DateTime fDate{ get; set; }
        public int Ref{ get; set; }
        public Int16 ACH{ get; set; }
        public string BankName{ get; set; }
        public int Sel{ get; set; }
        public int Batch{ get; set; }
        public int TransID{ get; set; }
        public bool IsRecon{ get; set; }
        public double Amount{ get; set; }
        public string VendorName{ get; set; }
        public string AcctNumber{ get; set; }
        public string Type{ get; set; }
        public int EN { get; set; }
        public string Company{ get; set; }
        public int PJID{ get; set; }
        public string BillRef { get; set; }
        public string BillfDesc{ get; set; }
        public double BillAmount{ get; set; }
        public double BillUseTax{ get; set; }
        public int Frequency{ get; set; }
        public int PaymentType { get; set; }
    }
}
