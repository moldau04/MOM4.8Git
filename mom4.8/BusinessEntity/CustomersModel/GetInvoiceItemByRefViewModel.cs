using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoiceItemByRefViewModel
    {
        public int Ref { get; set; }
        public Int16 Line { get; set; }
        public int Acct { get; set; }
        public double Quan { get; set; }
        public string fDesc { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public Int16 STax { get; set; }
        public int JobItem { get; set; }
        public string Measure { get; set; }
        public double Disc { get; set; }
        public string ProgressBillingNo { get; set; }
        public double staxAmt { get; set; }
        public double GTaxAmt { get; set; }
        public double TotalTax { get; set; }
        public double pricequant { get; set; }
        public string billcode { get; set; }
        public int code { get; set; }
        public double balance { get; set; }
        public double amtpaid { get; set; }
        public double Total { get; set; }
        public int JobOrg { get; set; }
        public double INVSTax { get; set; }
        public double INVAmount { get; set; }
        public double Taxable { get; set; }
        public string Description { get; set; }
        public string TaxRegion { get; set; }
    }
}
