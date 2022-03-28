using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class ListGetEscalationReportFiltersValue
    {
        public List<GetEscalationReportFiltersValueTable> lstTable { get; set; }
        public List<GetEscalationReportFiltersValueTable1> lstTable1 { get; set; }
        public List<GetEscalationReportFiltersValueTable2> lstTable2 { get; set; }
        public List<GetEscalationReportFiltersValueTable3> lstTable3 { get; set; }
        public List<GetEscalationReportFiltersValueTable4> lstTable4 { get; set; }
        public List<GetEscalationReportFiltersValueTable5> lstTable5 { get; set; }
        public List<GetEscalationReportFiltersValueTable6> lstTable6 { get; set; }
        public List<GetEscalationReportFiltersValueTable7> lstTable7 { get; set; }
        public List<GetEscalationReportFiltersValueTable8> lstTable8 { get; set; }
        public List<GetEscalationReportFiltersValueTable9> lstTable9 { get; set; }
        public List<GetEscalationReportFiltersValueTable10> lstTable10 { get; set; }
        public List<GetEscalationReportFiltersValueTable11> lstTable11 { get; set; }
        public List<GetEscalationReportFiltersValueTable12> lstTable12 { get; set; }
        public List<GetEscalationReportFiltersValueTable13> lstTable13 { get; set; }
        public List<GetEscalationReportFiltersValueTable14> lstTable14 { get; set; }
        public List<GetEscalationReportFiltersValueTable15> lstTable15 { get; set; }
        public List<GetEscalationReportFiltersValueTable16> lstTable16 { get; set; }
        public List<GetEscalationReportFiltersValueTable17> lstTable17 { get; set; }
        public List<GetEscalationReportFiltersValueTable18> lstTable18 { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable
    {
        public string LocationId { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable1
    {
        public string LocationName { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable2
    {
        public string ServiceType { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable3
    {
        public string Description { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable4
    {
        public string BillingFreqency { get; set; }
    }
    public class GetEscalationReportFiltersValueTable5
    {
        public string EscType { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable6
    {
        public string Action { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable7
    {
        public Int16 EscCycle { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable8
    {
        public double EscFactor { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable9
    {
        public string LastEsc { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable10
    {
        public string StartEsc { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable11
    {
        public string FinishEsc { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable12
    {
        public string NextDue { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable13
    {
        public double Amount { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable14
    {
        public double NewAmount { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable15
    {
        public Int16 Length { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable16
    {
        public int Contract { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable17
    {
        public string ExpirationDate { get; set; }
    }

    [Serializable]
    public class GetEscalationReportFiltersValueTable18
    {
        public string RenewalNotes { get; set; }
    }
    
    
}
