using System;
using System.Data;

namespace BusinessEntity
{
    public class JobT
    {
        public int Job; 
        public int Opsq;
        private DataSet _ds;
        private string _ConnConfig;
        public int ID;
        public string fDesc;
        public Int16 Type;
        public Int16 NRev;
        public Int16 NDed;
        public int Count;
        public string Remarks;
        public int InvExp;
        public int InvServ;
        public int Wage;
        public int? UnrecognizedRevenue;
        public int? UnrecognizedExpense;
        public int? RetainageReceivable;
        public string CType;
        public Int16 Status;
        public Int16 Charge;
        public Int16 Post;
        public Int16 fInt;
        public int GLInt;
        public Int16 JobClose;
        public string TemplateRev;
        public string RevRemarks;
        public int MilestoneType;
        public bool IsExist;
        public Int16 Line;
        public int ItemID;
        public double QtyReq;
        public double ScrapFact;
        public double BudgetUnit;
        public double BudgetExt;
        public string SearchValue;
        public bool IsDefault;
        
        public Int16 AlertType;
        public bool AlertMgr;
        public bool MilestoneMgr;
        //public bool FinanceMgr;
        //public bool ProjectMgr;
        public int ServiceTypeID;
        public string ServiceName;
        public bool IsExistRecurr;

        private DataSet _projectItem;
        private DataTable _projectDt;
        private DataTable _MilestoneDt;
        private DataTable _CustomTabItem;
        private DataTable _CustomItem;
        private DataTable _CustomItemDelete;
        private DataTable _EstimateData;
        public string PageUrl;
        public string UM;
        public string Code;
        public Int16 BomType;
        public int Phase;
        public int TypeId;
        public string Item;
        public string TypeName;
        public DateTime StartDate;
        public DateTime EndDate;
        public DataTable CustomItemDelete
        {
            get { return _CustomItemDelete; }
            set { _CustomItemDelete = value; }
        }
        public DataTable EstimateData
        {
            get { return _EstimateData; }
            set { _EstimateData = value; }
        }
        public DataTable CustomTabItem
        {
            get { return _CustomTabItem; }
            set { _CustomTabItem = value; }
        }
        public DataTable CustomItem
        {
            get { return _CustomItem; }
            set { _CustomItem = value; }
        }
        public DataTable MilestoneDt
        {
            get { return _MilestoneDt; }
            set { _MilestoneDt = value; }
        }
        public DataTable ProjectDt
        {
            get { return _projectDt; }
            set { _projectDt = value; }
        }
        public DataSet ProjectItem
        {
            get { return _projectItem; }
            set { _projectItem = value; }
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

        public int docid { get; set; }

        public int sort { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int JobTItemId { get; set; }
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string custom3 { get; set; }
        public string custom4 { get; set; }
        public string custom5 { get; set; }
        public string custom6 { get; set; }
        public string custom7 { get; set; }
        public string custom8 { get; set; }
        public string custom9 { get; set; }
        public string custom10 { get; set; }
        public string custom11 { get; set; }
        public string custom12 { get; set; }
        public string custom13 { get; set; }
        public string custom14 { get; set; }
        public string custom15 { get; set; }
        public string custom16 { get; set; }
        public string custom17 { get; set; }
        public string custom18 { get; set; }
        public string custom19 { get; set; }
        public string custom20 { get; set; }
        public string Username { get; set; }
        public int UserID { get; set; }
        public int TargetHPermission { get; set; }
        public double? OHPer { get; set; }
        public double? COMMSPer { get; set; }
        public double? MARKUPPer { get; set; }
        public string STaxName{ get; set; }
        public string EstimateType { get; set; }
        public bool IsSglBilAmt { get; set; }
        public bool IsBilFrmBOM { get; set; }
    }

    public class GetBomTypeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetRecurringCustomParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Job { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }
}
