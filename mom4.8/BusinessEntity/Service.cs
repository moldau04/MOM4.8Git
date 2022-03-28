using System;

namespace BusinessEntity
{
    [Serializable]
    public class Service
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
    }
}
