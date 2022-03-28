using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetEscalationContractsViewModel
    {
        public Int64 DuplicateCount { get; set; }
        public string locid { get; set; }
        public string Tag { get; set; }
        public string CType { get; set; }
        public string fdesc { get; set; }
        public string Freqency { get; set; }
        public string EscType { get; set; }
        public string Action { get; set; }
        public Int16 BEscCycle { get; set; }
        public Int16 BEscType { get; set; }
        public double BEscFact { get; set; }
        public string EscLast { get; set; }
        public string BStart { get; set; }
        public string Bfinish { get; set; }
        public string nextdue { get; set; }
        public double Bamt { get; set; }
        public double newamt { get; set; }
        public Int16 BLenght { get; set; }
        public int Job { get; set; }
        public string ExpirationDate { get; set; }
        public string RenewalNotes { get; set; }
        public string Status { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LocationCompanyName { get; set; }
    }
}
