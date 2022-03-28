using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetDefaultRouteTerr
    {
        public List<GetDefaultRouteTerrTable1> lstTable1 { get; set; }
        public List<GetDefaultRouteTerrTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetDefaultRouteTerrTable1
    {
        public int id { get; set; }
    }

    [Serializable]
    public class GetDefaultRouteTerrTable2
    {
        public int id { get; set; }
    }
}
