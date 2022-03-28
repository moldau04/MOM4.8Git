using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetLocationByID
    {
        public List<GetLocationByIDTable1> lstTable1 { get; set; }
        public List<GetLocationByIDTable2> lstTable2 { get; set; }
        public List<GetLocationByIDTable3> lstTable3 { get; set; }
        public List<GetLocationByIDTable4> lstTable4 { get; set; }
        public List<GetLocationByIDTable5> lstTable5 { get; set; }

    }

    [Serializable]
    public class GetLocationByIDTable1
    {
        public int Consult { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }
        public string locAddress { get; set; }
        public string locCity { get; set; }
        public string locState { get; set; }
        public string locZip { get; set; }
        public int Rol { get; set; }
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
        public int EN { get; set; }
        public string Company { get; set; }
        public int owner { get; set; }
        public string custname { get; set; }
        public string stax { get; set; }
        public string STax2 { get; set; }
        public string UTax { get; set; }
        public int Zone { get; set; }
        public string locCountry { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom14{ get; set; }
        public string custom15{ get; set; }  
        public string custom12{ get; set; }
        public string custom13 { get; set; }
        public Int16 status { get; set; }
        public string defwork { get; set; }
        public byte credit { get; set; }
        public byte dispalert { get; set; }
        public string creditreason { get; set; }
        public string custsageid { get; set; }
        public Int16 Billing { get; set; }
        public string qblocid { get; set; }
        public int defaultterms { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double RateMileage { get; set; }
        public double Rate { get; set; }
        public double GstRate { get; set; }
        public bool PrintInvoice { get; set; }
        public bool EmailInvoice { get; set; }
        public double Balance { get; set; }
        public int Loc { get; set; }
        public bool NoCustomerStatement { get; set; }
        public string LocationName { get; set; }
        public string Salesperson { get; set; }
        public string RouteName { get; set; }
        public string ConsultantName { get; set; }
        public string OwnerName { get; set; }
        public string Customer { get; set; }
        public int Elevs { get; set; }
        public int BusinessTypeID { get; set; }
        public Int16 sTaxType { get; set; }
    }

    [Serializable]
    public class GetLocationByIDTable2
    {
        public int contactid { get; set; }
        public string name { get; set; }
        public string  Phone { get; set; }
        public string  Fax { get; set; }
        public string  Cell { get; set; }
        public string  Email { get; set; }
        public string  Title { get; set; }
        public bool  EmailTicket { get; set; }
        public bool EmailRecInvoice { get; set; }
        public bool ShutdownAlert { get; set; }
        public bool EmailRecTestProp { get; set; }
    }

    [Serializable]
    public class GetLocationByIDTable3
    {
        public double Balance { get; set; }
    }

    [Serializable]
    public class GetLocationByIDTable4
    {
        public string fUser { get; set; }
        public string Screen { get; set; }
        public int Ref { get; set; }
        public string Field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime CreatedStamp { get; set; }
        public DateTime fDate { get; set; }
        public DateTime fTime { get; set; }
    }

    [Serializable]
    public class GetLocationByIDTable5
    {
        public int TicketID { get; set; }
        public Int16 Status { get; set; }
    }
}
