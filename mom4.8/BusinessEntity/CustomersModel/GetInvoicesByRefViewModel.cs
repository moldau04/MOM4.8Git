using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetInvoicesByRefViewModel
    {
        public DateTime fDate { get; set; }
        public int Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public double STax { get; set; }
        public double Total { get; set; }
        public string TaxRegion { get; set; }
        public double TaxRate { get; set; }
        public double TaxFactor { get; set; }
        public double Taxable { get; set; }
        public Int16 Type { get; set; }
        public int Job { get; set; }
        public int Loc { get; set; }
        public Int16 Terms { get; set; }
        public string PO { get; set; }
        public Int16 Status { get; set; }
        public int Batch { get; set; }
        public string Remarks { get; set; }
        public int TransID { get; set; }
        public double GTax { get; set; }
        public int Mech { get; set; }
        public Int16 Pricing { get; set; }
        public string TaxRegion2 { get; set; }
        public double TaxRate2 { get; set; }
        public Int16 BillToOpt { get; set; }
        public string BillTo { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public DateTime IDate { get; set; }
        public string fUser { get; set; }
        public string Custom3 { get; set; }
        public string QBInvoiceID { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime DDate { get; set; }
        public double GSTRate { get; set; }
        public int AssignedTo { get; set; }
        public int IsRecurring { get; set; }
        public DateTime DueDate { get; set; }
        public string CustomerName { get; set; }
        public Int16 Billing { get; set; }
        public string LocName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToZip { get; set; }
        public int owner { get; set; }
        public string Address { get; set; }
        public string ID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string EMail { get; set; }
        public string CCEMail { get; set; }
        public string TerrName { get; set; }
        public string RouteName { get; set; }
        public string StatusName { get; set; }
        public string MechName { get; set; }
        public string TypeName { get; set; }
        public string TermsText { get; set; }
        public Int16 Payterms { get; set; }
        public string PO1 { get; set; }
        public double PaidCC { get; set; }
        public double Paid { get; set; }
        public double Balance { get; set; }
        public double AmtPaid { get; set; }
        public int IsExistsEmail { get; set; }
        public bool EmailInvoice { get; set; }
        public string LocID { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string JobRemarks { get; set; }
        public string ProgressBillingNo { get; set; }
        public double Rate { get; set; }
        public string PSTReg { get; set; }
        public Int16 STaxType { get; set; }
    }
}
