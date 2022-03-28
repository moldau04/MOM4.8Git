using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class ListGetRecurringCustom
    {
        public List<GetRecurringCustomTable1> lstTable1 { get; set; }
        public List<GetRecurringCustomTable2> lstTable2 { get; set; }
        public List<GetRecurringCustomTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetRecurringCustomTable1
    {
        public int JobT { get; set; }
        public int ID { get; set; }
        public int tblTabID { get; set; }
        public string Label { get; set; }
        public Int16 Line { get; set; }
        public Int16 Format { get; set; }
        public bool IsDeleted { get; set; }
        public string FieldControl { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class GetRecurringCustomTable2
    {
        public int JobT { get; set; }
        public int ID { get; set; }
        public int tblCustomFieldsID { get; set; }
        public Int16 Line { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }
        public Int16 Format { get; set; }
        public int tblTabID { get; set; }
        public string FieldControl { get; set; }
    }

    [Serializable]
    public class GetRecurringCustomTable3
    {
        public int JobT { get; set; }
    }
}
