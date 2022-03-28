using System.Data;

namespace BusinessEntity
{
    public class tblConsult
    {
        private int _ID;
        private string _RolName;
        private int _RolID;
        private int _Count;
        private int _API;
        private string _Username;
        private string _Password;
        private string _IP;
        private string _ConnConfig;
        private DataSet _DsConsultant;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string RolName
        {
            get { return _RolName; }
            set { _RolName = value; }
        }

        public int RolID
        {
            get { return _RolID; }
            set { _RolID = value; }
        }

        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        public int API
        {
            get { return _API; }
            set { _API = value; }
        }
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DataSet DsConsultant
        {
            get { return _DsConsultant; }
            set { _DsConsultant = value; }
        }
    }


    public class GetSingleConsultantParam
    {
        private string _ConnConfig;
        private DataSet _DsConsultant;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsConsultant
        {
            get { return _DsConsultant; }
            set { _DsConsultant = value; }
        }

    }
}
