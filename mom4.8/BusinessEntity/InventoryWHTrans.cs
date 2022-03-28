using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{

    public class InventoryWHTrans
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int InvID;
        public string WarehouseID;
        public int LocationID;
        public Decimal Hand;
        public Decimal Balance;
        public Decimal fOrder;
        public Decimal Committed;
        public Decimal Available;
        public string Screen;
        public int ScreenID;
        public string Mode;
        public DateTime Date;
        public string TransType;
        public int Batch;
        public DateTime fDate;
    }

    public class AddReceiveInventoryWHTransParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public int ID;
        public int InvID;
        public string WarehouseID;
        public int LocationID;
        public Decimal Hand;
        public Decimal Balance;
        public Decimal fOrder;
        public Decimal Committed;
        public Decimal Available;
        public string Screen;
        public int ScreenID;
        public string Mode;
        public DateTime Date;
        public string TransType;
        public int Batch;
        public DateTime fDate;
    }

}
