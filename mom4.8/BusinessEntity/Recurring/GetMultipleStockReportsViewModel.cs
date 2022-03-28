using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetMultipleStockReportsViewModel
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public int UserId { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsAscendingOrder { get; set; }
        public string SortBy { get; set; }
        public bool IsStock { get; set; }
        public string Module { get; set; }
        public string Condition { get; set; }
    }
}
