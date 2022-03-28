﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class GetRecurringContractLogsViewModel
    {
        public string fUser { get; set; }
        public string Screen { get; set; }
        public Int64 Ref { get; set; }
        public string Field { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public DateTime CreatedStamp { get; set; }
        public DateTime fDate { get; set; }
        public DateTime fTime { get; set; }
    }
}
