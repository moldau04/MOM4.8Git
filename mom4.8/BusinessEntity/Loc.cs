using System;
using System.Data;

namespace BusinessEntity
{
    public class Loc
    {
        public int LocID;
        public int Owner;
        public string ID;
        public string Tag;
        public string Address;
        public string City;
        public string State;
        public Int16 Elevs;
        public Int16 Status;
        public double Balance;
        public int Rol;
        public string STax;
        public int Job;
        public string Remarks;
        public string Type;
        public Int16 Billing;
        public string Country;
        private DataSet _dsLoc;
        private string _ConnConfig;
        public string geoCode { get; set; }        //geoCode for vertex payroll
        public string MOMUSer { get; set; }
        public DataSet DsLoc
        {
            get { return _dsLoc; }
            set { _dsLoc = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAllLocationOnCustomerParam
    {
        private DataSet _dsLoc;
        private string _ConnConfig;
        public DataSet DsLoc
        {
            get { return _dsLoc; }
            set { _dsLoc = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ownerId { get; set; }

    }

    public class GetAccountLabelParam
    {
        public string Tag;
        private DataSet _dsLoc;
        private string _ConnConfig;
        public DataSet DsLoc
        {
            get { return _dsLoc; }
            set { _dsLoc = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string Address;
        public string City;
        public string State;
        public string Type;
        public Int32 IsSelesAsigned;
    }
}
