using System;
using System.Data;

namespace BusinessEntity
{
    public class Vendor
    {
        private string _ConnConfig;
        private string _DBName;

        private DataSet _ds;
        private DataSet _dsID;
        private DataSet _dsIsExist;
        private DataTable _VendorData;
        public int ID;
        public int Rol;
        public string Acct { get; set; }
        public string Type { get; set; }
        public Int16 Status { get; set; }
        public double Balance { get; set; }
        public double CLimit { get; set; }
        public Int16 Vendor1099 { get; set; }
        public Int16 Vendor1099Box { get; set; }
        public string FID { get; set; }
        public int DA { get; set; }
        public string AcctNumber { get; set; }
        public Int16 Terms { get; set; }
        public double Disc { get; set; }
        public Int16 Days { get; set; }
        public Int16 InUse { get; set; }
        public string Remit { get; set; }
        public Int16 OnePer { get; set; }
        public string DBank { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public string Custom6 { get; set; }
        public string Custom7 { get; set; }
        public DateTime Custom8 { get; set; }
        public DateTime Custom9 { get; set; }
        public DateTime Custom10 { get; set; }
        public string ShipVia { get; set; }
        public string QBVendorID { get; set; }
        private string _SearchValue;
        public String ContactName { get; set; }
        public String Phone { get; set; }

        public String Email { get; set; }
        public String Cell { get; set; }
        public String Fax { get; set; }

        public bool EmailRecPO { get; set; }

        public int RolId { get; set; }
        public bool IsExist { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public string StatusDisplay { get; set; }
        public string CourierAccount { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public DataSet DsIsExist
        {
            get { return _dsIsExist; }
            set { _dsIsExist = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        private string _cols;
        public string Cols
        {
            get
            {
                return _cols;
            }
            set
            {
                _cols = value;
            }
        }
        public string SearchBy;
        public DateTime StartDate;
        public DateTime EndDate;
        public int EN { get; set; }
        public int UserID { get; set; }

        public DataTable VendorData
        {
            get { return _VendorData; }
            set { _VendorData = value; }
        }
        public string MOMUSer { get; set; }
    }


    public class UpdateVendorBalanceParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }


    public class GetVendorRolDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetVendorByNameParam
    {
        private string _ConnConfig;
        private string _SearchValue;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int EN { get; set; }

    }

    public class GetOpenBillVendorByCompanyParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EN { get; set; }
    }

    public class GetOpenBillVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EN { get; set; }
    }

    public class GetVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetCreditBillVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetVendorAcctParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetAllVenderAjaxSearchParam
    {
        private string _ConnConfig;
        private string _SearchValue;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private string _cols;
        public string Cols
        {
            get
            {
                return _cols;
            }
            set
            {
                _cols = value;
            }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int UserID { get; set; }
        public int EN { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string StatusDisplay { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }

        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class IsExistVendorDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class DeleteVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }
    public class GetVendorEditParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }
    public class getVendorContactByRolIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Rol;
    }
    public class GetAPExpensesParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public string SearchBy;
        private string _SearchValue;
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;
    }

    public class GetVendorLogsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }
    public class IsExistForUpdateVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int Rol;
        public string Acct { get; set; }
    }
    public class UpdateVendorParam
    {
        //private string _ConnConfig;
        //public string ConnConfig
        //{
        //    get { return _ConnConfig; }
        //    set { _ConnConfig = value; }
        //}
        public Int16 Status { get; set; }
        public double Balance { get; set; }
        public double CLimit { get; set; }
        public string ShipVia { get; set; }
        public Int16 Terms { get; set; }
        public Int16 Days { get; set; }
        public Int16 Vendor1099 { get; set; }
        public Int16 Vendor1099Box { get; set; }
        public Int16 InUse { get; set; }
        public string Remit { get; set; }
        public string FID { get; set; }
        public int DA { get; set; }
        public string AcctNumber { get; set; }
        public string Type { get; set; }
        private DataTable _VendorData;
        public DataTable VendorData
        {
            get { return _VendorData; }
            set { _VendorData = value; }
        }
        public String ContactName { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Cell { get; set; }
        public String Fax { get; set; }
        public bool EmailRecPO { get; set; }
        public string MOMUSer { get; set; }
        public int ID { get; set; }
        public int Rol { get; set; }
        public string Acct { get; set; }
    }

    public class UpdateVendorTaxParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
    }

    public class IsExistsForInsertVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string Acct { get; set; }

    }
    public class AddVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int16 Status { get; set; }
        public double Balance { get; set; }
        public string ShipVia { get; set; }
        public Int16 Terms { get; set; }
        public Int16 Days { get; set; }
        public Int16 Vendor1099 { get; set; }
        public Int16 Vendor1099Box { get; set; }
        public Int16 InUse { get; set; }
        public string Remit { get; set; }
        public string FID { get; set; }
        public int DA { get; set; }
        public string AcctNumber { get; set; }
        public string Type { get; set; }
        private DataTable _VendorData;
        public DataTable VendorData
        {
            get { return _VendorData; }
            set { _VendorData = value; }
        }
        public String ContactName { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Cell { get; set; }
        public String Fax { get; set; }
        public bool EmailRecPO { get; set; }
        public string MOMUSer { get; set; }
        public double CLimit { get; set; }
        public int Rol;
        public string Acct { get; set; }
    }

    public class UpdateVendorContactParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int RolId { get; set; }
        private DataTable _VendorData;
        public DataTable VendorData
        {
            get { return _VendorData; }
            set { _VendorData = value; }
        }
    }

    public class GetVendorSearchParam
    {
        private string _ConnConfig;
        private string _SearchValue;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int EN { get; set; }

    }

    public class UpdateVendorSTaxParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string sTax;

        public int VendorId;

    }
    public class GetAllVendorParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }


    public class GetHistoryTransactionParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string id { get; set; }
        public string type { get; set; }
        public int vendor { get; set; }
        public int loc { get; set; }
        public string status { get; set; }
        public int tid { get; set; }
    }

}
