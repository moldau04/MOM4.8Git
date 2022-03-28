using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetMaintenanceUnitCountViewModel
    {
        public int Loc { get; set; }
        public string Tag { get; set; }
        public string LocType { get; set; }
        public string Address { get; set; }
        public int RouteID { get; set; }
        public string Route { get; set; }
        public string Mech { get; set; }
        public string UnitType { get; set; }
        public string Cat { get; set; }
        public int UnitCount { get; set; }
        public string Area { get; set; }
        public string Jobs { get; set; }
        public int TotalCount { get; set; }
        public int AreaCount { get; set; }
        public int RouteCount { get; set; }
        public int LocCount { get; set; }
        public int RouteCatTypeCount { get; set; }
        public int AreaCatTypeCount { get; set; }
        public int TotalCatTypeCount { get; set; }

    }
}
