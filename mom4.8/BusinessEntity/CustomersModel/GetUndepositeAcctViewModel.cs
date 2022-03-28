using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetUndepositeAcctViewModel
    {
        public int ID { get; set; }
        public string Acct { get; set; }
        public string fDesc { get; set; }
        public int Department { get; set; }
        public double Balance { get; set; }
        public Int16 Type { get; set; }
        public string Sub { get; set; }
        public string Remarks { get; set; }
        public Int16 Control { get; set; }
        public Int16 InUse { get; set; }
        public Int16 Detail { get; set; }
        public string CAlias { get; set; }
        public Int16 Status { get; set; }
        public string Sub2 { get; set; }
        public Int16 DAT { get; set; }
        public Int16 Branch { get; set; }
        public Int16 CostCenter { get; set; }
        public string AcctRoot { get; set; }
        public string QBAccountID { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string DefaultNo { get; set; }
        public string TimeStamp { get; set; }
        public int EN { get; set; }
    }
}
