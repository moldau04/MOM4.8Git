using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class TransactionViewModel
    {
        public int ID{ get; set; }
        public int BatchID{ get; set; }
        public DateTime TransDate{ get; set; }
        public int Acct{ get; set; }
        public int? AcctSub{ get; set; }
        public int Type{ get; set; }
        public int Line{ get; set; }
        public int Ref{ get; set; }
        public string TransDescription{ get; set; }
        public int VInt{ get; set; }
        public DateTime fDate { get; set; }
        public String fDesc { get; set; }
        public double Amount{ get; set; }
        public string Status{ get; set; }
        public int JobInt{ get; set; }
        public double PhaseDoub{ get; set; }
        public string strRef{ get; set; }
        public Int16 Sel{ get; set; }
        public double UseTax{ get; set; }
        public int fDateYear{ get; set; }
        public byte[] TimeStamp{ get; set; }
        public bool IsAccessible{ get; set; }
        public bool IsUseTax{ get; set; }
        public int UseTaxGL{ get; set; }
        public string UtaxName{ get; set; }
        public bool IsJob{ get; set; }

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
        public double VDoub{ get; set; }
        public string AcctNo{ get; set; }
        public string AcctName{ get; set; }
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
}
