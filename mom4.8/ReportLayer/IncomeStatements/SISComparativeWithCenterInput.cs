using System;
using System.Data;

namespace ReportLayer.IncomeStatements
{
    public class SISComparativeWithCenterInput
    {
        //DateTime startDate, DateTime endDate, int yearEnd, string departements, DataSet centrals, DataSet dsCompany
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int YearEnd { get; set; }
        public string Departments { get; set; }
        public string OfficeCenter { get; set; }
        public DataSet  Centrals { get; set; }
        public DataSet DsCompany { get; set; }
    }
}
