using System;

namespace BusinessEntity
{
    [Serializable]
    public class RetainFilter
    {
        public String FilterValue { get; set; }
        public String FilterColumn { get; set; }
    }
}
