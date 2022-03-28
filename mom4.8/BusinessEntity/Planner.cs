using System;

namespace BusinessEntity
{
    public class Planner
    {
        public string ConnConfig { get; set; }
        public int ProjectID { get; set; }
        public string Desc { get; set; }
        public int ParentID { get; set; }
        public string TaskName { get; set; }
        public int idx { get; set; }
        public string TaskType { get; set; }
        public Double Duration { get; set; }
        public string DurationUnit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool Summary { get; set; }
        public int PlannerID { get; set; }
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string Type { get; set; }
        public int RootVendorID { get; set; }
        public string RootVendorName { get; set; }
        public string ProjectName { get; set; }
        public int ItemRefID { get; set; }
        public Double ActualHours { get; set; }
    }
}
