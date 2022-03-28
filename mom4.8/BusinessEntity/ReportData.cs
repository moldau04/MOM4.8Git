using System.Data;

namespace BusinessEntity
{
    public class ReportData
    {

    }

    public class CustomerReport
    {
        private string _ConnConfig;
        private string _DBName;
        private int _ReportId;
        private string _ReportName;
        private string _ReportType;
        private int _UserId;
        private bool _IsGlobal;
        private string _SortBy;        
        private bool _IsAscending;
        private string _ColumnName;
        private string _ColumnWidth;
        private string _FilterColumns;
        private string _FilterValues;
        private DataSet _DsCustomer;
        private bool _IsReportExists;
        private bool _MainHeader;
        private string _CompanyName;
        private string _ReportTitle;
        private string _SubTitle;
        private string _DatePrepared;
        private bool _TimePrepared;
        private string _PageNumber;
        private string _ExtraFooterLine;
        private string _Alignment;
        private string _PDFSize;
        private bool _IsStock;
        private string _Module;
        private string _Condition;

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

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }


        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        public string ReportType
        {
            get { return _ReportType; }
            set { _ReportType = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public bool IsGlobal
        {
            get { return _IsGlobal; }
            set { _IsGlobal = value; }
        }

        public bool IsAscending
        {
            get { return _IsAscending; }
            set { _IsAscending = value; }
        }

        public string SortBy
        {
            get { return _SortBy; }
            set { _SortBy = value; }
        }       

        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }

        public string FilterColumns
        {
            get { return _FilterColumns; }
            set { _FilterColumns = value; }
        }

        public string FilterValues
        {
            get { return _FilterValues; }
            set { _FilterValues = value; }
        }

        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

        public bool IsReportExists
        {
            get { return _IsReportExists; }
            set { _IsReportExists = value; }
        }

        public bool MainHeader
        {
            get { return _MainHeader; }
            set { _MainHeader = value; }
        }

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value;}            
        }

        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }

        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }

        public string DatePrepared
        {
            get { return _DatePrepared; }
            set { _DatePrepared = value; }
        }

        public bool TimePrepared
        {
            get { return _TimePrepared; }
            set { _TimePrepared = value; }
        }

        public string PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }

        public string ExtraFooterLine
        {
            get { return _ExtraFooterLine; }
            set { _ExtraFooterLine = value; }
        }

        public string Alignment
        {
            get { return _Alignment; }
            set { _Alignment = value; }
        }

        public string PDFSize
        {
            get { return _PDFSize; }
            set { _PDFSize = value; }
        }

        public bool IsStock
        {
            get { return _IsStock; }
            set { _IsStock = value; }
        }

        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }

        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        //New Parameter for APi's
        public string reportAction;
    }

    public class GetSMTPByUserIDParam
    {
        private string _ConnConfig; 
        private int _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

    }

    public class CustomerReportParam
    {
        private string _ConnConfig;
        private string _DBName;
        private int _ReportId;
        private DataSet _DsCustomer;
        private int _UserId;
        private DataSet _dsUserAuthorization;
        private string _ColumnName;
        private string _ColumnWidth;

        public string reportType;
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

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }


    }

    public class GetReportDetailByIdParam
    {

        private string _ConnConfig;
        private string _DBName;
        private int _ReportId;
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

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }
    }

    public class CheckExistingReportParam
    {
        private string _ConnConfig;
        private int _ReportId;
        private string _ReportName;
        private DataSet _DsCustomer;

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public string reportAction;

    }

    public class InsertCustomerReportParam
    {
        private string _ConnConfig;
        private string _DBName;
        private int _ReportId;
        private string _ReportName;
        private string _ReportType;
        private int _UserId;
        private bool _IsGlobal;
        private string _SortBy;
        private bool _IsAscending;
        private string _ColumnName;
        private string _ColumnWidth;
        private string _FilterColumns;
        private string _FilterValues;
        private DataSet _DsCustomer;
        private bool _IsReportExists;
        private bool _MainHeader;
        private string _CompanyName;
        private string _ReportTitle;
        private string _SubTitle;
        private string _DatePrepared;
        private bool _TimePrepared;
        private string _PageNumber;
        private string _ExtraFooterLine;
        private string _Alignment;
        private string _PDFSize;
        private bool _IsStock;
        private string _Module;
        private string _Condition;

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

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }


        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        public string ReportType
        {
            get { return _ReportType; }
            set { _ReportType = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public bool IsGlobal
        {
            get { return _IsGlobal; }
            set { _IsGlobal = value; }
        }

        public bool IsAscending
        {
            get { return _IsAscending; }
            set { _IsAscending = value; }
        }

        public string SortBy
        {
            get { return _SortBy; }
            set { _SortBy = value; }
        }

        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }

        public string FilterColumns
        {
            get { return _FilterColumns; }
            set { _FilterColumns = value; }
        }

        public string FilterValues
        {
            get { return _FilterValues; }
            set { _FilterValues = value; }
        }

        public bool IsReportExists
        {
            get { return _IsReportExists; }
            set { _IsReportExists = value; }
        }

        public bool MainHeader
        {
            get { return _MainHeader; }
            set { _MainHeader = value; }
        }

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }

        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }

        public string DatePrepared
        {
            get { return _DatePrepared; }
            set { _DatePrepared = value; }
        }

        public bool TimePrepared
        {
            get { return _TimePrepared; }
            set { _TimePrepared = value; }
        }

        public string PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }

        public string ExtraFooterLine
        {
            get { return _ExtraFooterLine; }
            set { _ExtraFooterLine = value; }
        }

        public string Alignment
        {
            get { return _Alignment; }
            set { _Alignment = value; }
        }

        public string PDFSize
        {
            get { return _PDFSize; }
            set { _PDFSize = value; }
        }

        public bool IsStock
        {
            get { return _IsStock; }
            set { _IsStock = value; }
        }

        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }

        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class IsStockReportExistParam
    {
        private string _ConnConfig;
        private string _ReportName;
        private DataSet _DsCustomer;
        
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class UpdateCustomerReportParam
    {
        private string _ConnConfig;
        private string _DBName;
        private int _ReportId;
        private string _ReportName;
        private string _ReportType;
        private int _UserId;
        private bool _IsGlobal;
        private string _SortBy;
        private bool _IsAscending;
        private string _ColumnName;
        private string _ColumnWidth;
        private string _FilterColumns;
        private string _FilterValues;
        private DataSet _DsCustomer;
        private bool _IsReportExists;
        private bool _MainHeader;
        private string _CompanyName;
        private string _ReportTitle;
        private string _SubTitle;
        private string _DatePrepared;
        private bool _TimePrepared;
        private string _PageNumber;
        private string _ExtraFooterLine;
        private string _Alignment;
        private string _PDFSize;
        private bool _IsStock;
        private string _Module;
        private string _Condition;

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

        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }


        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        public string ReportType
        {
            get { return _ReportType; }
            set { _ReportType = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public bool IsGlobal
        {
            get { return _IsGlobal; }
            set { _IsGlobal = value; }
        }

        public bool IsAscending
        {
            get { return _IsAscending; }
            set { _IsAscending = value; }
        }

        public string SortBy
        {
            get { return _SortBy; }
            set { _SortBy = value; }
        }

        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }

        public string FilterColumns
        {
            get { return _FilterColumns; }
            set { _FilterColumns = value; }
        }

        public string FilterValues
        {
            get { return _FilterValues; }
            set { _FilterValues = value; }
        }

        public bool IsReportExists
        {
            get { return _IsReportExists; }
            set { _IsReportExists = value; }
        }

        public bool MainHeader
        {
            get { return _MainHeader; }
            set { _MainHeader = value; }
        }

        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }

        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }

        public string DatePrepared
        {
            get { return _DatePrepared; }
            set { _DatePrepared = value; }
        }

        public bool TimePrepared
        {
            get { return _TimePrepared; }
            set { _TimePrepared = value; }
        }

        public string PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }

        public string ExtraFooterLine
        {
            get { return _ExtraFooterLine; }
            set { _ExtraFooterLine = value; }
        }

        public string Alignment
        {
            get { return _Alignment; }
            set { _Alignment = value; }
        }

        public string PDFSize
        {
            get { return _PDFSize; }
            set { _PDFSize = value; }
        }

        public bool IsStock
        {
            get { return _IsStock; }
            set { _IsStock = value; }
        }

        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }

        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class GetCustomerTypeParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class GetCustomerNameParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class GetCustomerAddressParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }

    public class GetCustomerCityParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }

    }
    public class GetReportColByRepIdParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

    }

    public class GetReportFiltersByRepIdParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

    }
    public class DeleteCustomerReportParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId;
        private int _UserId;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

    }

    public class GetHeaderFooterDetailParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

    }

    public class GetColumnWidthByReportIdParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }

    }

    public class UpdateCustomerReportResizedWidthParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer;
        private int _ReportId; 
        private int _UserId;
        private string _ColumnName;
        private string _ColumnWidth;
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
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ReportId
        {
            get { return _ReportId; }
            set { _ReportId = value; }
        }
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        public string ColumnWidth
        {
            get { return _ColumnWidth; }
            set { _ColumnWidth = value; }
        }
    }
}
