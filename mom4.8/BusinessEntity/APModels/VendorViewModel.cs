using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class VendorViewModel
    {
        private string _ConnConfig;
        private string _DBName;

        private DataSet _ds;
        private DataSet _dsID;
        private DataSet _dsIsExist;
        private DataTable _VendorData;
        public int ID { get; set; }
        public int Rol { get; set; }
        public string Acct { get; set; }
        public Int16 Type { get; set; }
        public Int16 Status { get; set; }
        public string Vstatus { get; set; }
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
        public string SearchBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }

        public DataTable VendorData
        {
            get { return _VendorData; }
            set { _VendorData = value; }
        }
        public string MOMUSer { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public DateTime Since { get; set; }
        public DateTime Last { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string Company { get; set; }
        public string Remarks { get; set; }
        public Int64 Count { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Cellular { get; set; }
        public Int32 GeoLock { get; set; }
        public string DefaultAcct { get; set; }
        public Int16 intBox { get; set; }
        public string VType { get; set; }
        public string STax { get; set; }
        public string UTax { get; set; }
        public string VendorAddress { get; set; }
        public string RemitAddress { get; set; }
        public double Amount { get; set; }
        public string Label { get; set; }

        public int TotalRow { get; set; }
        public string desc { get; set; }
        public string VState { get; set; }
        public int Term { get; set; }
        public double STaxRate { get; set; }
        public double UTaxRate { get; set; }
        public int STaxType { get; set; }
        public int UTaxType { get; set; }
        public string STaxName { get; set; }
        public string UTaxName { get; set; }
        public int STaxGL { get; set; }
        public int SUaxGL { get; set; }

    }

    public class GetVendorAcctList
    {
        public int ID { get; set; }
        public string AcctNumber { get; set; }

    }
}
