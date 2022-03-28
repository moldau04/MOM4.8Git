using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListGetAPExpenses
    {
        public List<GetAPExpensesTable1> lstTable1 { get; set; }
        public List<GetAPExpensesTable2> lstTable2 { get; set; }

    }

    [Serializable]
    public class GetAPExpensesTable1
    {
        public int ID { get; set; }
        public DateTime fDate { get; set; }//datetime
        public string Ref { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public double RunTotal { get; set; }
        public string Type { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        public Int16 Status { get; set; }
        public string StatusName { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double Disc { get; set; }
        public double Balance { get; set; }
        public int TRID { get; set; }

    }

    [Serializable]
    public class GetAPExpensesTable2
    {
        public double Column1 { get; set; }
    }
}
