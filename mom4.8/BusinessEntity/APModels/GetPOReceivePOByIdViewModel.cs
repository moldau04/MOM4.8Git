using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListGetPOReceivePOById
    {
        public List<GetPOReceivePOByIdTable1> lstTable1 { get; set; }
        public List<GetPOReceivePOByIdTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetPOReceivePOByIdTable1
    {
        //Table 1
        public int PO { get; set; }
        public DateTime fDate { get; set; }
        public string fDesc { get; set; }
        public double Amount { get; set; }
        public int Vendor { get; set; }
        public string VendorName { get; set; }
        public Int16 Status { get; set; }
        public DateTime Due { get; set; }
        public string ShipVia { get; set; }
        public Int16 Terms { get; set; }
        public string FOB { get; set; }
        public string ShipTo { get; set; }
        public Int16 Approved { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string ApprovedBy { get; set; }
        public int ReqBy { get; set; }
        public string fBy { get; set; }
        public string PORevision { get; set; }
        public string CourrierAcct { get; set; }
        public string POReasonCode { get; set; }
        public int ID { get; set; }
        public string Ref { get; set; }
        public string WB { get; set; }
        public string Comments { get; set; }
        public double ReceivedAmount { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string Address { get; set; }
        public Int16 Days { get; set; }
        public int Term { get; set; }
        public string State { get; set; }
    }

    [Serializable]
    public class GetPOReceivePOByIdTable2
    {
        //Table 2
        public int RowID { get; set; }
        public int ID { get; set; }
        public Int16 Line { get; set; }
        public int AcctID { get; set; }
        public string AcctNo { get; set; }
        public string Account { get; set; }
        public string fDesc { get; set; }
        public double Quan { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public int JobID { get; set; }
        public string JobName { get; set; }
        public Int16 PhaseID { get; set; }
        public string Phase { get; set; }
        public string Loc { get; set; }
        public double Usetax { get; set; }
        public string UName { get; set; }
        public string UtaxGL { get; set; }
        public int ReceivePO { get; set; }
        public int TypeID { get; set; }
        public int ItemID { get; set; }
        public string ItemDesc { get; set; }
        public int Ticket { get; set; }
        public string OpSq { get; set; }
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
        public string Warehouse { get; set; }
        public int WHLocID { get; set; }
        public string Warehousefdesc { get; set; }
        public string Locationfdesc { get; set; }
        
    }
}
