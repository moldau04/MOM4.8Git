using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationDetailsReportViewModel
    {
        public int Loc { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public string LocAddress { get; set; }
        public string LocCity { get; set; }
        public string LocState { get; set; }
        public string LocZip { get; set; }
        public string LocCountry { get; set; }
        public int Rol { get; set; }
        public int Consult { get; set; }
        public string Type { get; set; }
        public int Route { get; set; }
        public int Terr { get; set; }
        public int Terr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public string Contact { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string EMail { get; set; }
        public string Cellular { get; set; }
        public string Fax { get; set; }
        public int Owner { get; set; }
        public string Stax { get; set; }
        public string STax2 { get; set; }
        public string UTax { get; set; }
        public int Zone { get; set; }
        public bool PrintInvoice { get; set; }
        public bool EmailInvoice { get; set; }
        public double Balance { get; set; }
        public Int16 Billing { get; set; }
        public string QBLocID { get; set; }
        public int DefaultTerms { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom12 { get; set; }
        public string Custom13 { get; set; }
        public string Custom14 { get; set; }
        public string Custom15 { get; set; }
        public Int16 Status { get; set; }
        public string DefWork { get; set; }
        public Int16 Credit { get; set; }
        public Int16 Dispalert { get; set; }
        public string CreditReason { get; set; }
        public string CustomerSageID { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double RateMileage { get; set; }
        public double Rate { get; set; }
        public double GstRate { get; set; }
        public bool NoCustomerStatement { get; set; }
        public string Salesperson { get; set; }
        public string RouteName { get; set; }
        public string ConsultantName { get; set; }
        public string OwnerName { get; set; }
        public string CustomerName { get; set; }
        public int BusinessTypeID { get; set; }
        public Int16 sTaxType { get; set; }
    }
}
