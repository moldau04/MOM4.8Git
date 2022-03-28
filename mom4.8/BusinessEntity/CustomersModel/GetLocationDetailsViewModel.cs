using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetLocationDetails
    {
        public List<GetLocationDetailsTable1> lstTable1 { get; set; }
        public List<GetLocationDetailsTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetLocationDetailsTable1
    {
        public string Acct{ get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Type { get; set; }
        public string Custom1 { get; set; }
        public string InvoiceToEmail { get; set; }
        public string InvoiceCCEmail { get; set; }
        public string ServiceToEmail { get; set; }
        public string ServiceCCEmail { get; set; }
        public string PrintInvoice { get; set; }
        public string EmailInvoice { get; set; }
        public string NoCustomerStatement { get; set; }
        public string Status { get; set; }
        public double BillingRate { get; set; }
        public string LocationSTax { get; set; }
        public Int16 EquipmentCounts { get; set; }
        public double Balance { get; set; }
        public string Terms { get; set; }
        public string SalesPerson { get; set; }
        public string DefaultWorker { get; set; }
        public string PreferredWorker { get; set; }
    }

    [Serializable]
    public class GetLocationDetailsTable2
    {
        public string TaxDesc { get; set; }
        public string TaxName { get; set; }
        public double TaxRate { get; set; }
    }
}
