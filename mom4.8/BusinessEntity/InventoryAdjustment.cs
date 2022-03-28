using System;
using System.Data;

namespace BusinessEntity
{
    public class InventoryAdjustment
    {
        #region ::StoreProc::
        public static string GET_ALL_INVENTORY_ADJUSTMENT = "spReadInventoryAdjustments";
        public static string GET_ALL_INVENTORY_ADJUSTMENT_BY_ID = "spReadInventoryAdjustments";
        #endregion
        #region ::Private Property Variable Declaration::
        private int _ID;
        private string _Name;
        private string _fDesc;
        private decimal _Quantity;
        private decimal _Amount;
        private DateTime? _fDate;
        private string _stdate;
        private string _enddate;
        private Inventory _inv;
        private Transaction _trans;
        private Chart _acct;
        #endregion
        #region::Public Property Declaration::

        public DataSet Ds;
        public string ConnConfig;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string fDesc { get { return _fDesc; } set { _fDesc = value; } }
        public decimal Quantity { get { return _Quantity; } set { _Quantity = value; } }
        public decimal Amount { get { return _Amount; } set { _Amount = value; } }
        public DateTime? fDate { get { return _fDate; } set { _fDate = value; } }
        public string Stdate { get { return _stdate; } set { _stdate = value; } }

        public string Enddate { get { return _enddate; } set { _enddate = value; } }

        public int EN { get; set; }
        public int UserID { get; set; }

        public int CompanyID { get; set; }

        public Inventory Inv { get { return _inv; } set { _inv = value; } }
        public Transaction Trans { get { return _trans; } set { _trans = value; } }

        public Chart Acct { get { return _acct; } set { _acct = value; } }

        public InvWarehouse InvWarehouse { get; set; }

        public IWarehouseLocAdj IWarehouseLocAdj { get; set; }

        #endregion
    }

    public class GetAllInventoryAdjustmentByDateParam
    {
        private string _stdate;
        private string _enddate;

        public string ConnConfig;
        public string GET_ALL_INVENTORY_ADJUSTMENT = "spReadInventoryAdjustments";
        public string Stdate { get { return _stdate; } set { _stdate = value; } }
        public string Enddate { get { return _enddate; } set { _enddate = value; } }
        public int EN { get; set; }
        public int UserID { get; set; }
    }

    public class DeleteAdjustmentParam
    {
        private int _ID;
        public string ConnConfig;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
    }

    public class GetInventoryAdjustmentByIDParam
    {
        public string ConnConfig; 
        private int _ID;
        public string GET_ALL_INVENTORY_ADJUSTMENT = "spReadInventoryAdjustments";
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

    }

    public class CreateInventoryAdjustmentsParam
    {
        public string ConnConfig;
        public int EN { get; set; }
        public int CompanyID { get; set; }
        //public Inventory Inv { get; set; }
        //public IWarehouseLocAdj IWarehouseLocAdj { get; set; }
        //public Chart Acct { get; set; }
        //public Transaction Trans { get; set; }
        private int _ID;
        private DateTime? _fDate;
        private string _fDesc;
        private decimal _Quantity;
        private decimal _Amount;
        private int _Inv_ID;
        private decimal _Inv_Hand;
        private decimal _Inv_Balance;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public DateTime? fDate { get { return _fDate; } set { _fDate = value; } }
        public string fDesc { get { return _fDesc; } set { _fDesc = value; } }
        public decimal Quantity { get { return _Quantity; } set { _Quantity = value; } }
        public decimal Amount { get { return _Amount; } set { _Amount = value; } }

        //Inventory Parameter
        public int Inv_ID
        {
            get
            {
                return _Inv_ID;
            }
            set
            {
                _Inv_ID = value;
            }
        }
        public decimal Inv_Hand { get { return _Inv_Hand; } set { _Inv_Hand = value; } }
        public decimal Inv_Balance { get { return _Inv_Balance; } set { _Inv_Balance = value; } }

        //IWarehouseLocAdj Parameter
        public int IWarehouseLocAdj_InvID { get; set; }
        public string IWarehouseLocAdj_WarehouseID { get; set; }
        public int IWarehouseLocAdj_locationID { get; set; }
        public Decimal IWarehouseLocAdj_Hand { get; set; }
        public Decimal IWarehouseLocAdj_Balance { get; set; }

        //Chart Parameter
        public int Acct_ID { get; set; }

        //Transaction Parameter
        public int Trans_ID; 
        public int Trans_BatchID;
        public byte[] Trans_TimeStamp;
    }

}
