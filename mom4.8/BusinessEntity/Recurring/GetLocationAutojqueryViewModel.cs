using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetLocationAutojqueryViewModel
    {
        public int value { get; set; }
        public string label { get; set; }
        public string desc { get; set; }
        public string custsageid { get; set; }
        public int rolid { get; set; }
        public string CompanyName { get; set; }
        public string STaxRate { get; set; }
        public string STax { get; set; }
    }
}
