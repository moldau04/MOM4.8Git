using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetLocationReportFiltersValue
    {
        public List<GetLocationReportFiltersValueTable1> lstTable1 { get; set; }
        public List<GetLocationReportFiltersValueTable2> lstTable2 { get; set; }
        public List<GetLocationReportFiltersValueTable3> lstTable3 { get; set; }
        public List<GetLocationReportFiltersValueTable4> lstTable4 { get; set; }
        public List<GetLocationReportFiltersValueTable5> lstTable5 { get; set; }
        public List<GetLocationReportFiltersValueTable6> lstTable6 { get; set; }
        public List<GetLocationReportFiltersValueTable7> lstTable7 { get; set; }
        public List<GetLocationReportFiltersValueTable8> lstTable8 { get; set; }
        public List<GetLocationReportFiltersValueTable9> lstTable9 { get; set; }
        public List<GetLocationReportFiltersValueTable10> lstTable10 { get; set; }
        public List<GetLocationReportFiltersValueTable11> lstTable11 { get; set; }
        public List<GetLocationReportFiltersValueTable12> lstTable12 { get; set; }
        public List<GetLocationReportFiltersValueTable13> lstTable13 { get; set; }
        public List<GetLocationReportFiltersValueTable14> lstTable14 { get; set; }
        public List<GetLocationReportFiltersValueTable15> lstTable15 { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable1
    {
        public string Customer { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable2
    {
        public string Location { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable3
    {
        public string City { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable4
    {
        public string State { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable5
    {
        public string Zip { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable6
    {
        public string Address { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable7
    {
        public string LocationSTax { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable8
    {
        public string Type { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable9
    {
        public string TaxDesc { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable10
    {
        public string TaxName { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable11
    {
        public double TaxRate { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable12
    {
        public string SalesPerson { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable13
    {
        public string DefaultWorker { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable14
    {
        public string Acct { get; set; }
    }

    [Serializable]
    public class GetLocationReportFiltersValueTable15
    {
        public string PreferredWorker { get; set; }
    }

}
