using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetPJAcctDetailByIDViewModel
    {
        public int ID { get; set; }
        public int Batch { get; set; }
        public DateTime fDate { get; set; }//datetime
        public Int16 Type { get; set; }
        public Int16 Line { get; set; }
        public int Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public int Acct { get; set; }
        public int AcctSub { get; set; }
        public string Status { get; set; }
        public Int16 Sel { get; set; }
        public int VInt { get; set; }
        public Int16 VDoub { get; set; }
        public int EN { get; set; }
        public string strRef { get; set; }
        //public TimeStamp TimeStamp { get; set; }
        public string AcctName { get; set; }
    }
}
