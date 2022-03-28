using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class ListGetCustReportFiltersValue
    {
        public List<GetCustReportFiltersValueTable> lstTable { get; set; }
        public List<GetCustReportFiltersValueTable1> lstTable1 { get; set; }
        public List<GetCustReportFiltersValueTable2> lstTable2 { get; set; }
        public List<GetCustReportFiltersValueTable3> lstTable3 { get; set; }
        public List<GetCustReportFiltersValueTable4> lstTable4 { get; set; }
        public List<GetCustReportFiltersValueTable5> lstTable5 { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable
    {
        public string Name { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable1
    {
        public string Address { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable2
    {
        public string City { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable3
    {
        public string State { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable4
    {
        public string Type { get; set; }
    }

    [Serializable]
    public class GetCustReportFiltersValueTable5
    {
        public string Status { get; set; }
    }
}
