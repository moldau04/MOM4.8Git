using System;
using System.Data;

namespace BusinessEntity
{
    public class Rol
    {
        public int ID;
        public string Name;
        public string City;
        public string State;
        public string Zip;
        public string Phone;
        public string Fax;
        public string Contact;
        public string Remarks;
        public int Type;
        public int fLong;
        public int Latt;
        public int GeoLock;
        public DateTime Since;
        public DateTime Last;
        public string Address;
        public int EN;
        public string EMail;
        public string Website;
        public string Cellular;
        public string Category;
        public string Position;
        public string Country;
        public string lat;
        public string lng;
        public DateTime LastUpdateDate;
        public string ConnConfig;

        public Bank objBank;
        private DataSet _dsRol;
        private DataSet _dsID;
      
        public DataSet DsRol
        {
            get { return _dsRol; }
            set { _dsRol = value; }
        }
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
        public string MOMUSer { get; set; }
    }

    public class DeleteRolByIDParam
    {
        public string ConnConfig;
        public int ID;
    }

    public class UpdateRolParam
    {
        public string ConnConfig;
        public int ID;
        public string Name;
        public string Address;
        public string City;
        public string State;
        public string Zip;
        public string Phone;
        public string Fax;
        public string Contact;
        public int Type;
        public int GeoLock;
        public DateTime Since;
        public DateTime Last;
        public string Country;
        public int EN;
        public string EMail;
        public string Website; 
        public string Cellular;
        public string MOMUSer { get; set; }
    }
    public class UpdateRolesParam
    {
        public string ConnConfig;
        public int ID;
        public string Remarks;
    }
    public class AddRolParam
    {
        public string ConnConfig;
        public string Name;
        public string Address;
        public string City;
        public string State;
        public string Zip;
        public string Phone;
        public string Fax;
        public string Contact;
        public int Type;
        public int GeoLock;
        public DateTime Since;
        public DateTime Last;
        public string Country;
        public int EN;
        public string EMail;
        public string Website;
        public string Cellular;
    }
}
