using System;
using System.Data;

namespace BusinessEntity
{
    public class Chart
    {
        private string _ConnConfig;
        private string _DBName;
        private string _SearchBy;
        private string _SearchValue;
        private int _SearchIndex;
       
        private DataSet _ds;
        private DataSet _dsID;
        private DataSet _dsStatus;
        private DataSet _dsIsExist;
        private string _condition;
        public int? SearchAcctType { get; set; }
        public int? SearchStatus { get; set; }

        public int ID { get; set; }
        public string Acct { get; set; }
        public string fDesc { get; set; }
        public int? Department { get; set; }
        public double Balance { get; set; }
        public int AcType { get; set; }
        public int Type { get; set; }
        public string Sub { get; set; }
        public string Remarks { get; set; }
        public int Control { get; set; }
        public int InUse { get; set; }
        public int Detail { get; set; }
        public string CAlias { get; set; }
        public int Status { get; set; }
        public string Sub2 { get; set; }
        public int Branch { get; set; }
        public int CostCenter { get; set; }
        public string QBAccountID { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public double Amount { get; set; } // Journal Entry Balance amount purpose.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsBankAcct { get; set; }
        public int Bank { get; set; }
        public int Batch { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public int GeoLock { get; set; }
        public DateTime Since { get; set; }
        public DateTime Last { get; set; }
        public string EMail { get; set; }
        public string Website { get; set; }
        public string Cellular { get; set; }
        public string Country { get; set; }
        public string NBranch { get; set; }
        public string NAcct { get; set; }
        public string NRoute { get; set; }
        public int NextC { get; set; }
        public int NextD { get; set; }
        public int NextE { get; set; }
        public double Rate { get; set; }
        public double CLimit { get; set; }
        public Int16 Warn { get; set; }
        public double Recon { get; set; }
        public int Rol { get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }
        public string Departments { get; set; }
        public string Budgets { get; set; }
        public string Periods { get; set; }
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; }
        }
        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        
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
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public DataSet DsIsExist
        {
            get { return _dsIsExist; }
            set { _dsIsExist = value; }
        }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public DataSet DsStatus
        {
            get { return _dsStatus; }
            set { _dsStatus = value; }
        }
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
        public DateTime YearStartDate { get; set; }
        public DateTime YearEndDate { get; set; }

        public String BankName { get; set; }
        public String Lat { get; set; }
        public String Long { get; set; }

        public double? filterDebit { get; set; }
        public double? filterCredit { get; set; }
        public double? filterBalance { get; set; }
        public DateTime? filterDate { get; set; }
        public String filterRef { get; set; }
        public String filterTypeText { get; set; }
        public String filterfDesc { get; set; }
        public int pageNumber { get; set; }
        public int PageSize { get; set; }
        public Int16 NoJE { get; set; }
    }


    public class GetAcctPayableParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateChartBalanceParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
        public double Amount { get; set; } // Journal Entry Balance amount purpose.
    }

    public class GetBankAcctIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Bank { get; set; }
    }

    public class GetChartParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
    }


    public class GetMaintenanceUnitCountRouteParam
    {
        private string _ConnConfig;
        private DataSet _ds;
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

    public class GetMaintenanceUnitCountParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string routes { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class GetPastDueMCPDataParam
    {
        private string _ConnConfig;
        private DataSet _ds;
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

    public class GetUndepositeAcctParam
    {
        private string _ConnConfig;
        private DataSet _ds;
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

    public class GetMonthlyRecurringHoursTEIParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string routes { get; set; }
    }

    public class GetMonthlyRecurringHoursParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
       public string routes { get; set; }
    }
}
