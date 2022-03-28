using System.Data;

namespace BusinessEntity
{
    public class CompanyOffice
    {
        private int _ID;
        private string _Name;
        private string _ConnConfig;
        private string _Manager;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Phone;
        private string _Fax;
        private string _CostCenter;
        private string _InvRemarks;
        private byte[] _Logo;
        private string _LogoPath;
        private string _BillRemit;
        private string _PORemit;
        private string _LocDTerr;
        private string _LocDRoute;
        private string _LocDZone;
        private string _LocDSTax;
        private string _LocType;
        private string _ARTerms;
        private string _ADP;
        private double _CB;
        private string _ARContact;
        private string _OType;
        private string _DArea;
        private string _DState;
        private double _MileRate;
        private double _PriceD1;
        private double _PriceD2;
        private double _PriceD3;
        private double _PriceD4;
        private double _PriceD5;
        private int _UTaxR;
        private string _UTax;
        private string _Company;
        private DataSet _ds;
        private string _DBName;
        private string _SearchBy;
        private string _SearchValue;
        private int _Status;
        private string _ChargeInt;
        private int _UserID;
        private int _CompanyID;
        private bool _IsSel;
        private string _Longitude;
        private string _Latitude;
        private string _Country;
        public bool IsSel
        {
            get { return _IsSel; }
            set { _IsSel = value; }
        }
        public int CompanyID
        {
            get { return _CompanyID; }
            set { _CompanyID = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string ChargeInt
        {
            get { return _ChargeInt; }
            set { _ChargeInt = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }
        public string UTax
        {
            get { return _UTax; }
            set { _UTax = value; }
        }
        public int UTaxR
        {
            get { return _UTaxR; }
            set { _UTaxR = value; }
        }
        public double PriceD5
        {
            get { return _PriceD5; }
            set { _PriceD5 = value; }
        }
        public double PriceD4
        {
            get { return _PriceD4; }
            set { _PriceD4 = value; }
        }
        public double PriceD3
        {
            get { return _PriceD3; }
            set { _PriceD3 = value; }
        }
        public double PriceD2
        {
            get { return _PriceD2; }
            set { _PriceD2 = value; }
        }
        public double PriceD1
        {
            get { return _PriceD1; }
            set { _PriceD1 = value; }
        }
        public double MileRate
        {
            get { return _MileRate; }
            set { _MileRate = value; }
        }
        public string DState
        {
            get { return _DState; }
            set { _DState = value; }
        }
        public string DArea
        {
            get { return _DArea; }
            set { _DArea = value; }
        }
        public string OType
        {
            get { return _OType; }
            set { _OType = value; }
        }
        public string ARContact
        {
            get { return _ARContact; }
            set { _ARContact = value; }
        }
        public double CB
        {
            get { return _CB; }
            set { _CB = value; }
        }
        public string ADP
        {
            get { return _ADP; }
            set { _ADP = value; }
        }
        public string ARTerms
        {
            get { return _ARTerms; }
            set { _ARTerms = value; }
        }

        public string LocType
        {
            get { return _LocType; }
            set { _LocType = value; }
        }

        public string LocDSTax
        {
            get { return _LocDSTax; }
            set { _LocDSTax = value; }
        }

        public string LocDZone
        {
            get { return _LocDZone; }
            set { _LocDZone = value; }
        }

        public string LocDRoute
        {
            get { return _LocDRoute; }
            set { _LocDRoute = value; }
        }

        public string LocDTerr
        {
            get { return _LocDTerr; }
            set { _LocDTerr = value; }
        }

        public string PORemit
        {
            get { return _PORemit; }
            set { _PORemit = value; }
        }

        public string BillRemit
        {
            get { return _BillRemit; }
            set { _BillRemit = value; }
        }

        public string LogoPath
        {
            get { return _LogoPath; }
            set { _LogoPath = value; }
        }

        public byte[] Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        }


        public string InvRemarks
        {
            get { return _InvRemarks; }
            set { _InvRemarks = value; }
        }

        public string CostCenter
        {
            get { return _CostCenter; }
            set { _CostCenter = value; }
        }

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }

        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public string Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int DInvAcct { get; set; }

        public string Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public string Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
    }

    public class getUserDefaultCompanyParam
    {
        private int _UserID;
        private string _DBName;
        private string _ConnConfig;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCompanyByCustomerParam
    {
        private int _UserID;
        private string _DBName;
        private string _ConnConfig;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCompanyByUserIDParam
    {
        private int _UserID;
        private string _DBName;
        private string _ConnConfig;
        private DataSet _ds;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }
}
