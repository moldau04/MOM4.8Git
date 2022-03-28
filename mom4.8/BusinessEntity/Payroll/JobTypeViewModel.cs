using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class JobTypeViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public int Color { get; set; }
        public string Remark { get; set; }
        public int IsDefault { get; set; }
        public string QBJobTypeID { get; set; }
        public DateTime LastUpdateDate{get;set;}
        public int PrimarySyncID { get; set; }


    }
}
