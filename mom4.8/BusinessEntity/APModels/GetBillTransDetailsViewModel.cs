using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBillTransDetailsViewModel
    {
        public int Batch { get; set; }
        public int ID { get; set; }
        public int JobId { get; set; }
        public string jobName { get; set; }
        public string Ticket { get; set; }
        public string TypeID { get; set; }
        public string PhaseID { get; set; }
        public string phase { get; set; }
        public string ItemID { get; set; }
        public string ItemDesc { get; set; }
        public string Warehouse { get; set; }
        public string Warehousefdesc { get; set; }
        public string WHLocID { get; set; }
        public string Locationfdesc { get; set; }
        public string AcctID { get; set; }
        public string AcctName { get; set; }
        public string Quan { get; set; }
        public string Amount { get; set; }
        public string line { get; set; }
        public string Ref { get; set; }
        public string Sel { get; set; }
        public string Type { get; set; }
        public string strRef { get; set; }
        public string AcctNo { get; set; }
        public string fDesc { get; set; }
        public string UseTax { get; set; }
        public string UtaxGL { get; set; }
        public string UName { get; set; }
        public string loc { get; set; }
        public string OpSq { get; set; }
        public string PrvIn { get; set; }
        public string PrvInQuan { get; set; }
        public string OutstandQuan { get; set; }
        public string OutstandBalance { get; set; }
        public Int16 STax { get; set; }
        public string STaxName { get; set; }
        public string STaxRate { get; set; }
        public string STaxAmt { get; set; }
        public string STaxGL { get; set; }
        public string GSTRate { get; set; }
        public string GTaxAmt { get; set; }
        public string GSTTaxGL { get; set; }
        public string STaxType { get; set; }
        public string UTaxType { get; set; }
        public int IsPO { get; set; }
        public Int16 GTax { get; set; }
    }
}
