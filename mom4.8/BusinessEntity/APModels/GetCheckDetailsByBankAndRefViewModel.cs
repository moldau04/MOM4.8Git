using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListCheckDetailsByBankAndRef
    {
        public List<CheckDetailsByBankAndRefTable1> lstCheckDetailsByBankAndRefTable1 { get; set; }
        public List<CheckDetailsByBankAndRefTable2> lstCheckDetailsByBankAndRefTable2 { get; set; }
    }

    [Serializable]
    public class CheckDetailsByBankAndRefTable1
    {
        public string Ref { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Refrerence { get; set; }
        public double Total { get; set; }
        public double Disc { get; set; }
        public double AmountPay { get; set; }
        public DateTime PayDate { get; set; }
        public int CheckNo { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        //public Int16 Type { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class CheckDetailsByBankAndRefTable2
    {
        public double Pay { get; set; }
        public string ToOrder { get; set; }
        public DateTime? Date { get; set; }
        public double CheckAmount { get; set; }
        public string ToOrerAddress { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string VendorAddress { get; set; }
        public string RemitAddress { get; set; }
        public string Memo { get; set; }
        public Int16 Status { get; set; }
        public int Vendor { get; set; }
        public int CheckNo { get; set; }

    }

    [Serializable]
    public class GetCheckDetailsByBankAndRefViewModel1
    {
        //Table1
        public string Ref { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Refrerence { get; set; }
        public double Total { get; set; }
        public double Disc { get; set; }
        public double AmountPay { get; set; }
        public DateTime PayDate { get; set; }
        public int CheckNo { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        //public Int16 Type { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        //Table2
        public double Pay { get; set; }
        public string ToOrder { get; set; }
        public DateTime Date { get; set; }
        public double CheckAmount { get; set; }
        public string ToOrerAddress { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string VendorAddress { get; set; }
        public string RemitAddress { get; set; }
        public string Memo { get; set; }
        public Int16 Status { get; set; }
    }
}
