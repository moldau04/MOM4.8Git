using System;

namespace BusinessEntity.Reports.IncomeStatements
{
    public class ComparativeFSWithCenterParam
    {
        public DateTime MonthStartDatePreviousYear { get; set; }
        public DateTime MonthEndDatePreviousYear { get; set; }
        public DateTime MonthStartDateCurrentYear { get; set; }
        public DateTime MonthEndDateCurrentYear { get; set; }
        public DateTime YTDStartDatePreviousYear { get; set; }
        public DateTime YTDEndDatePreviousYear { get; set; }
        public DateTime YTDStartDateCurrentYear { get; set; }
        public DateTime YTDEndDateCurrentYear { get; set; }
        public string Departments { get; set; }
    }
}
