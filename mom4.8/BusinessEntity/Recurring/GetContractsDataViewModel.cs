using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetContractsDataViewModel
    {
        public string SREMARKS { get; set; }
        public DateTime expirationdate { get; set; }
        public int Job { get; set; }
        public string ctype { get; set; }
        public string fdesc { get; set; }
        public double BAmt { get; set; }
        public double Hours { get; set; }
        public string locid { get; set; }
        public string Tag { get; set; }
        public Int16 credit { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public string State { get; set; }
        public string name { get; set; }
        public double MonthlyBill { get; set; }
        public double MonthlyHours { get; set; }
        public string Freqency { get; set; }
        public string TicketFreq { get; set; }
        public string Status { get; set; }
        public string Worker { get; set; }
    }
}
