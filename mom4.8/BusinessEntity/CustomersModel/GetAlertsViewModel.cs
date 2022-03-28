using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetAlerts
    {
        public List<GetAlertsTable1> lstTable1 { get; set; }
        public List<GetAlertsTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetAlertsTable1
    {
        public int AlertID { get; set; }
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public string AlertCode { get; set; }
        public string AlertSubject { get; set; }
        public string AlertMessage { get; set; }

    }

    [Serializable]
    public class GetAlertsTable2
    {
        public int ID { get; set; }
        public int ScreenID { get; set; }
        public string ScreenName { get; set; }
        public int AlertID { get; set; }
        public bool Email { get; set; }
        public bool Text { get; set; }
        public string AlertCode { get; set; }
        public string NAME { get; set; }
    }
}
