using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListGetBillingItems
    {
        public List<GetBillingItemsTable1> lstTable1 { get; set; }
        public List<GetBillingItemsTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetBillingItemsTable1
    {
        public int ID { get; set; }
        public string ItemDesc { get; set; }
        public int ItemID { get; set; }
        public int Ticket { get; set; }
        public string JobName { get; set; }
        public int JobID { get; set; }
        public string Phase { get; set; }
        public int PhaseID { get; set; }
        public int TypeID { get; set; }
        public string fDesc { get; set; }
        public string AcctNo { get; set; }
        public int AcctID { get; set; }
        public int Quan { get; set; }
        public double Amount { get; set; }
        public double UseTax { get; set; }
        public string Loc { get; set; }
        public string Uname { get; set; }
        public string UtaxGL { get; set; }
        public string OpSq { get; set; }
        public int RowNo { get; set; }
        public double PrvIn { get; set; }
        public double PrvInQuan { get; set; }
        public double OutstandQuan { get; set; }
        public double OutstandBalance { get; set; }
        public int STax { get; set; }
        public string STaxName { get; set; }
        public double STaxRate { get; set; }
        public double STaxAmt { get; set; }
        public int STaxGL { get; set; }
        public double GSTRate { get; set; }
        public double GTaxAmt { get; set; }
        public int GSTTaxGL { get; set; }
    }

    [Serializable]
    public class GetBillingItemsTable2
    {
        public string AcctNo { get; set; }
        public string ProjNo { get; set; }
        public string Code { get; set; }
        public string ItemDis { get; set; }
        public string Amount { get; set; }
        public int RowNo { get; set; }
    }
}
