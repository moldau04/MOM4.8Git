using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class ListGetBillReportFiltersValue
    {
        public List<GetBillReportFiltersValueTable> lstTable { get; set; }
        public List<GetBillReportFiltersValueTable1> lstTable1 { get; set; }
        public List<GetBillReportFiltersValueTable2> lstTable2 { get; set; }
        public List<GetBillReportFiltersValueTable3> lstTable3 { get; set; }
        public List<GetBillReportFiltersValueTable4> lstTable4 { get; set; }
        public List<GetBillReportFiltersValueTable5> lstTable5 { get; set; }
        public List<GetBillReportFiltersValueTable6> lstTable6 { get; set; }
        public List<GetBillReportFiltersValueTable7> lstTable7 { get; set; }

    }

    [Serializable]
    public class GetBillReportFiltersValueTable
    {
        public DateTime InvoiceDate { get; set;}
    }

    [Serializable]
    public class GetBillReportFiltersValueTable1
    {
        public string Ref { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable2
    {
        public string Description { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable3
    {
        public double Amount { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable4
    {
        public string Status { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable5
    {
        public double UseTax { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable6
    {
        public string VendorName { get; set; }
    }

    [Serializable]
    public class GetBillReportFiltersValueTable7
    {
        public string PostingDate { get; set; }
    }
}
