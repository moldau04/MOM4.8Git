using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetCustTemplateItemByID
    {
        public List<GetCustTemplateItemByIDTable1> lstTable1 { get; set; }
        public List<GetCustTemplateItemByIDTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetCustTemplateItemByIDTable1
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
        public string name { get; set; }
    }

    [Serializable]
    public class GetCustTemplateItemByIDTable2
    {
        public int ElevT { get; set; }
        public int ItemID { get; set; }
        public Int16 Line { get; set; }
        public string Value { get; set; }
    }
}
