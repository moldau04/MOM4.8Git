using System;
using System.Data;

namespace BusinessEntity
{
    /// <summary>
    /// This class is representing stages of sales and opportunities 

    [Serializable]
    public class Stage
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Description of stages
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// No. of Stages Count
        /// </summary>
        public int Count { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }
        public string Probability { get; set; }
        public string ChartColor { get; set; }
        public string DepartmentIDs { set; get; }
        public DataTable Items { get; set; }
        public DataTable DeleteItems { get; set; }
    }
}
