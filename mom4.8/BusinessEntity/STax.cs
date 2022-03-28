using System;
using System.Data;

namespace BusinessEntity
{
    public class STax
    {
        public string ConnConfig { get; set; }

        public int ID { get; set; }

        public String Name { get; set; }

        public String fDesc { get; set; }

        public decimal Rate { get; set; }

        public String State { get; set; }

        public String Remarks { get; set; }

        public int Count { get; set; }

        public int GL { get; set; }

        public int Type { get; set; }

        public int UType { get; set; }

        public String PSTReg { get; set; }

        public String QBStaxID { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public Boolean IsTaxable { get; set; }

        public DataSet ds { get; set; }
    }
}
