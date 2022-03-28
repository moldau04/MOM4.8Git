using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetMonthlyRecurringHoursViewModel
    {
        public int Job { get; set; }
        public int Loc { get; set; }
        public string LocID { get; set; }
        public string Tag { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int MonthlyHours { get; set; }
        public string RouteName { get; set; }
        public string MechName { get; set; }
        public int ActualHours { get; set; }
        public int EquipCount { get; set; }
    }
}
