using System;
using System.Data;

namespace BusinessEntity
{
    [Serializable]
    public class Customer
    {
        public int ProjectJobID { get; set; }
        private DataSet _DsCustomer;
        private string _ConnConfig;
        private int _ProspectID;
        private string _Name;
        private string _City;
        private string _State;
        private string _Country;
        private string _Zip;
        private string _Phone;
        private string _Cellular;
        private string _Contact;
        private string _Remarks;
        private string _SRemarks;
        private string _Type;
        private int _Status;
        private string _Email;
        private int _Mode;
        private int _LocID;
        private string _LocIDs;
        private int _Worker;
        private string _RouteSequence;
        private int _TemplateID;
        private DataTable _dtWorkerData;
        private string _SearchValue;
        private string _Center;
        private string _Radius;
        private string _Overlay;
        private string _PolygonCoord;
        private string _ReferralType;
        private string _SourceDescription;
        public int WageID { get; set; }
        public bool OnlinePaymentPermission { get; set; }
        public string LocationRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CustomerID { get; set; }
        public int JobTItemId { get; set; }
        public int OrderNo { get; set; }
        public int RoleID { get; set; }
        public int ticketID { get; set; }
        public DataTable dtItems { get; set; }
        public DataTable dtLaborItems { get; set; }
        public DataTable dtLaborItemsEstimate { get; set; }
        private int _Close;
        public int BucketID { get; set; }
        public double CADExchange { get; set; }
        public int IsItemEdited { get; set; }
        public int ItemID { get; set; }
        public Nullable<Int16> JobType { get; set; }
        public double Balance { get; set; }
        public int Ctype { get; set; }
        public DateTime ProjectCreationDate { get; set; }
        public string PO { get; set; }
        public string SO { get; set; }
        public Int16 Certified { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public DateTime Custom5 { get; set; }
        public string ctypeName { get; set; }
        public int InvExp { get; set; }
        public int InvServ { get; set; }
        public int Wage { get; set; }
        public int GLInt { get; set; }
        public string JobTempCtype { get; set; }
        public Int16 Post { get; set; }
        public int? UnrecognizedRevenue { get; set; }
        public int? UnrecognizedExpense { get; set; }
        public int? RetainageReceivable { get; set; }
        public string ArchitectName { get; set; }
        public string ArchitectAdress { get; set; }
        public Int16 PType { get; set; }
        public Int16 Charge { get; set; }
        public Int16 JobClose { get; set; }
        public Int16 fInt { get; set; }
        public double BillRate { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateTravel { get; set; }
        public double Mileage { get; set; }
        public DataTable dtCustom { get; set; }
        public DataTable _dtBOM { get; set; }
        public DataTable _dtBOMCannotDelete { get; set; }
        public DataTable _dtCustom { get; set; }
        public DataTable _dtBomEstimate { get; set; }
        public Stage Stage { get; set; }
        public BT BT { get; set; }
        public Service Service { get; set; }
        public string _strlabel { get; set; }
        public Int32 _intTab { get; set; }
        public decimal _decPercentage { get; set; }
        public decimal _decPerAmount { get; set; }
        public string _strcontact { get; set; }
        public DateTime _dtdate { get; set; }
        public int _intestimateno { get; set; }
        public string _strtype { get; set; }
        public string _sources { get; set; }
        public string _Referral { get; set; }
        public string _Stage { get; set; }
        public string _BT { get; set; }
        public string _Service { get; set; }
        public string RevisionNotes { get; set; }
        public string RevisionVersion { get; set; }
        public string RevisionUser { get; set; }
        public DateTime RevisionCreated { get; set; }
        public DateTime JBillingDate { get; set; }
        public DateTime JPeriodDate { get; set; }
        public DateTime JRevisionDate { get; set; }
        public string IRemarks { get; set; }
        public DateTime? ExpectedClosingDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public TaskCategory TaskCategory { get; set; }
        //milestone
        public DataTable _dtmilestone { get; set; }

        public Int16 Range;
        public string STax;
        //Company
        public int EN { get; set; }
        public int UserID { get; set; }
        public string EstimateType
        {
            get { return _strtype; }
            set { _strtype = value; }
        }
        public bool IsSglBilAmt { get; set; }
        public bool IsBilFrmBOM { get; set; }
        public string contact
        {
            get { return _strcontact; }
            set { _strcontact = value; }
        }

        public DateTime date
        {
            get { return _dtdate; }
            set { _dtdate = value; }
        }

        public DateTime? SoldDate { get; set; }

        public int estimateno
        {
            get { return _intestimateno; }
            set { _intestimateno = value; }
        }
        public DataTable milestone
        {
            get { return _dtmilestone; }
            set { _dtmilestone = value; }
        }

        public string label
        {
            get { return _strlabel; }
            set { _strlabel = value; }
        }
        public Int32 Tab
        {
            get { return _intTab; }
            set { _intTab = value; }
        }
        public decimal Percentage
        {
            get { return _decPercentage; }
            set { _decPercentage = value; }
        }
        public decimal PerAmount
        {
            get { return _decPerAmount; }
            set { _decPerAmount = value; }
        }
        public DataTable DtBomEstimate
        {
            get { return _dtBomEstimate; }
            set { _dtBomEstimate = value; }
        }
        public DataTable DtCustom
        {
            get { return _dtCustom; }
            set { _dtCustom = value; }
        }
        public DataTable DtBOM
        {
            get { return _dtBOM; }
            set { _dtBOM = value; }
        }

        public DataTable DtBOMCannotDelete
        {
            get { return _dtBOMCannotDelete; }
            set { _dtBOMCannotDelete = value; }
        }
        public int Close
        {
            get { return _Close; }
            set { _Close = value; }
        }

        private string _Fuser;

        public string Fuser
        {
            get { return _Fuser; }
            set { _Fuser = value; }
        }

        private string _AssignedToID;

        public string AssignedToID
        {
            get { return _AssignedToID; }
            set { _AssignedToID = value; }
        }

        private string _OpportunityStageID;

        public string OpportunityStageID
        {
            get { return _OpportunityStageID; }
            set { _OpportunityStageID = value; }
        }

        public string OpportunityName { get; set; }

        public int ProjectStageID { get; set; }

        private string _LastUpdateUser;

        public string LastUpdateUser
        {
            get { return _LastUpdateUser; }
            set { _LastUpdateUser = value; }
        }

        private string _NextStep;

        public string NextStep
        {
            get { return _NextStep; }
            set { _NextStep = value; }
        }

        private string _Source;

        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private double _Amount;

        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }


        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }


        private string _Desc;

        public string Desc
        {
            get { return _Desc; }
            set { _Desc = value; }
        }

        private int _OpportunityID;

        public int OpportunityID
        {
            get { return _OpportunityID; }
            set { _OpportunityID = value; }
        }

        private int _Probability;

        public int Probability
        {
            get { return _Probability; }
            set { _Probability = value; }
        }


        private string _Resolution;

        public string Resolution
        {
            get { return _Resolution; }
            set { _Resolution = value; }
        }

        private int _TaskID;

        public int TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; }
        }
        private string _AssignedTo;

        public string AssignedTo
        {
            get { return _AssignedTo; }
            set { _AssignedTo = value; }
        }
        private string _Subject;

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        private DateTime _TimeDue;

        public DateTime TimeDue
        {
            get { return _TimeDue; }
            set { _TimeDue = value; }
        }
        private DateTime _DueDate;

        public DateTime DueDate
        {
            get { return _DueDate; }
            set { _DueDate = value; }
        }

        public bool IsAlert { get; set; }
        public bool IsCertifiedProject { get; set; }
        private int _ROL;

        public int ROL
        {
            get { return _ROL; }
            set { _ROL = value; }
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

        private string _Lat;

        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        private string _Lng;

        public string Lng
        {
            get { return _Lng; }
            set { _Lng = value; }
        }

        private string _Website;

        public string Website
        {
            get { return _Website; }
            set { _Website = value; }
        }

        private string _Fax;

        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }

        private DataTable _ContactData;

        public DataTable ContactData
        {
            get { return _ContactData; }
            set { _ContactData = value; }
        }

        private string _BillPhone;

        public string BillPhone
        {
            get { return _BillPhone; }
            set { _BillPhone = value; }
        }


        private string _BillZip;

        public string BillZip
        {
            get { return _BillZip; }
            set { _BillZip = value; }
        }

        private string _BillState;

        public string BillState
        {
            get { return _BillState; }
            set { _BillState = value; }
        }

        private string _BillCountry;

        public string BillCountry
        {
            get { return _BillCountry; }
            set { _BillCountry = value; }
        }


        private string _BillCity;

        public string BillCity
        {
            get { return _BillCity; }
            set { _BillCity = value; }
        }

        private string _Billaddress;

        public string Billaddress
        {
            get { return _Billaddress; }
            set { _Billaddress = value; }
        }

        private int _Terr;

        public int Terr
        {
            get { return _Terr; }
            set { _Terr = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        private string _SearchBy;

        public string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }

        private bool _EquipID;

        public bool EquipID
        {
            get { return _EquipID; }
            set { _EquipID = value; }
        }

        private bool _NullAddressOnly;

        public bool NullAddressOnly
        {
            get { return _NullAddressOnly; }
            set { _NullAddressOnly = value; }
        }

        public string PolygonCoord
        {
            get { return _PolygonCoord; }
            set { _PolygonCoord = value; }
        }

        public string Overlay
        {
            get { return _Overlay; }
            set { _Overlay = value; }
        }

        public string Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }

        public string Center
        {
            get { return _Center; }
            set { _Center = value; }
        }

        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }

        public DataTable dtWorkerData
        {
            get { return _dtWorkerData; }
            set { _dtWorkerData = value; }
        }

        private DataTable _dtTemplateData;

        public DataTable DtTemplateData
        {
            get { return _dtTemplateData; }
            set { _dtTemplateData = value; }
        }

        public int TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }

        public string RouteSequence
        {
            get { return _RouteSequence; }
            set { _RouteSequence = value; }
        }

        public int Worker
        {
            get { return _Worker; }
            set { _Worker = value; }
        }

        public string LocIDs
        {
            get { return _LocIDs; }
            set { _LocIDs = value; }
        }

        public int LocID
        {
            get { return _LocID; }
            set { _LocID = value; }
        }

        public int Mode
        {
            get { return _Mode; }
            set { _Mode = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }


        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }

        public string SourceDescription
        {
            get { return _SourceDescription; }
            set { _SourceDescription = value; }
        }
        public string SRemarks
        {
            get { return _SRemarks; }
            set { _SRemarks = value; }
        }

        public string Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        public string Cellular
        {
            get { return _Cellular; }
            set { _Cellular = value; }
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

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public int ProspectID
        {
            get { return _ProspectID; }
            set { _ProspectID = value; }
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
        private string _DBName;
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }

        private int _RolType;
        public int RolType
        {
            get { return _RolType; }
            set { _RolType = value; }
        }
        private string _RolName;
        public string RolName
        {
            get { return _RolName; }
            set { _RolName = value; }
        }
        public DataTable tblGCandHomeOwner { get; set; }
        private string _RolRemarks;
        public string RolRemarks
        {
            get { return _RolRemarks; }
            set { _RolRemarks = value; }
        }
        private DataTable _dtTeam;
        public DataTable DtTeam
        {
            get { return _dtTeam; }
            set { _dtTeam = value; }
        }
        private DataTable _dtMilestone;
        public DataTable DtMilestone
        {
            get { return _dtMilestone; }
            set { _dtMilestone = value; }
        }

        public DataTable dtTaskCode { get; set; }

        public int job { get; set; }

        public int InvoiceNo { get; set; }

        public string taskcategory { get; set; }


        public int Handle { get; set; }

        public int IsRenewalNotes { get; set; }

        public string RenewalNotes { get; set; }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _HeaderStage;

        public string HeaderStage
        {
            get { return _HeaderStage; }
            set { _HeaderStage = value; }
        }
        private string _HeaderBT;

        public string HeaderBT
        {
            get { return _HeaderBT; }
            set { _HeaderBT = value; }
        }
        private string _HeaderServices;
        public string HeaderServices
        {
            get { return _HeaderServices; }
            set { _HeaderServices = value; }
        }

        public string ReferralType
        {
            get { return _ReferralType; }
            set { _ReferralType = value; }
        }

        public double BidPrice { get; set; }
        public double? Override { get; set; }
        public double OH { get; set; }
        public double Cont { get; set; }
        public double ContPer { get; set; }

        public double OHPer { get; set; }
        public double MarkupPer { get; set; }
        public double CommissionPer { get; set; }
        public double CommissionVal { get; set; }
        public double MarkupVal { get; set; }
        public double STaxVal { get; set; }
        public double MatExp { get; set; }
        public double LabExp { get; set; }
        public double OtherExp { get; set; }
        public double SubToalVal { get; set; }
        public double TotalCostVal { get; set; }
        public double PretaxTotalVal { get; set; }


        public double STaxRate { get; set; }

        public String Category { get; set; }
        public string CompanyName { get; set; }
        public string Sales_Tax { get; set; }

        public string OldSourceDescription { get; set; }

        public bool PWIP { get; set; }
        public int? WIPID { get; set; }
        public int? WIPStatus { get; set; }

        private DataSet _WIP;
        public DataSet WIP
        {
            get { return _WIP; }
            set { _WIP = value; }
        }
        private DataSet _Reverse;
        public DataSet Reverse
        {
            get { return _Reverse; }
            set { _Reverse = value; }
        }

        private DataSet _ProjectVariance;
        public DataSet ProjectVariance
        {
            get { return _ProjectVariance; }
            set { _ProjectVariance = value; }
        }

        public int OwnerID { get; set; }

        public double Duration { get; set; }
        public int StatusID { get; set; }
        public int PhoneID { get; set; }
        public bool IsLeadEquip { get; set; }
        public bool Discounted { get; set; }
        public string DiscountedNotes { get; set; }
        public string Comment { get; set; }
        private DataTable _dtEquips;
        public DataTable DtEquips
        {
            get { return _dtEquips; }
            set { _dtEquips = value; }
        }

        public string GroupName { get; set; }

        public int GroupId { get; set; }
        public string DefaultNote { get; set; }

        public DataTable DtGridUserSettings { get; set; }

        public int ProjectManagerUserID { get; set; }
        public int AssignedProjectUserID { get; set; }

        public string Notes { get; set; }

        public string LogScreen { get; set; }
        public int LogRefId { get; set; }

        public int TargetHPermission { get; set; }

        public string SearchValueFrDt { get; set; }
        public string SearchValueToDt { get; set; }

        public Boolean ShowAllNote { get; set; }
        public string Screen { get; set; }
        public int Ref { get; set; }

        public string SearchValueExt { get; set; }
        public int SupervisorUserID { get; set; }
    }

    public class GetVendorLabelParam
    {
        private string _ConnConfig;
        private string _Name;
        private string _City;
        private string _State;
        private string _Zip;
        private string _Address;
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

    }

    public class GetProspectByIDParam
    {
        private DataSet _DsCustomer;
        private int _ProspectID;
        private string _ConnConfig;
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public int ProspectID
        {
            get { return _ProspectID; }
            set { _ProspectID = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class DeleteOpportunityParam
    {
        private int _OpportunityID;
        private string _ConnConfig;
        public int OpportunityID
        {
            get { return _OpportunityID; }
            set { _OpportunityID = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetOpportunityOfCustomerParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
        public int CustomerID { get; set; }
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
    public class GetJobProjectParam
    {
        private string _ConnConfig;
        private string _SearchBy;
        private string _SearchValue;
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
        public Int16 Range;
        public Nullable<Int16> JobType { get; set; }
        public Int32 IsSalesAsigned { get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }
        public int IncludeClose { get; set; }
        public string Username { get; set; }
        public DataTable filtersData { get; set; }
    }

    public class GetCustomerLabelParam
    {
        private string _Name;
        private DataSet _DsCustomer;
        private string _State;
        private string _City;
        private string _Zip;
        private string _ConnConfig;
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
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
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
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        public Int32 IsSalesAsigned { get; set; }
    }

    public class GetDefaultWorkerHeaderParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetBTParam
    {
        private DataSet _DsCustomer;
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class ConvertLeadEquipmentParam
    {
        private string _ConnConfig;
        private int _ProspectID;
        public int ProspectID
        {
            get { return _ProspectID; }
            set { _ProspectID = value; }
        }

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetOpportunityNewParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
        private string _SearchValue;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int32 IsSalesAsigned;
        public DataSet DsCustomer
        {
            get { return _DsCustomer; }
            set { _DsCustomer = value; }
        }
        private string _SearchBy;

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
        private int _ROL;
        public int ROL
        {
            get { return _ROL; }
            set { _ROL = value; }
        }
        public int EN { get; set; }
        public int UserID { get; set; }
    }

    public class GetRepTemplateNameParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
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

    public class GetTemplateItemByIDParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
        private int _TemplateID;
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
        public bool IsLeadEquip { get; set; }
        public int TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }
    }

    public class GetCustomTemplateParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
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

    public class GetCustTemplateItemByIDParam
    {
        private string _ConnConfig;
        private DataSet _DsCustomer;
        private int _TemplateID;
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
        public int TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }
    }

    public class GetEquipmentShutdownForReportParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime endDate { get; set; }
    }

    public class GetEquipShutdownActivityForReportParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string eqId { get; set; }
        public bool filtered { get; set; }
    }

    public class GetEstimateByIDParam
    {
        public int _intestimateno { get; set; }
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int estimateno
        {
            get { return _intestimateno; }
            set { _intestimateno = value; }
        }

    }
}
