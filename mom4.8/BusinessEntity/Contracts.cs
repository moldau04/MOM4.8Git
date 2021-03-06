using System;
using System.Data;

namespace BusinessEntity
{
    public class Contracts
    {
        private DataSet _ds;
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
        private int _Loc;
        private int _Owner;
        private DateTime _Date;
        private int _Status;
        private int _CreditCard;
        private string _Remarks;
        private DateTime _BStart;
        private int _BCycle;
        private double _BAMT;
        private DateTime _SStart;
        private int _Cycle;
        private int _SWE;
        private DateTime _STime;
        private int _Sdate;
        private int _Sday;
        private DataTable _DtElevJob;
        private string _Route;
        private int _JobId;
        private double _Hours;
        private int _Level;
        private int _PerContract;
        private int _ContractRemarks;
        private DataTable _dtRecContr;
        private DateTime _StartDt;
        private DateTime _EndDt;
        private int _PaymentTerms;
        private int _Stax;
        private double _Taxable;
        private string _Description;
        private string _Ctype;
        private string _ProcessPeriod;
        private double _Amount;
        private double _Total;
        private string _taxRegion;
        private double _taxrate;
        private double _Taxfactor;
        private int _Terms;
        private string _PO;
        private int _Batch;
        private double _Gtax;
        private string _TaxRegion2;
        private double _Taxrate2;
        private string _BillTo;
        private DateTime _Idate;
        private string _Fuser;
        private int _StaxI;
        private int _Type;
        private int _Mech;
        private int _InvoiceID;
        private string _InvoiceIDCustom;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private int _CustID;
        private string _Locaname;
        private string _BcycleName;
        private string _ScycleName;
        private DateTime _TransDate;
        private string _CardNumber;
        private string _Response;
        private string _PaymentRefID;
        private string _UserID;
        private string _Screen;
        private string _Medium;
        private string _MerchantID;
        private string _LoginID;
        private string _PaymentUser;
        private string _PaymentPass;
        private int _MerchantInfoID;
        private int _TicketID;
        private string _Tickets;
        private string _TicketLineItems;
        private double _staxtotal;
        private string _QBCustomerID;
        private string _QBJobtypeID;
        private string _QBInvID;
        private string _QBTermsID;
        private int _TermsID;

        public string TicketCategory { get; set; }
        public DateTime OriginalContract { get; set; }
        public DateTime LastRenew { get; set; }
        public DateTime EscalationPriorto { get; set; }
        public DateTime EscalationDate { get; set; }
        public int ContractLength { get; set; }
        public DateTime PostDate { get; set; }
        public Int16 ContractBill { get; set; }
        public Int16 CustBilling { get; set; }
        public int Central { get; set; }
        public bool IsExistContract { get; set; }
        public int expirationfreq { get; set; }
        public DateTime? DueDate { get; set; }
        public int Expiration { get; set; }

        public DateTime ExpirationDate { get; set; }
        public string MileageItem { get; set; }
        public string LaborItem { get; set; }
        public string ExpenseItem { get; set; }
        public int RoleId { get; set; }

        public Guid PaymentUID { get; set; }

        public int Paid { get; set; }

        public int ProspectID { get; set; }

        public string Error { get; set; }

        public string GatewayOrderID { get; set; }

        public string ResponseCodes { get; set; }

        public string  Approved { get; set; }

        public int IsSuccess { get; set; }

        public int OnDemand { get; set; }
        public int JobTID { get; set; }
        public int EscalationType { get; set; }
        public int EscalationCycle { get; set; }
        public double EscalationFactor { get; set; }
        public DateTime EscalationLast { get; set; }

        private DateTime _LastUpdateDate;
        public int Ref;
        public int TransID;
        public int PaymentID;
        public int Acct;
        public double Quan;
        public int JobItem;
        public string Measure;
        public int Month;
        public int Year;
        public int Chart;
        public int InvoiceEndID;
        private DataTable _DtCustom;
        //public string ProcessPeriod;
        public int Rol { get;set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double Mileage { get; set; }
        public int EN { get; set; }

        public int DepartmentType { get; set; }

        public String DepartmentTypes { get; set; }

        public DataTable DtCustom
        {
            get { return _DtCustom; }
            set { _DtCustom = value; }
        }
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set { _LastUpdateDate = value; }
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

        public string QBCustomerID
        {
            get { return _QBCustomerID; }
            set { _QBCustomerID = value; }
        }

        public double Staxtotal
        {
            get { return _staxtotal; }
            set { _staxtotal = value; }
        }

        public string TicketLineItems
        {
            get { return _TicketLineItems; }
            set { _TicketLineItems = value; }
        }

        public string Tickets
        {
            get { return _Tickets; }
            set { _Tickets = value; }
        }

        public int TicketID
        {
            get { return _TicketID; }
            set { _TicketID = value; }
        }

        public int MerchantInfoID
        {
            get { return _MerchantInfoID; }
            set { _MerchantInfoID = value; }
        }

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

        public string Medium
        {
            get { return _Medium; }
            set { _Medium = value; }
        }

        public string Screen
        {
            get { return _Screen; }
            set { _Screen = value; }
        }

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string PaymentRefID
        {
            get { return _PaymentRefID; }
            set { _PaymentRefID = value; }
        }

        public string Response
        {
            get { return _Response; }
            set { _Response = value; }
        }

        public string CardNumber
        {
            get { return _CardNumber; }
            set { _CardNumber = value; }
        }

        public DateTime TransDate
        {
            get { return _TransDate; }
            set { _TransDate = value; }
        }

        public string ScycleName
        {
            get { return _ScycleName; }
            set { _ScycleName = value; }
        }

        public string BcycleName
        {
            get { return _BcycleName; }
            set { _BcycleName = value; }
        }

        public string Locaname
        {
            get { return _Locaname; }
            set { _Locaname = value; }
        }

        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        public string InvoiceIDCustom
        {
            get { return _InvoiceIDCustom; }
            set { _InvoiceIDCustom = value; }
        }

        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
        
        public int Mech
        {
            get { return _Mech; }
            set { _Mech = value; }
        }

        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public int StaxI
        {
            get { return _StaxI; }
            set { _StaxI = value; }
        }

        public string Fuser
        {
            get { return _Fuser; }
            set { _Fuser = value; }
        }

        public DateTime Idate
        {
            get { return _Idate; }
            set { _Idate = value; }
        }

        public string BillTo
        {
            get { return _BillTo; }
            set { _BillTo = value; }
        }

        public double Taxrate2
        {
            get { return _Taxrate2; }
            set { _Taxrate2 = value; }
        }

        public string TaxRegion2
        {
            get { return _TaxRegion2; }
            set { _TaxRegion2 = value; }
        }

        public double Gtax
        {
            get { return _Gtax; }
            set { _Gtax = value; }
        }

        public int Batch
        {
            get { return _Batch; }
            set { _Batch = value; }
        }

        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }

        public int Terms
        {
            get { return _Terms; }
            set { _Terms = value; }
        }

        public double Taxfactor
        {
            get { return _Taxfactor; }
            set { _Taxfactor = value; }
        }

        public double Taxrate
        {
            get { return _taxrate; }
            set { _taxrate = value; }
        }

        public string TaxRegion
        {
            get { return _taxRegion; }
            set { _taxRegion = value; }
        }

        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public string ProcessPeriod
        {
            get { return _ProcessPeriod; }
            set { _ProcessPeriod = value; }
        }

        public string Ctype
        {
            get { return _Ctype; }
            set { _Ctype = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public double Taxable
        {
            get { return _Taxable; }
            set { _Taxable = value; }
        }

        public int Stax
        {
            get { return _Stax; }
            set { _Stax = value; }
        }

        public int PaymentTerms
        {
            get { return _PaymentTerms; }
            set { _PaymentTerms = value; }
        }

        public DateTime EndDt
        {
            get { return _EndDt; }
            set { _EndDt = value; }
        }

        public DateTime StartDt
        {
            get { return _StartDt; }
            set { _StartDt = value; }
        }

        public DataTable DtRecContr
        {
            get { return _dtRecContr; }
            set { _dtRecContr = value; }
        }

        public int ContractRemarks
        {
            get { return _ContractRemarks; }
            set { _ContractRemarks = value; }
        }

        public int PerContract
        {
            get { return _PerContract; }
            set { _PerContract = value; }
        }

        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        public double Hours
        {
            get { return _Hours; }
            set { _Hours = value; }
        }

        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }

        public string Route
        {
            get { return _Route; }
            set { _Route = value; }
        }

        public DataTable DtElevJob
        {
            get { return _DtElevJob; }
            set { _DtElevJob = value; }
        }

        public int Sday
        {
            get { return _Sday; }
            set { _Sday = value; }
        }

        public int Sdate
        {
            get { return _Sdate; }
            set { _Sdate = value; }
        }

        public DateTime STime
        {
            get { return _STime; }
            set { _STime = value; }
        }

        public int SWE
        {
            get { return _SWE; }
            set { _SWE = value; }
        }

        public int Cycle
        {
            get { return _Cycle; }
            set { _Cycle = value; }
        }

        public DateTime SStart
        {
            get { return _SStart; }
            set { _SStart = value; }
        }

        public double BAMT
        {
            get { return _BAMT; }
            set { _BAMT = value; }
        }

        public int BCycle
        {
            get { return _BCycle; }
            set { _BCycle = value; }
        }

        public DateTime BStart
        {
            get { return _BStart; }
            set { _BStart = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public int CreditCard
        {
            get { return _CreditCard; }
            set { _CreditCard = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        public int Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
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

        public int isTS { get; set; }

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

        public int jobid { get; set; }

        public string jobids { get; set; }




        private string _searchAmtPaidUnpaid;
        public string SearchAmtPaidUnpaid
        {
            get
            {
                return _searchAmtPaidUnpaid;
            }
            set
            {
                _searchAmtPaidUnpaid = value;
            }
        }

        private string _Notes;
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }
        private int _Handel;
        public int Handel
        {
            get
            {
                return _Handel;
            }
            set
            {
                _Handel = value;
            }
        }
        public int FlagEN { get; set; }
        private string _RenewalNotes;
        public string RenewalNotes
        {
            get
            {
                return _RenewalNotes;
            }
            set
            {
                _RenewalNotes = value;
            }
        }
        private int _IsRenewalNotes;
        public int IsRenewalNotes
        {
            get
            {
                return _IsRenewalNotes;
            }
            set
            {
                _IsRenewalNotes = value;
            }
        }
        private int _IsRecurring;
        public int IsRecurring
        {
            get
            {
                return _IsRecurring;
            }
            set
            {
                _IsRecurring = value;
            }
        }
        public string lastUpdatedby { get; set; }
        public string strSdate { get; set; }
        public string strEdate { get; set; }
        public string strOwners { get; set; }
        public bool IsOverDue { get; set; }
        private string _searchPrintMail;
        public string SearchPrintMail
        {
            get
            {
                return _searchPrintMail;
            }
            set
            {
                _searchPrintMail = value;
            }
        }
        private int _Detail;
        public int Detail
        {
            get
            {
                return _Detail;
            }
            set
            {
                _Detail = value;
            }
        }


        public object Routing { get; set; }

        public object BankAC { get; set; }

        public object BankAcHolName { get; set; }

        public object FileName { get; set; }

        public object PayType { get; set; }

        public string filterBy { get; set; }

        public string filterValue { get; set; } 

        public string taskcategory { get; set; }
        public string StateVal { get; set; }
        public string MOMUSer { get; set; }

        public int AssignedTo { get; set; }

        public string LocationIDs { get; set; }

        public string CustomerIDs { get; set; }
        public bool HidePartial { get; set; }

        public bool isDBTotalService { get; set; }
        public DataTable dtFilterByColumn { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int isShowAll { get; set; }
        public Boolean isGridFilterInvoice { get; set; }
        public string SortOrderBy { get; set; }
        public string SortType { get; set; }

        public String TaxApply { get; set; }
        public Boolean isCanadaCompany { get; set; }

        public int EstimateId { get; set; }
        public int TaxType { get; set; }
    }

    public class GetARRevenueCustParam
    {
        private int _CustID;
        private DataSet _ds;
        private int _Loc;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
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
        public string filterBy { get; set; }
        public string filterValue { get; set; }
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

    public class GetARRevenueCustShowAllParam
    {
        private int _CustID;
        private DataSet _ds;
        private int _Loc;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public string filterBy { get; set; }
        public string filterValue { get; set; }
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
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
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
    }

    public class IsExistContractByLocParam
    {
        private string _ConnConfig;
        private int _Loc;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public bool IsExistContract { get; set; }

    }

    public class GetARRevenueParam
    {
        private DataSet _ds;
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
        private int _Loc;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private int _CustID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int CustID
        {
            get { return _CustID; }
            set { _CustID = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
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
        public string filterBy { get; set; }
        public string filterValue { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

    }

    public class UpdateCustomerBalanceParam
    {
        private string _ConnConfig;
        private int _Loc;
        private double _Amount;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }
    }

    public class GetInvoiceByCustomerIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Rol { get; set; }
        public int isTS { get; set; }
    }

    public class GetInvoicesByIDParam
    {
        private string _ConnConfig;
        private int _InvoiceID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }

    }

    public class GetInvoicesByRefParam
    {
        private string _ConnConfig;
        private int _InvoiceID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
    }
    public class GetInvoiceItemByRefParam
    {
        private string _ConnConfig;
        private int _InvoiceID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
    }

    public class GetTicketIDParam
    {
        private string _ConnConfig;
        private int _InvoiceID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
    }

    public class GetCustomerStatementByLocParam
    {
        private string _ConnConfig;
        private int _Loc;
        private string _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }

    public class GetCustomerStatementInvoicesSouthernParam
    {
        private string _ConnConfig;
        private int _Loc; 
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public bool includeCredit { get; set; }
        public bool IsOverDue { get; set; }
    }

    public class GetCustomerStatementInvoicesParam
    {
        private string _ConnConfig;
        private int _Loc;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public bool includeCredit { get; set; }
        public bool IsOverDue { get; set; }
    }

    public class GetEmailDetailByLocParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Ref { get; set; }
    }
    public class GetCustomerStatementByLocsParam
    {
        private string _ConnConfig;
        private string _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string LocationIDs { get; set; }
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }

    public class GetCustStatementInvByLocationParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string LocationIDs { get; set; }
        public bool includeCredit { get; set; }

    }

    public class GetAllTicketIDParam
    {
        private string _ConnConfig;
        private int _InvoiceID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int InvoiceID
        {
            get { return _InvoiceID; }
            set { _InvoiceID = value; }
        }
    }

    public class GetCustomerStatementCollectionParam
    {
        private string _ConnConfig;
        private string _UserID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public bool includeCredit { get; set; }
        public bool includeCustomerCredit { get; set; }
        public string LocationIDs { get; set; }
        public string CustomerIDs { get; set; }
        public bool IsOverDue { get; set; }
        public int EN { get; set; }
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }

    public class GetARAgingByTerritoryParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public bool isDBTotalService { get; set; }
        public string territories { get; set; }
    }

    public class GetARAgingParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        private int _Owner;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public int Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public bool HidePartial { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetARAgingByAsOfDateParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public bool HidePartial { get; set; }
        public bool isDBTotalService { get; set; }
    }

    public class GetInvoiceByDateParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetARAging360ByAsOfDateParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public bool isDBTotalService { get; set; }
    }
    public class GetARAgingByLocTypeParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public String DepartmentTypes { get; set; }
    }

    public class GetARAgingByLocTypeDetailParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public String DepartmentTypes { get; set; }
    }

    public class GetARAgingDepParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public String DepartmentTypes { get; set; }
    }

    public class GetARAgingByAsOfDateDepParam
    {
        private string _ConnConfig;
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public String DepartmentTypes { get; set; }
        public bool isDBTotalService { get; set; }
    }

    public class GetContractsDataParam
    {
        private string _ConnConfig; 
        private string _SearchBy;
        private string _SearchValue;
        private string _UserID;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
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
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public DateTime ExpirationDate { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class DeleteContractParam 
    {
        private int _JobId;
        private string _ConnConfig;
        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateExpirationDateParam
    {
        private string _ConnConfig;
        public DateTime ExpirationDate { get; set; }
        public string jobids { get; set; }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetEscalationContractsParam
    {
        private string _ConnConfig;
        private string _UserID; 
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime EscalationLast { get; set; }
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public int EN { get; set; }
    }

    public class EscalateContractParam
    {
        private string _Fuser;
        private string _ConnConfig;
        public DateTime ExpirationDate { get; set; }
        public string jobids { get; set; }
        public string Fuser
        {
            get { return _Fuser; }
            set { _Fuser = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetContractParam
    {
        private int _JobId;
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class GetElevContractParam
    {
        private int _JobId;
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

    public class GetJstatusParam
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

    public class UpdateContractParam
    {
        private string _ConnConfig;
        private int _Loc;
        private int _Owner;
        private DateTime _Date;
        private int _Status;
        private int _CreditCard;
        private string _Remarks;
        private DateTime _BStart;
        private int _BCycle;
        private double _BAMT;
        private DateTime _SStart;
        private int _Cycle;
        private int _SWE;
        private DateTime _STime;
        private int _Sdate;
        private int _Sday;
        private DataTable _DtElevJob;
        private int _JobId;
        private double _Hours;
        private string _Route;
        private string _Ctype;
        private string _Description;
        private DataTable _DtCustom;
        private string _PO;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public int Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int CreditCard
        {
            get { return _CreditCard; }
            set { _CreditCard = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public int BCycle
        {
            get { return _BCycle; }
            set { _BCycle = value; }
        }

        public DateTime BStart
        {
            get { return _BStart; }
            set { _BStart = value; }
        }
        public DateTime SStart
        {
            get { return _SStart; }
            set { _SStart = value; }
        }

        public double BAMT
        {
            get { return _BAMT; }
            set { _BAMT = value; }
        }
        public DateTime STime
        {
            get { return _STime; }
            set { _STime = value; }
        }

        public int SWE
        {
            get { return _SWE; }
            set { _SWE = value; }
        }

        public int Cycle
        {
            get { return _Cycle; }
            set { _Cycle = value; }
        }
        public DataTable DtElevJob
        {
            get { return _DtElevJob; }
            set { _DtElevJob = value; }
        }

        public int Sday
        {
            get { return _Sday; }
            set { _Sday = value; }
        }

        public int Sdate
        {
            get { return _Sdate; }
            set { _Sdate = value; }
        }
        public double Hours
        {
            get { return _Hours; }
            set { _Hours = value; }
        }

        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }

        public string Route
        {
            get { return _Route; }
            set { _Route = value; }
        }
        public string Ctype
        {
            get { return _Ctype; }
            set { _Ctype = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public DateTime ExpirationDate { get; set; }
        public int expirationfreq { get; set; }
        public int Expiration { get; set; }
        public Int16 ContractBill { get; set; }
        public Int16 CustBilling { get; set; }
        public int Central { get; set; }
        public int Chart { get; set; }
        public int JobTID { get; set; }
        public DataTable DtCustom
        {
            get { return _DtCustom; }
            set { _DtCustom = value; }
        }
        public int EscalationType { get; set; }
        public int EscalationCycle { get; set; }
        public double EscalationFactor { get; set; }
        public DateTime EscalationLast { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double Mileage { get; set; }
        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }
        private string _Notes;
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }
        private int _Handel;
        public int Handel
        {
            get
            {
                return _Handel;
            }
            set
            {
                _Handel = value;
            }
        }
        private string _RenewalNotes;
        public string RenewalNotes
        {
            get
            {
                return _RenewalNotes;
            }
            set
            {
                _RenewalNotes = value;
            }
        }
        private int _IsRenewalNotes;
        public int IsRenewalNotes
        {
            get
            {
                return _IsRenewalNotes;
            }
            set
            {
                _IsRenewalNotes = value;
            }
        }
        private int _Detail;
        public int Detail
        {
            get
            {
                return _Detail;
            }
            set
            {
                _Detail = value;
            }
        }
        public string MOMUSer { get; set; }

    }

    public class AddContractParam
    {
        private string _ConnConfig;
        private int _Loc;
        private int _Owner;
        private DateTime _Date;
        private int _Status;
        private int _CreditCard;
        private string _Remarks;
        private DateTime _BStart;
        private int _BCycle;
        private double _BAMT;
        private DateTime _SStart;
        private int _Cycle;
        private int _SWE;
        private DateTime _STime;
        private int _Sdate;
        private int _Sday;
        private DataTable _DtElevJob;
        private double _Hours;
        private string _Route;
        private string _Ctype;
        private string _Description;
        private DataTable _DtCustom;
        private string _PO;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Loc
        {
            get { return _Loc; }
            set { _Loc = value; }
        }
        public int Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public int CreditCard
        {
            get { return _CreditCard; }
            set { _CreditCard = value; }
        }
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
        public int BCycle
        {
            get { return _BCycle; }
            set { _BCycle = value; }
        }

        public DateTime BStart
        {
            get { return _BStart; }
            set { _BStart = value; }
        }
        public DateTime SStart
        {
            get { return _SStart; }
            set { _SStart = value; }
        }

        public double BAMT
        {
            get { return _BAMT; }
            set { _BAMT = value; }
        }
        public DateTime STime
        {
            get { return _STime; }
            set { _STime = value; }
        }

        public int SWE
        {
            get { return _SWE; }
            set { _SWE = value; }
        }

        public int Cycle
        {
            get { return _Cycle; }
            set { _Cycle = value; }
        }
        public DataTable DtElevJob
        {
            get { return _DtElevJob; }
            set { _DtElevJob = value; }
        }

        public int Sday
        {
            get { return _Sday; }
            set { _Sday = value; }
        }

        public int Sdate
        {
            get { return _Sdate; }
            set { _Sdate = value; }
        }
        public double Hours
        {
            get { return _Hours; }
            set { _Hours = value; }
        }
        public string Route
        {
            get { return _Route; }
            set { _Route = value; }
        }
        public string Ctype
        {
            get { return _Ctype; }
            set { _Ctype = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public DateTime ExpirationDate { get; set; }
        public int expirationfreq { get; set; }
        public int Expiration { get; set; }
        public Int16 ContractBill { get; set; }
        public Int16 CustBilling { get; set; }
        public int Central { get; set; }
        public int Chart { get; set; }
        public int JobTID { get; set; }
        public DataTable DtCustom
        {
            get { return _DtCustom; }
            set { _DtCustom = value; }
        }
        public int EscalationType { get; set; }
        public int EscalationCycle { get; set; }
        public double EscalationFactor { get; set; }
        public DateTime EscalationLast { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double Mileage { get; set; }
        public string PO
        {
            get { return _PO; }
            set { _PO = value; }
        }
        private string _Notes;
        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                _Notes = value;
            }
        }
        private int _Handel;
        public int Handel
        {
            get
            {
                return _Handel;
            }
            set
            {
                _Handel = value;
            }
        }
        private string _RenewalNotes;
        public string RenewalNotes
        {
            get
            {
                return _RenewalNotes;
            }
            set
            {
                _RenewalNotes = value;
            }
        }
        private int _IsRenewalNotes;
        public int IsRenewalNotes
        {
            get
            {
                return _IsRenewalNotes;
            }
            set
            {
                _IsRenewalNotes = value;
            }
        }
        private int _Detail;
        public int Detail
        {
            get
            {
                return _Detail;
            }
            set
            {
                _Detail = value;
            }
        }
        public string MOMUSer { get; set; }
        public string taskcategory { get; set; }
        public int EstimateId { get; set; }



    }
    
    public class GetRecurringContractLogsParam
    {
        private string _ConnConfig;
        private int _JobId;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int JobId
        {
            get { return _JobId; }
            set { _JobId = value; }
        }
    }
}
