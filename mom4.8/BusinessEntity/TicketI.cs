using System;
using System.Data;

namespace BusinessEntity
{
    public class TicketI
    {
        public int TicketID { get; set; }
        public int Line { get; set; }
        public int Item { get; set; }
        public double Quan { get; set; }
        public string fDesc { get; set; }
        public int Charge { get; set; }
        public double Amount { get; set; }
        public int Phase { get; set; }
        public string ConnConfig { get; set; }
        public DataSet DS { get; set; }
        public Guid TicketUID { get; set; }
        public int TypeID { get; set; }
        public string WarehouseID { get; set; }
        public int WHLocationID { get; set; }
        public string PhaseName { get; set; }
    }
}
