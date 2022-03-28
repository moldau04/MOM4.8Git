using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListAutoSelectPayment
    {
        public List<AutoSelectPaymentTable1> lstTable1 { get; set; }
        public List<AutoSelectPaymentTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class AutoSelectPaymentTable1
    {
        //Table 1
        public string Name { get; set; }
        public int Vendor { get; set; }
        public DateTime fDate { get; set; }
        public DateTime Due { get; set; }
        public Int16 Type { get; set; }
        public string fDesc { get; set; }
        public double Original { get; set; }
        public double Balance { get; set; }
        public double Selected { get; set; }
        public double Disc { get; set; }
        public int PJID { get; set; }
        public int TRID { get; set; }
        public double Discount { get; set; }
        public string Ref { get; set; }
        public Int16 Status { get; set; }
        public Int16 Spec { get; set; }
        public string StatusName { get; set; }
        public double Payment { get; set; }
        public string billDesc { get; set; }
        public bool IsSelected { get; set; }
        //public double Duepayment { get; set; }

    }

    [Serializable]
    public class AutoSelectPaymentTable2
    {
        //Table 2
        public Int32 NCount { get; set; }
        public double NAmt { get; set; }
    }
}
