using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetEquipReportFiltersValue
    {
        public List<GetEquipReportFiltersValueTable> lstTable { get; set; }
        public List<GetEquipReportFiltersValueTable1> lstTable1 { get; set; }
        public List<GetEquipReportFiltersValueTable2> lstTable2 { get; set; }
        public List<GetEquipReportFiltersValueTable3> lstTable3 { get; set; }
        public List<GetEquipReportFiltersValueTable4> lstTable4 { get; set; }
        public List<GetEquipReportFiltersValueTable5> lstTable5 { get; set; }
        public List<GetEquipReportFiltersValueTable6> lstTable6 { get; set; }
        public List<GetEquipReportFiltersValueTable7> lstTable7 { get; set; }
        public List<GetEquipReportFiltersValueTable8> lstTable8 { get; set; }
        public List<GetEquipReportFiltersValueTable9> lstTable9 { get; set; }
        public List<GetEquipReportFiltersValueTable10> lstTable10 { get; set; }
        public List<GetEquipReportFiltersValueTable11> lstTable11 { get; set; }
        public List<GetEquipReportFiltersValueTable12> lstTable12 { get; set; }
        public List<GetEquipReportFiltersValueTable13> lstTable13 { get; set; }
        public List<GetEquipReportFiltersValueTable14> lstTable14 { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable
    {
        public string Location { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable1
    {
        public int OwnerID { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable2
    {
        public string OwnerName { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable3
    {
        public string equipment { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable4
    {
        public string Unique { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable5
    {
        public string five_year_Insp_Date { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable6
    {
        public string annual_Insp_Date { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable7
    {
        public string customer { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable8
    {
        public string Inspector_Name { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable9
    {
        public string Manuf { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable10
    {
        public string EquipmentType { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable11
    {
        public string ServiceType { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable12
    {
        public DateTime InstalledOn { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable13
    {
        public string BuildingType { get; set; }
    }

    [Serializable]
    public class GetEquipReportFiltersValueTable14
    {
        public string EquipmentState { get; set; }
    }
}
