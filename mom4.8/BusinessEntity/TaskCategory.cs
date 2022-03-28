using System;

namespace BusinessEntity
{
    [Serializable]
    public class TaskCategory
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Primary Key
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of BT
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// No. of Stages Count
        /// </summary>
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    

    }
}
