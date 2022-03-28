using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetLeadEquipByID
    {
        public List<GetLeadEquipByIDTable> lstTable { get; set; }
        public List<GetLeadEquipByIDTable1> lstTable1 { get; set; }
        public List<GetLeadEquipByIDTable2> lstTable2 { get; set; }
        public List<GetLeadEquipByIDTable3> lstTable3 { get; set; }
        public List<GetLeadEquipByIDTable4> lstTable4 { get; set; }
    }

    [Serializable]
    public class GetLeadEquipByIDTable
    {
        public int Lead { get; set; }
        public int Owner { get; set; }
        public string Unit { get; set; }
        public string fDesc { get; set; }
        public string Type { get; set; }
        public string Cat { get; set; }
        public string Manuf { get; set; }
        public string Serial { get; set; }
        public string State { get; set; }
        public DateTime Since { get; set; }
        public DateTime Last { get; set; }
        public double Price { get; set; }
        public Int16 Status { get; set; }
        public string Building { get; set; }
        public string Remarks { get; set; }
        public string fGroup { get; set; }
        public int Template { get; set; }
        public string InstallBy { get; set; }
        public DateTime install { get; set; }
        public string category { get; set; }
        public int unitid { get; set; }
        public string Classification { get; set; }
        public bool shut_down { get; set; }
        public string ShutdownReason { get; set; }
    }

    [Serializable]
    public class GetLeadEquipByIDTable1
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int EquipT { get; set; }
        public string fDesc { get; set; }
        public DateTime Lastdate { get; set; }
        public DateTime NextDateDue { get; set; }
        public int Frequency { get; set; }
        public string Section { get; set; }
        public string Notes { get; set; }

    }

    [Serializable]
    public class GetLeadEquipByIDTable2
    {
        public int orderno { get; set; }
        public int ID { get; set; }
        public int ElevT { get; set; }
        public int Elev { get; set; }
        public int CustomID { get; set; }
        public string fDesc { get; set; }
        public Int16 Line { get; set; }
        public string Value { get; set; }
        public string Format { get; set; }
        public Int16 fExists { get; set; }
        public int PrimarySyncID { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdateUser { get; set; }
        public int OrderNo { get; set; }
        public Int16 LeadEquip { get; set; }
        public string formatMOM { get; set; }

    }

    [Serializable]
    public class GetLeadEquipByIDTable3
    {
        public int ElevT { get; set; }
        public int ItemID { get; set; }
        public Int16 Line { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class GetLeadEquipByIDTable4
    {
        public string fUser { get; set; }
        public string Screen { get; set; }
        public int Ref { get; set; }
        public string Field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime CreatedStamp { get; set; }
        public DateTime fDate { get; set; }
        public DateTime fTime { get; set; }
    }
}
