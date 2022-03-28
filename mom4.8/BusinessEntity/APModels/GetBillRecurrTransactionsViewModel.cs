using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBillRecurrTransactionsViewModel
    {
        public int rowid { get; set; }
        public double Quan { get; set; }
        public int AcctID { get; set; }
        public string amount { get; set; }
        public int Batch { get; set; }
        public int id { get; set; }
        public Int16 line { get; set; }
        public int Ref { get; set; }
        public Int16 sel { get; set; }
        public Int16 type { get; set; }
        public Int16 PhaseID { get; set; }
        public int JobId { get; set; }
        public string strRef { get; set; }
        public string AcctNo { get; set; }
        public string fDesc { get; set; }
        public string AcctName { get; set; }
        public string UseTax { get; set; }
        public int UtaxGL { get; set; }
        public int Ticket { get; set; }
        public int TypeID { get; set; }
        public string UName { get; set; }
        public string jobName { get; set; }
        public string phase { get; set; }
        public string ItemDesc { get; set; }
        public string Warehouse { get; set; }
        public string Warehousefdesc { get; set; }
        public int WHLocID { get; set; }
        public string Locationfdesc { get; set; }
        public int ItemID { get; set; }
        public string loc { get; set; }
        public string OpSq { get; set; }
        public double PrvIn { get; set; }
        public double PrvInQuan { get; set; }
        public double OutstandQuan { get; set; }
        public double OutstandBalance { get; set; }
        public Int16 STax { get; set; }
        public string STaxName { get; set; }
        public double STaxRate { get; set; }
        public double STaxAmt { get; set; }
        public int STaxGL { get; set; }
        public double GSTRate { get; set; }
        public double GTaxAmt { get; set; }
        public int GSTTaxGL { get; set; }
        public int STaxType { get; set; }
        public int UTaxType { get; set; }
        public int IsPO { get; set; }
        public Int16 GTax { get; set; }
    }
}
