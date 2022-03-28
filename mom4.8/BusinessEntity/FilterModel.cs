using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class FilterModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public int SortOrder { get; set; }
        public Nullable<int> Active { get; set; }
        public Dictionary<string, string> Filters { get; set; }
    }
}
