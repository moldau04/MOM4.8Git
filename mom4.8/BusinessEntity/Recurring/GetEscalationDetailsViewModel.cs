using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetEscalationDetailsViewModel
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public string BillingFreqency { get; set; }
        public string EscType { get; set; }
        public string Action { get; set; }
        public Int16 EscCycle { get; set; }
        public double EscFactor { get; set; }
        public string LastEsc { get; set; }
        public string StartEsc { get; set; }
        public string FinishEsc { get; set; }
        public string NextDue { get; set; }
        public double Amount { get; set; }
        public double NewAmount { get; set; }
        public Int16 Length { get; set; }
        public int Contract { get; set; }
        public string ExpirationDate { get; set; }
        public string RenewalNotes { get; set; }

    }
}
