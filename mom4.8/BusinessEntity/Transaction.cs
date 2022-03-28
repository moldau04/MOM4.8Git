using System;
using System.Data;

namespace BusinessEntity
{
    public class Transaction
    {
        public int ID;
        public int BatchID;
        public DateTime TransDate;
        public int Acct;
        public int? AcctSub;
        public int Type;
        public int Line;
        public long Ref;
        public string TransDescription;
        public DateTime fDate { get; set; }
        public String fDesc { get; set; }
        public double Amount;
        public string Status;
        public int JobInt;
        public double PhaseDoub;
        public string strRef;
        public Int16 Sel;
        public double UseTax;
        public int fDateYear;
        public byte[] TimeStamp;
        public bool IsAccessible;
        public bool IsUseTax;
        public int UseTaxGL;
        public string UtaxName;
        public bool IsJob;
        
        private string _ConnConfig;
        private DataSet _dsTrans;
        private string _SearchValue;
        private string _tableName;

        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet DsTrans
        {
            get { return _dsTrans; }
            set { _dsTrans = value; }
        }
        public string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public int EN { get; set; }

        public int UserID { get; set; }

        //New parameter for API
        public IWarehouseLocAdj _IWarehouseLocAdj { get; set; }
    }
    public class TransactionModel
    {
        public TransactionModel(int _val, string _field)
        {
            FieldValue = _val;
            Field = _field;
        }

        public int FieldValue { get; set; }
        public string Field { get; set; }
    }

    public class GetTransByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int BatchID;
    }

    public class UpdateTransVoidCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Type;
        public int ID;
        public string TransDescription;
    }

    public class UpdateTransVoidCheckByBatchParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int16 Sel;
        public int Type;
        public int BatchID;
        public string TransDescription;
    }

    public class GetTransByBatchParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int BatchID;
    }


    public class AddJournalTransParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int BatchID;
        public int Ref;
        public DateTime TransDate;
        public int Line;
        public string TransDescription;
        public int Acct;
        public double Amount;
        public Int16 Sel;
        public int Type;
        public int JobInt;
        public double PhaseDoub;
        public int? AcctSub;
        public string strRef;
    }

    public class UpdateTransVoidCheckOpenParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Type;
        public double Amount;
        public string TransDescription;
        public int ID;
    }

    public class UpdateTransVoidCheckByBatchOpenParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Type;
        public double Amount;
        public int BatchID;
        public Int16 Sel;
        public string TransDescription;
    }

    public class GetTransByBatchTypeParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int BatchID;
        public int Type;
    }
    public class UpdateTransDateByBatchParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int BatchID;
        public DateTime TransDate;
        public long Ref;

    }
    public class CreateReceivePOInvWarehouseTransParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int BatchID;
        public int Line;
        public byte[] TimeStamp;
        public int Acct;
        public int? AcctSub;
        public int Type;
        public string strRef;
        public double Amount;
        public string Status;
        public DateTime fDate { get; set; }
        public String fDesc { get; set; }
        public int Ref;
        //public IWarehouseLocAdj _IWarehouseLocAdj { get; set; }
        public Decimal IWarehouseLocAdj_Hand { get; set; }
        public Decimal IWarehouseLocAdj_Balance { get; set; }
        public Decimal IWarehouseLocAdj_fOrder { get; set; }
        public Decimal IWarehouseLocAdj_Committed { get; set; }
        public Decimal IWarehouseLocAdj_Available { get; set; }
        public int IWarehouseLocAdj_InvID { get; set; }
        public string IWarehouseLocAdj_WarehouseID { get; set; }
        public int IWarehouseLocAdj_locationID { get; set; }

        public string CREATE__INVENTORY_RECEIVEPOINVWAREHOUSE = "spCreateReceivePOInvWarehouse";
    }
    public class ReceivePOInvWarehouseTransParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int BatchID;
        public int Line;
        public byte[] TimeStamp;
        public string strRef;
        public double Amount;
        public string Status;
        public DateTime fDate { get; set; }
        public String fDesc { get; set; }
       
    }

    public class UpdateTransCheckNoByBatchParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int BatchID;
        
        public int Ref;

    }
    public class UpdateDepositTransBankParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID { get; set; }
        public int Acct { get; set; }
        public int? AcctSub { get; set; }
        public DateTime TransDate { get; set; }
    }

}

