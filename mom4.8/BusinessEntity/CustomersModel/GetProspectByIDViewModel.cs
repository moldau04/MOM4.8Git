using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetProspectByID
    {
        public List<GetProspectByIDTable1> lstTable1 { get; set; }
        public List<GetProspectByIDTable2> lstTable2 { get; set; }
        public List<GetProspectByIDTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetProspectByIDTable1
    {
        public int ID { get; set; }
        public int rol { get; set; }
        public Int16 status { get; set; }
        public string type { get; set; }
        public string billaddress { get; set; }
        public string billcity { get; set; }
        public string billstate { get; set; }
        public string billzip { get; set; }
        public string billphone { get; set; }
        public string CustomerName { get; set; }
        public int Terr { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Cellular { get; set; }
        public string email { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Contact { get; set; }
        public string Remarks { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Source { get; set; }
        public string Country { get; set; }
        public string billCountry { get; set; }
        public string Referral { get; set; }
        public string ReferralType { get; set; }
        public string BusinessType { get; set; }
        public int BusinessTypeID { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
    }


    [Serializable]
    public class GetProspectByIDTable2
    {
        public int contactid { get; set; }
        public string name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
    }

    [Serializable]
    public class GetProspectByIDTable3
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
    }
}
