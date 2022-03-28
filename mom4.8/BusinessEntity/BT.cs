using System;

namespace BusinessEntity
{
    [Serializable]
    public class BT
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Description of BT
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// No. of Stages Count
        /// </summary>
        public int Count { get; set; }

        public string Label { get; set; }
    

    }
}
