using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetRouteViewModel
    {
        public List<GetRouteTable1> lstTable1 { get; set; }
        public List<GetRouteTable2> lstTable2 { get; set; }
        public List<GetRouteTable3> lstTable3 { get; set; }
    }

    [Serializable]
    public class GetRouteTable1
    {
        public string name { get; set; }
        public int id { get; set; }
        public string remarks { get; set; }
        public string Color { get; set; }
        public string mechname { get; set; }
        public string label { get; set; }

    }

    [Serializable]
    public class GetRouteTable2
    {
        public string fUser { get; set; }
        public int ID { get; set; }
    }

    [Serializable]
    public class GetRouteTable3
    {
        public string fUser { get; set; }
        public int ID { get; set; }
    }
}
