using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class ComparativeStatementRequest
    {
        public int Line { get; set; }

        public int Index { get; set; }

        public string Type { get; set; }

        public string Label { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Column1 { get; set; }

        public int? Column2 { get; set; }
    }

    [Serializable]
    public class DashboardColumnRequest
    {
        public int Index { get; set; }

        public string Field { get; set; }

        public string Type { get; set; }

        public string Label { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Column1 { get; set; }

        public int? Column2 { get; set; }
    }
}
