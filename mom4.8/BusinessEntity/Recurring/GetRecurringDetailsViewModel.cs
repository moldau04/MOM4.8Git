using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetRecurringDetailsViewModel
    {
        public string Customer { get; set; }

        [Display(Name = "Location Id")]
        public string LocationId { get; set; }
        public string Location { get; set; }

        [Display(Name = "Loc Type")]
        public string LocType { get; set; }
        public string ServiceType { get; set; }
        public string Description { get; set; }
        public string PreferredWorker { get; set; }
        public string TicketStart { get; set; }
        public DateTime TicketTime { get; set; }
        public double Hours { get; set; }
        public string TicketFreq { get; set; }
        public string BillStart { get; set; }
        public double BillAmount { get; set; }
        public string BillFreqency { get; set; }
        public string Status { get; set; }
        public Int16 Expiration { get; set; }
        public string ExpirationDate { get; set; }
        public string Equipment { get; set; }
        public string PhoneMonitoring { get; set; }
        public string ContractType { get; set; }
        public string OccupancyDiscount { get; set; }
        public string Exclusions { get; set; }
        public string TermofContract { get; set; }
        public string PriceAdjustmentCap { get; set; }
        public string FireServiceTestingIncluded { get; set; }
        public string SpecialRates { get; set; }
        public string ContractExpiration { get; set; }
        public string ProratedItems { get; set; }
        public string AnnualTestIncluded { get; set; }
        public string FiveYearStateTestIncluded { get; set; }
        public string FireServiceTestedIncluded { get; set; }
        public string CancellationNotificationDays { get; set; }
        public string PriceAdjustmentNotificationDays { get; set; }
        public string AfterHoursCallsIncluded { get; set; }
        public string OGServiceCallsIncluded { get; set; }
        public string ContractHours { get; set; }
        public string ContractFormat { get; set; }
    }
}
