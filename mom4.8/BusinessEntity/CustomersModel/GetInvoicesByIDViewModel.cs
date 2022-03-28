using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetInvoicesByID
    {
        public List<GetInvoicesByIDTable1> lstTable1 { get; set; }
        public List<GetInvoicesByIDTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetInvoicesByIDTable1
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
        public string JobDecs { get; set; }
        public string JobRemarks { get; set; }
        public Int16 SPHandle { get; set; }
        public string SRemarks { get; set; }
        public int InvServ { get; set; }
        //public string ProgressBillingNo { get; set; }
        public double TotalTax { get; set; }
        public string customerName { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string CCEMail { get; set; }
        public string locname { get; set; }
        public int owner { get; set; }
        public string Address { get; set; }
        public string statusname { get; set; }
        public string MechName { get; set; }
        public string typeName { get; set; }
        public string termsText { get; set; }
        public Int16 paidcc { get; set; }
        public double balance { get; set; }
        public double amtpaid { get; set; }
        public string LocID { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public Int16 jobStatus { get; set; }
        public Int16 locStatus { get; set; }
    }

    [Serializable]
    public class GetInvoicesByIDTable2
    {
        public int Ref { get; set; }
        public Int16 Line { get; set; }
        public int Acct { get; set; }
        public double Quan { get; set; }
        public string fDesc { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public Int16 STax { get; set; }
        public double GTaxAmt { get; set; }
        public int Job { get; set; }
        public int JobItem { get; set; }
        public int TransID { get; set; }
        public string Measure { get; set; }
        public double Disc { get; set; }
        public int JobOrg { get; set; }
        public double StaxAmt { get; set; }
        public double TotalTax { get; set; }
        public double pricequant { get; set; }
        public string billcode { get; set; }
        public int code { get; set; }
        public Int16 INVType { get; set; }
        public string Warehouse { get; set; }
        public int WHLocID { get; set; }
        public Int16 InvStatus { get; set; }
        public Int16 AStatus { get; set; }
        public string ProgressBillingNo { get; set; }

    }
}
