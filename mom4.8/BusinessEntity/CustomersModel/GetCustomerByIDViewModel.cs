using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetCustomerByID
    {
        public List<GetCustomerByIDTable1> lstTable1 { get; set; }
        public List<GetCustomerByIDTable2> lstTable2 { get; set; }
        public List<GetCustomerByIDTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetCustomerByIDTable1
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public Int16 GeoLock { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string fLogin { get; set; }
        public string Password { get; set; }
        public Int16 Status { get; set; }
        public Int16 TicketO { get; set; }
        public Int16 TicketD { get; set; }
        public Int16 Internet { get; set; }
        public int Rol { get; set; }
        public int EN { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string EMail { get; set; }
        public string Cellular { get; set; }
        public Int16 ledger { get; set; }
        public string msmpass { get; set; }
        public string msmuser { get; set; }
        public Int16 CPEquipment { get; set; }
        public string sageid { get; set; }
        public string ownerid { get; set; }
        public Int16 Billing { get; set; }
        public int Central { get; set; }
        public string QBcustomerID { get; set; }
        public Int16 GroupbyWO { get; set; }
        public Int16 openticket { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double RateMileage { get; set; }
        public string fax { get; set; }
        public string Title { get; set; }
        public string ProfileImage { get; set; }
        public string CoverImage { get; set; }
        public double Balance { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string CNotes { get; set; }
    }

    [Serializable]
    public class GetCustomerByIDTable2
    {
        public int contactid { get; set; }
        public string name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public bool EmailTicket { get; set; }
        public bool EmailRecInvoice { get; set; }
        public bool ShutdownAlert { get; set; }
        public bool EmailRecTestProp { get; set; }
    }

    [Serializable]
    public class GetCustomerByIDTable3
    {
        public string locid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Int16 Elevs { get; set; }
        public Int16 Status { get; set; }
        public double Balance { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int loc { get; set; }
        public int roleid { get; set; }
    }

}
