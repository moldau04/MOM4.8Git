using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessEntity
{
    public class User
    {
        private int _Consult;
        private string _Username;
        private string _Password;
        private int _UserID;
        private int _VendorID;
        private int _PDA;
        private int _Field;
        private int _Status;
        private string _Title;
        private string _FirstName;
        private string _LastNAme;
        private string _MiddleName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Tele;
        private string _Cell;
        private string _Email;
        private DateTime _DtHired;
        private DateTime _DtFired;
        private DataSet _dsUserAuthorization;
        private string _CreateTicket;
        private string _WorkDate;
        private string _LocationRemarks;
        private string _ServiceHist;
        private string _PurchaseOrd;
        private string _Expenses;
        private string _ProgFunctions;
        private string _AccessUser;
        private int _RolId;
        private int _WorkId;
        private int _EmpId;
        private string _SearchBy;
        private string _SearchValue;
        private string _SearchValueUnAssignedCalls;
        private string _Remarks;
        private int _Schedule;
        private int _Mapping;
        private int _TypeID;
        private string _Country;
        private int _CustomerID;
        private DataTable _ContactData;
        private int _Internet;
        private string _MainContact;
        private string _Phone;
        private string _Website;
        private string _Type;
        private string _AccountNo;
        private string _Locationname;
        private int _Route;
        public bool IsOnlinePaymentApply;
        //Default Salesperson
        private int _Territory;
        //Second Salesperson
        private int _Territory2;
        private string _RolAddress;
        private string _RolCity;
        private string _RolState;
        private string _RolZip;
        private string _Fax;
        private int _LocID;
        private string _ConnConfig;
        private string _MSM;
        private string _DSN;
        private string _DBName;
        private string _Script;
        private int ctrlID;
        private string _DeviceID;
        private DateTime _Edate;
        private string _FieldEmp;
        private string _Pager;
        private string _ContactName;
        private string _Supervisor;
        private string _CustomerType;
        private int _Salesperson;
        private bool _SalesAssigned;
        private string _Reg;
        private string _MAPAddress;
        private string _EquipType;
        private string _ServiceType;
        private string _Manufacturer;
        private string _ServiceDate;
        private string _InstallDate;
        private string _Price;
        private string _Unit;
        private string _Cat;
        private string _Serial;
        private string _UniqueID;
        private DateTime _InstallDateTime;
        private DateTime _LastServiceDate;
        private double _EquipPrice;
        private int _EquipID;
        private string _UserLic;
        private int _UserLicID;
        private string _SalesTax;
        private string _SalesDescription;
        private double _SalesRate;
        private int _CatStatus;
        private double _Balance;
        private string _Measure;
        private int _BillCode;
        private string _Stax;
        private int _IsSuper;
        private string _Lat;
        private string _Lng;
        private string _WarehouseID;
        private string _WarehouseName;
        private string _CategoryName;
        private int _CategoryTypeID;
        private int _CategoryCount;
        private string _CategoryRemarks;
        private byte[] _Logo;
        private string _Custom1;
        private string _Custom2;
        private string _ToMail;
        private string _CCMail;
        private int _JobtypeID;
        private string _MailToInv;
        private string _MailCCInv;
        private string _QBCustomerID;
        private string _QBlocationID;
        private DateTime _LastUpdateDate;
        private string _QBCustomerTypeID;
        private string _QBSalesTaxID;
        private int _CustWeb;
        private int _ConsultAPI;
        private string _WorkOrder;
        private string _QBPath;
        private int _Default;
        private string _Category;
        private int _DiagnosticType;
        private int _DepartmentID;
        private string _Lang;
        private int _multiLang;
        private DateTime _InstallDateimport;
        private string _Description;
        private int _QBInteg;
        private string _MerchantID;
        private string _LoginID;
        private string _PaymentUser;
        private string _PaymentPass;
        private int _MerchantInfoId;
        private int _IsTaxable;
        private string _IsTS;
        private string _QBJobtypeID;
        private string _QBInvID;
        private string _QBTermsID;
        private int _TermsID;
        private string _QBAccountID;
        private int _AccountID;
        private string _QBEmployeeID;
        private int _DefaultWorker;
        private int _EmailMS;
        private int _QBFirstSync;
        private DataTable _dtItems;
        private DataTable _dtItemsDeleted;
        private string _Dispatch;
        private int _REPtemplateID;
        public int _Mode;
        private int _ItemsOnly;
        private string _Code;
        private int _YE;
        private int _FChart;
        private int _FGLAdj;
        private int _FDeposit;
        private int _FCustomerPayment;
        private int _FinanStatement;

        private int _addChart;
        private int _editChart;
        private int _viewChart;
        private int _addGLAdj;
        private int _editGLAdj;
        private int _viewGLAdj;
        private int _addDeposit;
        private int _editDeposit;
        private int _viewDeposit;
        private int _addCustomerPayment;
        private int _editCustomerPayment;
        private int _viewCustomerPayment;

        private string _APVendor;
        private string _APBill;
        private int _APBillSelect;
        private string _APBillPay;

        private DateTime _fStart;
        private DateTime _fEnd;
        private DataSet _ds;
        private Int16 _contractBill;        //added by Mayuri 24th dec,15
        private string _gstReg;
        private Int16 _stype;
        private string _PSTReg;
        private int _PageID;
        private int _MSAuthorisedDeviceOnly;
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double RateMileage;
        public int JobId;
        public string _EmNum;
        public string _EmName;
        public string _authdevID;
        private DateTime _lastCloseoutDate;
        private DateTime _lastPeriodCloseoutDate;
        private string _lastCloseoutby;
        private int _CompanyID;
        private DateTime _coDt;
        private int _Payroll;

        public double HolidayAm;
        public double VacAm;
        public double ZoneAm;
        public double ReimbAm;
        public double MilageAm;
        public double BonusAm;
        public double SickAm;
        public int CompanyID
        {
            get { return _CompanyID; }
            set { _CompanyID = value; }
        }
        public int PreferenceID { get; set; }
        public int PreferenceValues { get; set; }
        public int EN { get; set; }
        public int PageID
        {
            get { return _PageID; }
            set { _PageID = value; }
        }
        public int MSAuthorisedDeviceOnly
        {
            get { return _MSAuthorisedDeviceOnly; }
            set { _MSAuthorisedDeviceOnly = value; }
        }
        private string _TermsConditions;
        public string TermsConditions
        {
            get { return _TermsConditions; }
            set { _TermsConditions = value; }
        }
        public DataTable dtDocs { get; set; }

        public int WageID { get; set; }
        public string WageName { get; set; }
        public bool Chargeable { get; set; }
        public Int16 sType
        {
            get { return _stype; }
            set { _stype = value; }
        }
        public string PSTReg
        {
            get { return _PSTReg; }
            set { _PSTReg = value; }
        }
        public string GSTReg
        {
            get { return _gstReg; }
            set { _gstReg = value; }
        }
        public Int16 ContractBill
        {
            get { return _contractBill; }
            set { _contractBill = value; }
        }

        private int _billing; //added by komal 12-23-2015
        private int _Central; //added by komal 12-23-2015

        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string APVendor
        {
            get { return _APVendor; }
            set { _APVendor = value; }
        }
        public string APBill
        {
            get { return _APBill; }
            set { _APBill = value; }
        }
        public int APBillSelect
        {
            get { return _APBillSelect; }
            set { _APBillSelect = value; }
        }
        public string APBillPay
        {
            get { return _APBillPay; }
            set { _APBillPay = value; }
        }

        public DateTime FStart
        {
            get { return _fStart; }
            set { _fStart = value; }
        }
        public DateTime FEnd
        {
            get { return _fEnd; }
            set { _fEnd = value; }
        }

        public DateTime CODt
        {
            get { return _coDt; }
            set { _coDt = value; }
        }

        public int FChart
        {
            get { return _FChart; }
            set { _FChart = value; }
        }
        public int FGLAdj
        {
            get { return _FGLAdj; }
            set { _FGLAdj = value; }
        }
        public int FinanStatement
        {
            get { return _FinanStatement; }
            set { _FinanStatement = value; }
        }
        public int AddChart
        {
            get { return _addChart; }
            set { _addChart = value; }
        }
        public int EditChart
        {
            get { return _editChart; }
            set { _editChart = value; }
        }
        public int ViewChart
        {
            get { return _viewChart; }
            set { _viewChart = value; }
        }
        public int AddGLAdj
        {
            get { return _addGLAdj; }
            set { _addGLAdj = value; }
        }
        public int EditGLAdj
        {
            get { return _editGLAdj; }
            set { _editGLAdj = value; }
        }
        public int ViewGLAdj
        {
            get { return _viewGLAdj; }
            set { _viewGLAdj = value; }
        }
        public int FDeposit
        {
            get { return _FDeposit; }
            set { _FDeposit = value; }
        }
        public int AddDeposit
        {
            get { return _addDeposit; }
            set { _addDeposit = value; }
        }
        public int EditDeposit
        {
            get { return _editDeposit; }
            set { _editDeposit = value; }
        }
        public int ViewDeposit
        {
            get { return _viewDeposit; }
            set { _viewDeposit = value; }
        }
        public int FCustomerPayment
        {
            get { return _FCustomerPayment; }
            set { _FCustomerPayment = value; }
        }
        public int AddCustomerPayment
        {
            get { return _addCustomerPayment; }
            set { _addCustomerPayment = value; }
        }
        public int EditCustomerPayment
        {
            get { return _editCustomerPayment; }
            set { _editCustomerPayment = value; }
        }
        public int ViewCustomerPayment
        {
            get { return _viewCustomerPayment; }
            set { _viewCustomerPayment = value; }
        }
        public int saved { get; set; }

        public int unsaved { get; set; }
        public DataTable dtTicketData { get; set; }
        public int PayMethod { get; set; }
        public double PHours { get; set; }
        public double Salary { get; set; }
        public string Department { get; set; }
        public int RoleID { get; set; }
        public int TransferTimeSheet { get; set; }
        public int TransferInvoice { get; set; }
        public string QbserviceItemlabor { get; set; }
        public string QBserviceItemExp { get; set; }
        public DataTable EmpData { get; set; }
        public string MOMUSer { get; set; }
        public string MOMPASS { get; set; }
        public DateTime Startdt { get; set; }
        public DateTime Enddt { get; set; }
        public string SageLocID { get; set; }
        public string SageCustID { get; set; }

        public DataTable dtGroupdata { get; set; }

        public int MassReview { get; set; }

        public int ProspectID { get; set; }

        public int EstimateID { get; set; }

        public int IsTSDatabase { get; set; }


        public string InServer { get; set; }
        public string InServerType { get; set; }

        public string InUsername { get; set; }

        public string InPassword { get; set; }

        public int InPort { get; set; }

        public string OutServer { get; set; }

        public string OutUsername { get; set; }

        public string OutPassword { get; set; }

        public int OutPort { get; set; }

        public string BccEmail { get; set; }

        public bool SSL { get; set; }

        public bool TakeASentEmailCopy { get; set; }

        public int EmailAccount { get; set; }

        public double HourlyRate { get; set; }

        public int EmpMaintenance { get; set; }

        public int Timestampfix { get; set; }

        public string EmpRefID { get; set; }

        public int PayPeriod { get; set; }

        public double MileageRate { get; set; }
        public string SSN { get; set; }
        public string Sex { get; set; }
        public DateTime DBirth { get; set; }
        public string Race { get; set; }

        private string _QBWageID;

        public int AddEquip { get; set; }

        public int EditEquip { get; set; }

        public int CustomTemplateID { get; set; }

        public DataTable dtcustom { get; set; }

        public DataTable dtCustomValues { get; set; }

        public int InvID { get; set; }


        public string Employee { get; set; }
        public string PRProcess { get; set; }
        public string PRRegister { get; set; }
        public string PRReport { get; set; }
        public string PRWage { get; set; }
        public string PRDeduct { get; set; }
        public bool PR { get; set; }



        public string QBWageID
        {
            get { return _QBWageID; }
            set { _QBWageID = value; }
        }

        private double _SalesAmount;

        public double SalesAmount
        {
            get { return _SalesAmount; }
            set { _SalesAmount = value; }
        }

        private double _AnnualAmount;

        public double AnnualAmount
        {
            get { return _AnnualAmount; }
            set { _AnnualAmount = value; }
        }

        private int _Month;

        public int Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        private int _SalesMgr;

        public int SalesMgr
        {
            get { return _SalesMgr; }
            set { _SalesMgr = value; }
        }

        private string _StartDate;

        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private string _EndDate;

        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private string _CreditReason;

        public string CreditReason
        {
            get { return _CreditReason; }
            set { _CreditReason = value; }
        }
        private int _CreditHold;

        public int CreditHold
        {
            get { return _CreditHold; }
            set { _CreditHold = value; }
        }

        private int _CreditFlag;

        public int CreditFlag
        {
            get { return _CreditFlag; }
            set { _CreditFlag = value; }
        }

        private int _DispAlert;

        public int DispAlert
        {
            get { return _DispAlert; }
            set { _DispAlert = value; }
        }

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public int ItemsOnly
        {
            get { return _ItemsOnly; }
            set { _ItemsOnly = value; }
        }

        public int Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public int REPtemplateID
        {
            get { return _REPtemplateID; }
            set { _REPtemplateID = value; }
        }

        public string Dispatch
        {
            get { return _Dispatch; }
            set { _Dispatch = value; }
        }

        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }

        public DataTable DtItemsDeleted
        {
            get { return _dtItemsDeleted; }
            set { _dtItemsDeleted = value; }
        }

        public int QBFirstSync
        {
            get { return _QBFirstSync; }
            set { _QBFirstSync = value; }
        }

        public int EmailMS
        {
            get { return _EmailMS; }
            set { _EmailMS = value; }
        }

        public int DefaultWorker
        {
            get { return _DefaultWorker; }
            set { _DefaultWorker = value; }
        }

        public string QBEmployeeID
        {
            get { return _QBEmployeeID; }
            set { _QBEmployeeID = value; }
        }

        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public string QBAccountID
        {
            get { return _QBAccountID; }
            set { _QBAccountID = value; }
        }

        public int TermsID
        {
            get { return _TermsID; }
            set { _TermsID = value; }
        }

        public string QBTermsID
        {
            get { return _QBTermsID; }
            set { _QBTermsID = value; }
        }

        public string QBInvID
        {
            get { return _QBInvID; }
            set { _QBInvID = value; }
        }

        public string QBJobtypeID
        {
            get { return _QBJobtypeID; }
            set { _QBJobtypeID = value; }
        }

        public string IsTS
        {
            get { return _IsTS; }
            set { _IsTS = value; }
        }

        public int IsTaxable
        {
            get { return _IsTaxable; }
            set { _IsTaxable = value; }
        }

        public int MerchantInfoId
        {
            get { return _MerchantInfoId; }
            set { _MerchantInfoId = value; }
        }

        public DataTable dtPageData { get; set; }

        public string PaymentPass
        {
            get { return _PaymentPass; }
            set { _PaymentPass = value; }
        }

        public string PaymentUser
        {
            get { return _PaymentUser; }
            set { _PaymentUser = value; }
        }

        public string LoginID
        {
            get { return _LoginID; }
            set { _LoginID = value; }
        }

        public string MerchantID
        {
            get { return _MerchantID; }
            set { _MerchantID = value; }
        }

        public int QBInteg
        {
            get { return _QBInteg; }
            set { _QBInteg = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }

        public int MultiLang
        {
            get { return _multiLang; }
            set { _multiLang = value; }
        }

        public string Lang
        {
            get { return _Lang; }
            set { _Lang = value; }
        }

        public int DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }

        public int DiagnosticType
        {
            get { return _DiagnosticType; }
            set { _DiagnosticType = value; }
        }


        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public int Default
        {
            get { return _Default; }
            set { _Default = value; }
        }

        public string QBPath
        {
            get { return _QBPath; }
            set { _QBPath = value; }
        }

        public string WorkOrder
        {
            get { return _WorkOrder; }
            set { _WorkOrder = value; }
        }

        public int CustWeb
        {
            get { return _CustWeb; }
            set { _CustWeb = value; }
        }

        public int ConsultAPI
        {
            get { return _ConsultAPI; }
            set { _ConsultAPI = value; }
        }
        public int Payroll
        {
            get { return _Payroll; }
            set { _Payroll = value; }
        }

        public string QBSalesTaxID
        {
            get { return _QBSalesTaxID; }
            set { _QBSalesTaxID = value; }
        }

        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }

        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
        }

        public string QBlocationID
        {
            get { return _QBlocationID; }
            set { _QBlocationID = value; }
        }

        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }

        public string MailCCInv
        {
            get { return _MailCCInv; }
            set { _MailCCInv = value; }
        }

        public string MailToInv
        {
            get { return _MailToInv; }
            set { _MailToInv = value; }
        }

        public int JobtypeID
        {
            get { return _JobtypeID; }
            set { _JobtypeID = value; }
        }

        public string CCMail
        {
            get { return _CCMail; }
            set { _CCMail = value; }
        }

        public string ToMail
        {
            get { return _ToMail; }
            set { _ToMail = value; }
        }

        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }

        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }

        public byte[] Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        }
        public int CategoryTypeID
        {
            get { return _CategoryTypeID; }
            set { _CategoryTypeID = value; }
        }
        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }
        public int CategoryCount
        {
            get { return _CategoryCount; }
            set { _CategoryCount = value; }
        }
        public string CategoryRemarks
        {
            get { return _CategoryRemarks; }
            set { _CategoryRemarks = value; }
        }


        public string WarehouseName
        {
            get { return _WarehouseName; }
            set { _WarehouseName = value; }
        }

        public string WarehouseID
        {
            get { return _WarehouseID; }
            set { _WarehouseID = value; }
        }
        public Boolean IsMultiValue { get; set; }

        public int WHLocID { get; set; }
        public Boolean IsEdit { get; set; }
        public String WareHouseLocation { get; set; }

        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        public int IsSuper
        {
            get { return _IsSuper; }
            set { _IsSuper = value; }
        }

        public string Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }

        public int BillCode
        {
            get { return _BillCode; }
            set { _BillCode = value; }
        }

        public string Measure
        {
            get { return _Measure; }
            set { _Measure = value; }
        }

        public double Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }

        public int CatStatus
        {
            get { return _CatStatus; }
            set { _CatStatus = value; }
        }

        public double SalesRate
        {
            get { return _SalesRate; }
            set { _SalesRate = value; }
        }

        public string SalesDescription
        {
            get { return _SalesDescription; }
            set { _SalesDescription = value; }
        }

        public string SalesTax
        {
            get { return _SalesTax; }
            set { _SalesTax = value; }
        }

        public int UserLicID
        {
            get { return _UserLicID; }
            set { _UserLicID = value; }
        }

        public string UserLic
        {
            get { return _UserLic; }
            set { _UserLic = value; }
        }

        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }

        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }

        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }

        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }

        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }

        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }

        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        public string Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public string PriceString { get; set; }

        public string InstallDate
        {
            get { return _InstallDate; }
            set { _InstallDate = value; }
        }

        public string InstallDateString { get; set; }

        public string ServiceDate
        {
            get { return _ServiceDate; }
            set { _ServiceDate = value; }
        }

        public string ServiceDateString { get; set; }

        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }

        public string ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }

        public string EquipType
        {
            get { return _EquipType; }
            set { _EquipType = value; }
        }

        public string MAPAddress
        {
            get { return _MAPAddress; }
            set { _MAPAddress = value; }
        }

        public string Reg
        {
            get { return _Reg; }
            set { _Reg = value; }
        }

        public int Salesperson
        {
            get { return _Salesperson; }
            set { _Salesperson = value; }
        }
        public bool SalesAssigned
        {
            get { return _SalesAssigned; }
            set { _SalesAssigned = value; }
        }
        private bool _NotificationOnAddOpportunity;
        public bool NotificationOnAddOpportunity
        {
            get { return _NotificationOnAddOpportunity; }
            set { _NotificationOnAddOpportunity = value; }
        }
        public bool EstApproveProposal { get; set; }
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }

        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }

        public string ContactName
        {
            get { return _ContactName; }
            set { _ContactName = value; }
        }

        public string Pager
        {
            get { return _Pager; }
            set { _Pager = value; }
        }

        public string FieldEmp
        {
            get { return _FieldEmp; }
            set { _FieldEmp = value; }
        }

        public DateTime Edate
        {
            get { return _Edate; }
            set { _Edate = value; }
        }

        public string DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        public int CtrlID
        {
            get { return ctrlID; }
            set { ctrlID = value; }
        }

        public string Script
        {
            get { return _Script; }
            set { _Script = value; }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public string _DBType { get; set; }
        public string DBType
        {
            get { return _DBType; }
            set { _DBType = value; }
        }
        public string DSN
        {
            get { return _DSN; }
            set { _DSN = value; }
        }

        public string MSM
        {
            get { return _MSM; }
            set { _MSM = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }


        public string RolZip
        {
            get { return _RolZip; }
            set { _RolZip = value; }
        }
        public string RolCountry { get; set; }
        public string RolState
        {
            get { return _RolState; }
            set { _RolState = value; }
        }

        public string RolCity
        {
            get { return _RolCity; }
            set { _RolCity = value; }
        }

        public string RolAddress
        {
            get { return _RolAddress; }
            set { _RolAddress = value; }
        }
        /// <summary>
        /// Default Salesperson
        /// </summary>
        public int Territory
        {
            get { return _Territory; }
            set { _Territory = value; }
        }
        /// <summary>
        /// Second Salesperson
        /// </summary>
        public int Territory2
        {
            get { return _Territory2; }
            set { _Territory2 = value; }
        }

        public int Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        public string Locationname
        {
            get { return _Locationname; }
            set { _Locationname = value; }
        }

        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int Count { get; set; }
        public int sAcct { get; set; }
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }

        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }
        public string QBAccountNumber { get; set; }
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        public int Cust { get; set; }

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        public int TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        public int Mapping
        {
            get { return _Mapping; }
            set { _Mapping = value; }
        }

        public int Schedule
        {
            get { return _Schedule; }
            set { _Schedule = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        public string SearchValueUnAssignedCalls
        {
            get { return _SearchValueUnAssignedCalls; }
            set { _SearchValueUnAssignedCalls = value; }
        }

        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }

        public int EmpId
        {
            get { return _EmpId; }
            set { _EmpId = value; }
        }

        public int WorkId
        {
            get { return _WorkId; }
            set { _WorkId = value; }
        }

        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }

        public string AccessUser
        {
            get { return _AccessUser; }
            set { _AccessUser = value; }
        }

        public string ProgFunctions
        {
            get { return _ProgFunctions; }
            set { _ProgFunctions = value; }
        }

        public string Expenses
        {
            get { return _Expenses; }
            set { _Expenses = value; }
        }

        public string PurchaseOrd
        {
            get { return _PurchaseOrd; }
            set { _PurchaseOrd = value; }
        }

        public string ServiceHist
        {
            get { return _ServiceHist; }
            set { _ServiceHist = value; }
        }

        public string LocationRemarks
        {
            get { return _LocationRemarks; }
            set { _LocationRemarks = value; }
        }

        public string WorkDate
        {
            get { return _WorkDate; }
            set { _WorkDate = value; }
        }

        public string CreateTicket
        {
            get { return _CreateTicket; }
            set { _CreateTicket = value; }
        }

        public DateTime DtFired
        {
            get { return _DtFired; }
            set { _DtFired = value; }
        }

        public DateTime DtHired
        {
            get { return _DtHired; }
            set { _DtHired = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        public string Tele
        {
            get { return _Tele; }
            set { _Tele = value; }
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

        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }

        public string LastNAme
        {
            get { return _LastNAme; }
            set { _LastNAme = value; }
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public int Field
        {
            get { return _Field; }
            set { _Field = value; }
        }


        public int PDA
        {
            get { return _PDA; }
            set { _PDA = value; }
        }

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public int Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }
        public int Central
        {
            get { return _Central; }
            set { _Central = value; }
        }

        public string PageName { get; set; }
        public int grpbyWO { get; set; }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }


        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public int YE
        {
            get { return _YE; }
            set { _YE = value; }
        }

        public int GLAccount { get; set; }
        public int Vendor { get; set; }
        public int UType { get; set; }

        public short openticket { get; set; }

        public DataTable dtAlerts { get; set; }

        public DataTable dtAlertContacts { get; set; }

        public DateTime bend { get; set; }

        public DateTime bstart { get; set; }

        public int UpdateTicket { get; set; }
        public DataTable DtWage
        {
            get { return _dtWage; }
            set { _dtWage = value; }
        }
        private DataTable _dtWage;
        public Int16 JobCostLabor { get; set; }

        public bool TaskCode { get; set; }

        public int codes { get; set; }

        public string SageLocKey { get; set; }
        public string SagejobKey { get; set; }
        public string SageID { get; set; }

        public string BCycle { get; set; }
        public string SCycle { get; set; }

        public DateTime Stime { get; set; }
        public DateTime BStartdt { get; set; }
        public DateTime SStartdt { get; set; }
        public string SageRoute { get; set; }


        public bool MSVisible { get; set; }

        public int DocID { get; set; }

        public DataTable tblGCandHomeOwner { get; set; }
        

        private string _BankReconciliationPermissions;
        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View  Char of JournalEntry
        /// </summary>
        public string BankReconciliationPermissions
        {
            get { return _BankReconciliationPermissions; }
            set { _BankReconciliationPermissions = value; }
        }

        private string _JournalEntryPermissions;
        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View  Char of JournalEntry
        /// </summary>
        public string JournalEntryPermissions
        {
            get { return _JournalEntryPermissions; }
            set { _JournalEntryPermissions = value; }
        }

        /// <summary>
        /// Set =Financial module  For Add / Edit / Delete / View  
        /// </summary>
        private string _Financialmodule;

        public string Financialmodule
        {
            get { return _Financialmodule; }
            set { _Financialmodule = value; }
        }
        private string _ChartPermissions;
        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View  Char of Accounts
        /// </summary>
        public string ChartPermissions
        {
            get { return _ChartPermissions; }
            set { _ChartPermissions = value; }
        }

        private string _CustomerPermissions;
        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Customer
        /// </summary>
        public string CustomerPermissions
        {
            get { return _CustomerPermissions; }
            set { _CustomerPermissions = value; }
        }

        private string _InventoryItemPermissions;
        public string InventoryItemPermissions
        {
            get { return _InventoryItemPermissions; }
            set { _InventoryItemPermissions = value; }
        }
        private string _InventoryAdjustmentPermissions;
        public string InventoryAdjustmentPermissions
        {
            get { return _InventoryAdjustmentPermissions; }
            set { _InventoryAdjustmentPermissions = value; }
        }
        private string _InventoryWarehousePermissions;
        public string InventoryWarehousePermissions
        {
            get { return _InventoryWarehousePermissions; }
            set { _InventoryWarehousePermissions = value; }
        }

        private string _InventorysetupPermissions;
        public string InventorysetupPermissions
        {
            get { return _InventorysetupPermissions; }
            set { _InventorysetupPermissions = value; }
        }

        private string _InventoryFinancePermissions;
        public string InventoryFinancePermissions
        {
            get { return _InventoryFinancePermissions; }
            set { _InventoryFinancePermissions = value; }
        }

        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Apply
        /// </summary>
        private string _ApplyPermissions;

        public string ApplyPermissions
        {
            get { return _ApplyPermissions; }
            set { _ApplyPermissions = value; }
        }

        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View / Report / Approve Online Payment
        /// </summary>
        private string _OnlinePaymentPermissions;

        public string OnlinePaymentPermissions
        {
            get { return _OnlinePaymentPermissions; }
            set { _OnlinePaymentPermissions = value; }
        }

        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Deposit
        /// </summary>
        private string _DepositPermissions;

        public string DepositPermissions
        {
            get { return _DepositPermissions; }
            set { _DepositPermissions = value; }
        }

        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Collections
        /// </summary>
        private string _CollectionsPermissions;

        public string CollectionsPermissions
        {
            get { return _CollectionsPermissions; }
            set { _CollectionsPermissions = value; }
        }

        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Location
        /// </summary>
        private string _LocationrPermissions;

        public string LocationrPermissions
        {
            get { return _LocationrPermissions; }
            set { _LocationrPermissions = value; }
        }


        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Project 
        /// </summary>
        private string _ProjectPermissions;

        public string ProjectPermissions
        {
            get { return _ProjectPermissions; }
            set { _ProjectPermissions = value; }
        }


        /// <summary>
        /// Set Permissions For Add / Edit / Delete / View Project Temp Permissions 
        /// </summary>
        private string _ProjectTempPermissions;

        public string ProjectTempPermissions
        {
            get { return _ProjectTempPermissions; }
            set { _ProjectTempPermissions = value; }
        }

        private string _TicketDelete;

        public string TicketDelete
        {
            get { return _TicketDelete; }
            set { _TicketDelete = value; }
        }

        public int DeleteEquip { get; set; }

        public int ViewEquip { get; set; }

        public bool EmailInvoice { get; set; }

        public bool PrintInvoice { get; set; }

        public string ProjectListPermission { get; set; }

        public string FinancePermission { get; set; }

        public string BOMPermission { get; set; }

        public string WIPPermission { get; set; }

        public string MilestonesPermission { get; set; }

        /// <summary>
        /// Set Document Permissions For Add / Edit / Delete / View  
        /// </summary>
        private string _DocumentPermissions;

        public string DocumentPermissions
        {
            get { return _DocumentPermissions; }
            set { _DocumentPermissions = value; }
        }

        /// <summary>
        /// Set Contact Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _ContactPermission;

        public string ContactPermission
        {
            get { return _ContactPermission; }
            set { _ContactPermission = value; }
        }

        /// <summary>
        /// Set Vendors Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _VendorsPermission;

        public string VendorsPermission
        {
            get { return _VendorsPermission; }
            set { _VendorsPermission = value; }
        }

        /// <summary>
        /// Set Color
        /// </summary>
        private string _Color;

        public string Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        public string building { get; set; }

        private int _ID;
        private string _Body;
        private string _Subject;
        private bool _BitMap;
        private string _BodyMulitple;
        private int _EmailID;
        private string _Fields;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        public bool BitMap
        {
            get { return _BitMap; }
            set { _BitMap = value; }
        }
        public string BodyMulitple
        {
            get { return _BodyMulitple; }
            set { _BodyMulitple = value; }
        }
        public int EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; }
        }
        public string Fields
        {
            get { return _Fields; }
            set { _Fields = value; }
        }
        public decimal POLimit { get; set; }
        public Int16 POApprove { get; set; }
        public Int16? POApproveAmt { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public string authdevID
        {
            get { return _authdevID; }
            set { _authdevID = value; }
        }
        public string EmNum
        {
            get { return _EmNum; }
            set { _EmNum = value; }
        }
        public string EmName
        {
            get { return _EmName; }
            set { _EmName = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public int Consult
        {
            get { return _Consult; }
            set { _Consult = value; }
        }
        public string ProfileImage { get; set; }

        public string CoverImage { get; set; }

        public string STax2 { get; set; }
        public string UTax { get; set; }
        public int Zone { get; set; }
        public int ZoneID { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public double Bonus { get; set; }
        public double Price1 { get; set; }
        public int Tax { get; set; }
        public string ZoneRemarks { get; set; }

        /// <summary>
        ///   Set Invoive Permissions For Add / Edit / Delete / View 
        /// </summary>
        private string _InvoivePermissions;

        public string InvoivePermissions
        {
            get { return _InvoivePermissions; }
            set { _InvoivePermissions = value; }
        }

        /// <summary>
        /// Set PO Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _POPermission;

        public string POPermission
        {
            get { return _POPermission; }
            set { _POPermission = value; }
        }
        /// <summary>
        /// Set Billing Codes Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _BillingCodesPermission;

        public string BillingCodesPermission
        {
            get { return _BillingCodesPermission; }
            set { _BillingCodesPermission = value; }
        }

        /// <summary>
        /// Set Payment History Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _PaymentHistoryPermission;

        public string PaymentHistoryPermission
        {
            get { return _PaymentHistoryPermission; }
            set { _PaymentHistoryPermission = value; }
        }
        /// <summary>
        /// Set _Purchasing module  For Add / Edit / Delete / View  
        /// </summary>
        private string _Purchasingmodule;

        public string Purchasingmodule
        {
            get { return _Purchasingmodule; }
            set { _Purchasingmodule = value; }
        }

        /// <summary>
        /// Set _AccountPayable module  For Add / Edit / Delete / View  
        /// </summary>
        private string _AccountPayablemodule;

        public string AccountPayablemodule
        {
            get { return _AccountPayablemodule; }
            set { _AccountPayablemodule = value; }
        }

        /// <summary>
        /// Set =_Customermodule module  For Add / Edit / Delete / View  
        /// </summary>
        private string _Customermodule;

        public string Customermodule
        {
            get { return _Customermodule; }
            set { _Customermodule = value; }
        }
        /// <summary>
        /// Set Billing module  For Add / Edit / Delete / View  
        /// </summary>
        private string _Billingmodule;

        public string Billingmodule
        {
            get { return _Billingmodule; }
            set { _Billingmodule = value; }
        }

        /// <summary>
        /// Set Billing module  For Add / Edit / Delete / View  
        /// </summary>
        private string _Recurringmodule;

        public string Recurringmodule
        {
            get { return _Recurringmodule; }
            set { _Recurringmodule = value; }
        }

        /// <summary>
        /// Set Recurring TicketsPermission  For Add / Edit / Delete / View  
        /// </summary>
        private string _ProcessT;

        public string ProcessT
        {
            get { return _ProcessT; }
            set { _ProcessT = value; }
        }

        /// <summary>
        /// Set Recurring Contracts Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _RecurringContractsPermission;

        public string RecurringContractsPermission
        {
            get { return _RecurringContractsPermission; }
            set { _RecurringContractsPermission = value; }
        }
        /// <summary>
        /// Recurring Invoices Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _ProcessCPermission;

        public string ProcessC
        {
            get { return _ProcessCPermission; }
            set { _ProcessCPermission = value; }
        }
        /// <summary>
        /// Set Safety Tests Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _SafetyTestsPermission;

        public string SafetyTestsPermission
        {
            get { return _SafetyTestsPermission; }
            set { _SafetyTestsPermission = value; }
        }
        /// <summary>
        /// Set Renew Escalate Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _RenewEscalatePermission;

        public string RenewEscalatePermission
        {
            get { return _RenewEscalatePermission; }
            set { _RenewEscalatePermission = value; }
        }

        private string _ViolationPermission;
        public string ViolationPermission
        {
            get { return _ViolationPermission; }
            set { _ViolationPermission = value; }
        }

        public bool CheckNullCODt { get; set; }
        public int YearEndClose { get; set; }
        public string RetainedGLAcct { get; set; }
        public string CurrentGLAcct { get; set; }
        public string LastDate { get; set; }
        public string LastPeriod { get; set; }

        public string Classification{get;set;}

        public bool Shutdown { get; set; }

        public int ShutdownAlert { get; set; }

        public string ShutdownReason { get; set; }

        public string ShutdownLongDesc { get; set; }

        public bool PlannedShutdown { get; set; }
        /// <summary>
        /// Set Receive PO Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _RPOPermission;

        public string RPOPermission
        {
            get { return _RPOPermission; }
            set { _RPOPermission = value; }
        }

        public bool IsActiveInactive { get; set; }
        public bool NoCustomerStatement { get; set; }
        public bool IsLeadEquip { get; set; }

        /// <summary>
        /// Set Schedule module View  
        /// </summary>
        private string _Schedulemodule;

        public string Schedulemodule
        {
            get { return _Schedulemodule; }
            set { _Schedulemodule = value; }
        }
        /// <summary>
        /// Set Ticket Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _ScheduleBoardPermission;

        public string ScheduleBoardPermission
        {
            get { return _ScheduleBoardPermission; }
            set { _ScheduleBoardPermission = value; }
        }
        
        /// <summary>
        /// Set Ticket Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _TicketPermission;

        public string TicketPermission
        {
            get { return _TicketPermission; }
            set { _TicketPermission = value; }
        }
        /// <summary>
        /// Set Manual Timesheet Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _MTimesheetPermission;

        public string MTimesheetPermission
        {
            get { return _MTimesheetPermission; }
            set { _MTimesheetPermission = value; }
        }

        /// <summary>
        /// Set e-Timesheet Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _ETimesheetPermission;

        public string ETimesheetPermission
        {
            get { return _ETimesheetPermission; }
            set { _ETimesheetPermission = value; }
        }

        /// <summary>
        /// Set Map  Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _MapRPermission;

        public string MapRPermission
        {
            get { return _MapRPermission; }
            set { _MapRPermission = value; }
        }
        /// <summary>
        /// Set RouteBuilder,  Permission  For Add / Edit / Delete / View  
        /// </summary>
        private string _RouteBuilderPermission;

        public string RouteBuilderPermission
        {
            get { return _RouteBuilderPermission; }
            set { _RouteBuilderPermission = value; }
        }
        /// <summary>
        /// Set Schedule module View  
        /// </summary>
        private string _MassResolvePDATickets;

        public string MassTimesheetCheck
        {
            get { return _MassResolvePDATickets; }
            set { _MassResolvePDATickets = value; }
        }

        /// <summary>
        /// Set _TicketResolvedPermission For Add / Edit / Delete / View  
        /// </summary>
        private string _TicketResolvedPermission;

        public string TicketResolvedPermission
        {
            get { return _TicketResolvedPermission; }
            set { _TicketResolvedPermission = value; }
        }

        /// <summary>
        /// Set _TimeStamsFixedPermission For Add / Edit / Delete / View  
        /// </summary>
        private string _TimeStamsFixedPermission;

        public string TimeStamsFixedPermission
        {
            get { return _TimeStamsFixedPermission; }
            set { _TimeStamsFixedPermission = value; }
        }
        /// <summary>
        /// Set CreditHold Permission For Add / Edit / Delete / View  
        /// </summary>
        private string _CreditHoldPermission;

        public string CreditHoldPermission
        {
            get { return _CreditHoldPermission; }
            set { _CreditHoldPermission = value; }
        }

        /// <summary>
        /// Set CreditFlag Permission For Add / Edit / Delete / View 
        /// </summary>
        private string _CreditFlagPermission;

        public string CreditFlagPermission
        {
            get { return _CreditFlagPermission; }
            set { _CreditFlagPermission = value; }
        }

        public String CoCode;

        public string GridId { get; set; }

        /// <summary>
        /// Set Sales For Add / Edit / Delete / View  
        /// </summary>
        private string _SalesPermission;

        public string SalesPermission
        {
            get { return _SalesPermission; }
            set { _SalesPermission = value; }
        }

        /// <summary>
        /// Set Tasks For Add / Edit / Delete / View  
        /// </summary>
        private int _TasksPermission;

        public int TasksPermission
        {
            get { return _TasksPermission; }
            set { _TasksPermission = value; }
        }

        /// <summary>
        /// Set CompleteTasksPermission For Add / Edit / Delete / View  
        /// </summary>
        private int _CompleteTasksPermission;

        public int CompleteTasksPermission
        {
            get { return _CompleteTasksPermission; }
            set { _CompleteTasksPermission = value; }
        }

        /// <summary>
        /// Set FollowUpPermission For Add / Edit / Delete / View  
        /// </summary>
        private string _FollowUpPermission;

        public string FollowUpPermission
        {
            get { return _FollowUpPermission; }
            set { _FollowUpPermission = value; }
        }
        /// <summary>
        /// Set Proposal Permission For Add / Edit / Delete / View  
        /// </summary>
        private string _ProposalPermission;

        public string ProposalPermission
        {
            get { return _ProposalPermission; }
            set { _ProposalPermission = value; }
        }
        /// <summary>
        /// Set Estimate  Permission For Add / Edit / Delete / View  
        /// </summary>
        private string _EstimatePermission;

        public string EstimatePermission
        {
            get { return _EstimatePermission; }
            set { _EstimatePermission = value; }
        }
        /// <summary>
        /// Set ConvertEstimatePermission   For Add / Edit / Delete / View  
        /// </summary>
        private string _ConvertEstimatePermission;

        public string ConvertEstimatePermission
        {
            get { return _ConvertEstimatePermission; }
            set { _ConvertEstimatePermission = value; }
        }
        /// <summary>
        /// Set SalesSetupPermission   For Add / Edit / Delete / View  
        /// </summary>
        private string _SalesSetupPermission;

        public string SalesSetupPermission
        {
            get { return _SalesSetupPermission; }
            set { _SalesSetupPermission = value; }
        }

        public string PONotification { get; set; }

        public string Projectmodule { get; set; }

        public string Inventorymodule { get; set; }

        public string JobClosePermission { get; set; }

        public string JobCompletedPermission { get; set; }

        public string JobReopenPermission { get; set; }

        public bool inclInactive;
        public string wirteOff { get; set; }

        public bool IsProjectManager { get; set; }
        public bool IsAssignedProject { get; set; }
        public int BusinessTypeID { get; set; }

        public int TargetHPermission { get; set; }
        public bool ScheduleCategoryStatus { get; set; }

        public bool ApplyPasswordRules { get; set; }
        public bool ApplyPwRulesToFieldUser { get; set; }
        public bool ApplyPwRulesToOfficeUser { get; set; }
        public bool ApplyPwRulesToCustomerUser { get; set; }
        public bool ApplyPwResetDays { get; set; }
        public int? PwResetDays { get; set; }
        public int? PwResetting { get; set; }
        public string EmailAdministrator { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int LoginFailedAttempts { get; set; }
        public string NewPassword { get; set; }
        public int TicketVoidPermission { get; set; }

        public string MassPayrollTicket { get; set; }
        
        public int ForgotPwRequest { get; set; }
        public int ApplyUserRolePermission { get; set; }

        //New Parameter For API
        public string query;

        public bool IsReCalculateLaborExpense { get; set; }
        public string oldClassification { get; set; }

        public DataTable Cus_UserCustomValue;
        public List<NotificationUserCustomChange> Cus_EmailToTeamMember;
        public List<NotificationUserCustomChange> Cus_CreateTask;

    }


    public class ActualvsBudgetDataResponse
    {
        public double NTotal { get; set; }
        public double NBudget { get; set; }
        public string NMonth { get; set; }
        public int TypeValue { get; set; }
        public DateTime? Period { get; set; }
    }
    public class UpdateDashboardDockStatesParam
    {
        public int DashboardId { get; set; }
        public string ConnString { get; set; }
        public string DockStates { get; set; }
    }
    public class AvgEstimateResponse
    {
        public double avgIYear { get; set; }
        public double avgIIYear { get; set; }
        public string result { get; set; }
    }

    public class EquipmentTypeCountResponse
    {
        public string TypeName { get; set; }
        public int  Total { get; set; }
    }

    public class EquipmentBuildingCountResponse
    {
        public string Building { get; set; }
        public int Total { get; set; }
    }
    public class RecurringHoursChartResponse
    {
        public List<LeadAverageResponse> Table1 { get; set; }
        public List<LeadAverageResponse> Table2 { get; set; }
    }
    public class LeadAverageResponse
    {
        public string SalesPerson { get; set; }
        public double Avg { get; set; }
    }
    public class RecurringChartResponse
    {
        public List<ResponseTable1> ResponseTable1 { get; set; }
        public List<ResponseTable2> ResponseTable2 { get; set; }
    }

    public class RecurringHoursRemainingResponse
    {
        public string Category { get; set; }
        public double TotalHours { get; set; }
        public double Completed { get; set; }
        public double Open { get; set; }
    }

    public class TroubleCallsEquipmentResponse
    {
        public string Unit { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }
        public string ContractNumber { get; set; }
        public string CType { get; set; }
    }

    public class BudgetResponse
    {
        public int BudgetID { get; set; }
        public string Budget { get; set; }
    }
    public class GetCategoryTicketCountParam 
    {
        private DateTime _StartDate;
        private DateTime _EndDate;
        private int _UserID;
        private int _EN;
        private string _Categories;
        private string _ConnConfig;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public int EN
        {
            get { return _EN; }
            set { _EN = value; }
        }
        public string Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }        

    }
    public class CategoryTicketCountResponse
    {
        public string Category { get; set; }
        public string TicketCount { get; set; }
    }
    


    public class CategoryResponse
    {
        public string Status { get; set; }
        public string type { get; set; }
       
    }

    public class RevenueResponse
    {
        public string Department { get; set; }
        public string Revenue { get; set; }
    }
    public class CompanyResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int OfficeID { get; set; }
        public string IsSel { get; set; }
    }

    
    public class RouteHour
    {
        public double Total { get; set; }
        public string Route { get; set; }
    }
    public class ResponseTable1
    {
        public string SalesPerson { get; set; }
        public double Avg { get; set; }
    }
    public class ResponseTable2
    {   public string SalesPerson { get; set; }
        public double Avg { get; set; }
    }
    public class GetControlBranchParam
    {
        private string _ConnConfig;
        private int _LocID;
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCheckTemplateParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string MOMUSer { get; set; }
    }

    public class updateCheckTemplateParam
    {
        private string _ConnConfig;
        private string _UserLic;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string MOMUSer { get; set; }
        public string UserLic
        {
            get { return _UserLic; }
            set { _UserLic = value; }
        }
    }


    public class GetUserByIdParam
    {
        private string _ConnConfig;
        private int _TypeID;
        private int _UserID;
        private string _DBName;
        private DataSet _dsUserAuthorization;

        public int TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }
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
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

    }

    public class getTimesheetParam
    {
        private string _ConnConfig;
        private string _Supervisor;
        private int _Status;
        private DataSet _dsUserAuthorization;
        private int _WorkId;
        private int _DepartmentID;
        private int _UserID;
        private int _EmpId;
        private DateTime _fStart;
        private DateTime _fEnd;

        public DateTime Startdt { get; set; }
        public DateTime Enddt { get; set; }
        public int EN { get; set; }
        public int saved { get; set; }
        public int unsaved { get; set; }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

        public int WorkId
        {
            get { return _WorkId; }
            set { _WorkId = value; }
        }
        public int DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int EmpId
        {
            get { return _EmpId; }
            set { _EmpId = value; }
        }

        public DateTime FStart
        {
            get { return _fStart; }
            set { _fStart = value; }
        }
        public DateTime FEnd
        {
            get { return _fEnd; }
            set { _fEnd = value; }
        }

    }


    public class getConnectionConfigParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _DBName;

        public String CoCode;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class AddTimesheetParam
    {
        private string _ConnConfig;
        private int _IsSuper;
        public DateTime Startdt { get; set; }
        public DateTime Enddt { get; set; }
        public DataTable EmpData { get; set; }

        public DataTable dtTicketData { get; set; }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int IsSuper
        {
            get { return _IsSuper; }
            set { _IsSuper = value; }
        }

    }

    public class AddUserParam
    {
        private string _ConnConfig;
        private string _Username;
        private string _Supervisor;
        private DataSet _dsUserAuthorization;

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

    }

    public class getVendorTypeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetStockReportsParam
    {
        private string _ConnConfig;
        private string _DBName;
        private int _UserID;
        private string _Type;
        private DataSet _dsUserAuthorization;
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
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public int ID { get; set; }
        public string ReportName { get; set; }
        public bool IsGlobal { get; set; }



    }

    public class GetTermsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class getUseTaxParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class getSTaxParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetCompanyDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetInvDefaultAcctParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetBillReportFiltersValueParam
    {
        private string _ConnConfig;
        private string _DBName;
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
    }
    public class GetBillDetailsParam
    {
        private string _ConnConfig;
        private string _DBName;
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
    }
    public class GetAccountSummaryListingDetailParam
    {
        private string _ConnConfig;
        private string _DBName;
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
    }

    public class GetOwnersParam
    {
        private string _ConnConfig;
        private string _DBName;
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
        public string query;
    }

    public class UpdateDocInfoParam
    {
        private string _ConnConfig;
        private string _Username;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable dtDocs { get; set; }
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
    }

    public class GetInventoryActiveWarehouseParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EN { get; set; }

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

    }


    public class DeleteInventoryWareHouseParam
    {
        private int _AccountID;
        private string _WarehouseID;
        private string _ConnConfig;
        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }

        public string WarehouseID
        {
            get { return _WarehouseID; }
            set { _WarehouseID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetDynamicReportsParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _DsCustomer; 
        private int _UserID;
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
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string Type { get; set; }

    }

    public class Dashboard
    {
        public int _ID;
        private int _UserID;
        private string _Name;
        private string _DockStates;
        private bool _IsDefault;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string DockStates
        {
            get { return _DockStates; }
            set { _DockStates = value; }
        }
        public Boolean IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }
    }
    public class GetDashboardParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private int _DashboardId { get; set; }
        private int _UserId { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int DashboardId
        {
            get { return _DashboardId; }
            set { _DashboardId = value; }
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
    }
    
    public class QBLastSyncResParam
    {
        private DateTime _QBLastSync { get; set; }        
        private int _QbIntegration { get; set; }
       
        public DateTime QBLastSync
        {
            get { return _QBLastSync; }
            set { _QBLastSync = value; }
        }
        public int QbIntegration
        {
            get { return _QbIntegration; }
            set { _QbIntegration = value; }
        }
        
    }
    public class KPI
    {
        private int _ID;
        private string _Name;
        private string _Module;
        private string _Screen;
        private int _Type;
        private string _UserControl;
        
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }
        public string Screen
        {
            get { return _Screen; }
            set { _Screen = value; }
        }
        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string UserControl
        {
            get { return _UserControl; }
            set { _UserControl = value; }
        }
    }
    public class GetListDashKPIParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int _dashboardId { get; set; }
        public int dashboardId
        {
            get { return _dashboardId; }
            set { _dashboardId = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetEMPParam
    {
        private string _ConnConfig;
        private int _Status;
        private string _Username; 
        private string _Supervisor;
        private int _WorkId;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; }
        }
        public int WorkId
        {
            get { return _WorkId; }
            set { _WorkId = value; }
        }

    }

    public class GetCustReportFiltersValueParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _DBName;
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
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class getCustomerDetailsTestParam
    {
        private string _ConnConfig;
        private string _DBName;
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

    public class DeleteCustomerParam
    {
        private int _CustomerID;
        private string _ConnConfig;
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class AddQBCustomerTypeParam
    {
        private string _ConnConfig;
        private string _QBCustomerTypeID;
        private string _CustomerType;
        private string _Remarks;
        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

    }

    public class GetMSMCustomertypeParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateQBcustomertypeIDParam
    {
        private string _ConnConfig;
        private string _QBCustomerTypeID;
        private string _CustomerType;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
    }

    public class AddQBLocTypeParam
    {
        private string _CustomerType;
        private string _Remarks;
        private string _QBCustomerTypeID;
        private string _ConnConfig;
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetMSMLoctypeParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateQBJobtypeIDParam
    {
        private string _ConnConfig;
        private string _QBCustomerTypeID;
        private string _CustomerType;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string QBCustomerTypeID
        {
            get { return _QBCustomerTypeID; }
            set { _QBCustomerTypeID = value; }
        }
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
    }

    public class AddQBSalesTaxParam
    {
        private string _QBSalesTaxID;
        private string _SalesTax;
        private string _SalesDescription;
        private double _SalesRate;
        private string _State;
        private int _IsTaxable;
        private string _Remarks;
        private DateTime _LastUpdateDate;
        private string _ConnConfig;
        public string QBSalesTaxID
        {
            get { return _QBSalesTaxID; }
            set { _QBSalesTaxID = value; }
        }
        public string SalesTax
        {
            get { return _SalesTax; }
            set { _SalesTax = value; }
        }
        public string SalesDescription
        {
            get { return _SalesDescription; }
            set { _SalesDescription = value; }
        }
        public double SalesRate
        {
            get { return _SalesRate; }
            set { _SalesRate = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public int IsTaxable
        {
            get { return _IsTaxable; }
            set { _IsTaxable = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetMSMSalesTaxParam
    {
        private string _SearchValue;
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateQBsalestaxIDParam
    {
        private string _QBSalesTaxID;
        private string _SalesTax;
        private string _ConnConfig;
        public string SalesTax
        {
            get { return _SalesTax; }
            set { _SalesTax = value; }
        }
        public string QBSalesTaxID
        {
            get { return _QBSalesTaxID; }
            set { _QBSalesTaxID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class AddCustomerQBParam
    {
        private string _Username;
        private string _Password;
        private int _Status;
        private string _FirstName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Remarks;
        private int _Mapping;
        private string _Country;
        private int _Schedule;
        private int _Internet;
        private string _MainContact;
        private string _Phone;
        private string _Website;
        private string _Cell;
        private string _Email;
        private string _Type;
        private string _QBCustomerID;
        private DateTime _LastUpdateDate;
        private double _Balance;
        private string _ConnConfig;
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
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public int Mapping
        {
            get { return _Mapping; }
            set { _Mapping = value; }
        }
        public int Schedule
        {
            get { return _Schedule; }
            set { _Schedule = value; }
        }
        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
        }
        public double Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class AddQBLocationParam
    {
        private string _AccountNo;
        private string _Locationname;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Remarks;
        private int _Status;
        private string _MainContact;
        private string _Phone;
        private string _RolAddress;
        private string _RolCity;
        private string _RolState;
        private string _RolZip;
        private string _Fax;
        private string _Cell;
        private string _Email;
        private int _CustomerID;
        private string _QBCustomerID;
        private string _QBlocationID;
        private DateTime _LastUpdateDate;
        private string _Type;
        private double _Balance;
        private string _Stax;
        private string _ConnConfig;
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }
        public string Locationname
        {
            get { return _Locationname; }
            set { _Locationname = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public string RolAddress
        {
            get { return _RolAddress; }
            set { _RolAddress = value; }
        }
        public string RolCity
        {
            get { return _RolCity; }
            set { _RolCity = value; }
        }
        public string RolZip
        {
            get { return _RolZip; }
            set { _RolZip = value; }
        }
        public string RolState
        {
            get { return _RolState; }
            set { _RolState = value; }
        }
        public string QBlocationID
        {
            get { return _QBlocationID; }
            set { _QBlocationID = value; }
        }
        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }
        public double Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetMSMCustomersParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateQBCustomerIDParam
    {
        private string _ConnConfig;
        private int _CustomerID;
        private string _QBCustomerID;
        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
    }

    public class GetQBCustomersParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetMSMLocationParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateQBLocationIDParam
    {
        private string _QBlocationID;
        private int _LocID;
        private string _ConnConfig;
        public string QBlocationID
        {
            get { return _QBlocationID; }
            set { _QBlocationID = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetQBLocationParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCustomerSearchParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _SearchBy;
        private string _SearchValue;
        private string _DBName;
        private int _UserID;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public int EN { get; set; }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public bool inclInactive { get; set; }
        public Int32 IsSalesAsigned { get; set; }
    }

    public class GetCustomersParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _DBName;
        private int _UserID;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
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
        public int EN { get; set; }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public bool inclInactive { get; set; }
        public Int32 IsSalesAsigned { get; set; }

    }

    public class getCustomerTypeParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetCustomerByIDParam
    {
        private int _CustomerID;
        private string _DBName;
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
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
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public Int32 IsSalesAsigned { get; set; }
    }

    public class UpdateCustomerParam
    {
        private string _Username;
        private string _Password;
        private int _Status;
        private string _FirstName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Country;
        private string _Remarks;
        private int _Schedule;
        private int _Mapping;
        private int _CustomerID;
        private DataTable _ContactData;
        private int _Internet;
        private string _MainContact;
        private string _Phone;
        private string _Website;
        private string _Cell;
        private string _Email;
        private string _Type;
        private int _EquipID;
        private string _AccountNo;
        private int _billing;
        private int _Central;
        private string _Fax;
        private string _Lat;
        private string _Lng;
        private string _Custom1;
        private string _Custom2;
        private string _ConnConfig;
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
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public int Mapping
        {
            get { return _Mapping; }
            set { _Mapping = value; }
        }
        public int Schedule
        {
            get { return _Schedule; }
            set { _Schedule = value; }
        }
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }
        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }

        public int Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }
        public int Central
        {
            get { return _Central; }
            set { _Central = value; }
        }
        public int grpbyWO { get; set; }
        public DataTable dtDocs { get; set; }
        public short openticket { get; set; }
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double MileageRate { get; set; }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public int EN { get; set; }
        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }
        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }
        public string MOMUSer { get; set; }
        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }
        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public bool CopyToLocAndJob;
        public int IsTSDatabase { get; set; }
        public int ShutdownAlert { get; set; }       
        public string ShutdownReason { get; set; }
       
    }

    public class AddCustomerParam
    {
        private string _Username;
        private string _Password;
        private int _Status;
        private string _FirstName;
        private string _Address;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Country;
        private string _Remarks;
        private int _Schedule;
        private int _Mapping;
        private int _Internet;
        private string _MainContact;
        private string _Phone;
        private string _Website;
        private string _Cell;
        private string _Email;
        private string _Type;
        private int _EquipID;
        private string _AccountNo;
        private int _billing;
        private string _Fax;
        private string _Lat;
        private string _Lng;
        private string _Custom1;
        private string _Custom2;
        private int _CustomerID;
        private string _ConnConfig;
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
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public int Mapping
        {
            get { return _Mapping; }
            set { _Mapping = value; }
        }
        public int Schedule
        {
            get { return _Schedule; }
            set { _Schedule = value; }
        }
        public int Internet
        {
            get { return _Internet; }
            set { _Internet = value; }
        }
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }

        public int Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }
        public int grpbyWO { get; set; }
        public short openticket { get; set; }
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double MileageRate { get; set; }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        public int EN { get; set; }
        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }
        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }
        public string MOMUSer { get; set; }
        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }
        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public int ShutdownAlert { get; set; }
        public string ShutdownReason { get; set; }
    }

    public class UpdateCustomerContactRecordLogParam
    {
        private DataTable _ContactData;
        private int _RolId;
        private string _ConnConfig;
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }
        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }
        public string MOMUSer { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class DeleteLocationParam
    {
        private int _LocID;
        private string _ConnConfig;
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetElevParam
    {
        private string _SearchBy;
        private string _SearchValue;
        private int _UserID;
        private string _InstallDate;
        private string _ServiceDate;
        private string _Manufacturer;
        private string _Price;
        private int _LocID;
        private int _CustomerID;
        private string _Category;
        private string _Type;
        private int _Status;
        private DataSet _dsUserAuthorization;
        private int _EmpId;
        private string _ConnConfig;
        public int EN { get; set; }
        public Int32 IsSalesAsigned { get; set; }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string InstallDate
        {
            get { return _InstallDate; }
            set { _InstallDate = value; }
        }
        public string InstallDateString { get; set; }
        public string ServiceDate
        {
            get { return _ServiceDate; }
            set { _ServiceDate = value; }
        }
        public string ServiceDateString { get; set; }
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        public string Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        public string PriceString { get; set; }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public int RoleID { get; set; }
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Classification { get; set; }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string building { get; set; }
        public bool IsAssignedProject { get; set; }

        public int EmpId
        {
            get { return _EmpId; }
            set { _EmpId = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class DeleteEquipmentParam
    {
        private string _ConnConfig;
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCategoryParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetDepartmentParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _DBName;

        public String CoCode;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetLocationByCustomerIDParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _DBName;
        private int _CustomerID;

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }

        public Int32 IsSalesAsigned { get; set; }
    }

    public class GetCustomersLogsParam
    {
        private int _CustomerID; 
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
    }

    public class GetContactLogByCustomerIDParam
    {
        private int _LocID;
        private string _ConnConfig;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }
    public class GetContactByRolIDParam
    {
        private int _RolId;
        private string _ConnConfig;
        private string _DBName;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }

        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        
    }

    public class GetZoneParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetTerritoryParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private int _LocID;
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public Int32 IsSalesAsigned { get; set; }
        public int EstimateID { get; set; }
        public int OpportunityID { get; set; }
    }


    public class GetLocationDataSearchParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _SearchBy;
        private string _SearchValue;
        private string _DBName;
        private int _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int32 IsSalesAsigned { get; set; }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public int EN { get; set; }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public bool inclInactive;
    }

    public class GetLocationsDataParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _SearchBy;
        private string _SearchValue;
        private string _DBName;
        private int _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int32 IsSalesAsigned { get; set; }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public int EN { get; set; }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public bool inclInactive;
    }

    public class GetLocationTypeParam
    {
        private string _ConnConfig;
        private string _Type;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
    public class getlocationTypeParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }
    public class GetRouteParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public int IsActive { get; set; }
        public Int32 LocID { get; set; }
        public Int32 ContractID { get; set; }
    }

    public class GetLocationByIDParam
    {
        private string _ConnConfig;
        private int _LocID;
        private DataSet _dsUserAuthorization;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetGCandHowerLocIDParam
    {
        private string _ConnConfig;
        private int _LocID;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class getSalesTax2Param
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateLocationParam
    {
        private string _Locationname;
        private string _Address; 
        private int _Status; 
        private string _AccountNo; 
        private string _City;
        private string _State;
        private string _Zip;
        private int _Territory;
        private int _Territory2;
        private string _RolAddress;
        private string _RolCity;
        private string _RolState;
        private string _RolZip;
        private string _Fax;
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }
        public string Locationname
        {
            get { return _Locationname; }
            set { _Locationname = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
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
        public int Territory
        {
            get { return _Territory; }
            set { _Territory = value; }
        }

        private int _Route;
        public int Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _MainContact;
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Cell;
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        private string _Website;
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        public string RolState
        {
            get { return _RolState; }
            set { _RolState = value; }
        }
        public string RolCity
        {
            get { return _RolCity; }
            set { _RolCity = value; }
        }
        public string RolAddress
        {
            get { return _RolAddress; }
            set { _RolAddress = value; }
        }
        public string RolZip
        {
            get { return _RolZip; }
            set { _RolZip = value; }
        }

        private DataTable _ContactData;
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private int _LocID;
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        private int _CustomerID;
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        private string _Stax;
        public string Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }

        private string _Lng;
        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        private string _Lat;
        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        private string _Custom2;
        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }

        private string _Custom1;
        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }

        private string _CCMail;
        public string CCMail
        {
            get { return _CCMail; }
            set { _CCMail = value; }
        }

        private string _ToMail;
        public string ToMail
        {
            get { return _ToMail; }
            set { _ToMail = value; }
        }

        private string _MailCCInv;
        public string MailCCInv
        {
            get { return _MailCCInv; }
            set { _MailCCInv = value; }
        }

        private string _MailToInv;
        public string MailToInv
        {
            get { return _MailToInv; }
            set { _MailToInv = value; }
        }

        private int _CreditHold;
        public int CreditHold
        {
            get { return _CreditHold; }
            set { _CreditHold = value; }
        }

        private int _CreditFlag;
        public int CreditFlag
        {
            get { return _CreditFlag; }
            set { _CreditFlag = value; }
        }

        private int _DispAlert;
        public int DispAlert
        {
            get { return _DispAlert; }
            set { _DispAlert = value; }
        }

        private string _CreditReason;
        public string CreditReason
        {
            get { return _CreditReason; }
            set { _CreditReason = value; }
        }

        private Int16 _contractBill;
        public Int16 ContractBill
        {
            get { return _contractBill; }
            set { _contractBill = value; }
        }

        private int _TermsID;
        public int TermsID
        {
            get { return _TermsID; }
            set { _TermsID = value; }
        }
        public DataTable dtDocs { get; set; }
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double MileageRate { get; set; }
        public DataTable dtAlerts { get; set; }
        public DataTable dtAlertContacts { get; set; }
        public DataTable tblGCandHomeOwner { get; set; }
        public bool EmailInvoice { get; set; }
        public bool PrintInvoice { get; set; }
        public bool CopyToLocAndJob;
        public int Territory2
        {
            get { return _Territory2; }
            set { _Territory2 = value; }
        }
        public string STax2 { get; set; }
        public string UTax { get; set; }
        public int Zone { get; set; }
        public string MOMUSer { get; set; }

        private int _Consult;
        public int Consult
        {
            get { return _Consult; }
            set { _Consult = value; }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string RolCountry { get; set; }
        public bool NoCustomerStatement { get; set; }
        public int BusinessTypeID { get; set; }
        public int ApplyServiceTypeRule;
        public string ServiceTypeName;
        public int ProjectPerDepartmentCount;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private string _MAPAddress;
        public string MAPAddress
        {
            get { return _MAPAddress; }
            set { _MAPAddress = value; }
        }
    }

    public class AddLocationParam
    {
        private string _Locationname;
        private string _Address;
        private int _Status;
        private string _AccountNo;
        private string _City;
        private string _State;
        private string _Zip;
        private int _Territory;
        private int _Territory2;
        private string _RolAddress;
        private string _RolCity;
        private string _RolState;
        private string _RolZip;
        private string _Fax;
        public string AccountNo
        {
            get { return _AccountNo; }
            set { _AccountNo = value; }
        }
        public string Locationname
        {
            get { return _Locationname; }
            set { _Locationname = value; }
        }
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
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
        public int Territory
        {
            get { return _Territory; }
            set { _Territory = value; }
        }

        private int _Route;
        public int Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _MainContact;
        public string MainContact
        {
            get { return _MainContact; }
            set { _MainContact = value; }
        }
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _Cell;
        public string Cell
        {
            get { return _Cell; }
            set { _Cell = value; }
        }

        private string _Website;
        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }
        public string RolState
        {
            get { return _RolState; }
            set { _RolState = value; }
        }
        public string RolCity
        {
            get { return _RolCity; }
            set { _RolCity = value; }
        }
        public string RolAddress
        {
            get { return _RolAddress; }
            set { _RolAddress = value; }
        }
        public string RolZip
        {
            get { return _RolZip; }
            set { _RolZip = value; }
        }

        private DataTable _ContactData;
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private int _LocID;
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        private int _CustomerID;
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        private string _Stax;
        public string Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }

        private string _Lng;
        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        private string _Lat;
        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        private string _Custom2;
        public string Custom2
        {
            get { return _Custom2; }
            set { _Custom2 = value; }
        }

        private string _Custom1;
        public string Custom1
        {
            get { return _Custom1; }
            set { _Custom1 = value; }
        }

        private string _ToMail;
        public string ToMail
        {
            get { return _ToMail; }
            set { _ToMail = value; }
        }
        private string _MailCCInv;
        public string MailCCInv
        {
            get { return _MailCCInv; }
            set { _MailCCInv = value; }
        }

        private string _MailToInv;
        public string MailToInv
        {
            get { return _MailToInv; }
            set { _MailToInv = value; }
        }

        private int _CreditHold;
        public int CreditHold
        {
            get { return _CreditHold; }
            set { _CreditHold = value; }
        }

        private int _CreditFlag;
        public int CreditFlag
        {
            get { return _CreditFlag; }
            set { _CreditFlag = value; }
        }

        private int _DispAlert;
        public int DispAlert
        {
            get { return _DispAlert; }
            set { _DispAlert = value; }
        }

        private string _CreditReason;
        public string CreditReason
        {
            get { return _CreditReason; }
            set { _CreditReason = value; }
        }
        public int ProspectID { get; set; }

        public int EstimateID { get; set; }
        private Int16 _contractBill;
        public Int16 ContractBill
        {
            get { return _contractBill; }
            set { _contractBill = value; }
        }

        private int _TermsID;
        public int TermsID
        {
            get { return _TermsID; }
            set { _TermsID = value; }
        }
        public double BillRate;
        public double RateOT;
        public double RateNT;
        public double RateDT;
        public double RateTravel;
        public double MileageRate { get; set; }
        public DataTable tblGCandHomeOwner { get; set; }
        public bool EmailInvoice { get; set; }
        public bool PrintInvoice { get; set; }
        public int EN { get; set; }
        public int Territory2
        {
            get { return _Territory2; }
            set { _Territory2 = value; }
        }
        public string STax2 { get; set; }
        public string UTax { get; set; }
        public int Zone { get; set; }
        public string MOMUSer { get; set; }

        private int _Consult;
        public int Consult
        {
            get { return _Consult; }
            set { _Consult = value; }
        }

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        public string RolCountry { get; set; }
        public bool NoCustomerStatement { get; set; }
        public int BusinessTypeID { get; set; }
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private string _MAPAddress;
        public string MAPAddress
        {
            get { return _MAPAddress; }
            set { _MAPAddress = value; }
        }
    }

    public class UpdateLocationContactRecordLogParam
    {
        private string _ConnConfig;
        private DataTable _ContactData;
        private int _RolId;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }
        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }
        public string MOMUSer { get; set; }
    }

    public class GetDefaultRouteTerrParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetGCCustomerParam
    {
        private string _ConnConfig;
        private int _CustomerID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
    }
    
    public class GetLocationLogParam
    {
        private string _ConnConfig;
        private int _LocID;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetContactLogByLocIDParam
    {
        private string _ConnConfig;
        private int _LocID;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetLocContactByRolIDParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        private int _RolId;
        public int RolId
        {
            get { return _RolId; }
            set { _RolId = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class spGetLocationServiceTypeinfoParam
    {
        public string ConnectionString { get; set; }
        public string LocType { get; set; }
        public int RoutID { get; set; }
        public int LocationID { get; set; }
    }

    public class GetUserEmailParam
    {
        private string _ConnConfig;
        private string _Username;
        private string _Email;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
    }

    public class GetLocationReportFiltersValueParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetLocationDetailsParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        private string _DBName;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetEquiptypeParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetEquipmentCategoryParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetActiveServiceTypeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetBuildingElevParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetEquipmentParam
    {
        private string _SearchBy;
        private string _SearchValue;
        private string _InstallDate;
        private string _ServiceDate;
        private string _Manufacturer;
        private string _Price;
        private int _LocID;
        private int _CustomerID;
        private string _ConnConfig;
        private string _Category;
        private string _Type;
        private int _Status;
        private int _UserID;
        private int _EmpId;
        private DataSet _dsUserAuthorization;
        public int EN { get; set; }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string InstallDate
        {
            get { return _InstallDate; }
            set { _InstallDate = value; }
        }
        public string InstallDateString { get; set; }
        public string ServiceDate
        {
            get { return _ServiceDate; }
            set { _ServiceDate = value; }
        }
        public string ServiceDateString { get; set; }
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        public string Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        public string PriceString { get; set; }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public int RoleID { get; set; }
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Classification { get; set; }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string building { get; set; }
        public bool IsAssignedProject { get; set; }
        public int EmpId
        {
            get { return _EmpId; }
            set { _EmpId = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 IsSalesAsigned { get; set; }
    }

    public class GetEquipClassificationParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class AddMassMCPParam
    {
        private string _ConnConfig;
        private DataTable _dtItems;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }
    }

    public class GetLeadEquipByIDParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private int _EquipID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetequipByIDParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private int _EquipID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetBuildingLeadEquipParam
    {
        private DataSet _dsUserAuthorization;
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetequipREPDetailsParam
    {
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
        private int _EquipID;
        private int _Status;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        private string _StartDate;

        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private string _EndDate;

        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int Cust { get; set; }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetLeadEquipmentCategoryParam
    {
        private string _ConnConfig; 
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetLeadEquiptypeParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateLeadEquipmentParam
    {
        private string _ConnConfig;
        private string _Unit;
        private int _LocID; 
        private DateTime _InstallDateimport;
        private string _Description;
        private string _Type;
        private string _Cat;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        private string _Manufacturer;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        private string _Serial;
        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }
        private string _UniqueID;
        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }
        private DateTime _InstallDateTime;
        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }
        private DateTime _LastServiceDate;
        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }
        private double _EquipPrice;
        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private DataTable _dtItems;
        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }
        private int _ItemsOnly;
        public int ItemsOnly
        {
            get { return _ItemsOnly; }
            set { _ItemsOnly = value; }
        }
        public int CustomTemplateID { get; set; }
        public DataTable dtcustom { get; set; }
        public string building { get; set; }
        public string MOMUSer { get; set; }
        public string Classification { get; set; }
        public bool Shutdown { get; set; }
        public string ShutdownReason { get; set; }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string ShutdownLongDesc { get; set; }
    }

    public class UpdateEquipmentParam
    {
        private string _ConnConfig;
        private string _Unit;
        private int _LocID;
        private DateTime _InstallDateimport;
        private string _Description;
        private string _Type;
        private string _Cat;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        private string _Manufacturer;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        private string _Serial;
        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }
        private string _UniqueID;
        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }
        private DateTime _InstallDateTime;
        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }
        private DateTime _LastServiceDate;
        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }
        private double _EquipPrice;
        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private DataTable _dtItems;
        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }
        private int _ItemsOnly;
        public int ItemsOnly
        {
            get { return _ItemsOnly; }
            set { _ItemsOnly = value; }
        }
        public int CustomTemplateID { get; set; }
        public DataTable dtcustom { get; set; }
        public string building { get; set; }
        public string MOMUSer { get; set; }
        public string Classification { get; set; }
        public bool Shutdown { get; set; }
        public string ShutdownReason { get; set; }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string ShutdownLongDesc { get; set; }
        public bool PlannedShutdown { get; set; }
    }

    public class AddEquipmentForLeadParam
    {
        private string _ConnConfig;
        private string _Unit;
        private int _LocID;
        private DateTime _InstallDateimport;
        private string _Description;
        private string _Type;
        private string _Cat;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        private string _Manufacturer;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        private string _Serial;
        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }
        private string _UniqueID;
        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }
        private DateTime _InstallDateTime;
        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }
        private DateTime _LastServiceDate;
        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }
        private double _EquipPrice;
        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private DataTable _dtItems;
        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }
        private int _ItemsOnly;
        public int ItemsOnly
        {
            get { return _ItemsOnly; }
            set { _ItemsOnly = value; }
        }
        public int CustomTemplateID { get; set; }
        public DataTable dtcustom { get; set; }
        public string building { get; set; }
        public string MOMUSer { get; set; }
        public string Classification { get; set; }
        public bool Shutdown { get; set; }
        public string ShutdownReason { get; set; }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string ShutdownLongDesc { get; set; }
        public bool PlannedShutdown { get; set; }
    }

    public class AddEquipmentParam
    {
        private string _ConnConfig;
        private string _Unit;
        private int _LocID;
        private DateTime _InstallDateimport;
        private string _Description;
        private string _Type;
        private string _Cat;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }

        private string _Manufacturer;
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }
        private string _Serial;
        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }
        private string _UniqueID;
        public string UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }
        private DateTime _InstallDateTime;
        public DateTime InstallDateTime
        {
            get { return _InstallDateTime; }
            set { _InstallDateTime = value; }
        }
        private DateTime _LastServiceDate;
        public DateTime LastServiceDate
        {
            get { return _LastServiceDate; }
            set { _LastServiceDate = value; }
        }
        private double _EquipPrice;
        public double EquipPrice
        {
            get { return _EquipPrice; }
            set { _EquipPrice = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public DateTime InstallDateimport
        {
            get { return _InstallDateimport; }
            set { _InstallDateimport = value; }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        private DataTable _dtItems;
        public DataTable DtItems
        {
            get { return _dtItems; }
            set { _dtItems = value; }
        }
        //private int _ItemsOnly;
        //public int ItemsOnly
        //{
        //    get { return _ItemsOnly; }
        //    set { _ItemsOnly = value; }
        //}
        public int CustomTemplateID { get; set; }
        public DataTable dtcustom { get; set; }
        public string building { get; set; }
        public string MOMUSer { get; set; }
        public string Classification { get; set; }
        public bool Shutdown { get; set; }
        public string ShutdownReason { get; set; }
        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public string ShutdownLongDesc { get; set; }

    }
    public class GetContractTypeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
    }
    public class GetEquipmentTestsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
    }
    public class GetLeadEquipClassificationParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetShutdownReasonsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetShutdownReasonByIDParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int eqsdReasonID { get; set; }
    }

    public class AddShutdownReasonParam
    {
        private string _ConnConfig;
        public string eqsdReason { get; set; }
        public bool eqsdPlanned { get; set; }
        public string MOMUSer { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class EditShutdownReasonParam
    {
        private string _ConnConfig;
        public int eqsdID { get; set; }
        public string eqsdReason { get; set; }
        public bool eqsdPlanned { get; set; }
        public string MOMUSer { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetEquipShutdownLogsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        private int _EquipID;
        public int EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }
    }

    public class GetEquipReportFiltersValueParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetEquipmentInspectionParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetScreensByUserParam
    {
        private string _Username;
        private string _ConnConfig;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string PageName { get; set; }

    }

    public class GetActiveBillingCodeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }
    public class GetAutoCompleteBillCodesParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string prefixText { get; set; }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetAllTerritoryParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }
    public class ViolationStatus
    {
        public string ConnConfig { get; set; }
        public int ID { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
        public int Count { get; set; }
    }

    public class GetMultipleStockReportsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _Type;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }

    public class GetActiveServiceTypeContractParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string LocType { get; set; }
        public string EditSType { get; set; }
        public int department { get; set; }
        public int route { get; set; }
    }

    public class GetLocationAutojqueryParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        private string _SearchValue;
        private int _CustomerID;
        private int _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public int EN { get; set; }
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }

    public class GetServiceTypeByTypeParam
    {
        private string _ConnConfig;
        private string _Type;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }

    public class GetRecReportFiltersValueParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetRecurringDetailsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class GetDefaultCategoryParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetRouteActiveParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }
    public class ViolationCode
    {
        public string ConnConfig { get; set; }
        public int ID { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public int SectionID { get; set; }
        public int CategoryID { get; set; }

        public String CategoryName { get; set; }

        public String SectionName { get; set; }
    }

    public class GetEscalationReportFiltersValueParam
    {
        private string _ConnConfig;
        private string _DBName;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
    }

    public class GetEscalationDetailsParam
    {
        private string _ConnConfig;
        private DataSet _dsUserAuthorization;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsUserAuthorization
        {
            get { return _dsUserAuthorization; }
            set { _dsUserAuthorization = value; }
        }
    }

    public class NotificationUserCustomChange
    {
        public String SubjectEmail;
        public String UserName;
        public String lsTeamMember;
        public String label;
        public String lsRole;
        public String EmailContent()
        {
            return UserName + " has edited " + label;
        }
    }

}
